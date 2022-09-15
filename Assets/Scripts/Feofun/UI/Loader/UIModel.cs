using Feofun.UI.Components;
using UnityEngine;

namespace Feofun.UI.Loader
{
    public class UIModel<TUIObject, TParam> where TUIObject : MonoBehaviour, IUiInitializable<TParam>
    {
        public TParam InitParameter { get; private set; }
        public Transform UIContainer { get; private set; }
        public string UIPath { get; private set; }
        public TUIObject UIPrefab { get; private set; }

        public static UIModel<TUIObject, TParam> Create(TParam initParameter)
        {
            return new UIModel<TUIObject, TParam>() {
                    InitParameter = initParameter,
            };
        }
        public UIModel<TUIObject, TParam> Container(Transform container)
        {
            UIContainer = container;
            return this;
        }  
        public UIModel<TUIObject, TParam> Prefab(TUIObject prefab)
        {
            UIPrefab = prefab;
            return this;
        }
        public UIModel<TUIObject, TParam> Path(string path)
        {
            UIPath = path;
            return this;
        }
    }
}