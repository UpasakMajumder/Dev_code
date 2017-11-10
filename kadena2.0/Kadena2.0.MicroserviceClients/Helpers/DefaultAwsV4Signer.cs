using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using Kadena2.MicroserviceClients.Helpers;
using System.Web;
using System.Collections.Specialized;
using Amazon.Runtime;

namespace Kadena.KOrder.PaymentService.Infrastucture.Helpers
{
    class DefaultAwsV4Signer : IAwsV4Signer
    {
        public const string Iso8601DateTimeFormat = "yyyyMMddTHHmmssZ";
        public const string Iso8601DateFormat = "yyyyMMdd";
        public const string AwsSignAlgo = "AWS4-HMAC-SHA256";

        private string awsSecretKey;
        private string awsAccessKey;
        private string sessionToken;
        private string awsService = "execute-api";
        private string awsRegion = "us-east-1";
        private DateTime requestDateTime;

        public async Task SignRequest(HttpRequestMessage request)
        {
            this.SetCredentials();
            await this.Sign(request).ConfigureAwait(false);
        }

        public async Task SignRequest(HttpRequestMessage request, AssumeRoleResponse assumedRole)
        {
            this.SetCredentials(assumedRole);
            await this.Sign(request).ConfigureAwait(false);
        }

        public async Task SignRequest(HttpRequestMessage request, string gatewayApiRole)
        {
            var assumedRole = await GetTemporaryRole(gatewayApiRole).ConfigureAwait(false);
            await this.SignRequest(request, assumedRole).ConfigureAwait(false);
        }

        public async Task SignRequest(HttpRequestMessage request, string accessKey, string secretKey)
        {
            this.SetCredentials(accessKey, secretKey);
            await this.Sign(request).ConfigureAwait(false);
        }

        private void SetService(string service, string region)
        {
            this.awsService = service;
            this.awsRegion = region;
        }

        private static async Task<AssumeRoleResponse> GetTemporaryRole(string gatewayApiRole)
        {
            var credentials = GetCredentialsDefault();
            var amazonSecurityTokenService = new AmazonSecurityTokenServiceClient(credentials);
            var assumedRole = await amazonSecurityTokenService.AssumeRoleAsync(new AssumeRoleRequest()
            {
                RoleArn = gatewayApiRole,
                RoleSessionName = "sessionNumber1"
            }).ConfigureAwait(false);
            return assumedRole;
        }

        private static AWSCredentials GetCredentialsDefault()
        {
            return new StoredProfileAWSCredentials();
        }

        /// <summary>
        /// Calculates request signature string using Signature Version 4.
        /// http://docs.aws.amazon.com/general/latest/gr/sigv4_signing.html
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Signature</returns>
        private async Task Sign(HttpRequestMessage request)
        {
            this.requestDateTime = DateTime.UtcNow;
            var signedHeaders = this.AddAndGetSignedHeaders(request);
            var canonicalRequest = await GetCanonicalRequest(request, signedHeaders).ConfigureAwait(false);
            var stringToSign = this.GetStringToSign(canonicalRequest);
            var signiture = this.GetSignature(stringToSign);
            var authHeader = this.GetAuthHeader(signiture, signedHeaders);
            request.Headers.Authorization = new AuthenticationHeaderValue(AwsSignAlgo, authHeader);
        }

        // http://docs.aws.amazon.com/general/latest/gr/sigv4-create-canonical-request.html
        private static async Task<string> GetCanonicalRequest(HttpRequestMessage request, string[] signedHeaders)
        {
            var canonicalRequest = new StringBuilder();
            canonicalRequest.AppendFormat("{0}\n", request.Method.Method);
            canonicalRequest.AppendFormat("{0}\n", request.RequestUri.AbsolutePath);
            canonicalRequest.AppendFormat("{0}\n", GetCanonicalQueryParameters(HttpUtility.ParseQueryString(request.RequestUri.Query)));
            canonicalRequest.AppendFormat("{0}\n", GetCanonicalHeaders(request, signedHeaders));
            canonicalRequest.AppendFormat("{0}\n", String.Join(";", signedHeaders).ToLowerInvariant());
            canonicalRequest.Append(await GetPayloadHash(request).ConfigureAwait(false));
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

        private static async Task<string> GetPayloadHash(HttpRequestMessage request)
        {
            var payload = request.Content != null
                ? await request.Content.ReadAsStringAsync().ConfigureAwait(false)
                : string.Empty;

            return Utils.ToHex(Utils.Hash(payload));
        }

        // http://docs.aws.amazon.com/general/latest/gr/sigv4-create-string-to-sign.html
        private string GetStringToSign(string canonicalRequest)
        {
            var dateStamp = this.requestDateTime.ToString(Iso8601DateFormat, CultureInfo.InvariantCulture);
            var scope = $"{dateStamp}/{this.awsRegion}/{this.awsService}/aws4_request";

            var stringToSign = new StringBuilder();
            stringToSign.Append($"{AwsSignAlgo}\n");
            stringToSign.Append($"{this.requestDateTime.ToString(Iso8601DateTimeFormat, CultureInfo.InvariantCulture)}\n");
            stringToSign.Append($"{scope}\n");
            stringToSign.Append(Utils.ToHex(Utils.Hash(canonicalRequest)));
            return stringToSign.ToString();
        }

        private string GetAuthHeader(string signiture, IEnumerable<string> signedHeaders)
        {
            var dateStamp = this.requestDateTime.ToString(Iso8601DateFormat, CultureInfo.InvariantCulture);
            var scope = $"{dateStamp}/{this.awsRegion}/{this.awsService}/aws4_request";
            var signedHeadersAfterJoin = string.Join(";", signedHeaders).Trim();

            return
                $"Credential={this.awsAccessKey}/{scope}, SignedHeaders={signedHeadersAfterJoin}, Signature={signiture}";
        }

        // http://docs.aws.amazon.com/general/latest/gr/sigv4-calculate-signature.html
        private string GetSignature(string stringToSign)
        {
            var kSigning = this.GetSigningKey();
            return Utils.ToHex(Utils.GetKeyedHash(kSigning, stringToSign));
        }

        private byte[] GetSigningKey()
        {
            var dateStamp = this.requestDateTime.ToString(Iso8601DateFormat, CultureInfo.InvariantCulture);
            var kDate = Utils.GetKeyedHash("AWS4" + this.awsSecretKey, dateStamp);
            var kRegion = Utils.GetKeyedHash(kDate, this.awsRegion);
            var kService = Utils.GetKeyedHash(kRegion, this.awsService);
            return Utils.GetKeyedHash(kService, "aws4_request");
        }

        private void SetCredentials(AssumeRoleResponse assumedRole)
        {
            this.awsSecretKey = assumedRole.Credentials.SecretAccessKey;
            this.awsAccessKey = assumedRole.Credentials.AccessKeyId;
            this.sessionToken = assumedRole.Credentials.SessionToken;
        }

        private void SetCredentials(string accessKey, string secretKey)
        {
            this.awsSecretKey = secretKey;
            this.awsAccessKey = accessKey;
        }

        private void SetCredentials()
        {
            var credentials = GetCredentialsDefault()?.GetCredentials();
            if (credentials != null)
            {
                this.awsAccessKey = credentials.AccessKey;
                this.awsSecretKey = credentials.SecretKey;
                this.sessionToken = credentials.Token;
            }
        }

        private string[] AddAndGetSignedHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("Host", request.RequestUri.Host);
            request.Headers.Add("X-Amz-Date", this.requestDateTime.ToString(Iso8601DateTimeFormat, CultureInfo.InvariantCulture));

            if (!string.IsNullOrEmpty(this.sessionToken))
            {
                request.Headers.Add("X-Amz-Security-Token", this.sessionToken);
            }

            return GetSignedHeaders(request);
        }

        private static string[] GetSignedHeaders(HttpRequestMessage request)
        {
            return request.Headers.Select(httpRequestHeader => httpRequestHeader.Key.ToLowerInvariant())
                .OrderBy(t => t)
                .ToArray();
        }
    }
}
