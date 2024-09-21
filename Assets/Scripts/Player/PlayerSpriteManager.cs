using System.Collections;
using UnityEngine;

public class PlayerSpriteManager : MonoBehaviour
{
    public Sprite[] healthStatesSprites;
    public SpriteRenderer[] playerRenderers;
    public float delayRendererBlinking;
    public ParticleSystem smokeParticles;

    private PlayerDamager playerDamager;
    private Coroutine blinkingCoroutine;

    private Shader defualtShader;
    private bool isInvincible = false;

    private void Start()
    {
        playerRenderers = transform.GetComponentsInChildren<SpriteRenderer>();
        playerDamager = GetComponent<PlayerDamager>();
        playerDamager.onInvicibilityStateChanged += HandleBlinking;
        playerDamager.onHealthModified += UpdateDamageStateSprite;

        var emission = smokeParticles.emission;
        emission.rateOverTime = 0;

        defualtShader = playerRenderers[0].material.shader;
    }

    public void HandleBlinking(bool isInvincible, float duration)
    {
        this.isInvincible = isInvincible;
        if (isInvincible) StartCoroutine(BlinkingCoroutine());
    }

    private IEnumerator BlinkingCoroutine()
    {
        if (!isInvincible)
        {
            ManageSprites(true);
            yield break;
        }

        ManageSprites();
        yield return new WaitForSeconds(delayRendererBlinking);
        StartCoroutine(BlinkingCoroutine());
    }

    private void ManageSprites(bool endTheAnimation = false)
    {
        foreach (var item in playerRenderers)

            item.material.shader = item.material.shader.name.Contains("GUI") || endTheAnimation ? defualtShader : Instances.ShaderGUIMaterial;
        // item.gameObject.SetActive(endTheAnimation || !item.gameObject.activeSelf);
    }

    private void UpdateDamageStateSprite(float health, float previousHealth, float initialHealth)
    {
        float healthQuarter = initialHealth / 4;
        if (health >= 3 * healthQuarter)
        {
            transform.Find("Base").GetComponent<SpriteRenderer>().sprite = healthStatesSprites[0];

        }
        else if (health >= 2 * healthQuarter)
        {
            transform.Find("Base").GetComponent<SpriteRenderer>().sprite = healthStatesSprites[1];
            var emission = smokeParticles.emission;
            emission.rateOverTime = 5;
        }
        else if (health >= healthQuarter)
        {
            transform.Find("Base").GetComponent<SpriteRenderer>().sprite = healthStatesSprites[2];
            var emission = smokeParticles.emission;
            emission.rateOverTime = 50;
        }
        else
        {
            transform.Find("Base").GetComponent<SpriteRenderer>().sprite = healthStatesSprites[3];
            var emission = smokeParticles.emission;
            emission.rateOverTime = 40;
        }
    }
}