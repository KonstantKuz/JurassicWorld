using Dino.Location.Workbench;
using Dino.Util;
using JetBrains.Annotations;
using UniRx;

namespace Dino.UI.Hud.Workbench
{
    public class WorkbenchHudModel
    {
        private readonly ReactiveProperty<CraftProgress> _craftProgress;
        private readonly Location.Workbench.Workbench _workbench;

        private CompositeDisposable _crafterDisposable;  
        private CompositeDisposable _timerDisposable;

        public readonly string Icon;

        public IReadOnlyReactiveProperty<CraftProgress> CraftProgress => _craftProgress;

        public WorkbenchHudModel(Location.Workbench.Workbench workbench)
        {
            _workbench = workbench;
            _craftProgress = new ReactiveProperty<CraftProgress>(new CraftProgress());
            Icon = IconPath.GetForCraft(workbench.CraftItemId);
            _workbench.OnCrafterCreated += OnCrafterCreated;
            _workbench.OnCrafterRemoved += OnCrafterRemoved;
        }

        public void Dispose()
        {
            DisposeCrafter();
            _workbench.OnCrafterCreated -= OnCrafterCreated;
            _workbench.OnCrafterRemoved -= OnCrafterRemoved;
        }

        private void OnCrafterCreated(CrafterByTimer crafter)
        {
            DisposeCrafter();
            _crafterDisposable = new CompositeDisposable();
            crafter.HasActiveTimer.Subscribe(hasActiveTimer => OnTimerActivate(crafter, hasActiveTimer)).AddTo(_crafterDisposable);
        }
        private void OnTimerActivate(CrafterByTimer crafter, bool hasActiveTimer)
        {
            DisposeTimer();
            if (hasActiveTimer) {
                _timerDisposable = new CompositeDisposable();
                crafter.CraftTimer.Progress.Subscribe(_ => OnTimerUpdate(crafter.CraftTimer)).AddTo(_timerDisposable);
            } else {
                _craftProgress.SetValueAndForceNotify(new CraftProgress());
            }
        }
        private void OnTimerUpdate(ActionTimer timer) => _craftProgress.SetValueAndForceNotify(new CraftProgress(true, timer.Progress.Value / timer.Duration));

        private void OnCrafterRemoved()
        {
            DisposeCrafter();
            _craftProgress.SetValueAndForceNotify(new CraftProgress());
        }
        private void DisposeTimer()
        {
            _timerDisposable?.Dispose();
            _timerDisposable = null;
        }
        private void DisposeCrafter()
        {
            _crafterDisposable?.Dispose();
            _crafterDisposable = null;
            DisposeTimer();
        }
    }
}