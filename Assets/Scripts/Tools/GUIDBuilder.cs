using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using StageSceneContents.Interactor;
using UnityEngine;

public class GUIDBuilder : MonoBehaviour
{
    public Transform target;

    [ContextMenu("CreateAndGrantGUID")]
    public void CreateAndGrantGUID()
    {
        int index = 70;
        foreach (Transform child in target)
        {
            string guid = Guid.NewGuid().ToString();

            var compo = child.GetComponent<InteractableObject>();
            var view = child.GetComponent<PhotonView>();
            view.ViewID = index++;
            compo.guid = guid;
        }
    }
}
