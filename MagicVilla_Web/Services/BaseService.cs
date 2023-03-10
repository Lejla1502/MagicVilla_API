using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            this.httpClient = httpClient;
        }
        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Headers.Add("Accept", "application/json");
                //requestMessage.Headers.Add("Content-Type", "application/json");
                requestMessage.RequestUri = new Uri(apiRequest.Url); //API url

                //if we are creating or updating we will have some data in apiRequest which we need to
                //serialize
                if(apiRequest.Data!= null)
                {
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");   
                }

                switch(apiRequest.ApiType)
                {
                    case StaticDetails.ApiType.POST:
                        requestMessage.Method = HttpMethod.Post;
                        break;
                    case StaticDetails.ApiType.PUT:
                        requestMessage.Method = HttpMethod.Put; 
                        break;
                    case StaticDetails.ApiType.DELETE:
                        requestMessage.Method = HttpMethod.Delete; 
                        break;
                    default:
                        requestMessage.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;

                //checking if token is not null - allows us to authorize apis properly
                if(!String.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }


                apiResponse = await client.SendAsync(requestMessage);

                //when we receive API response, we need to extract API content from there
                var apiContent = await apiResponse.Content.ReadAsStringAsync();

               
                try
                { 
                    //here we could've used generic object, but since our response will alway be API response,
                    //we've made APIResponse variable
                    APIResponse deserializedAPIResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);

                    if(deserializedAPIResponse!=null &&(apiResponse.StatusCode == HttpStatusCode.BadRequest
                        || apiResponse.StatusCode == HttpStatusCode.NotFound)) 
                    {
                        deserializedAPIResponse.StatusCode = HttpStatusCode.BadRequest;
                        deserializedAPIResponse.IsSuccess = false;

                        //reverting to generic
                        var res = JsonConvert.SerializeObject(deserializedAPIResponse);
                        var returnObj = JsonConvert.DeserializeObject<T>(res);
                        return returnObj;
                    }

                }
                catch(Exception e)
                {
                    //if we run into exception, we fallback to generic
                    var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return exceptionResponse;
                }

                var deserializedApiResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return deserializedApiResponse;

            }
            catch (Exception e)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };

                //returning dto directly won't work because we need to return generic T object
                //tht is why we need to serialize and deserialize the object
                var res= JsonConvert.SerializeObject(dto);
                var apiRes = JsonConvert.DeserializeObject<T>(res);
                return apiRes;
            }
        }
    }
}
