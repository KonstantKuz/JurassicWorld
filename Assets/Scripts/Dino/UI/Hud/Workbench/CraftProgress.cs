namespace Dino.UI.Hud.Workbench
{
    public readonly struct CraftProgress
    {
        public readonly bool Enabled;
        public readonly float Progress;
        public CraftProgress(bool enabled, float progress)
        {
            Enabled = enabled;
            Progress = progress;
        }
    }
}