using CAS;

namespace Analytics
{
    public class AnalyticsService
    {
        public static void HandleAdRevenueEvent(AdRevenueEvent e)
        {
            YandexAppMetricaAdRevenue revenue = new YandexAppMetricaAdRevenue((decimal) e.Value, e.Currency);

            switch (e.Format)
            {
                case AdType.Banner:
                    {
                        revenue.AdType = YandexAppMetricaAdRevenue.AdTypeEnum.Banner;
                        break;
                    }
                case AdType.Interstitial:
                    {
                        revenue.AdType = YandexAppMetricaAdRevenue.AdTypeEnum.Interstitial;
                        break;
                    }
                case AdType.Rewarded:
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
            revenue.AdNetwork = e.Network.ToString();

            AppMetrica.Instance.ReportAdRevenue(revenue);
        }
    }
    
    public class AdRevenueEvent
    {
        public AdType Format;
        public AdNetwork Network;
        public string Currency;
        public double Value;

        public AdRevenueEvent(AdType format, AdNetwork network, string currency, double value)
        {
            Format = format;
            Network = network;
            Currency = currency;
            Value = value;
        }
    }
}