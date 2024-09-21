using System.Collections;
using UnityEngine;

public class HomingMissile : Projectile
{
    private Transform nearestEnemy;
    private ParticleSystem smokeParticles;
    public float rotationSpeedMultiplier = 5;
    private float magicSpeed;

    private bool isDestinationReached = false;

    private static AudioSource audioSource; // This is static as an experiemntal approach to make all instances play a single shot instead of playing launch sound on every instance

    private new void Start()
    {
        base.Start();
        smokeParticles = transform.GetComponentInChildren<ParticleSystem>();

        float baseDistance = 0.25f;
        float distance = baseDistance + Mathf.Abs(id) * 0.25f;

        magicSpeed = directionalMover.movementSpeed * distance / baseDistance;
        magicSpeed /= 30;

        directionalMover.MoveTowardsPoint(transform.position + Vector3.right * Mathf.Sign(id) * distance);
        directionalMover.movementSpeed /= 5;
        directionalMover.movementSpeed *= magicSpeed;
        directionalMover.onDestinationReached.AddListener(OnDestinationReached);
        animator.enabled = false;

        onProjectileDestroy += OnProjectileDestroy;

        Destroy(gameObject, 30);
    }

    private void OnDestinationReached()
    {
        StartCoroutine(HomingCoroutine());
        isDestinationReached = true;
    }

    private void Update()
    {
        if (!isDestinationReached) return;

        directionalMover.MoveInDirection(transform.up);
        orientationHandler.rotationSpeed += rotationSpeedMultiplier * Time.fixedDeltaTime;
    }

    private Transform GetNearestEnemy()
    {
        if (!Instances.WavesManager || Instances.WavesManager.spawnedEnemies.Count == 0)
        {
            orientationHandler.StartRotatingTowardsPoint(new Vector3(Random.Range(-20, 20) * 20, Random.Range(-20, 20) * 20, 0) - transform.position);
            return null;
        }

        var allEnemies = Instances.WavesManager.spawnedEnemies;
        float nearestDist = float.MaxValue;
        Transform nearest = allEnemies[0].transform;
        foreach (var item in allEnemies)
        {
            float dist = Vector2.Distance(transform.position, item.transform.position);
            if (dist < nearestDist)
            {
                nearest = item.transform;
                nearestDist = dist;
            }
        }

        if (nearest.TryGetComponent(out EnemyAIBase enemyAIBase1))
            enemyAIBase1.onEnemyDestroy += OnNearestEnemyDestroyed;

        orientationHandler.StartRotatingTowardsTransform(nearest);

        return nearest;
    }

    private void OnNearestEnemyDestroyed(EnemyAIBase enemyAIBase, int waveId)
    {
        enemyAIBase.onEnemyDestroy -= OnNearestEnemyDestroyed;
        nearestEnemy = GetNearestEnemy();
    }

    private void PreserveSmokeParticle()
    {
        smokeParticles.transform.parent = null;
        smokeParticles.transform.localScale = new(1, 1, 1);
        var em = smokeParticles.emission;
        em.enabled = false;
        Destroy(smokeParticles.gameObject, 5f);
    }

    private IEnumerator HomingCoroutine()
    {
        nearestEnemy = GetNearestEnemy();

        animator.enabled = true;
        var em = smokeParticles.emission;
        em.enabled = true;

        directionalMover.onDestinationReached.RemoveListener(OnDestinationReached);
        directionalMover.MoveInDirection(transform.up);
        directionalMover.movementSpeed /= magicSpeed;
        directionalMover.movementSpeed *= 5;

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(shootingSettings.soundEffect);
        }

        yield return new WaitForSeconds(shootingSettings.soundEffect.length);

        audioSource = null;
    }

    private void OnProjectileDestroy()
    {
        PreserveSmokeParticle();
        audioSource = null;

        if (nearestEnemy && nearestEnemy.TryGetComponent<EnemyAIBase>(out var enemyAIBase))
            enemyAIBase.onEnemyDestroy -= OnNearestEnemyDestroyed;
    }

    private void OnDisable()
    {
        if (nearestEnemy && nearestEnemy.TryGetComponent<EnemyAIBase>(out var enemyAIBase))
            enemyAIBase.onEnemyDestroy -= OnNearestEnemyDestroyed;
    }
}