using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MainSceneContents;
using Manager;
using ScriptableObjects;
using UnityEngine;

public class StageController : MonoBehaviour
{
    private List<StageInteractor> stageObjects;
    
    private void Start()
    {
        var stageData = DataManager.Instance.GetStageData();
        
        var chileComponents = transform.GetComponentsInChildren<StageInteractor>();
        stageObjects = chileComponents.OrderBy(c => c.transform.GetSiblingIndex()).ToList();
        
        for (int i = 0; i < stageObjects.Count; i++)
        {
            stageObjects[i].gameObject.SetActive(i < stageData.Count);
            if (i >= stageData.Count) continue;
            
            stageObjects[i].Initialize(stageData[i]);
        }
    }
}
