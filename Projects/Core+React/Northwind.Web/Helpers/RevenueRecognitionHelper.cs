using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Northwind.Web.Helpers
{
    public class RevenueRecognitionHelper
    {
        private readonly IConfiguration _configuration;

        public RevenueRecognitionHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static bool IsValidForRevenueRecognitionRequest(IConfiguration configuration, string contractType, decimal? awardAmount, decimal? fundingAmount)
        {
            decimal firmFixedAwardAmount = configuration.GetSection("RevenueRecognitionTrigger").GetValue<decimal>("FirmFixedAwardAmount");
            decimal costPlusAwardAmount = configuration.GetSection("RevenueRecognitionTrigger").GetValue<decimal>("CostPlusAwardAmount");
            switch (contractType)
            {
                case "CostPlusAwardFee":
                    if (awardAmount >= costPlusAwardAmount)
                        return true;
                    else return false;
                case "CostPlusFixedFee":
                    if (awardAmount >= costPlusAwardAmount)
                        return true;
                    else return false;
                case "CostPlusIncentiveFee":
                    if (awardAmount >= costPlusAwardAmount)
                        return true;
                    else return false;
                case "PerformanceBase(CostPlus)":
                    if (awardAmount >= costPlusAwardAmount)
                        return true;
                    else return false;
                case "FirmFixedPrice":
                    if (awardAmount >= firmFixedAwardAmount)
                        return true;
                    else return false;
                default:
                    return false;
            }
        }

        public static decimal GetAmountByContractType(IConfiguration configuration, string contractType)
        {
            decimal firmFixedAwardAmount = configuration.GetSection("RevenueRecognitionTrigger").GetValue<decimal>("FirmFixedAwardAmount");
            decimal costPlusAwardAmount = configuration.GetSection("RevenueRecognitionTrigger").GetValue<decimal>("CostPlusAwardAmount");
            switch (contractType)
            {
                case "FirmFixedPrice":
                        return firmFixedAwardAmount;
                default:
                    return costPlusAwardAmount;
            }
        }
    }
}