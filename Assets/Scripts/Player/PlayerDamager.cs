using System.Collections;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    public float health = 5;
    [HideInInspector] public float initHealth;
    public Sprite[] healthStatesSprites;
    [Space]
    [Header("Invincibility")]
    public float invicibleTime = 2.5f;
    public SpriteRenderer[] playerRenderers;
    public float delayRendererBlinking;


    private bool isInvincible = false;
    private float previousHealth = 0;

    // private GameObject shieldGO;
    // private ShieldState shieldState;
    private SoundPlayer soundPlayer;
    private PlayerShieldManager playerShieldManager;

    public System.Action<float, float, float> onHealthModified; // (currentHealth, previousHealth,initialHealth) 
    public System.Action<bool, float> onInvicibilityStateChanged; // (isInvicible, invincibleTime) 

    private void Start()
    {
        initHealth = health;
        previousHealth = health;

        soundPlayer = GetComponent<SoundPlayer>();
        playerRenderers = transform.GetComponentsInChildren<SpriteRenderer>();
        playerShieldManager = gameObject.AddComponent<PlayerShieldManager>();
    }

    public void TakeDamage(float damage)
    {
        if (playerShieldManager.shieldState == ShieldState.Active)
        {
            soundPlayer.Play("shield-deflect");
            return;
        }

        if (isInvincible) return;
        soundPlayer.PlayStandalone("player-damage");
        StartCoroutine(EnableInvicibility());
        CameraShaker.ShakeGlitching(.2f, .2f);

        EditHealth(-1);

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        Instances.WavesManager.ClearAllEnemies();
        Instances.WavesManager.StopGeneratingWaves();
        Instances.WavesManager.gameObject.SetActive(false);

        Instances.UIGameOver.Display();
        CameraShaker.Glitch();

        var coroutine = Instances.AuthInfo.SaveScoreCoroutine(UIScoreManager.score,
        res =>
        {
            Instances.UIGameOver.SetScoreText(res.score, res.newHighScore);
        },
        err => { Debug.LogError(err.error); });

        Instances.Instance.StartCoroutine(coroutine);
        Destroy(gameObject);
    }

    private IEnumerator EnableInvicibility()
    {
        isInvincible = true;
        onInvicibilityStateChanged?.Invoke(isInvincible, invicibleTime);
        yield return new WaitForSeconds(invicibleTime);
        isInvincible = false;
        onInvicibilityStateChanged?.Invoke(isInvincible, invicibleTime);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // All enemies and obstacles has the EnemyAIBase script
        if (other.TryGetComponent<EnemyAIBase>(out var x))
            TakeDamage(1);
    }

    public void EditInitHealth(int amountToBeAdded)
    {
        initHealth += amountToBeAdded;
        previousHealth = health;
        health += amountToBeAdded;
        onHealthModified?.Invoke(health, previousHealth, initHealth);
    }

    public void EditHealth(int amountToBeAdded)
    {
        previousHealth = health;
        health += amountToBeAdded;
        onHealthModified?.Invoke(health, previousHealth, initHealth);
    }

    public void SetHealth(int current, int init)
    {
        health = current;
        initHealth = init;
        onHealthModified?.Invoke(current, health, init);
    }
}
