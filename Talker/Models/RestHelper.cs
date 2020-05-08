using ElectronNET.API;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Talker.Models.DTO;

namespace Talker.Models
{
    public class RestHelper<T> where T : class, new()
    {


        RestClient client;

        public static string restServerUrl = "http://talkerapi-env.eba-banadszz.eu-west-1.elasticbeanstalk.com/";

        public static int restServerRequestTimeout = 4000;

        public bool UseSessionManager;

        public RestHelper(){
            client = new RestClient(restServerUrl);
            client.Timeout = restServerRequestTimeout;

            UseSessionManager = false;
        }

        public RestHelper(bool useSessionManager)
        {
            client = new RestClient(restServerUrl);
            client.Timeout = restServerRequestTimeout;

            UseSessionManager = useSessionManager;
        }


        public async Task<T> sendAsyncRequest(string resource){
            return await sendAsyncRequest(resource, null, null);
        }

        public async Task<T> sendAsyncRequest(string resource, object jsonBody)
        {
            return await sendAsyncRequest(resource, jsonBody, null);
        }


        public async Task<T> sendAsyncRequest(string resource, object jsonBody, Dictionary<string, string> additionalHeaders)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();

            var requeset = new RestRequest(resource, Method.POST);
            requeset.RequestFormat = DataFormat.Json;

            if (jsonBody != null)
            {
                requeset.AddJsonBody(jsonBody);
            }

            if(additionalHeaders != null)
            {
                foreach (var header in additionalHeaders)
                {
                    string key = header.Key;
                    string value = header.Value;
                    requeset.AddHeader(key, value);
                }
            }


            if(UseSessionManager)
            {
                requeset.AddHeader("Authorization", string.Format("Bearer {0}", SessionManager.UserToken.token));
            }

            var result = await ExecuteAsyncTask(client, requeset);

            if (result.ErrorException != null)
            {
                const string message = "Error retrieving response.";
                throw new Exception(message, result.ErrorException);
            }

            if (result.StatusCode == HttpStatusCode.OK)
            {
                taskCompletionSource.SetResult(result.Data);
            }
            else
            {
                var eventResponse = JsonConvert.DeserializeObject<ExceptionRestStatus>(result.Content);
                throw eventResponse;
            }
            return await taskCompletionSource.Task;
        }


        private async Task<IRestResponse<T>> ExecuteAsyncTask(RestClient client, IRestRequest request)
        {
            var taskCompletionSource = new TaskCompletionSource<IRestResponse<T>>();

            #pragma warning disable CS0618
            client.ExecuteAsync<T>(request, (restResponse, asyncHandle) =>
            {
                taskCompletionSource.SetResult(restResponse);
            });
            #pragma warning restore CS0618

            return await taskCompletionSource.Task;
        }

    }
}
