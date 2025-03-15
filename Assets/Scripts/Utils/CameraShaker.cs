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

    private bool isBossGlitching = false;

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
        if (isBossGlitching) return;
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
        isBossGlitching = true;
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
        }

        if (gradualTime >= duration)
        {
            digitalGlitch.intensity = 1;
            StartCoroutine(TimeCoroutine(audio, duration, 0.02f));
            Debug.Log("digitalGlitch.intensity = " + digitalGlitch.intensity);
            yield break;
        }
        else
        {
            digitalGlitch.intensity += 0.1f / duration * step;

            analogGlitch.colorDrift += 0.1f / duration * step;
            analogGlitch.scanLineJitter += 0.1f / duration * step;
            analogGlitch.verticalJump += 0.1f / duration * step;
            analogGlitch.horizontalShake += 0.1f / duration * step;
        }

        yield return new WaitForSecondsRealtime(step);
        gradualTime += step;
        StartCoroutine(GradualGlitchCoroutine(duration, audio));
    }

    private IEnumerator TimeCoroutine(AudioSource audio, float duration, float step)
    {

        audio.pitch = Time.timeScale += step * duration;
        audio.pitch = Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);

        digitalGlitch.intensity -= 0.1f * duration * step;

        analogGlitch.colorDrift -= 0.1f * duration * step;
        analogGlitch.scanLineJitter -= 0.1f * duration * step;
        analogGlitch.verticalJump -= 0.1f * duration * step;
        analogGlitch.horizontalShake -= 0.1f * duration * step;

        digitalGlitch.intensity = Mathf.Clamp(digitalGlitch.intensity, 0, 1);
        analogGlitch.colorDrift = Mathf.Clamp(analogGlitch.colorDrift, 0, 1);
        analogGlitch.scanLineJitter = Mathf.Clamp(analogGlitch.scanLineJitter, 0, 1);
        analogGlitch.verticalJump = Mathf.Clamp(analogGlitch.verticalJump, 0, 1);
        analogGlitch.horizontalShake = Mathf.Clamp(analogGlitch.horizontalShake, 0, 1);

        yield return new WaitForSecondsRealtime(step);
        Debug.Log("digitalGlitch.intensity = " + digitalGlitch.intensity);
        if (digitalGlitch.intensity <= 0)
        {
            Debug.Log("Exiting........................");
            audio.pitch = Time.timeScale = 1;
            StopBossGlitching();
            yield break;
        }
        StartCoroutine(TimeCoroutine(audio, duration, step));
    }

    public void StopBossGlitching()
    {
        digitalGlitch.intensity = 0;
        analogGlitch.colorDrift = 0;
        analogGlitch.scanLineJitter = 0;
        analogGlitch.verticalJump = 0;
        analogGlitch.horizontalShake = 0;

        isBossGlitching = false;
    }
}