using System.Collections;
using UnityEngine;

public enum ShieldState { Charging, Active, Activating, Disabled }

public class PlayerShieldManager : MonoBehaviour
{
    [HideInInspector] public ShieldState shieldState;

    private float invincibleTimeCounter = 0;
    private bool isInvincible = false;

    private GameObject shieldGO;
    private SoundPlayer soundPlayer;

    private float blinkngDelay = 1;

    private void Start()
    {
        soundPlayer = GetComponent<SoundPlayer>();
        shieldGO = transform.Find("Shield").gameObject;

        UIPlayerShield.OnShielStateChanged += OnShielStateChanged;
    }

    private void OnShielStateChanged(ShieldState state)
    {
        if (state == ShieldState.Charging || state == ShieldState.Disabled)
        {
            if (shieldGO)
                shieldGO.SetActive(false);
        }

        if (state == ShieldState.Activating)
        {
            soundPlayer.PlayStandalone("shield-activated");
            StartCoroutine(ShieldActivationCoroutine());
        }

        shieldState = state;
    }

    private IEnumerator ShieldActivationCoroutine()
    {
        shieldGO.SetActive(!shieldGO.activeSelf);
        yield return new WaitForSeconds(blinkngDelay);
        if (shieldState == ShieldState.Activating)
            StartCoroutine(ShieldActivationCoroutine());
        else
        {
            shieldGO.SetActive(true);
        }
    }
}