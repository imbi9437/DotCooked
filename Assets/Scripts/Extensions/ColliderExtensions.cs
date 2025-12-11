using UnityEngine;

namespace CustomExtensions
{
    public static class ColliderExtensions
    {
        public static void ResizingCollider2D(this Collider2D collider, Sprite sprite)
        {
            Vector2 center = sprite.bounds.center;
            Vector2 extents = sprite.bounds.extents;
            
            if (collider is BoxCollider2D boxCollider)
            {
                boxCollider.size = new Vector2(extents.x * 2, extents.y * 2);
                boxCollider.offset = new Vector2(center.x, center.y);
            }
            else if (collider is CircleCollider2D circleCollider)
            {
                circleCollider.offset = center;
                circleCollider.radius = Mathf.Max(extents.x, extents.y);
            }
        }
    }
}
