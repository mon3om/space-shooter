using UnityEngine;

public class RecoilHandler : MonoBehaviour
{
    public Transform objectInAction;
    public float maxRecoilDistanceVariation = 0.2f;
    public float maxRecoilTime = 0.4f;

    private ShootingBase shootingPlugin;
    private bool recoilEffectStarted = false;
    private bool processRunning = false;

    private Vector3 from, to, initPosition;

    private void Start()
    {
        shootingPlugin = GetComponent<ShootingBase>();
        initPosition = objectInAction.localPosition;
    }

    private void FixedUpdate()
    {
        RecoilEffect();
    }

    public void PlayRecoilEffect(Vector2 direction)
    {
        recoilEffectStarted = true;
        from = initPosition;
        to = (direction - (Vector2)initPosition).normalized;
        processRunning = true;
    }

    private void RecoilEffect()
    {
        if (!processRunning) return;
        if (shootingPlugin == null)
        {
            if (!TryGetComponent<ShootingBase>(out shootingPlugin)) return;
        }

        maxRecoilTime = Mathf.Clamp(maxRecoilTime, 0.8f * 1 / shootingPlugin.shootingSettings.fireRate, 0.4f);
        // recoil effect
        if (recoilEffectStarted)
        {
            float traveledDistance = Vector2.Distance(objectInAction.localPosition, initPosition);
            if (traveledDistance < maxRecoilDistanceVariation)
            {
                float speed = maxRecoilDistanceVariation / maxRecoilTime / 2;
                objectInAction.localPosition += (to - from).normalized * maxRecoilDistanceVariation * speed;
            }
            else
            {
                recoilEffectStarted = false;
            }
        }

        // reset and end the effect
        if (!recoilEffectStarted)
        {
            float traveledDistance = Vector2.Distance(objectInAction.localPosition, initPosition);
            if (traveledDistance > 0.05f)
            {
                float speed = maxRecoilDistanceVariation / maxRecoilTime / 2;
                objectInAction.localPosition += (from - to).normalized * maxRecoilDistanceVariation * speed;
            }
            else
            {
                objectInAction.localPosition = initPosition;
                processRunning = false;
            }
        }
    }
}