using Dino.Weapon.Components;
using JetBrains.Annotations;

namespace Dino.UI.Screen.World.Inventory.Model
{
    public class WeaponViewModel
    {
        public bool AmmoCountEnabled { get; }
        [CanBeNull]
        public WeaponWrapper WeaponWrapper { get; }

        public bool Enabled => WeaponWrapper != null;
        
        public WeaponViewModel(bool ammoCountEnabled, [CanBeNull] WeaponWrapper weaponWrapper)
        {
            AmmoCountEnabled = ammoCountEnabled;
            WeaponWrapper = weaponWrapper;
        }

        public static WeaponViewModel CreateDisable()
        {
            return new WeaponViewModel(false, null);
        }

    }
}