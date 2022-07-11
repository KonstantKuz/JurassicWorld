using System.Runtime.Serialization;

namespace DinoWorldSurvival.Cheats.Data
{
    [DataContract]
    public class CheatSettings
    {
        [DataMember]
        public bool ConsoleEnabled; 
        [DataMember]
        public bool FPSMonitorEnabled;
        
    }
}