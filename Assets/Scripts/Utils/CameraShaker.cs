using Unity.VisualScripting;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private static float shake = 0;
    public static float shakeAmount = 0.2f;
    private Vector3 initPosition;

    private void Start()
    {
        initPosition = transform.localPosition;
    }

    private void FixedUpdate()
    {
        if (shake > 0)
        {
            var temp = Random.insideUnitSphere * shakeAmount;
            transform.localPosition = new(temp.x, temp.y, initPosition.z);
            shake -= Time.fixedDeltaTime;
        }
        else
        {
            shake = 0.0f;
            transform.localPosition = initPosition;
        }
    }

    public static void Shake(float duration, float shakeAmount)
    {
        if (shake == 0)
        {
            shake = duration;
            CameraShaker.shakeAmount = shakeAmount;
        }
    }
}