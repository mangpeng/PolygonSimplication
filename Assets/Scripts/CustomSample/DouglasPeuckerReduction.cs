using System.Collections.Generic;
using UnityEngine;

public class DouglasPeuckerReduction
{
    public static List<Vector2> GetDouglasPeuckerReduction(List<Vector2> points, float tolerance)
    {
        if (points == null || points.Count < 3)
            return points;
        
        int firstIndex = 0;
        int lastIndex = points.Count - 1;
        List<int> pointIndexsToKeep = new List<int>();

        //Add the first and last index to the keepers
        pointIndexsToKeep.Add(firstIndex);
        pointIndexsToKeep.Add(lastIndex);

        //The first and the last point cannot be the same
        while (points[firstIndex].Equals(points[lastIndex]))
        {
            lastIndex--;
        }

        DouglasPeuckerReductionRecursive(points, firstIndex, lastIndex, tolerance, ref pointIndexsToKeep);
        
        List<Vector2> returnPoints = new List<Vector2>();
        pointIndexsToKeep.Sort();
        foreach (int i in pointIndexsToKeep)
        {
            returnPoints.Add(points[i]);
        }
        
        return returnPoints;
    }

    /// <summary>
    /// DouglasPeucker 알고리즘을 이용해 단순화된 경로를 구합니다.
    /// </summary>
    /// <param name="paths"></param>
    /// <param name="first"></param>
    /// <param name="last"></param>
    /// <param name="simplePath"></param>
    /// <param name="tolerance"></param>
    /// <param name="result"></param>
    private static void DouglasPeuckerReductionRecursive(List<Vector2> paths, int first, int last, float tolerance, ref List<int> result)
    {
        float maxDistance = 0;
        int farthestIndex = 0;

        for (int i = first; i < last; i++)
        {
            Vector2 firstPoint = paths[first];
            Vector2 lastPoint = paths[last];
            Vector2 targetPoint = paths[i];
            
            float distance = GetPerpendicularDistance(firstPoint, lastPoint, targetPoint);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestIndex = i;
            }
        }
        

        if (maxDistance > tolerance && farthestIndex != 0)
        {
            result.Add(farthestIndex);

            DouglasPeuckerReductionRecursive(paths, first, farthestIndex, tolerance, ref result);
            DouglasPeuckerReductionRecursive(paths, farthestIndex, last, tolerance, ref result);
        }
    }

    private static float GetPerpendicularDistance(Vector2 Point1, Vector2 Point2, Vector2 Point)
    {
        float height = 0;
        
        // 삼각형 사선 공식(삼각형 세 꼭지점이 주어질 때 삼각형 넓이를 구한다)
        // 꼭지점 A(x1, y1) B(x2,y2) C(x3,y3)이 주어질 때,
        // 넓이는 = 1/2 * (x1 * y2) + (x2 * y3) + (x3 * y1) - ((x2 * y1) + (x3 * y2) + (x1 * y3))
        // {
        //     float area = 
        //         Mathf.Abs(.5f * (
        //             Point1.x * Point2.y + Point2.x * Point.y + Point.x * Point1.y 
        //             - Point2.x * Point1.y - Point.x * Point2.y - Point1.x * Point.y));
        //     float bottom = Mathf.Sqrt(Mathf.Pow(Point1.x - Point2.x, 2f) + 
        //                               Mathf.Pow(Point1.y - Point2.y, 2f));
        //     height = area / bottom * 2f;
        // }

        // 한점과 직선의 거리
        // 점(x1, y1)과 직선 ax + by + c = 0 사이의 거리 d
        // d = |ax1 + by1 + c| / sqrt(a^2 + b^2)
        
        // 두점을 지나는 직선
        // a(x1, y1) b(x2, y2)
        // y = (y2 - y1) / (x2 - x1) * (x-x1) + y1
        {
            float a = (Point2.y - Point1.y) / (Point2.x - Point1.x);
            float b = -1;
            float c = Point1.y -1 * ((Point2.y - Point1.y)/(Point2.x - Point1.x)) * Point1.x;
            
            height = Mathf.Abs((a * Point.x) + (b * Point.y) + c) 
                     / ( Mathf.Sqrt((Mathf.Pow(a, 2)) + Mathf.Pow(b, 2)));
        }


        return height;
    }


}
