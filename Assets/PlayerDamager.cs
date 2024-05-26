using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    public float health = 20;
    public float invicibleTime = 2.5f;
    [Space]
    [Header("Invincibility")]
    public GameObject playerRenderer;
    public float delayRendererBlinking;

    private float invincibleTimeCounter = 0;
    private bool isInvincible = false;

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;
        health -= damage;
        EnableInvicibility();
        if (health > 0)
        {
        }
        else
        {
            // Destroy(gameObject);
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
        playerRenderer.SetActive(!playerRenderer.activeSelf);
        yield return new WaitForSeconds(delayRendererBlinking);

        if (Time.time < invincibleTimeCounter)
        {
            StartCoroutine(StartBlinking());
        }
        else
        {
            isInvincible = false;
            playerRenderer.SetActive(true);
        }
    }
}
