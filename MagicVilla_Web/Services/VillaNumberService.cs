using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private string villaUrl;
        public VillaNumberService(IHttpClientFactory httpClient, IConfiguration config) : base(httpClient)
        {
            //extracting api url from appsettings.json
            villaUrl = config.GetValue<string>("ServiceUrls:VillaAPI");
        }
        public async Task<T> CreateAsync<T>(VillaNumberCreateDto createDto)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = createDto,
                Url = villaUrl + "/api/villaNumber"
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = villaUrl + "/api/villaNumber/" + id
            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = villaUrl + "/api/villaNumber"
            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = villaUrl + "/api/villaNumber/" + id
            });
        }

        public async Task<T> UpdateAsync<T>(VillaNumberUpdateDto updateDto)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = updateDto,
                Url = villaUrl + "/api/villaNumber"
            });
        }
    }
}
