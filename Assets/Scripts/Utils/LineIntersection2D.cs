using System.Collections.Generic;
using UnityEngine;

public enum ScreenEdge { EdgeTop, EdgeBottom, EdgeRight, EdgeLeft }

public class LineIntersection2D
{
    public static void GetIntersectionWithScreenEdge(GameObject gameObjectForName, Vector2 start, Vector2 end, out Vector2 intersection, out ScreenEdge edge)
    {
        intersection = default(Vector2);
        edge = default(ScreenEdge);
        float closestDistance = float.MaxValue;
        bool found = false;

        foreach (var item in System.Enum.GetValues(typeof(ScreenEdge)))
        {
            var edgePoints = GetEdgePoints((ScreenEdge)item);
            if (LineSegmentsIntersection(start, end, edgePoints[0], edgePoints[1], out Vector2 tempIntersection))
            {
                float distance = Vector2.Distance(start, tempIntersection);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    intersection = tempIntersection;
                    edge = (ScreenEdge)item;
                    found = true;
                }
            }
        }

        if (!found)
        {
            Debug.LogWarning("No intersection found on enemy " + gameObjectForName.name);
        }
    }

    private static List<Vector2> GetEdgePoints(ScreenEdge edge)
    {
        List<Vector2> points = new();

        switch (edge)
        {
            case ScreenEdge.EdgeTop:
                points.Add(new(CameraUtils.CameraRect.xMin, CameraUtils.CameraRect.yMax));
                points.Add(new(CameraUtils.CameraRect.xMax, CameraUtils.CameraRect.yMax));
                break;
            case ScreenEdge.EdgeLeft:
                points.Add(new(CameraUtils.CameraRect.xMin, CameraUtils.CameraRect.yMax));
                points.Add(new(CameraUtils.CameraRect.xMin, CameraUtils.CameraRect.yMin));
                break;
            case ScreenEdge.EdgeRight:
                points.Add(new(CameraUtils.CameraRect.xMax, CameraUtils.CameraRect.yMax));
                points.Add(new(CameraUtils.CameraRect.xMax, CameraUtils.CameraRect.yMin));
                break;
            case ScreenEdge.EdgeBottom:
                points.Add(new(CameraUtils.CameraRect.xMin, CameraUtils.CameraRect.yMin));
                points.Add(new(CameraUtils.CameraRect.xMax, CameraUtils.CameraRect.yMin));
                break;
            default:
                throw new System.Exception("Error");
        }

        return points;
    }

    private static bool LineSegmentsIntersection(Vector2 A, Vector2 B, Vector2 C, Vector2 D, out Vector2 intersection)
    {
        intersection = Vector2.zero;

        Vector2 AB = B - A;
        Vector2 CD = D - C;

        float denominator = AB.x * CD.y - AB.y * CD.x;

        // Lines are parallel if denominator is 0
        if (Mathf.Approximately(denominator, 0))
        {
            return false;
        }

        Vector2 AC = C - A;
        float numerator1 = AC.x * CD.y - AC.y * CD.x;
        float numerator2 = AC.x * AB.y - AC.y * AB.x;

        float t = numerator1 / denominator;
        float u = numerator2 / denominator;

        // Check if the intersection point is within the line segments
        if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
        {
            intersection = A + t * AB;
            return true;
        }

        return false;
    }
}