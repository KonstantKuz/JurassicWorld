using System.Collections.Generic;
using JetBrains.Annotations;

namespace Dino.Analytics
{
    public interface IAnalyticsImpl
    {
        void Init();
        void ReportEventWithParams(string eventName, [CanBeNull] Dictionary<string, object> eventParams,
            IEventParamProvider eventParamProvider);
    }
}