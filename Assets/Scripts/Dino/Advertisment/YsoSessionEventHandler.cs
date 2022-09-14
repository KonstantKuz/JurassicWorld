using Dino.Session.Messages;
using Dino.Session.Model;
using SuperMaxim.Messaging;

namespace Dino.Advertisment
{
    public class YsoSessionEventHandler
    {
        public YsoSessionEventHandler(IMessenger messenger)
        {
            messenger.Subscribe<SessionStartMessage>(OnSessionStart);
            messenger.Subscribe<SessionEndMessage>(OnSessionEnd);
        }

        private void OnSessionEnd(SessionEndMessage msg)
        {
            YsoCorp.GameUtils.YCManager.instance.OnGameFinished( msg.Result == SessionResult.Win);
        }

        private void OnSessionStart(SessionStartMessage msg)
        {
            YsoCorp.GameUtils.YCManager.instance.OnGameStarted(msg.LevelNumber);
        }
    }
}