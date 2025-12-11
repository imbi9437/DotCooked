using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScriptableObjects
{
    public enum SceneType
    {
        Title,
        Loading,
        Main,
        Stage,
    }   
    
    [CreateAssetMenu(menuName = "ScriptableObjects/SceneInfo")]
    public class SceneInfo : UnityEngine.ScriptableObject
    {
        public SceneType sceneType;
        public string sceneName;
        public int sceneIndex;
        
#if UNITY_EDITOR
        public UnityEditor.SceneAsset sceneAsset;
        private void OnValidate()
        {
            if (sceneAsset == false) return;

            string objectPath = UnityEditor.AssetDatabase.GetAssetPath(this);
            string scenePath = UnityEditor.AssetDatabase.GetAssetPath(sceneAsset);

            sceneName = sceneAsset.name;
            sceneIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
            
            UnityEditor.AssetDatabase.RenameAsset(objectPath, sceneName);
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}