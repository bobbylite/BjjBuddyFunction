using Amazon.Lambda.APIGatewayEvents;
using System.Net;
using Amazon.Lambda.Core;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using BjjBuddy.Model;

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
                            ResponseMessage = $"Bjj Buddy Response - {DateTime.Now.ToString("hh:mm:ss")}"
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

            return response;
        }
    }
}
