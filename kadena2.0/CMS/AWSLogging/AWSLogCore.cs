using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using Amazon.Runtime;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kadena.AWSLogging
{
    public partial class AWSLoggerCore : IAWSLoggerCore
    {
        #region Private Members

        const string EMPTY_MESSAGE = "\t";
        private ConcurrentQueue<InputLogEvent> _pendingMessageQueue = new ConcurrentQueue<InputLogEvent>();
        private string _currentStreamName = null;
        private LogEventBatch _repo = new LogEventBatch();
        private CancellationTokenSource _cancelStartSource;
        private AWSLoggerConfig _config;
        private IAmazonCloudWatchLogs _client;
        private bool _isTerminated = false;
        private DateTime _maxBufferTimeStamp = new DateTime();
        private string _logType;
        const double MAX_BUFFER_TIMEDIFF = 5;
        private bool isInit = false;
        #endregion
        public AWSLoggerCore(AWSLoggerConfig config, string logType)
        {
            _config = config;
            _logType = logType;
            _client = new AmazonCloudWatchLogsClient(_config.RegionEndpoint);
            ((AmazonCloudWatchLogsClient)this._client).BeforeRequestEvent += ServiceClientBeforeRequestEvent;
            StartMonitor();
            RegisterShutdownHook();

            // Disable adding messages when logger is not properly configured
            isInit = !(string.IsNullOrWhiteSpace(_config?.LogGroup));
        }

        private void RegisterShutdownHook()
        {
            AppDomain.CurrentDomain.DomainUnload += ProcessExit;
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
        }

        private void ProcessExit(object sender, EventArgs e)
        {
            Close();
        }

        public void Close()
        {
            _isTerminated = true;
            _cancelStartSource.Cancel();
            _config.ShutDown();
            Task.Run(async () =>
            {
                await Monitor(CancellationToken.None);
            }).Wait();
        }


        /// <summary>
        /// A Concurrent Queue is used to store the messages from 
        /// the logger
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(string message)
        {
            if (!isInit)
            {
                return;
            }

            if (string.IsNullOrEmpty(message))
            {
                message = EMPTY_MESSAGE;
            }
            if (_pendingMessageQueue.Count >= _config.MaxQueuedMessages)
            {
                if ((_maxBufferTimeStamp == DateTime.MinValue) || (DateTime.Now > _maxBufferTimeStamp.Add(TimeSpan.FromMinutes(MAX_BUFFER_TIMEDIFF))))
                {
                    _maxBufferTimeStamp = DateTime.Now;
                    message = "The AWS Logger in-memory buffer has reached maximum capacity";
                    _pendingMessageQueue.Enqueue(new InputLogEvent
                    {
                        Timestamp = DateTime.Now,
                        Message = message,
                    });
                }
            }
            else
            {
                _pendingMessageQueue.Enqueue(new InputLogEvent
                {
                    Timestamp = DateTime.Now,
                    Message = message,
                });
            }
        }

        ~AWSLoggerCore()
        {
            if (_cancelStartSource != null)
            {
                _cancelStartSource.Dispose();
            }
        }
        /// <summary>
        /// Kicks off the Poller Thread to keep tabs on the PutLogEvent request and the
        /// Concurrent Queue
        /// </summary>
        /// <param name="PatrolSleepTime"></param>
        public void StartMonitor()
        {
            _cancelStartSource = new CancellationTokenSource();
            Task.Run(async () =>
            {
                await Monitor(_cancelStartSource.Token);
            });
        }

        /// <summary>
        /// Patrolling thread. keeps tab on the PutLogEvent request and the
        /// Concurrent Queue
        /// </summary>
        private async Task Monitor(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                if (_currentStreamName == null)
                {
                    await LogEventTransmissionSetup(token).ConfigureAwait(false);
                }
                while (!token.IsCancellationRequested)
                {
                    if (!_pendingMessageQueue.IsEmpty)
                    {
                        while (!_pendingMessageQueue.IsEmpty)
                        {
                            InputLogEvent ev;
                            if (_pendingMessageQueue.TryDequeue(out ev))
                            {

                                // See if new message will cause the current batch to violote the size constraint.
                                // If so send the current batch now before adding more to the batch of messages to send.
                                if (_repo.IsSizeConstraintViolated(ev.Message))
                                {
                                    await SendMessages(token).ConfigureAwait(false);
                                }
                                _repo.AddMessage(ev);
                                if (_repo.ShouldSendRequest && !_isTerminated)
                                {
                                    await SendMessages(token).ConfigureAwait(false);
                                }

                            }
                        }
                    }
                    else
                    {
                        // If the logger is being terminated and all the messages have been sent exit out of loop.
                        // If there are messages keep pushing the remaining messages before the process dies.
                        if (_isTerminated && _repo._request.LogEvents.Count == 0)
                        {
                            break;
                        }
                        await Task.Delay(Convert.ToInt32(_config.MonitorSleepTime.TotalMilliseconds));
                        if (_repo.ShouldSendRequest)
                        {
                            await SendMessages(token).ConfigureAwait(false);
                        }
                    }
                }
            }
            catch (OperationCanceledException oc)
            {
                LogLibraryError(oc);
                throw;
            }
            catch (Exception e)
            {
                LogLibraryError(e);
            }

        }

        /// <summary>
        /// Method to transmit the PutLogEvent Request
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task SendMessages(CancellationToken token)
        {
            try
            {
                var response = await _client.PutLogEventsAsync(_repo._request, token).ConfigureAwait(false);
                _repo.Reset(response.NextSequenceToken);
            }
            catch (TaskCanceledException tc)
            {
                LogLibraryError(tc);
                throw;
            }
            catch (Exception e)
            {
                //In case the NextSequenceToken is invalid for the last sent message, a new stream would be 
                //created for the said application.

                LogLibraryError(e);

                await LogEventTransmissionSetup(token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Creates and Allocates resources for message trasnmission
        /// </summary>
        /// <returns></returns>
        /// 
        private async Task LogEventTransmissionSetup(CancellationToken token)
        {
            try
            {
                var logGroupResponse = await _client.DescribeLogGroupsAsync(new DescribeLogGroupsRequest
                {
                    LogGroupNamePrefix = _config.LogGroup
                }, token).ConfigureAwait(false);

                if (logGroupResponse.LogGroups.FirstOrDefault(x => string.Equals(x.LogGroupName, _config.LogGroup, StringComparison.Ordinal)) == null)
                {
                    await _client.CreateLogGroupAsync(new CreateLogGroupRequest { LogGroupName = _config.LogGroup }, token);
                }
                _currentStreamName = DateTime.Now.ToString("yyyy/MM/ddTHH.mm.ss") + " - " + _config.LogStreamNameSuffix;

                var streamResponse = await _client.CreateLogStreamAsync(new CreateLogStreamRequest
                {
                    LogGroupName = _config.LogGroup,
                    LogStreamName = _currentStreamName
                }, token).ConfigureAwait(false);

                _repo = new LogEventBatch(_config.LogGroup, _currentStreamName, Convert.ToInt32(_config.BatchPushInterval.TotalSeconds), _config.BatchSizeInBytes);
            }
            catch (Exception e)
            {
                LogLibraryError(e);
                throw;
            }

        }        

        const string UserAgentHeader = "User-Agent";
        void ServiceClientBeforeRequestEvent(object sender, RequestEventArgs e)
        {
            Amazon.Runtime.WebServiceRequestEventArgs args = e as Amazon.Runtime.WebServiceRequestEventArgs;
            if (args == null || !args.Headers.ContainsKey(UserAgentHeader))
                return;

            args.Headers[UserAgentHeader] = args.Headers[UserAgentHeader] + " AWSLogger/" + _logType;
        }

        public static void LogLibraryError(Exception ex)
        {

            /*
              
             TODO: If logging itself fails, it is possible to write the error into a file
             
            try
            {
                using (StreamWriter w = File.AppendText(@"C:\log\awslog.txt"))
                {
                    w.WriteLine("Log Entry : ");
                    w.WriteLine("{0}", DateTime.Now.ToString());
                    w.WriteLine("  :");
                    w.WriteLine("  :{0}", ex.ToString());
                    w.WriteLine("-------------------------------");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught when writing error log to file" + e.ToString());
            }
            */
        }
    }
}