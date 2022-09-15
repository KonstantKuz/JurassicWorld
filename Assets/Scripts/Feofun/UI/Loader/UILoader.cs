using System;
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
                throw new Exception("Path to prefab is null");
            }
            var loadedPrefab = Resources.Load<TUIObject>(model.UIPath);
            var uiObject = InstancePrefab<TUIObject>(loadedPrefab, model.UIContainer);
            uiObject.Init(model.InitParameter);
            return uiObject;
        }
        public TUIObject Instance<TUIObject, TParam>(UIModel<TUIObject, TParam> model) where TUIObject : MonoBehaviour, IUiInitializable<TParam>
        {
            if (model.UIPrefab == null) {
                throw new Exception("Instance prefab is null");
            }
            var uiObject = InstancePrefab<TUIObject>(model.UIPrefab, model.UIContainer);
            uiObject.Init(model.InitParameter);
            return uiObject;
        }
        private TUIObject InstancePrefab<TUIObject>(Object prefab, Transform position) where TUIObject : MonoBehaviour
        {
            return _container.InstantiatePrefab(prefab, position).GetComponent<TUIObject>();
        }
    }
}