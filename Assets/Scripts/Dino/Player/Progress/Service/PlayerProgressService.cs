using Dino.Player.Progress.Model;
using Dino.Session.Messages;
using Dino.Session.Model;
using SuperMaxim.Messaging;
using UniRx;

namespace Dino.Player.Progress.Service
{
    public class PlayerProgressService
    {
        private readonly PlayerProgressRepository _repository;
        
        private readonly IntReactiveProperty _gameCount; 
       
        public IReadOnlyReactiveProperty<int> GameCount => _gameCount;
        public PlayerProgress Progress => _repository.Get() ?? PlayerProgress.Create();

        public PlayerProgressService(PlayerProgressRepository repository)
        {
            _repository = repository;
            _gameCount = new IntReactiveProperty(Progress.GameCount);
        }
        
        public void OnSessionStarted(string levelId)
        {
            var progress = Progress;
            progress.IncreasePassCount(levelId);
            SetProgress(progress);
        }
        
        public void OnSessionFinished(SessionResult sessionResult)
        {
            var progress = Progress;
            progress.GameCount++;
            if (sessionResult == SessionResult.Win) {
                progress.WinCount++;
            }
            SetProgress(progress);
        }
        
        private void SetProgress(PlayerProgress progress)
        {
            _repository.Set(progress);
            _gameCount.Value = progress.GameCount;
        }
        
        public void AddKill()
        {
            var progress = Progress;
            progress.Kills++;
            SetProgress(progress);  //TODO: check that it's not very slow 
        }
    }
}