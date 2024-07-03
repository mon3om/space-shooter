using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornersPositionHelper : MonoBehaviour
{
    public static Vector2 GetCorner(int linesCount, int columnsCount, Vector2 targetArea)
    {
        float screenWidth = CameraUtils.CameraRect.xMax * 2;
        float screenHeight = CameraUtils.CameraRect.yMax * 2;
        float singleAreaWidth = screenWidth / columnsCount;
        float singleAreaHeight = screenHeight / linesCount;

        Vector2 singleAreaPosition = new(
            CameraUtils.CameraRect.xMin + (singleAreaWidth * targetArea.x) + (singleAreaWidth / 2),
            CameraUtils.CameraRect.yMax - (singleAreaHeight * targetArea.y) - (singleAreaHeight / 2)
        );

        return singleAreaPosition;
    }

    public static float GetAreaWidth(int columnsCount)
    {
        float screenWidth = CameraUtils.CameraRect.xMax * 2;
        float singleAreaWidth = screenWidth / columnsCount;

        return singleAreaWidth;
    }

    public static float GetAreaHeight(int linesCount)
    {
        float screenHeight = CameraUtils.CameraRect.yMax * 2;
        float singleAreaHeight = screenHeight / linesCount;

        return singleAreaHeight;
    }

    public static List<Vector2> GetCorners(int linesCount, int columnsCount, List<Vector2> targetAreas)
    {
        List<Vector2> areas = new();

        foreach (var targetArea in targetAreas)
            areas.Add(GetCorner(linesCount, columnsCount, targetArea));

        return areas;
    }
}