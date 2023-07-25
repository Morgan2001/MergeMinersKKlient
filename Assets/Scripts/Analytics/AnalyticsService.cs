using CAS;
using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;


namespace Analytics
{
    public class AnalyticsService
    {



        private void Start()
        {
            FirebaseInit();
        }


        private static void SendEvent(string message)
        {
            Debug.Log(message);
            AppMetrica.Instance.ReportEvent(message);
            FirebaseAnalytics.LogEvent(RemoveSpaces(message));

            //Appodeal.LogEvent(message);
        }

        #region Firebase

        bool isFirebaseInitialized;

        void FirebaseInit() // обычна€ инициализаци€ фаербейса
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                if (task.Result == DependencyStatus.Available)
                {
                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                    //FirebaseAnalytics.SetUserId(SystemInfo.deviceUniqueIdentifier);
                    FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 5, 0));
                    Debug.Log($"Analytics initialized successfully: {task.Result}");

                    isFirebaseInitialized = true;
                }
                else
                {
                    Debug.Log($"Analytics initialization failed: {task.Result}");
                }
            });
        }

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

            //отбраковываем данные от јдћоба, чтобы не отправл€ть их в фаербейс
            if (e.Network == "GoogleAds")
            {
                Debug.Log("GoogleAds report skiped");
                return;
            }

            //заполн€ем параметры адмоба и отправл€ем евент
            var eventParams = new List<Parameter>();
            eventParams.Add(new Parameter(FirebaseAnalytics.ParameterAdPlatform, e.Platform));
            eventParams.Add(new Parameter(FirebaseAnalytics.ParameterAdFormat, e.Format));
            eventParams.Add(new Parameter(FirebaseAnalytics.ParameterAdSource, e.Network));
            eventParams.Add(new Parameter(FirebaseAnalytics.ParameterCurrency, e.Currency));
            eventParams.Add(new Parameter(FirebaseAnalytics.ParameterValue, e.Value));
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression, eventParams.ToArray());

            /* 
            eventParams.Clear();
            eventParams.Add(new Parameter(FirebaseAnalytics.ParameterValue, e.Value));
            eventParams.Add(new Parameter(FirebaseAnalytics.ParameterCurrency, e.Currency));
            FirebaseAnalytics.LogEvent("ad_cas", eventParams.ToArray()); */


            

                    Firebase.Analytics.Parameter[] AdParameters = {
            new Firebase.Analytics.Parameter("ad_platform", "CAS"),
            new Firebase.Analytics.Parameter("ad_source", e.Network),
           
            new Firebase.Analytics.Parameter("ad_format",e.Format),
            new Firebase.Analytics.Parameter("currency", "USD"), // All CAS revenue is sent in USD 
            new Firebase.Analytics.Parameter("value", e.Value)
              };
            Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", AdParameters);


        }

        #endregion
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