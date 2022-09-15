using Dino.Location.Workbench;
using Dino.Util;
using UniRx;

namespace Dino.UI.Hud.Workbench
{
    public class WorkbenchHudModel
    {
        private readonly ReactiveProperty<float> _craftProgress = new ReactiveProperty<float>();
        private readonly ReactiveProperty<bool> _isCrafting = new ReactiveProperty<bool>();
        private readonly Location.Workbench.Workbench _workbench;

        private CompositeDisposable _crafterDisposable;
        private CompositeDisposable _timerDisposable;

        public readonly string Icon;

        public IReactiveProperty<float> CraftProgress => _craftProgress;
        public IReactiveProperty<bool> IsCrafting => _isCrafting;

        public WorkbenchHudModel(Location.Workbench.Workbench workbench)
        {
            _workbench = workbench;
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
            _isCrafting.SetValueAndForceNotify(hasActiveTimer);
            if (!hasActiveTimer) {
                return;
            }
            _timerDisposable = new CompositeDisposable();
            crafter.CraftTimer.Progress.Subscribe(_ => OnTimerUpdate(crafter.CraftTimer)).AddTo(_timerDisposable);

        }
        private void OnTimerUpdate(ActionTimer timer) => _craftProgress.SetValueAndForceNotify(timer.Progress.Value / timer.Duration);

        private void OnCrafterRemoved()
        {
            DisposeCrafter();
            _isCrafting.SetValueAndForceNotify(false);
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