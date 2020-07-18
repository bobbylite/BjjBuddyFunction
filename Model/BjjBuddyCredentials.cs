using Amazon.Runtime;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BjjBuddy.Model
{
	public class BjjBuddyCredentials : AWSCredentials
	{
		public override ImmutableCredentials GetCredentials()
		{
            CredentialModel sessionCredentials = GetBjjCredentials().Result;
            return new ImmutableCredentials(sessionCredentials.Key, sessionCredentials.Secret, sessionCredentials.Token);
		}

        private async Task<CredentialModel> GetBjjCredentials()
        {
            using (var stsClient = new AmazonSecurityTokenServiceClient())
            {
                var getSessionTokenRequest = new GetSessionTokenRequest
                {
                    DurationSeconds = 7200 // seconds
                };

                GetSessionTokenResponse sessionTokenResponse =
                              await stsClient.GetSessionTokenAsync(getSessionTokenRequest);

                Credentials credentials = sessionTokenResponse.Credentials;

                var sessionCredentials =
                    new SessionAWSCredentials(credentials.AccessKeyId,
                                              credentials.SecretAccessKey,
                                              credentials.SessionToken);
                return new CredentialModel
                {
                    Key = credentials.AccessKeyId,
                    Secret = credentials.SecretAccessKey,
                    Token = credentials.SessionToken
                };
            }
        }

    }
}
