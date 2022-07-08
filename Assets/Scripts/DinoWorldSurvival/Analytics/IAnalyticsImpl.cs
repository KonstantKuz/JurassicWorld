﻿using System.Collections.Generic;
using JetBrains.Annotations;
using Survivors.Session.Config;

namespace Survivors.Analytics
{
    public interface IAnalyticsImpl
    {
        void Init();
        void ReportEventWithParams(string eventName, [CanBeNull] Dictionary<string, object> eventParams,
            IEventParamProvider eventParamProvider);
    }
}