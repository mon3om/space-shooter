using UnityEngine;

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
}