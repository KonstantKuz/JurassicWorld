using Dino.Session.Model;

namespace Dino.Session.Messages
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