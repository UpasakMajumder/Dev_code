using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Specialized;
using Amazon.Runtime;

namespace Kadena2.MicroserviceClients.Helpers
{
    static class AwsSignerHelper
    {
        private const string Iso8601DateTimeFormat = "yyyyMMddTHHmmssZ";
        private const string Iso8601DateFormat = "yyyyMMdd";
        private const string AwsSignAlgo = "AWS4-HMAC-SHA256";
        private const string awsService = "execute-api";
        private const string awsRegion = "us-east-1";

        private static AWSCredentials GetCredentialsDefault()
        {
            return FallbackCredentialsFactory.GetCredentials();
        }

        /// <summary>
        /// Calculates request signature string using Signature Version 4.
        /// http://docs.aws.amazon.com/general/latest/gr/sigv4_signing.html
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Signature</returns>
        public static async Task Sign(HttpRequestMessage request)
        {
            var requestDateTime = DateTime.UtcNow;
            var credentials = GetCredentialsDefault()?.GetCredentials();
            if (credentials == null)
            {
                throw new InvalidOperationException("Failed to get AWS Credentials.");
            }

            AddAwsHeaders(request, requestDateTime, credentials.Token);
            var signedHeaders = GetSignedHeaders(request);
            var canonicalRequest = await GetCanonicalRequest(request, signedHeaders).ConfigureAwait(false);
            var stringToSign = GetStringToSign(canonicalRequest, requestDateTime);
            var signiture = GetSignature(stringToSign, requestDateTime, credentials.SecretKey);
            var authHeader = GetAuthHeader(signiture, credentials.AccessKey, signedHeaders, requestDateTime);
            request.Headers.Authorization = new AuthenticationHeaderValue(AwsSignAlgo, authHeader);
        }

        // http://docs.aws.amazon.com/general/latest/gr/sigv4-create-canonical-request.html
        private static async Task<string> GetCanonicalRequest(HttpRequestMessage request, string[] signedHeaders)
        {
            var canonicalRequest = new StringBuilder();
            canonicalRequest.AppendFormat("{0}\n", request.Method.Method);
            canonicalRequest.AppendFormat("{0}\n", Uri.EscapeUriString(request.RequestUri.AbsolutePath));
            canonicalRequest.AppendFormat("{0}\n", GetCanonicalQueryParameters(HttpUtility.ParseQueryString(request.RequestUri.Query)));
            canonicalRequest.AppendFormat("{0}\n", GetCanonicalHeaders(request, signedHeaders));
            canonicalRequest.AppendFormat("{0}\n", String.Join(";", signedHeaders).ToLowerInvariant());
            canonicalRequest.Append(await GetContentHash(request).ConfigureAwait(false));
            return canonicalRequest.ToString();
        }

        private static string GetCanonicalQueryParameters(NameValueCollection queryParameters)
        {
            var canonicalQueryParameters = new StringBuilder();

            var orderedQueryKeys = queryParameters.AllKeys.OrderBy(t => t);
            foreach (var key in orderedQueryKeys)
            {
                canonicalQueryParameters.AppendFormat("{0}={1}&", Utils.UrlEncode(key),
                    Utils.UrlEncode(queryParameters[key]));
            }

            // remove trailing '&'
            if (canonicalQueryParameters.Length > 0)
                canonicalQueryParameters.Remove(canonicalQueryParameters.Length - 1, 1);

            return canonicalQueryParameters.ToString();
        }

        private static string GetCanonicalHeaders(HttpRequestMessage request, IEnumerable<string> signedHeaders)
        {
            var headers = request.Headers.ToDictionary(x => x.Key.Trim().ToLowerInvariant(),
                x => string.Join(" ", x.Value).Trim());

            if (request.Content != null)
            {
                var contentHeaders = request.Content.Headers.ToDictionary(x => x.Key.Trim().ToLowerInvariant(),
                    x => String.Join(" ", x.Value).Trim());
                foreach (var contentHeader in contentHeaders)
                {
                    headers.Add(contentHeader.Key, contentHeader.Value);
                }
            }

            var sortedHeaders = new SortedDictionary<string, string>(headers);

            var canonicalHeaders = new StringBuilder();
            foreach (var header in sortedHeaders.Where(header => signedHeaders.Contains(header.Key)))
            {
                canonicalHeaders.AppendFormat("{0}:{1}\n", header.Key, header.Value);
            }
            return canonicalHeaders.ToString();
        }

        private static async Task<string> GetContentHash(HttpRequestMessage request)
        {
            var content = request.Content != null
                ? await request.Content.ReadAsStringAsync().ConfigureAwait(false)
                : string.Empty;

            return Utils.ToHex(Utils.Hash(content));
        }

        // http://docs.aws.amazon.com/general/latest/gr/sigv4-create-string-to-sign.html
        private static string GetStringToSign(string canonicalRequest, DateTime requestTime)
        {
            var dateStamp = requestTime.ToString(Iso8601DateFormat, CultureInfo.InvariantCulture);
            var timeStamp = requestTime.ToString(Iso8601DateTimeFormat, CultureInfo.InvariantCulture);
            var scope = $"{dateStamp}/{awsRegion}/{awsService}/aws4_request";

            var stringToSign = new StringBuilder();
            stringToSign.Append($"{AwsSignAlgo}\n");
            stringToSign.Append($"{timeStamp}\n");
            stringToSign.Append($"{scope}\n");
            stringToSign.Append(Utils.ToHex(Utils.Hash(canonicalRequest)));
            return stringToSign.ToString();
        }

        private static string GetAuthHeader(string signiture, string awsAccessKey, IEnumerable<string> signedHeaders, DateTime requestTime)
        {
            var dateStamp = requestTime.ToString(Iso8601DateFormat, CultureInfo.InvariantCulture);
            var scope = $"{dateStamp}/{awsRegion}/{awsService}/aws4_request";
            var signedHeadersAfterJoin = string.Join(";", signedHeaders).Trim();

            return
                $"Credential={awsAccessKey}/{scope}, SignedHeaders={signedHeadersAfterJoin}, Signature={signiture}";
        }

        // http://docs.aws.amazon.com/general/latest/gr/sigv4-calculate-signature.html
        private static string GetSignature(string stringToSign, DateTime requestTime, string awsSecretKey)
        {
            var kSigning = GetSigningKey(requestTime, awsSecretKey);
            return Utils.ToHex(Utils.GetKeyedHash(kSigning, stringToSign));
        }

        private static byte[] GetSigningKey(DateTime requestTime, string awsSecretKey)
        {
            var dateStamp = requestTime.ToString(Iso8601DateFormat, CultureInfo.InvariantCulture);
            var kDate = Utils.GetKeyedHash("AWS4" + awsSecretKey, dateStamp);
            var kRegion = Utils.GetKeyedHash(kDate, awsRegion);
            var kService = Utils.GetKeyedHash(kRegion, awsService);
            return Utils.GetKeyedHash(kService, "aws4_request");
        }

        private static void AddAwsHeaders(HttpRequestMessage request, DateTime requestTime, string sessionToken)
        {
            request.Headers.Add("Host", request.RequestUri.Host);
            request.Headers.Add("X-Amz-Date", requestTime.ToString(Iso8601DateTimeFormat, CultureInfo.InvariantCulture));

            if (!string.IsNullOrEmpty(sessionToken))
            {
                request.Headers.Add("X-Amz-Security-Token", sessionToken);
            }
        }

        private static string[] GetSignedHeaders(HttpRequestMessage request)
        {
            return request.Headers.Select(httpRequestHeader => httpRequestHeader.Key.ToLowerInvariant())
                .OrderBy(t => t)
                .ToArray();
        }
    }
}
