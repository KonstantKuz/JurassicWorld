using System;
using Feofun.Extension;
using Feofun.UI.Components;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Feofun.UI.Loader
{
    public class UILoader
    {
        [Inject]
        private DiContainer _container;

        public TUIObject Load<TUIObject, TParam>(UIModel<TUIObject, TParam> model) where TUIObject : MonoBehaviour, IUiInitializable<TParam>
        {
            if (model.UIPath == null) {
                throw new NullReferenceException("Path to prefab is null");
            }
            var loadedPrefab = Resources.Load<TUIObject>(model.UIPath);
            model.Prefab(loadedPrefab);
            return Instance(model);
        }
        public TUIObject Instance<TUIObject, TParam>(UIModel<TUIObject, TParam> model) where TUIObject : MonoBehaviour, IUiInitializable<TParam>
        {
            if (model.UIPrefab == null) {
                throw new NullReferenceException("Instance prefab is null");
            }
            var uiObject = InstancePrefab<TUIObject>(model.UIPrefab, model.UIContainer);
            uiObject.Init(model.InitParameter);
            return uiObject;
        }
        private TUIObject InstancePrefab<TUIObject>(Object prefab, Transform position) where TUIObject : MonoBehaviour
        {
            return _container.InstantiatePrefab(prefab, position).RequireComponent<TUIObject>();
        }
    }
}