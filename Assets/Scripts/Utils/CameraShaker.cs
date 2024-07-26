using Kino;
using Unity.VisualScripting;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private static float shake = 0;
    public static float shakeAmount = 0.2f;
    private Vector3 initPosition;

    private AnalogGlitch analogGlitch;
    private DigitalGlitch digitalGlitch;

    private bool isGlitching = false;
    private float analogGlitchReductionRate;
    private float digitalGlitchReductionRate;

    private static CameraShaker Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        analogGlitch = GetComponent<AnalogGlitch>();
        digitalGlitch = GetComponent<DigitalGlitch>();
        initPosition = transform.localPosition;
    }

    private void FixedUpdate()
    {
        if (shake > 0)
        {
            var temp = Random.insideUnitSphere * shakeAmount;
            transform.localPosition = new(temp.x, temp.y, initPosition.z);
            shake -= Time.fixedDeltaTime;

            if (isGlitching)
            {
                digitalGlitch.intensity -= digitalGlitchReductionRate * Time.deltaTime;
                analogGlitch.colorDrift -= analogGlitchReductionRate * Time.deltaTime;
                analogGlitch.scanLineJitter -= analogGlitchReductionRate * Time.deltaTime;
                analogGlitch.verticalJump -= analogGlitchReductionRate * Time.deltaTime;
                analogGlitch.horizontalShake -= analogGlitchReductionRate * Time.deltaTime;
            }
        }
        else
        {
            shake = 0.0f;
            transform.localPosition = initPosition;

            if (isGlitching)
            {
                digitalGlitch.intensity = 0;
                isGlitching = false;
                analogGlitch.colorDrift = 0;
                analogGlitch.scanLineJitter = 0;
                analogGlitch.verticalJump = 0;
                analogGlitch.horizontalShake = 0;
            }
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

    public static void ShakeGlitching(float duration, float shakeAmount)
    {
        if (shake == 0)
        {
            shake = duration;
            CameraShaker.shakeAmount = shakeAmount;

            Instance.DamageGlitch(duration);
        }
    }

    public void DamageGlitch(float duration)
    {
        isGlitching = true;
        digitalGlitch.intensity = 0.5f;
        digitalGlitchReductionRate = digitalGlitch.intensity / duration;
        analogGlitchReductionRate = 0.1f / duration;

        analogGlitch.colorDrift = 0.1f;
        analogGlitch.scanLineJitter = 0.1f;
        analogGlitch.verticalJump = 0.1f;
        analogGlitch.horizontalShake = 0.1f;
    }

    public void GameOverGlitch()
    {
        digitalGlitch.intensity = 0.3f;
        analogGlitch.colorDrift = 0.25f;
        analogGlitch.scanLineJitter = 0.25f;
        analogGlitch.verticalJump = 0.1f;
        analogGlitch.horizontalShake = 0.1f;

        isGlitching = false;
    }

    public static void Glitch()
    {
        Instance.GameOverGlitch();
    }
}