using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    public float health = 20;
    public Sprite[] heathStatesSprites;
    [Space]
    [Header("Invincibility")]
    public float invicibleTime = 2.5f;
    public SpriteRenderer[] playerRenderers;
    public float delayRendererBlinking;

    private float invincibleTimeCounter = 0;
    private bool isInvincible = false;

    private float initHealth;

    private GameObject shieldGO;
    private ShieldState shieldState;
    private SoundPlayer soundPlayer;

    public System.Action<DamageData> onDamageTaken; // (damage, currentHealth, initialHealth) 

    private void Start()
    {
        initHealth = health;

        soundPlayer = GetComponent<SoundPlayer>();
        playerRenderers = transform.GetComponentsInChildren<SpriteRenderer>();
        shieldGO = transform.Find("Shield").gameObject;
        UIPlayerShield.OnShielStateChanged += (state) =>
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
        };
    }

    public void TakeDamage(float damage)
    {
        if (shieldState == ShieldState.Active)
        {
            soundPlayer.Play("shield-deflect");
            return;
        }

        if (isInvincible) return;

        soundPlayer.PlayStandalone("player-damage");

        health -= damage;

        EnableInvicibility();
        onDamageTaken?.Invoke(new(damage, health, initHealth));
        CameraShaker.Shake(.2f, .2f);
        UpdateDamageStateSprite();

        if (health > 0)
        {

        }
        else
        {
            GameObject.FindWithTag(Tags.UI_GAMEOVER).transform.GetChild(0).gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

    private void EnableInvicibility()
    {
        isInvincible = true;
        invincibleTimeCounter = Time.time + invicibleTime;
        StartCoroutine(StartBlinking());
    }

    private IEnumerator StartBlinking()
    {
        HandleBlinking();
        yield return new WaitForSeconds(delayRendererBlinking);

        if (Time.time < invincibleTimeCounter)
        {
            StartCoroutine(StartBlinking());
        }
        else
        {
            isInvincible = false;
            HandleBlinking(true);
        }
    }

    private void HandleBlinking(bool endTheAnimation = false)
    {
        foreach (var item in playerRenderers)
        {
            item.gameObject.SetActive(endTheAnimation || !item.gameObject.activeSelf);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // All enemies and obstacles has the EnemyAIBase script
        if (other.TryGetComponent<EnemyAIBase>(out var x))
            TakeDamage(1);
    }

    private void UpdateDamageStateSprite()
    {
        float healthQuarter = initHealth / 4;
        if (health >= 3 * healthQuarter)
            transform.Find("Base").GetComponent<SpriteRenderer>().sprite = heathStatesSprites[0];
        else if (health >= 2 * healthQuarter)
            transform.Find("Base").GetComponent<SpriteRenderer>().sprite = heathStatesSprites[1];
        else if (health >= healthQuarter)
            transform.Find("Base").GetComponent<SpriteRenderer>().sprite = heathStatesSprites[2];
        else
            transform.Find("Base").GetComponent<SpriteRenderer>().sprite = heathStatesSprites[3];
    }

    private IEnumerator ShieldActivationCoroutine()
    {
        shieldGO.SetActive(!shieldGO.activeSelf);
        yield return new WaitForSeconds(delayRendererBlinking);
        if (shieldState == ShieldState.Activating)
            StartCoroutine(ShieldActivationCoroutine());
        else
        {
            shieldGO.SetActive(true);
        }
    }
}
