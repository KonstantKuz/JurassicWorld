using System.Collections.Generic;

namespace Dino.Analytics
{
    public interface IEventParamProvider
    {
        Dictionary<string, object> GetParams(IEnumerable<string> paramNames);
    }
}