using UnityEngine;

[RequireComponent(typeof(EnemyDamager))]
[RequireComponent(typeof(DirectionalMover))]
[RequireComponent(typeof(OrientationHandler))]
public class EnemyAIBase : MonoBehaviour
{
    protected OrientationHandler orientationHandler;
    protected DirectionalMover directionalMover;
    protected EnemyDamager enemyDamager;

    protected WaverBase waverBase;

    [Header("Death animation")]
    public GameObject deathAnimation;
    public AudioClip deathSound;

    public void Start()
    {
        directionalMover = GetComponent<DirectionalMover>();
        orientationHandler = GetComponent<OrientationHandler>();
        enemyDamager = GetComponent<EnemyDamager>();
        tag = Tags.ENEMY_SHIP;
    }

    public void InstantiateDeathAnimation(bool playAnimation = true, bool playSound = true)
    {
        GameObject go = null;

        // Death animation
        if (deathAnimation == null)
        {
            Debug.LogWarning(GetType().ToString() + " has no death animation");
            return;
        }
        else
        {
            if (playAnimation)
                go = Instantiate(deathAnimation, transform.position, Quaternion.identity);
            else
                go = new GameObject("DeathAnimation");
        }

        // DeathSound
        if (deathSound == null)
        {
            Debug.LogWarning(GetType().ToString() + " has no death sound");
            return;
        }
        else
        {
            if (playSound)
            {
                go.PlaySound(deathSound);
            }
        }

        if (go != null)
            Destroy(go, 5);
    }
}