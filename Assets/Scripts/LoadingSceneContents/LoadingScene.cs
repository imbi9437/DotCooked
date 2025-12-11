using System.Collections;
using System.Collections.Generic;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SceneScripts
{
    public class LoadingScene : MonoBehaviour
    {
        [SerializeField] private float delay;

        [SerializeField] private TMP_Text loadingText;
        [SerializeField] private List<Image> obj;
        public float distance;
        
        private WaitForSeconds waitText = new WaitForSeconds(1f);
        private WaitForSeconds waitCircle = new WaitForSeconds(0.1f);
        
        private void Start()
        {
            SceneController.ActiveTargetScene(delay);
            StartCoroutine(LoadingTextCo());
            StartCoroutine(LoadingCircleCo());
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        IEnumerator LoadingTextCo()
        {
            string text = "Loading...";
            while (true)
            {
                for (int i = 0; i < 4; i++)
                {
                    loadingText.text = text.Substring(0, 7 + i);
                    yield return waitText;
                }
            }
        }

        IEnumerator LoadingCircleCo()
        {
            int startIndex = 0;
            while (true)
            {
                for (int i = 0; i < obj.Count; i++)
                {
                    int t = startIndex + i;
                    if (t > obj.Count) t -= obj.Count;
                    Color color = Color.Lerp(Color.white, Color.clear, t / (float) obj.Count);
                    
                    obj[i].color = color;
                }

                startIndex++;
                if (startIndex > obj.Count) startIndex = 0;
                yield return waitCircle;
            }
        }
    }
}
