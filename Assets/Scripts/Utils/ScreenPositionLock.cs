using UnityEngine;

public class ScreenPositionLock : MonoBehaviour
{
    public bool preventLeavingScreen = false;
    public float tempOffset = 1.5f;

    public System.Action<Vector2, Vector2> onScreenSideReached; //<currentPosition, screenSideExpressedInVectors>

    private readonly float eventOffsetMargin = 0.1f;
    private bool willEmitEvent = false;
    private Vector2 screenSide; // The side reached expressed in Vectors

    private void Start()
    {
        if (TryGetComponent(out DirectionalMover directionalMover))
        {
            directionalMover.onDestinationReached.AddListener(() =>
            {
                preventLeavingScreen = true;
            });
        }
    }

    private void LateUpdate()
    {
        if (transform.position.x > CameraUtils.CameraRect.xMax - tempOffset - eventOffsetMargin) // right
        {
            screenSide = Vector2.right;
            willEmitEvent = true;
        }
        else if (transform.position.x < CameraUtils.CameraRect.xMin + tempOffset + eventOffsetMargin) // left
        {
            screenSide = Vector2.left;
            willEmitEvent = true;
        }
        else if (transform.position.y > CameraUtils.CameraRect.yMax - tempOffset - eventOffsetMargin)
        {
            screenSide = Vector2.up;
            willEmitEvent = true;
        }
        else if (transform.position.y < CameraUtils.CameraRect.yMin + tempOffset + eventOffsetMargin)
        {
            screenSide = Vector2.down;
            willEmitEvent = true;
        }

        if (willEmitEvent)
        {
            onScreenSideReached?.Invoke(transform.position, screenSide);
            willEmitEvent = false;
        }

        if (preventLeavingScreen)
        {
            var posLock = new Vector3(
               Mathf.Clamp(transform.position.x, CameraUtils.CameraRect.xMin + tempOffset, CameraUtils.CameraRect.xMax - tempOffset),
               Mathf.Clamp(transform.position.y, CameraUtils.CameraRect.yMin + tempOffset, CameraUtils.CameraRect.yMax - tempOffset),
               transform.position.z);
            transform.position = Vector3.Lerp(transform.position, posLock, 0.1f);
        }
    }
}