﻿using System.Collections.Generic;
using System.Linq;
using AppsFlyerSDK;
using Logger.Extension;

namespace Dino.Analytics.Wrapper
{
    public class AppsFlyerAnalyticsWrapper : IAnalyticsImpl
    {
        private const string DEV_KEY = "9gdCn4p9McTuPMAjnzTk4Y";
        private const string APP_ID = "1633821704";

        public void Init()
        {
            this.Logger().Info("Initializing AppsFlyer SDK");
            AppsFlyer.initSDK(DEV_KEY, APP_ID);
            AppsFlyer.startSDK();
        }

        private void ReportEvent(string message, Dictionary<string, string> parameters)
        {
            AppsFlyer.sendEvent(message, parameters);
        }

        public void ReportEventWithParams(string eventName, Dictionary<string, object> eventParams,
            IEventParamProvider eventParamProvider)
        {
            eventParams ??= new Dictionary<string, object>();
            ReportEvent(eventName, eventParams.ToDictionary(it => it.Key, it => it.Value.ToString()));
        }
    }
}