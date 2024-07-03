using System.Collections.Generic;
using UnityEngine;

public class EnteringScreenHelper
{
    #region EnterFromHorizontal
    public static void EnterFromHorizontalCornersBottom(List<Transform> transforms)
    {
        int linesCount = transforms.Count / 2 + (transforms.Count / 2 % 2 == 0 ? 3 : 2);
        int columnsCount = 6;
        List<Vector2> cornersIndexes = new();
        int y = 0;
        for (int i = 0; i < transforms.Count / 2f; i++)
        {
            cornersIndexes.Add(new(0, linesCount - 1 - y));
            cornersIndexes.Add(new(columnsCount - 1, linesCount - 1 - y));
            y++;
        }
        var positions = CornersPositionHelper.GetCorners(linesCount, columnsCount, cornersIndexes);
        var areaWidth = CornersPositionHelper.GetAreaWidth(columnsCount);
        for (int i = 0; i < transforms.Count; i++)
        {
            transforms[i].position = new(positions[i].x + (positions[i].x > 0 ? areaWidth + 1 : -areaWidth - 1), positions[i].y, transforms[i].position.z);
            transforms[i].GetComponent<EnemyAIBase>().enteringTargetPosition = positions[i];
        }
    }

    public static void EnterFromHorizontalCornersCenter(List<Transform> transforms)
    {
        int linesCount = transforms.Count / 2 + (transforms.Count / 2 % 2 == 0 ? 3 : 2);
        int columnsCount = 6;
        List<Vector2> cornersIndexes = new();
        int y = 0;
        for (int i = 0; i < transforms.Count / 2f; i++)
        {
            int variableIndexValue = Mathf.FloorToInt(linesCount / 2f) + (i % 2 == 0 ? -y : y);
            cornersIndexes.Add(new(0, variableIndexValue));
            cornersIndexes.Add(new(columnsCount - 1, variableIndexValue));
            if (i % 2 == 0) y++;
        }
        var positions = CornersPositionHelper.GetCorners(linesCount, columnsCount, cornersIndexes);
        float singleAreaHeight = CornersPositionHelper.GetAreaHeight(linesCount);
        var areaWidth = CornersPositionHelper.GetAreaWidth(columnsCount);
        for (int i = 0; i < transforms.Count; i++)
        {
            // When the sum of transforms is even and their half is also even, positions won't be centered
            // So we add half of a single area height to each one of them
            if (transforms.Count % 2 == 0 && transforms.Count / 2 % 2 == 0)
                positions[i] += Vector2.up * singleAreaHeight / 2;

            transforms[i].position = new(positions[i].x + (positions[i].x > 0 ? areaWidth + 1 : -areaWidth - 1), positions[i].y, transforms[i].position.z);
            transforms[i].GetComponent<EnemyAIBase>().enteringTargetPosition = positions[i];
        }

    }

    public static void EnterFromHorizontalCornersTop(List<Transform> transforms)
    {
        int linesCount = transforms.Count / 2 + (transforms.Count / 2 % 2 == 0 ? 3 : 2);
        int columnsCount = 6;
        List<Vector2> cornersIndexes = new();
        int y = 0;
        for (int i = 0; i < transforms.Count / 2f; i++)
        {
            cornersIndexes.Add(new(0, y));
            cornersIndexes.Add(new(columnsCount - 1, y));
            y++;
        }
        var positions = CornersPositionHelper.GetCorners(linesCount, columnsCount, cornersIndexes);
        var areaWidth = CornersPositionHelper.GetAreaWidth(columnsCount);
        for (int i = 0; i < transforms.Count; i++)
        {
            transforms[i].position = new(positions[i].x + (positions[i].x > 0 ? areaWidth + 1 : -areaWidth - 1), positions[i].y, transforms[i].position.z);
            transforms[i].GetComponent<EnemyAIBase>().enteringTargetPosition = positions[i];
        }
    }
    #endregion

    #region EnterFromVertical
    public static void EnterFromVerticalCornersLeft(List<Transform> transforms)
    {
        int columnsCount = transforms.Count / 2 + (transforms.Count / 2 % 2 == 0 ? 5 : 4);
        int linesCount = 4;
        List<Vector2> cornersIndexes = new();
        int x = 0;
        for (int i = 0; i < transforms.Count / 2f; i++)
        {
            cornersIndexes.Add(new(x, 0));
            cornersIndexes.Add(new(x, linesCount - 1));
            x++;
        }
        var positions = CornersPositionHelper.GetCorners(linesCount, columnsCount, cornersIndexes);
        var areaHeight = CornersPositionHelper.GetAreaHeight(linesCount);
        for (int i = 0; i < transforms.Count; i++)
        {
            transforms[i].position = new(positions[i].x, positions[i].y + (positions[i].y > 0 ? areaHeight + 1 : -areaHeight - 1), transforms[i].position.z);
            transforms[i].GetComponent<EnemyAIBase>().enteringTargetPosition = positions[i];
        }
    }

    public static void EnterFromVerticalCornersRight(List<Transform> transforms)
    {
        int columnsCount = transforms.Count / 2 + (transforms.Count / 2 % 2 == 0 ? 5 : 4);
        int linesCount = 4;
        List<Vector2> cornersIndexes = new();
        int x = 0;
        for (int i = 0; i < transforms.Count / 2f; i++)
        {
            cornersIndexes.Add(new(columnsCount - 1 - x, 0));
            cornersIndexes.Add(new(columnsCount - 1 - x, linesCount - 1));
            x++;
        }
        var positions = CornersPositionHelper.GetCorners(linesCount, columnsCount, cornersIndexes);
        var areaHeight = CornersPositionHelper.GetAreaHeight(linesCount);
        for (int i = 0; i < transforms.Count; i++)
        {
            transforms[i].position = new(positions[i].x, positions[i].y + (positions[i].y > 0 ? areaHeight + 1 : -areaHeight - 1), transforms[i].position.z);
            transforms[i].GetComponent<EnemyAIBase>().enteringTargetPosition = positions[i];
        }
    }

    public static void EnterFromVerticalCornersMiddle(List<Transform> transforms)
    {
        int columnsCount = transforms.Count / 2 + (transforms.Count / 2 % 2 == 0 ? 5 : 4);
        int linesCount = 4;
        List<Vector2> cornersIndexes = new();
        int x = 0;
        for (int i = 0; i < transforms.Count / 2f; i++)
        {
            int variableIndexValue = Mathf.FloorToInt(columnsCount / 2f) + (i % 2 == 0 ? -x : x);
            cornersIndexes.Add(new(variableIndexValue, 0));
            cornersIndexes.Add(new(variableIndexValue, linesCount - 1));
            if (i % 2 == 0) x++;
        }
        var positions = CornersPositionHelper.GetCorners(linesCount, columnsCount, cornersIndexes);
        float singleAreaWidth = CornersPositionHelper.GetAreaWidth(columnsCount);
        var areaHeight = CornersPositionHelper.GetAreaHeight(linesCount);
        for (int i = 0; i < transforms.Count; i++)
        {
            // When the sum of transforms is even and their half is also even, positions won't be centered
            // So we add half of a single area height to each one of them
            if (transforms.Count % 2 == 0 && transforms.Count / 2 % 2 == 0)
                positions[i] += Vector2.left * singleAreaWidth / 2;

            transforms[i].position = new(positions[i].x, positions[i].y + (positions[i].y > 0 ? areaHeight + 1 : -areaHeight - 1), transforms[i].position.z);
            transforms[i].GetComponent<EnemyAIBase>().enteringTargetPosition = positions[i];
        }
    }
    #endregion

    #region EnterFromSides
    public static void EnterFromTop(List<Transform> transforms, int maxEnemiesPerLine)
    {
        if (maxEnemiesPerLine == -1) maxEnemiesPerLine = transforms.Count;

        int columnsCount = maxEnemiesPerLine;

        List<Vector2> cornersIndexes = new();
        for (int i = 0; i < columnsCount; i++)
            cornersIndexes.Add(new(i, 0));

        var positions = CornersPositionHelper.GetCorners(maxEnemiesPerLine, columnsCount, cornersIndexes);

        int processedCounter = 0;
        var areaHeight = CornersPositionHelper.GetAreaHeight(maxEnemiesPerLine);
        for (int currentLine = 0; currentLine < columnsCount; currentLine++)
            for (int i = 0; i < maxEnemiesPerLine; i++)
            {
                Vector2 position = positions[i];
                transforms[processedCounter].position = position + Vector2.up * (areaHeight + 1 + currentLine * 2);
                transforms[processedCounter].GetComponent<EnemyAIBase>().enteringTargetPosition = position;

                processedCounter++;
                if (processedCounter == transforms.Count) return;
            }
    }

    public static void EnterFromBottom(List<Transform> transforms, int maxEnemiesPerLine)
    {
        if (maxEnemiesPerLine == -1) maxEnemiesPerLine = transforms.Count;

        int columnsCount = maxEnemiesPerLine;

        List<Vector2> cornersIndexes = new();
        for (int i = 0; i < columnsCount; i++)
            cornersIndexes.Add(new(i, maxEnemiesPerLine - 1));

        var positions = CornersPositionHelper.GetCorners(maxEnemiesPerLine, maxEnemiesPerLine, cornersIndexes);

        int processedCounter = 0;
        var areaHeight = CornersPositionHelper.GetAreaHeight(maxEnemiesPerLine);
        for (int currentLine = 0; currentLine < columnsCount; currentLine++)
            for (int i = 0; i < maxEnemiesPerLine; i++)
            {
                Vector2 position = positions[i];
                transforms[processedCounter].position = position + Vector2.down * (areaHeight + 1 + currentLine * 2);
                transforms[processedCounter].GetComponent<EnemyAIBase>().enteringTargetPosition = position;

                processedCounter++;
                if (processedCounter == transforms.Count) return;
            }
    }

    public static void EnterFromRight(List<Transform> transforms, int maxEnemiesPerColumn)
    {
        if (maxEnemiesPerColumn == -1) maxEnemiesPerColumn = transforms.Count;

        int columnsCount = Mathf.CeilToInt(transforms.Count / (float)maxEnemiesPerColumn);
        int linesCount = maxEnemiesPerColumn;

        List<Vector2> cornersIndexes = new();
        for (int i = 0; i < linesCount; i++)
            cornersIndexes.Add(new(maxEnemiesPerColumn - 1, i));

        var positions = CornersPositionHelper.GetCorners(linesCount, maxEnemiesPerColumn, cornersIndexes);

        int processedCounter = 0;
        var areaWidth = CornersPositionHelper.GetAreaWidth(linesCount);
        for (int currentLine = 0; currentLine < columnsCount; currentLine++)
            for (int i = 0; i < maxEnemiesPerColumn; i++)
            {
                Vector2 position = positions[i];
                transforms[processedCounter].position = position + Vector2.right * (areaWidth + 1 + currentLine * 2);
                transforms[processedCounter].GetComponent<EnemyAIBase>().enteringTargetPosition = position;

                processedCounter++;
                if (processedCounter == transforms.Count) return;
            }
    }

    public static void EnterFromLeft(List<Transform> transforms, int maxEnemiesPerColumn)
    {
        if (maxEnemiesPerColumn == -1) maxEnemiesPerColumn = transforms.Count;

        int columnsCount = Mathf.CeilToInt(transforms.Count / (float)maxEnemiesPerColumn);
        int linesCount = maxEnemiesPerColumn;

        List<Vector2> cornersIndexes = new();
        for (int i = 0; i < linesCount; i++)
            cornersIndexes.Add(new(0, i));

        var positions = CornersPositionHelper.GetCorners(linesCount, maxEnemiesPerColumn, cornersIndexes);

        int processedCounter = 0;
        var areaWidth = CornersPositionHelper.GetAreaWidth(linesCount);
        for (int currentLine = 0; currentLine < columnsCount; currentLine++)
            for (int i = 0; i < maxEnemiesPerColumn; i++)
            {
                Vector2 position = positions[i];
                transforms[processedCounter].position = position + Vector2.left * (areaWidth + 1 + currentLine * 2);
                transforms[processedCounter].GetComponent<EnemyAIBase>().enteringTargetPosition = position;

                processedCounter++;
                if (processedCounter == transforms.Count) return;
            }
    }

    public static void EnterFromRandomTopHalf(List<Transform> transforms)
    {
        foreach (var item in transforms)
        {
            Vector2 position = new(Random.Range(CameraUtils.CameraRect.xMin, CameraUtils.CameraRect.xMax), Random.Range(0, CameraUtils.CameraRect.yMax));
            Vector2 targetPosition = new(Random.Range(CameraUtils.CameraRect.xMin + 2, CameraUtils.CameraRect.xMax - 2), CameraUtils.CameraRect.yMin);

            position.x += position.x > 0 ? (CameraUtils.CameraRect.xMax - position.x + 3) : (CameraUtils.CameraRect.xMin - position.x - 3);
            position.y += CameraUtils.CameraRect.yMax - position.y + 3;

            item.position = position;
            item.GetComponent<EnemyAIBase>().enteringTargetPosition = targetPosition;
        }
    }

    #endregion
}