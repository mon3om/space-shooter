using System.Collections;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [SerializeField] private float shieldHealth = 20;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioClip destructionSound;
    [SerializeField] private GameObject destructionEffect;

    [Space]
    public GameObject supportShipPrefab;
    public float supportShipInterval = 8;
    [HideInInspector] public GameObject supportShipInstance;

    private float initShieldHelath;
    private float regeneratedAmount;
    private ParticleSystem ps;

    private void Start()
    {
        ps = transform.Find("RegenerationEffect").GetComponent<ParticleSystem>();
        initShieldHelath = shieldHealth;
        regeneratedAmount = initShieldHelath;
        StartCoroutine(SpawnSupportShip());

        GetComponent<EnemyAIBase>().onEnemyDestroy += (v1, v2) =>
        {
            if (supportShipInstance)
                Destroy(supportShipInstance);
        };
    }

    private void Update()
    {
        if (Mathf.Abs(regeneratedAmount - shieldHealth) >= .1f && supportShipInstance != null)
        {
            shieldHealth = Mathf.Lerp(shieldHealth, regeneratedAmount, 0.1f);

            var newColor = new Color(1, 1, 1, Mathf.Clamp(shieldHealth / initShieldHelath, 0.25f, 1f));
            spriteRenderer.color = newColor;

            ps.enableEmission = true;
        }
        else
        {
            regeneratedAmount = shieldHealth;
            ps.enableEmission = false;
        }
    }

    private void OnSupportShipDestroyed(EnemyAIBase enemyAIBase, int waveId)
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(SpawnSupportShip());
    }

    private IEnumerator SpawnSupportShip()
    {
        yield return new WaitForSeconds(supportShipInterval);
        supportShipInstance = Instantiate(supportShipPrefab, Vector3.zero + Vector3.up * (CameraUtils.CameraRect.xMax + 2), Quaternion.identity);
        supportShipInstance.GetComponent<ShieldSupportEnemy>().boss = GetComponent<EnemyAIBase>();
        supportShipInstance.GetComponent<EnemyAIBase>().onEnemyDestroy += OnSupportShipDestroyed;
    }

    public bool TakeDamageIfShieldActive(float amount)
    {
        if (shieldHealth <= 0) return false;

        shieldHealth -= amount;
        regeneratedAmount = shieldHealth;
        spriteRenderer.color = new Color(1, 1, 1, Mathf.Clamp(shieldHealth / initShieldHelath, 0.25f, 1f));
        if (shieldHealth <= 0)
        {
            regeneratedAmount = 0;
            shieldHealth = 0;

            var audio = gameObject.AddComponent<AudioSource>();
            audio.clip = destructionSound;
            audio.Play();

            if (destructionEffect != null)
                Instantiate(destructionEffect, transform.position, Quaternion.identity);

            spriteRenderer.color = new Color(1, 1, 1, 0);
        }

        return true;
    }

    public void Regenerate(float amount)
    {
        regeneratedAmount += amount * initShieldHelath / 100;
        regeneratedAmount = Mathf.Clamp(regeneratedAmount, 0, initShieldHelath);
    }
}