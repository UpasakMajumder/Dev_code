using Amazon;
using Amazon.Runtime;
using System;

namespace Kadena.AWSLogging
{
    /// <summary>
    /// This class contains all the configuration options for logging messages to AWS. As messages from the application are 
    /// sent to the logger they are queued up in a batch. The batch will be sent when either BatchPushInterval or BatchSizeInBytes
    /// are exceeded.
    /// 
    /// <para>
    /// AWS Credentials are determined using the following steps.
    /// 1) If the Credentials property is set
    /// 2) If the Profile property is set and the can be found
    /// 3) Use the AWS SDK for .NET fall back mechanism to find enviroment credentials.
    /// </para>
    /// </summary>
    public class AWSLoggerConfig
    {
        #region Public Properties

        /// <summary>
        /// Gets and sets the LogGroup property. This is the name of the CloudWatch Logs group where 
        /// streams will be created and log messages written to.
        /// </summary>
        public string LogGroup { get; set; }

        /// <summary>
        /// Gets and sets the Profile property. The profile is used to look up AWS credentials in the profile store.
        /// <para>
        /// For understanding how credentials are determine view the top level documentation for AWSLoggerConfig class.
        /// </para>
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// Gets and sets the ProfilesLocation property. If this is not set the default profile store is used by the AWS SDK for .NET 
        /// to look up credentials. This is most commonly used when you are running an application of on-priemse under a service account.
        /// <para>
        /// For understanding how credentials are determine view the top level documentation for AWSLoggerConfig class.
        /// </para>
        /// </summary>
        public string ProfilesLocation { get; set; }
        
        public RegionEndpoint RegionEndpoint { get; set; }

        /// <summary>
        /// Gets and sets the BatchPushInterval property. For performance the log messages are sent to AWS in batch sizes. BatchPushInterval 
        /// dictates the frequency of when batches are sent. If either BatchPushInterval or BatchSizeInBytes are exceeded the batch will be sent.
        /// <para>
        /// The default is 3 seconds.
        /// </para>
        /// </summary>
        public TimeSpan BatchPushInterval { get; set; } = TimeSpan.FromMilliseconds(3000);

        /// <summary>
        /// Gets and sets the BatchSizeInBytes property. For performance the log messages are sent to AWS in batch sizes. BatchSizeInBytes 
        /// dictates the total size of the batch in bytes when batches are sent. If either BatchPushInterval or BatchSizeInBytes are exceeded the batch will be sent.
        /// <para>
        /// The default is 100 Kilobytes.
        /// </para>
        /// </summary>
        public int BatchSizeInBytes { get; set; } = 102400;

        /// <summary>
        /// Gets and sets the MaxQueuedMessages property. This specifies the maximum number of log messages that could be stored in-memory. MaxQueuedMessages 
        /// dictates the total number of log messages that can be stored in-memory. If this exceeded, incoming log messages will be dropped.
        /// <para>
        /// The default is 10000.
        /// </para>
        /// </summary>
        public int MaxQueuedMessages { get; set; } = 10000;

        /// <summary>
        /// Internal MonitorSleepTime property. This specifies the timespan after which the Monitor wakes up. MonitorSleepTime 
        /// dictates the timespan after which the Monitor checks the size and time constarint on the batch log event and the existing in-memory buffer for new messages. 
        /// <para>
        /// The value is 500 Milliseconds.
        /// </para>
        /// </summary>
        internal TimeSpan MonitorSleepTime = TimeSpan.FromMilliseconds(500);
        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AWSLoggerConfig()
        {
        }

        /// <summary>
        /// Construct instance and sets the LogGroup
        /// </summary>
        /// <param name="logGroup">The CloudWatch Logs log group.</param>
        public AWSLoggerConfig(string logGroup)
        {
            LogGroup = logGroup;
        }

        internal void ShutDown()
        {
            MonitorSleepTime = TimeSpan.FromMilliseconds(0);
            BatchPushInterval = TimeSpan.FromSeconds(0);
        }

        /// <summary>
        /// Gets and sets the LogStreamNameSuffix property. The LogStreamName consists of a DateTimeStamp as the prefix and a user defined suffix value that can 
        /// be set using the LogStreamNameSuffix property defined here.
        /// The LogstreamName then follows the pattern '[DateTime.Now.ToString("yyyy/MM/ddTHH.mm.ss")]-[LogstreamNameSuffix]'
        /// <para>
        /// The default is going to a Guid.
        /// </para>
        /// </summary>
        public string LogStreamNameSuffix { get; set; } = Guid.NewGuid().ToString();
    }
}