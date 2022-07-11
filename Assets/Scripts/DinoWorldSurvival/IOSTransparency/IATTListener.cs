using System;

namespace DinoWorldSurvival.IOSTransparency
{
    public interface IATTListener
    { 
        event Action OnStatusReceived;
        void Init();
    }
}