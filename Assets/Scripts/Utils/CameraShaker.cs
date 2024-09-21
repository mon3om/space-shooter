using System.Collections;
using Kino;
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

    private void Awake()
    {
        Instances.CameraShaker = this;
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

            Instances.CameraShaker.DamageGlitch(duration);
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
        Instances.CameraShaker.GameOverGlitch();
    }

    public void StartBossGlitching(float duration, AudioSource audio)
    {
        gradualTime = 0;
        StopBossGlitching();
        StartCoroutine(GradualGlitchCoroutine(duration, audio));
    }

    float gradualTime = 0;
    float slowingTime = 2f;
    private IEnumerator GradualGlitchCoroutine(float duration, AudioSource audio)
    {
        float step = 0.1f;

        if (gradualTime >= duration - slowingTime * 2)
        {
            step = 0.02f;
            float time = Time.timeScale - (1 / (duration - slowingTime) * step);
            audio.pitch = Time.timeScale = Mathf.Clamp(time, 0.25f, 1);
            Debug.Log("Time.timeScale = " + Time.timeScale);
        }

        yield return new WaitForSecondsRealtime(step);
        gradualTime += step;

        if (gradualTime >= duration)
        {
            StartCoroutine(TimeCoroutine(audio));
            yield break;
        }
        else
        {

            digitalGlitch.intensity += 0.5f / duration * step;

            analogGlitch.colorDrift += 0.1f / duration * step;
            analogGlitch.scanLineJitter += 0.1f / duration * step;
            analogGlitch.verticalJump += 0.1f / duration * step;
            analogGlitch.horizontalShake += 0.1f / duration * step;
        }

        StartCoroutine(GradualGlitchCoroutine(duration, audio));
    }

    public float timeRegainDuration = 0.02f;
    private IEnumerator TimeCoroutine(AudioSource audio)
    {

        StopBossGlitching();
        audio.pitch = Time.timeScale += 0.02f / 3f;

        digitalGlitch.intensity -= 0.5f / 50 / 3;

        analogGlitch.colorDrift -= 0.1f / 50 / 3;
        analogGlitch.scanLineJitter -= 0.1f / 50 / 3;
        analogGlitch.verticalJump -= 0.1f / 50 / 3;
        analogGlitch.horizontalShake -= 0.1f / 50 / 3;

        yield return new WaitForSecondsRealtime(timeRegainDuration);
        if (Time.timeScale >= 1)
        {
            audio.pitch = Time.timeScale = 1;
            StopBossGlitching();
            yield break;
        }
        StartCoroutine(TimeCoroutine(audio));
    }

    public void StopBossGlitching()
    {
        digitalGlitch.intensity = 0;
        analogGlitch.colorDrift = 0;
        analogGlitch.scanLineJitter = 0;
        analogGlitch.verticalJump = 0;
        analogGlitch.horizontalShake = 0;
    }
}