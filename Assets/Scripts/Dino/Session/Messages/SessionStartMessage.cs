namespace Dino.Session.Messages
{
    public struct SessionStartMessage
    {
        public readonly string LevelId;
        public readonly int LevelNumber;

        public SessionStartMessage(string levelId, int levelNumber)
        {
            LevelId = levelId;
            LevelNumber = levelNumber;
        }
    }
}