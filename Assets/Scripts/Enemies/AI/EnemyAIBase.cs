using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyDamager))]
public class EnemyAIBase : MonoBehaviour
{
    protected OrientationHandler orientationHandler;
    protected DirectionalMover directionalMover;
    protected EnemyDamager enemyDamager;

    [HideInInspector] public int waveId;
    [HideInInspector] public List<EnemyAIBase> waveSiblings;

    [Header("Death animation")]
    public GameObject deathAnimation;
    public AudioClip deathSound;

    protected Transform player;

    public void Start()
    {
        var playerGo = GameObject.FindGameObjectWithTag(Tags.PLAYER_SHIP);
        if (playerGo)
            player = playerGo.transform;

        directionalMover = GetComponent<DirectionalMover>();
        orientationHandler = GetComponent<OrientationHandler>();
        enemyDamager = GetComponent<EnemyDamager>();
        tag = Tags.ENEMY_SHIP;
    }

    public void InstantiateDeathAnimation(bool playAnimation = true, bool playSound = true)
    {
        GameObject go = null;
        Destroy(go, 5);

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
    }
}