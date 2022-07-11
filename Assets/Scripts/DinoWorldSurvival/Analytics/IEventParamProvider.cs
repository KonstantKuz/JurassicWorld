using System.Collections.Generic;

namespace DinoWorldSurvival.Analytics
{
    public interface IEventParamProvider
    {
        Dictionary<string, object> GetParams(IEnumerable<string> paramNames);
    }
}