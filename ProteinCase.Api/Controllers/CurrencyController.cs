using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProteinCase.Application.Dto;
using ProteinCase.Application.Services;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ProteinCase.Api.Controllers
{
    [Route("api/currency")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        public readonly CurrencyService _currencyService;

        public CurrencyController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }


        [HttpGet]
        [Route("getAllCurrencies")]
        public async Task<ApiResponse> GetAllCurrencies()
        {
            try
            {
                return await _currencyService.GetAllCurrencies();
            }
            catch (Exception e)
            {
                return new ApiResponse((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("getDailyChanges")]
        public async Task<ApiResponse> GetDailyChanges(string currencyCode)
        {
            try
            {
                return await _currencyService.GetDailyChanges(currencyCode);
            }
            catch (Exception e)
            {
                return new ApiResponse((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
