using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wave.Helper
{
    public class PositionManager : MonoBehaviour
    {
        public static void SetPositions(List<Transform> transforms, WaveEnemy waveEnemy)
        {
            WaveShape waveShape = GetWaveShape(waveEnemy.waveShapes);
            WaveEnteringPosition enteringPosition = GetWaveEnteringPositions(waveEnemy.waveEnteringPositions);

            // For setting entering points
            switch (enteringPosition)
            {
                case WaveEnteringPosition.ShouldEnterFromHorizontalCornersBottom:
                    EnteringScreenHelper.EnterFromHorizontalCornersBottom(transforms);
                    break;
                case WaveEnteringPosition.ShouldEnterFromHorizontalCornersCenter:
                    EnteringScreenHelper.EnterFromHorizontalCornersCenter(transforms);
                    break;
                case WaveEnteringPosition.ShouldEnterFromHorizontalCornersTop:
                    EnteringScreenHelper.EnterFromHorizontalCornersTop(transforms);
                    break;
                case WaveEnteringPosition.ShouldEnterFromVerticalCornersLeft:
                    EnteringScreenHelper.EnterFromVerticalCornersLeft(transforms);
                    break;
                case WaveEnteringPosition.ShouldEnterFromVerticalCornersRight:
                    EnteringScreenHelper.EnterFromVerticalCornersRight(transforms);
                    break;
                case WaveEnteringPosition.ShouldEnterFromVerticalCornersMiddle:
                    EnteringScreenHelper.EnterFromVerticalCornersMiddle(transforms);
                    break;
                case WaveEnteringPosition.ShouldEnterFromTop:
                    EnteringScreenHelper.EnterFromTop(transforms, waveEnemy.maxEnemiesPerLine);
                    break;
                case WaveEnteringPosition.ShouldEnterFromBottom:
                    EnteringScreenHelper.EnterFromBottom(transforms, waveEnemy.maxEnemiesPerLine);
                    break;
                case WaveEnteringPosition.ShouldEnterFromLeft:
                    EnteringScreenHelper.EnterFromLeft(transforms, waveEnemy.maxEnemiesPerLine);
                    break;
                case WaveEnteringPosition.ShouldEnterFromRight:
                    EnteringScreenHelper.EnterFromRight(transforms, waveEnemy.maxEnemiesPerLine);
                    break;
                case WaveEnteringPosition.ShouldEnterFromRandom:
                    EnteringScreenHelper.EnterFromRandomTopHalf(transforms);
                    break;
                default:
                    break;
            }

            // For setting wave shapes
            switch (waveShape)
            {
                case WaveShape.InIShape:
                    break;
                case WaveShape.InWaveShape:
                    for (int i = 0; i < transforms.Count; i++)
                    {
                        Transform transform = transforms[i];
                        transform.position = new(transform.position.x, transform.position.y + (i % 2 == 0 ? 2 : 0), 0);
                    }
                    break;
                case WaveShape.InVShape:

                    int j = 0;
                    for (int i = 0; i < transforms.Count; i++)
                    {
                        if (i < transforms.Count / 2f) j = i;
                        else j = transforms.Count + i + 1;

                        Transform transform = transforms[i];
                        transform.position = new(transform.position.x, transform.position.y + j, 0);
                    }
                    break;
                case WaveShape.InRandomYPosition:
                    foreach (var item in transforms)
                        SetRandomYPosition(item, 3, 0);
                    break;
                default:
                    break;
            }

            foreach (var item in transforms)
            {
                if (item.TryGetComponent(out EnemyAIBase enemyAIBase))
                {
                    enemyAIBase.enemyIdentifier.waveShape = waveShape;
                    enemyAIBase.enemyIdentifier.waveEnteringPosition = enteringPosition;
                    enemyAIBase.enemyIdentifier.initPosition = item.position;
                }
            }
        }

        private static WaveShape GetWaveShape(WaveShape[] conditions)
        {

            if (conditions == null || conditions.Length == 0) return WaveShape.InIShape;
            return conditions[Random.Range(0, conditions.Length)];
        }

        private static WaveEnteringPosition GetWaveEnteringPositions(WaveEnteringPosition[] positions)
        {

            if (positions.Length == 0) return WaveEnteringPosition.ShouldEnterFromTop;
            return positions[Random.Range(0, positions.Length)];
        }

        private static void SetRandomYPosition(Transform origin, float range, int iteration)
        {
            if (iteration > 20)
            {
                Destroy(origin.gameObject);
                Debug.Log("Destroyed after 20 failed iterations");
                return;
            }

            origin.position += origin.up * Random.Range(0f, 10f);
            iteration++;

            RaycastHit2D raycastHit2D;
            raycastHit2D = Physics2D.Raycast(origin.position, Vector2.up, range);
            if (raycastHit2D.collider && raycastHit2D.collider.CompareTag(Tags.ENEMY_SHIP))
                SetRandomYPosition(origin, range, iteration);
            raycastHit2D = Physics2D.Raycast(origin.position, Vector2.down, range);
            if (raycastHit2D.collider && raycastHit2D.collider.CompareTag(Tags.ENEMY_SHIP))
                SetRandomYPosition(origin, range, iteration);
        }

        private static void MakeTheWaveCloserToScreen(List<Transform> transforms)
        {
            float closestPoint = transforms[0].position.y;

            for (int i = 0; i < transforms.Count - 1; i++)
            {
                if (transforms[i + 1].position.y < transforms[i].position.y)
                    closestPoint = transforms[i + 1].position.y;
            }

            float targetDistance;
            targetDistance = closestPoint - CameraUtils.CameraRect.yMax;
            foreach (var item in transforms)
            {
                item.position -= Vector3.up * (targetDistance - 1);
            }
        }
    }

}