using ProteinCase.Application.Dto;
using ProteinCase.Dal.Concreate;
using ProteinCase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;

namespace ProteinCase.Application.Services
{
    public class CurrencyService
    {
        public readonly CurrencyMasterRepository _currencyMasterRepository;
        public readonly CurrencyDetailRepository _currencyDetailRepository;

        public CurrencyService(CurrencyMasterRepository CurrencyMasterRepository, CurrencyDetailRepository currencyDetailRepository)
        {
            _currencyMasterRepository = CurrencyMasterRepository;
            _currencyDetailRepository = currencyDetailRepository;
        }

        public async Task<ApiResponse> GetAllCurrencies()
        {
            try
            {
                var isCurrenciesRecorded = await _currencyMasterRepository.GetAsync(x => x.CreatedDate == DateTime.Now.Date);

                if (isCurrenciesRecorded != null)
                {
                    return await GetCurrencies(isCurrenciesRecorded.Id);
                }
                else
                {
                    CurrencyMaster currencyMaster = new CurrencyMaster();
                    await _currencyMasterRepository.Add(currencyMaster);

                    var currencyNodesFromWebService = GetCurrenciesFromWebService();

                    foreach (XmlNode item in currencyNodesFromWebService)
                    {
                        if (item.Attributes[2].Value != "XDR")
                        {
                            CurrencyDetail currencyDetail = new CurrencyDetail();
                            currencyDetail.CurrencyMasterId = currencyMaster.Id;
                            currencyDetail.Name = item["Isim"].InnerText;
                            currencyDetail.Code = item.Attributes[2].Value;
                            currencyDetail.ForexBuying = Convert.ToDecimal(item["ForexBuying"].InnerText);
                            currencyDetail.ForexSelling = Convert.ToDecimal(item["ForexSelling"].InnerText);

                            await _currencyDetailRepository.Add(currencyDetail);
                        }
                    }

                    return await GetCurrencies(currencyMaster.Id);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return new ApiResponse("");
        }

        private async Task<ApiResponse> GetCurrencies(int masterId)
        {
            var currencies = await _currencyDetailRepository.GetAllAsync(x => x.CurrencyMasterId == masterId);

            List<CurrencyResponseDto> currencyResponseDtoList = new List<CurrencyResponseDto>();

            foreach (var item in currencies)
            {
                CurrencyResponseDto currencyResponseDto = new CurrencyResponseDto();
                currencyResponseDto.Currency = item.Code + "-TRY";
                currencyResponseDto.CurrentRate = item.ForexSelling;
                currencyResponseDto.LastUpdated = item.CreatedDate.ToString("d").Replace("/", ".");
                currencyResponseDtoList.Add(currencyResponseDto);
            }

            currencyResponseDtoList = currencyResponseDtoList.OrderBy(x => x.Currency).ThenBy(x => x.CurrentRate).ToList();

            return new ApiResponse(currencyResponseDtoList);
        }

        public async Task<ApiResponse> GetDailyChanges(string currencyCode)
        {
            try
            {
                List<CurrencyChangesDto> currencyChangesDtos = new List<CurrencyChangesDto>();

                var currencyDetails = await _currencyDetailRepository.GetAllAsync(x => x.Code == currencyCode);
                currencyDetails = currencyDetails.OrderBy(x => x.CreatedDate).ToList();

                for (int i = 0; i < currencyDetails.Count; i++)
                {
                    CurrencyChangesDto currencyChangesDto = new CurrencyChangesDto();
                    currencyChangesDto.Currency = currencyDetails[i].Code + "-TRY";
                    currencyChangesDto.Date = currencyDetails[i].CreatedDate.ToString("d").Replace("/", ".");
                    currencyChangesDto.Rate = currencyDetails[i].ForexSelling;
                    currencyChangesDto.Changes = i != 0 ? CalculateChanges(currencyDetails[i - 1].ForexSelling, currencyDetails[i].ForexSelling) : "-";
                    currencyChangesDtos.Add(currencyChangesDto);
                }

                return new ApiResponse(currencyChangesDtos);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string CalculateChanges(decimal oldSellingVal, decimal newSellingVal)
        {
            var result = "";

            var diff = newSellingVal - oldSellingVal;

            if (diff < 0)
            {
                var percentComplete = Math.Round((100 * diff) / oldSellingVal);
                result = ((100 * diff) / oldSellingVal).ToString("0.00") + "%";
            }
            else
            {
                var percentComplete = Math.Round((100 * diff) / oldSellingVal);
                result = "+" + ((100 * diff) / oldSellingVal).ToString("0.00") + "%";
            }

            return result;
        }

        private XmlNodeList GetCurrenciesFromWebService()
        {
            using (var wb = new WebClient())
            {
                var response = wb.DownloadString("https://www.tcmb.gov.tr/kurlar/today.xml");

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);

                XmlNodeList nodes = doc.SelectNodes("//Currency");

                return nodes;
            }
        }
    }
}
