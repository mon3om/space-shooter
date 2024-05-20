using UnityEngine;

public enum CameraBasedPosition
{
    TopRight, TopLeft, TopCenter, MiddleLeft, MiddleRight, MiddleCenter
}
public class CameraUtils : MonoBehaviour
{
    public static Rect CameraRect;
    public static Vector3 TopRight, TopLeft, TopCenter, MiddleLeft, MiddleRight, MiddleCenter;

    private void Start()
    {
        CalculateScreenBoundaries(Camera.main);

        TopRight = Vector3.up * CameraRect.yMax - Vector3.up + (Vector3.right * CameraRect.xMax - Vector3.right);
        TopLeft = Vector3.up * CameraRect.yMax - Vector3.up + (Vector3.left * CameraRect.xMax + Vector3.right);
        TopCenter = Vector3.up * CameraRect.yMax - Vector3.up;
    }

    private void CalculateScreenBoundaries(Camera cam)
    {
        var bottomLeft = cam.ScreenToWorldPoint(Vector3.zero);
        var topRight = cam.ScreenToWorldPoint(new Vector3(
            cam.pixelWidth, cam.pixelHeight));

        CameraRect = new Rect(
           bottomLeft.x,
           bottomLeft.y,
           topRight.x - bottomLeft.x,
           topRight.y - bottomLeft.y);
    }

    public static Vector3 GetPosition(CameraBasedPosition cameraBasedPosition)
    {
        switch (cameraBasedPosition)
        {
            case CameraBasedPosition.TopLeft:
                return TopLeft;
            case CameraBasedPosition.TopRight:
                return TopRight;
            case CameraBasedPosition.TopCenter:
                return TopCenter;
            case CameraBasedPosition.MiddleCenter:
                return MiddleCenter;
            case CameraBasedPosition.MiddleLeft:
                return MiddleLeft;
            case CameraBasedPosition.MiddleRight:
                return MiddleRight;
            default:
                return default;
        }
    }
}