using System.Collections.Generic;
using UnityEngine;

namespace CustomSample
{
    public static class Extention
    {
        public static void ResetPath(this PolygonCollider2D polygon)
        {
            Sprite sprite = polygon.GetComponent<SpriteRenderer>().sprite;
            for (int i = 0; i < polygon.pathCount; i++)
            {
                polygon.SetPath(i, new Vector2[]{});
            }
        
            polygon.pathCount = sprite.GetPhysicsShapeCount();
        
            List<Vector2> path = new List<Vector2>();
            for (int i = 0; i < polygon.pathCount; i++)
            {
                path.Clear();
                sprite.GetPhysicsShape(i, path);
                polygon.SetPath(i, path.ToArray());
            }
        }
    }
}