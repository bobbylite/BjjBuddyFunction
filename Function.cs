using Amazon.Lambda.APIGatewayEvents;
using System.Net;
using Amazon.Lambda.Core;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using BjjBuddy.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using System.Threading.Tasks;
using Amazon.Runtime;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace BjjBuddy
{
    public class Function
    {
        
        /// <summary>
        /// API Gateway Proxy Response 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            APIGatewayProxyResponse response;

            try
            {
                response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = JsonConvert.SerializeObject(
                        new ServerSuccess
                        {
                            ResponseMessage = $"Bjj Buddy Response - {DateTime.Now.ToString("hh:mm:ss")}",
                            Data = string.Empty
                        }),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
            catch (Exception exception)
            {
                response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = JsonConvert.SerializeObject(
                        new ServerError
                        {
                            ExceptionMessage = $"Bjj Buddy Error - {exception.Message}"
                        })
                };
            }

            AmazonDynamoDbQuery("tablename", "context");
            return response;
        }

        /// <summary>
        /// Amazon DynamoDB Query Test
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<DynamoObject<Document>> AmazonDynamoDbQuery(string tableName, string context)
        {
            DynamoObject<Document> dynamoQueryResponse = new DynamoObject<Document>();

            try
            {
                using(var client = new AmazonDynamoDBClient(new BjjBuddyCredentials()))
                {
                    var test = client.Config;
                    Table bjjBuddy = Table.LoadTable(client, "BjjBuddy");
                    dynamoQueryResponse.Data = await bjjBuddy.GetItemAsync("Test");
                }
            }
            catch (Exception exception)
            {
            }

            return dynamoQueryResponse;
        }
    }
}
