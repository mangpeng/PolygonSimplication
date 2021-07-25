using System;
using System.Collections.Generic;
using CustomSample;
using UnityEngine;

[AddComponentMenu("Custom/Collider/Edge Simplication")]
[RequireComponent(typeof(PolygonCollider2D), typeof(EdgeCollider2D))]
public class EdgeSimplication : MonoBehaviour 
{
    public bool refreshOnValidate;
    public bool refreshOnUpdate;
    
    public Vector2 edgeNormalOpposite = Vector2.down;
    public int rayBudget = 1000;
    public float tolerance = 0;
    
    public Vector2[] PlolygonPath => _polygon.points;
    public Vector2[] EdgePath => _edge.points;
    
    private EdgeCollider2D _edge;
    private PolygonCollider2D _polygon;

    private void Awake()
    {
        _edge = GetComponent<EdgeCollider2D>();
        _polygon = GetComponent<PolygonCollider2D>();
        
        Debug.Log(1);
    }

    void Update()
    {
        if (!refreshOnUpdate)
        {
            return;
        }
        
        if (_polygon == null || _edge == null)
        {
            return;
        }
        
        _polygon.ResetPath();
        Refresh();
    }
    
    void OnValidate()
    {
        if (_polygon == null || _edge == null)
        {
            return;
        }
        
        _polygon.ResetPath();
        
        if (!refreshOnValidate)
        {
            return;
        }

        Refresh();
    }

    public void Refresh()
    {
        List<Vector2> path = new List<Vector2>();
        Vector2 upperRight = _polygon.bounds.max;
        Vector2 upperLeft = _polygon.bounds.min;
        
        upperLeft.y = upperRight.y;
        
        for(int i = 0; i < rayBudget; i++)
        {
            float t = (float)i/(float)rayBudget;
            Vector2 rayOrigin = Vector2.Lerp(upperLeft, upperRight, t);
            
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, edgeNormalOpposite, _polygon.bounds.size.y);

            for(int j = 0; j < hits.Length; j++)
            {
                RaycastHit2D hit = hits[j];
                if(hit.collider == _polygon)
                {
                    Vector2 localHitPoint = transform.InverseTransformPoint(hit.point);
                    path.Add(localHitPoint);
                    break;
                }
            }
        }
        
        if(tolerance > 0) path = DouglasPeuckerReduction.GetDouglasPeuckerReduction(path, tolerance);
        _edge.points = path.ToArray();
    }


}