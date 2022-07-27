
namespace Dino.Player.Progress.Model
{
    public class PlayerProgress
    {
        
        public int GameCount { get; set; }
        public int WinCount { get; set; }
        public int LoseCount => GameCount - WinCount;
        public int LevelNumber => WinCount;
        public int Kills { get; set; }

        public static PlayerProgress Create() => new PlayerProgress();
 
    }
}