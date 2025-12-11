using System;
using System.Collections.Generic;
using System.Linq;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StageSceneContents.UI
{
    public class OrderUICanvas : MonoBehaviour
    {
        [SerializeField] private Transform layoutGroup;
        [SerializeField] OrderPanel orderPanelPrefab;
        
        private List<OrderPanel> orderPanels = new List<OrderPanel>();

        private void Awake()
        {
            EventManager.Instance.OnGetOrder += CreateOrderPanel;
            EventManager.Instance.OnFailedOrder += RemoveOrderPanel;
            EventManager.Instance.OnSuccessOrder += RemoveOrderPanel;
        }
    
        private void OnDestroy()
        {
            if (EventManager.Instance == null) return;
            
            EventManager.Instance.OnGetOrder -= CreateOrderPanel;
            EventManager.Instance.OnFailedOrder -= RemoveOrderPanel;
            EventManager.Instance.OnSuccessOrder -= RemoveOrderPanel;
        }
    
        private void CreateOrderPanel(Order order)
        {
            var panel = Instantiate(orderPanelPrefab, layoutGroup);
            panel.Init(order);
            orderPanels.Add(panel);
        }

        private void RemoveOrderPanel(Order targetOrder)
        {
            var panel = orderPanels.FirstOrDefault(s => s.CurOrder == targetOrder);
            if (panel == false) return;
        
            orderPanels.Remove(panel);
            panel.RemovePanel();
        }
    }
}
