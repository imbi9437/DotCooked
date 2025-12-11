using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using CustomExtensions;
using Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class SceneController : MonoSingleton<SceneController>
    {
        private static (SceneType, int) targetScene;
        private static (SceneType, int) currentScene;
        private static (SceneType, int) previousScene;
    
        private Dictionary<SceneType, List<SceneInfo>> sceneDic = new Dictionary<SceneType, List<SceneInfo>>();
        public List<SceneInfo> sceneInfos;

        private SceneInfo LoadingSceneInfo => sceneDic[SceneType.Loading][0];
        private SceneInfo MainSceneInfo => sceneDic[SceneType.Main][0];
        private SceneInfo TitleSceneInfo => sceneDic[SceneType.Title][0];
    
        #region UnityMessage
    
        protected override void Awake()
        {
            base.Awake();
        
            foreach (var type in Enum.GetValues(typeof(SceneType)))
            {
                sceneDic.TryAdd((SceneType)type, new List<SceneInfo>());
            }

            foreach (var info in sceneInfos)
            {
                sceneDic[info.sceneType].Add(info);
            }
        }
    
        #endregion

        public static void LoadScene(SceneInfo sceneInfo)
        {
            SceneType type = sceneInfo.sceneType;
            int index = Instance.sceneDic[type].FindIndex(s => s == sceneInfo);
            
            LoadScene(type,index);
        }
        
        /// <summary>
        /// Loads a specified scene of the given type and index asynchronously, starting from a loading scene.
        /// </summary>
        /// <param name="type">The type of the scene to load.</param>
        /// <param name="index">The index of the scene within its type's scene list. Defaults to 0 if not provided.</param>
        public static void LoadScene(SceneType type, int index = 0) => Instance.LoadSceneAsync(type, index).Forget();

        /// <summary>
        /// Activates the target scene of the specified type and index after an optional delay.
        /// </summary>
        /// <param name="delay">The delay in seconds before activating the target scene.</param>
        public static void ActiveTargetScene(float delay) => Instance.ActiveTargetSceneAsync(delay).Forget();
    
        /// <summary>
        /// 목표 씬 정보 들고 로딩 씬으로 이동
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        private async UniTaskVoid LoadSceneAsync(SceneType type, int index = 0)
        {
            EventManager.Instance.OnBeforeLoadScene?.Invoke();
            
            targetScene = (type, index);
            var operation = SceneManager.LoadSceneAsync(LoadingSceneInfo.sceneIndex);
            operation.allowSceneActivation = false;

            await UniTask.WaitUntil(operation.WaitUntilLoadScene);
            operation.allowSceneActivation = true;
        }
    
        /// <summary>
        /// 로딩 씬에서 목표 씬으로 이동
        /// </summary>
        /// <param name="delay"></param>
        private async UniTaskVoid ActiveTargetSceneAsync(float delay)
        {
            SceneInfo targetSceneInfo = sceneDic[targetScene.Item1][targetScene.Item2];
            var operation = SceneManager.LoadSceneAsync(targetSceneInfo.sceneIndex);
            operation.allowSceneActivation = false;
        
            var loadingTask = UniTask.WaitUntil(operation.WaitUntilLoadScene);
            var delayTask = UniTask.Delay(TimeSpan.FromSeconds(delay));
        
            await UniTask.WhenAll(loadingTask, delayTask);
            operation.allowSceneActivation = true;
        }

        private async UniTask<bool> FadeEffect(bool isFadeIn)
        {
            //todo : 씬 전환 Fade In,Out 관련 로직 추가
            await UniTask.Delay(TimeSpan.FromSeconds(0));

            return true;
        }
    }
}
