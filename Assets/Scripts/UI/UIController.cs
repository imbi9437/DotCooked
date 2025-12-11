using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TitleSceneContents.UI
{
    public abstract class UIController : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        
        protected Canvas canvas;
        
        protected Dictionary<int, UIPanel> panelDic = new Dictionary<int, UIPanel>();
        protected Stack<int> panelStack = new Stack<int>();

        protected virtual void Start()
        {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;

            closeButton.onClick.AddListener(ReturnPanel);
            var panels = GetComponentsInChildren<UIPanel>(true);

            foreach (var panel in panels)
            {
                panelDic.Add(panel.PanelIndex, panel);
                panel.gameObject.SetActive(false);
                panel.Initialize(this);
            }
            
            OpenPanel(0);
        }

        protected virtual void OnDestroy()
        {
            closeButton.onClick.RemoveAllListeners();
        }

        public virtual void ToggleUI()
        {
            if (canvas.enabled) CloseUI();
            else OpenUI();
        }

        public virtual void OpenUI() => canvas.enabled = true;
        public virtual void CloseUI() => canvas.enabled = false;
        
        public void ReturnPanel()
        {
            if (panelStack.Count <= 2)
            {
                CloseUI();
                return;
            }
            
            ClosePanel();
        }

        public void ReturnAllPanel()
        {
            while (panelStack.Count > 2)
            {
                ClosePanel();
            }
        }
        public void OpenPanel(int index)
        {
            if (panelDic.TryGetValue(index, out var panel) == false) return;
            if (panel.gameObject.activeSelf) return;
            
            panelStack.Push(index);
            panelDic[index].gameObject.SetActive(true);
        }
        public void ClosePanel()
        {
            int index = panelStack.Pop();
            panelDic[index].gameObject.SetActive(false);
        }
        public void ChangePanel(int index)
        {
            ClosePanel();
            OpenPanel(index);
        }
    }
}
