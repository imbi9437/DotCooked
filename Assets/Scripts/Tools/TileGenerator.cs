using Unity.VisualScripting;
using UnityEngine;

namespace Tools
{
    public enum PlacingType
    {
        Single,
        Alternate,
        Random,
        Checkerboard,
    }

    public class TileGenerator : MonoBehaviour
    {
        public Sprite[] tileSprites;
        public Vector2 cellSize;
        public Vector2 size;
        public Transform parent;
        public PlacingType placingType;


        [ContextMenu("Generate")]
        private void Generate()
        {
            if (parent == null) return;
            if (parent.TryGetComponent(out Grid grid) == false)
            {
                grid = parent.AddComponent<Grid>();
            }

            grid.cellSize = cellSize;

            int index = 0;
            
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    var obj = new GameObject("Tile",typeof(SpriteRenderer));
                    obj.transform.SetParent(parent);
                    obj.transform.localPosition = grid.GetCellCenterLocal(new Vector3Int(x,y,0));
                    
                    switch (placingType)
                    {
                        case PlacingType.Single:
                            index = 0;
                            break;
                        case PlacingType.Alternate:
                            index = (index + 1) % tileSprites.Length;
                            break;
                        case PlacingType.Random:
                            index = UnityEngine.Random.Range(0, tileSprites.Length);
                            break;
                        case PlacingType.Checkerboard:
                            index = (x + y) % 2;
                            break;
                    }
                    
                    obj.TryGetComponent(out SpriteRenderer renderer);
                    renderer.sprite = tileSprites[index];
                }
            }
        }
    }
}