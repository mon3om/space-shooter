using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyDamager))]
public class EnemyAIBase : MonoBehaviour
{
    protected OrientationHandler orientationHandler;
    protected DirectionalMover directionalMover;
    protected EnemyDamager enemyDamager;
    [HideInInspector] public Vector2 enteringTargetPosition;

    [HideInInspector] public bool isNowStatic = false;
    [HideInInspector] public List<EnemyAIBase> waveSiblings;
    public EnemyIdentifier enemyIdentifier;

    [Header("Death animation")]
    public GameObject deathAnimation;
    public AudioClip deathSound;

    protected Transform player;

    public System.Action<EnemyAIBase, int> onEnemyDestroy; // <this, waveId>

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

        CameraShaker.Shake(0.15f, 0.15f);

        // No death animation
        if (deathAnimation == null)
        {
            Debug.LogWarning(GetType().ToString() + " has no death animation");
        }
        // Death animation
        else
        {
            if (playAnimation)
            {
                go = Instantiate(deathAnimation, transform.position, transform.rotation);
                go.transform.localScale = transform.localScale;
                Destroy(go, go.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
            }
            else
            {
                go = new GameObject("DeathAnimation");
                Destroy(go, 5);
            }
        }

        PlayDeathSound(playSound, go);
    }

    private void PlayDeathSound(bool playSound, GameObject soundGameObject)
    {
        if (deathSound == null)
        {
            Debug.LogWarning(GetType().ToString() + " has no death sound");
            return;
        }
        else
        {
            if (playSound)
            {
                if (soundGameObject == null)
                {
                    soundGameObject = new GameObject("TempSoundPlayer");
                    Destroy(soundGameObject, deathSound.length);
                }
                soundGameObject.PlaySound(deathSound);
            }
        }
    }

    public void DestroyEnemy(bool withEventBroadcasting = true)
    {
        if (withEventBroadcasting)
            onEnemyDestroy?.Invoke(this, enemyIdentifier.waveId);

        Destroy(gameObject);
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(1);
        DestroyEnemy();
    }

    private Coroutine _destroyCoroutine = null;
    private void OnBecameInvisible()
    {
        if (gameObject.activeInHierarchy && _destroyCoroutine == null)
            _destroyCoroutine = StartCoroutine(DestroyCoroutine());
    }
    private void OnBecameVisible()
    {
        if (_destroyCoroutine == null) return;

        StopCoroutine(_destroyCoroutine);
        _destroyCoroutine = null;
    }

    public int GetId()
    {
        int firstIndex = name.IndexOf("id=") + 3;
        int length = name.IndexOf("-waveId") - firstIndex;
        try
        {
            if (int.TryParse(name.Substring(firstIndex, length), out int id))
            {
                return id;
            }
        }
        catch (System.Exception)
        {
            return 0;
        }

        throw new System.Exception("Name doesn't contain an ID");
    }

    protected bool CheckIfVerticalPositionOccupied()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, (enteringTargetPosition - (Vector2)transform.position).normalized, 50);
        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent(out EnemyAIBase enemyAIBase))
                if (enemyAIBase.isNowStatic)
                {
                    return true;
                }
        }

        return false;
    }

    public virtual void SetDifficultyLevel(int level)
    {
        throw new System.NotImplementedException();
    }
}