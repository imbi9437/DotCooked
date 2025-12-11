using UnityEngine;

namespace TitleSceneContents.UI
{
    public abstract class UIPanel : MonoBehaviour
    {
        public abstract int PanelIndex { get; }
        protected UIController Controller;

        public virtual void Initialize(UIController controller)
        {
            this.Controller = controller;
        }
    }
}
