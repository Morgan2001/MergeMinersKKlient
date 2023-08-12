using CAS;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Analytics
{
    public class AnalyticsService
    {
       

        static string RemoveSpaces(string str)
        {
            return str.Replace(" ", "_");
        }

        public static void HandleAdRevenueEvent(AdRevenueEvent e)
        {
            YandexAppMetricaAdRevenue revenue = new YandexAppMetricaAdRevenue((decimal)e.Value, e.Currency);

            switch (e.Format)
            {
                case "Banner":
                    {
                        revenue.AdType = YandexAppMetricaAdRevenue.AdTypeEnum.Banner;
                        break;
                    }
                case "Interstitial":
                    {
                        revenue.AdType = YandexAppMetricaAdRevenue.AdTypeEnum.Interstitial;
                        break;
                    }
                case "Rewarded":
                    {
                        revenue.AdType = YandexAppMetricaAdRevenue.AdTypeEnum.Rewarded;
                        break;
                    }
                default:
                    {
                        revenue.AdType = YandexAppMetricaAdRevenue.AdTypeEnum.Other;
                        break;
                    }

            }
            revenue.AdNetwork = e.Network;

            AppMetrica.Instance.ReportAdRevenue(revenue);

        }
    }

    public class AdRevenueEvent
    {
        public string Platform;
        public string Format;
        public string Network;
        public string Currency;
        public double Value;

        public AdRevenueEvent(string platform, string format, string network, string currency, string hzChto, double value)
        {
            Platform = platform;
            Format = format;
            Network = network;
            Currency = currency;
            Value = value;
        }
    }
}