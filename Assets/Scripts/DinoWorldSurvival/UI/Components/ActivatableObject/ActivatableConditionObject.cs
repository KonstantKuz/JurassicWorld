using System.Linq;
using DinoWorldSurvival.UI.Components.ActivatableObject.Conditions;
using SuperMaxim.Core.Extensions;
using UniRx;
using UnityEngine;

namespace DinoWorldSurvival.UI.Components.ActivatableObject
{
    public class ActivatableConditionObject : MonoBehaviour
    {
        [SerializeField]
        private ActivatableObject[] _activatableObjects;

        private void Awake()
        {
            var conditions = GetComponents<ICondition>();
            var observable = conditions.Select(it => it.IsAllow()).CombineLatest().Select(it => it.All(b => b));
            _activatableObjects.ForEach(it => it.Init(observable));
        }
    }
}