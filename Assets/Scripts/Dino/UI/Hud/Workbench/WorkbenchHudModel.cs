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
            DisposeCraftTimer();
            _workbench.OnCrafterCreated -= OnCrafterCreated;
            _workbench.OnCrafterRemoved -= OnCrafterRemoved;
        }

        private void OnCrafterCreated(CrafterByTimer crafter)
        {
            DisposeCraftTimer();
            _timerDisposable = new CompositeDisposable();
            crafter.CraftTimerProperty.Subscribe(OnTimerUpdatable).AddTo(_timerDisposable);
        }

        private void OnTimerUpdatable([CanBeNull] CraftTimer timer)
        {
            var craftProgress = new CraftProgress();
            if (timer != null) {
                craftProgress = new CraftProgress(true, timer.Progress / timer.Duration);
            }
            _craftProgress.SetValueAndForceNotify(craftProgress);
        }

        private void OnCrafterRemoved()
        {
            DisposeCraftTimer();
            _craftProgress.SetValueAndForceNotify(new CraftProgress());
        }

        private void DisposeCraftTimer()
        {
            _timerDisposable?.Dispose();
            _timerDisposable = null;
        }
    }
}