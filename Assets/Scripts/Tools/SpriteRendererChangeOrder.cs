using UnityEngine;

namespace Tools
{
    public class SpriteRendererChangeOrder : MonoBehaviour
    {
        public Transform target;
        
        public string layerName;
        public int order;


        [ContextMenu("ChangeOrder")]
        public void ChangeOrder()
        {
            var renderers = target.GetComponentsInChildren<SpriteRenderer>(true);

            foreach (var render in renderers)
            {
                render.sortingOrder = order;
            }
        }
        
        [ContextMenu("ChangeLayer")]
        public void ChangeLayer()
        {
            var renderers = target.GetComponentsInChildren<SpriteRenderer>(true);
            
            foreach (var render in renderers)
            {
                render.sortingLayerName = layerName;
            }
        }
    }
}
