using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Talker.Models.DTO;

namespace Talker.Models
{

    /// <summary>
    /// 
    /// Responsible for sending crucial data out and in within the server. Uses bearer token authorized requers.
    /// This is generic class within returns a DTO given as a T object in the constructor.
    /// 
    /// </summary>
    public class RestHelper<T> where T : class, new()
    {

        /// <summary>
        /// Internet address of REST Api sever
        /// </summary>
        private static string RestServerUrl = "http://talkerapi-env.eba-banadszz.eu-west-1.elasticbeanstalk.com/";

        /// <summary>
        /// Port number of REST Api sever
        /// </summary>
        public static int RestServerRequestTimeout = 2000;


        /// <summary>
        /// Instance of the RestClient fom ResrSharb function by which it is possible to sent HTTP requests.
        /// </summary>
        private RestClient Client;


        /// <summary>
        /// Define whether the authentication headers have to be added to the http requestes.
        /// </summary>
        private bool UseSessionManager;


        /// <summary>
        /// The constructor initializes the variables.
        /// Creates new instance od RestClient object and sets operation timeout.
        /// </summary>
        public RestHelper(){
            Client = new RestClient(RestServerUrl);
            Client.Timeout = RestServerRequestTimeout;

            UseSessionManager = false;
        }


        /// <summary>
        /// The constructor initializes the variables.
        /// Creates new instance od RestClient object and sets operation timeout.
        /// This constructios also allowes to set useSessionManager option
        /// </summary>
        /// <param name="useSessionManager">Define whether the authentication headers have to be added to the http requestes.</param>
        public RestHelper(bool useSessionManager)
        {
            Client = new RestClient(RestServerUrl);
            Client.Timeout = RestServerRequestTimeout;

            UseSessionManager = useSessionManager;
        }


        /// <summary>
        /// Asynchronous method which allows to get data from REST Api Server. 
        /// </summary> 
        /// <param name="resource">Path on the server</param>
        public async Task<T> sendAsyncRequest(string resource){
            return await sendAsyncRequest(resource, null, null);
        }


        /// <summary>
        /// Asynchronous method which allows to get data from REST Api Server. 
        /// Additionaly user can add object that will be serialized as JSON and added to request body can be added.
        /// </summary> 
        /// <returns>
        /// Returns DTO given as a T object in the constructor
        /// </returns>
        /// <param name="resource">Path on the server</param>
        /// <param name="jsonBody">object that will be serialized to JOSN</param>
        public async Task<T> sendAsyncRequest(string resource, object jsonBody)
        {
            return await sendAsyncRequest(resource, jsonBody, null);
        }


        /// <summary>
        /// Asynchronous method which allows to get data from REST Api Server. 
        /// Object that will be serialized as JSON and added to header body can be added.
        /// Additionally custom headers can be added
        /// </summary> 
        /// <returns>
        /// Returns DTO given as a T object in the constructor
        /// </returns>
        /// <param name="resource">Path on the server</param>
        /// <param name="jsonBody"> object that will be serialized as JSON and added to request body can be added.</param>
        /// <param name="additionalHeaders">Custom headers saved as string, strin dictionary</param>
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

            var result = await ExecuteAsyncTask(Client, requeset);

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



        /// <summary>
        /// Asynchronous method that executes new HTTP Request.
        /// </summary> 
        /// <returns>
        /// Returns DTO given as a T object in the constructor
        /// </returns>
        /// <param name="client">Http client object</param>
        /// <param name="request">Http request that has to be sent</param>
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
