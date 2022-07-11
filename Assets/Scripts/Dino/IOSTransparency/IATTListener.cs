using System;

namespace Dino.IOSTransparency
{
    public interface IATTListener
    { 
        event Action OnStatusReceived;
        void Init();
    }
}