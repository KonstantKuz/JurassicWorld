using DinoWorldSurvival.Session.Model;

namespace DinoWorldSurvival.Session.Messages
{
    public readonly struct SessionEndMessage
    {
        public readonly SessionResult Result;

        public SessionEndMessage(SessionResult result)
        {
            Result = result;
        }
    }
}