using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string villaUrl;
        public VillaNumberService(IHttpClientFactory clientFactory, IConfiguration config) : base(clientFactory)
        {
            //extracting api url from appsettings.json
            villaUrl = config.GetValue<string>("ServiceUrls:VillaAPI");
            _clientFactory = clientFactory;
        }
        public async Task<T> CreateAsync<T>(VillaNumberCreateDto createDto, string token)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = createDto,
                Url = villaUrl + "/api/v1/villaNumber",
                Token = token
            });
        }

        public async Task<T> DeleteAsync<T>(int id, string token)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = villaUrl + "/api/v1/villaNumber/" + id,
                Token = token
            });
        }

        public async Task<T> GetAllAsync<T>(string token)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = villaUrl + "/api/v1/villaNumber",
                Token = token
            });
        }

        public async Task<T> GetAsync<T>(int id, string token)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = villaUrl + "/api/v1/villaNumber/" + id,
                Token = token
            });
        }

        public async Task<T> UpdateAsync<T>(VillaNumberUpdateDto updateDto, string token)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = updateDto,
                Url = villaUrl + "/api/v1/villaNumber/" + updateDto.VillaNo,
                Token = token
            });
        }
    }
}
