using System.Collections.Generic;
using CustomSample;
using UnityEngine;

[AddComponentMenu("Custom/Collider/Polygon Simplication")]
[RequireComponent(typeof(PolygonCollider2D))]
public class PolygonSimplication : MonoBehaviour
{
    public bool refreshOnValidate;
    public bool refreshOnUpdate;
    
    public float tolerance = 0f;

    public Vector2[] Path => _polygon.points;


    private List<List<Vector2>> originalPaths = new List<List<Vector2>>();
    private PolygonCollider2D _polygon;

    private void Awake()
    {
        _polygon = GetComponent<PolygonCollider2D>();

        for (int i = 0; i < _polygon.pathCount; i++)
        {
            List<Vector2> path = new List<Vector2>(_polygon.GetPath(i));
            originalPaths.Add(path);
        }
    }

    private void Update()
    {
        if (!refreshOnUpdate)
        {
            return;
        }
        
        if (_polygon == null)
        {
            return;
        }
        
        _polygon.ResetPath();
        originalPaths.Clear();
        for (int i = 0; i < _polygon.pathCount; i++)
        {
            List<Vector2> path = new List<Vector2>(_polygon.GetPath(i));
            originalPaths.Add(path);
        }
        
 

        Refresh();
    }
    
    private void OnValidate()
    {
        if (_polygon == null)
        {
            return;
        }
        
        _polygon.ResetPath();
        originalPaths.Clear();
        for (int i = 0; i < _polygon.pathCount; i++)
        {
            List<Vector2> path = new List<Vector2>(_polygon.GetPath(i));
            originalPaths.Add(path);
        }
        
        if (!refreshOnValidate)
        {
            return;
        }

        Refresh();
    }

    public void Reset()
    {
        for (int i = 0; i < originalPaths.Count; i++)
        {
            List<Vector2> path = originalPaths[i];
            _polygon.SetPath(i, path.ToArray());
        }
    }

    public void Refresh()
    {
        if (tolerance <= 0)
        {
            Reset();
            return;
        }

        for (int i = 0; i < originalPaths.Count; i++)
        {
            List<Vector2> path = originalPaths[i];
            path = DouglasPeuckerReduction.GetDouglasPeuckerReduction(path, tolerance);
            _polygon.SetPath(i, path.ToArray());
        }
    }
    
   
}
