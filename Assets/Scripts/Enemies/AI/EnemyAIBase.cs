using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public float damage;

    [Header("Death animation")]
    public GameObject[] deathAnimation;
    public AudioClip deathSound;

    protected Transform player;

    public System.Action<EnemyAIBase, int> onEnemyDestroy; // <this, waveId>

    private List<SpriteRenderer> renderers = new();
    private Shader defaultShader;
    [HideInInspector] public bool killedByPlayer = false;

    public void Start()
    {
        var playerGo = GameObject.FindGameObjectWithTag(Tags.PLAYER_SHIP);
        if (playerGo)
            player = playerGo.transform;

        directionalMover = GetComponent<DirectionalMover>();
        orientationHandler = GetComponent<OrientationHandler>();
        enemyDamager = GetComponent<EnemyDamager>();
        tag = Tags.ENEMY_SHIP;

        renderers = GetComponentsInChildren<SpriteRenderer>().ToList();
        defaultShader = renderers[0].material.shader;
    }

    public void InstantiateDeathAnimation(bool playAnimation = true, bool playSound = true)
    {
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
                foreach (var item in deathAnimation)
                {
                    if (!item) continue;

                    GameObject anim;
                    anim = Instantiate(item, transform.position, item.transform.rotation, Instances.ProjectileHolder);
                    if (anim.TryGetComponent<Animator>(out var animator))
                    {
                        anim.transform.localScale = transform.localScale;
                        anim.transform.rotation = transform.rotation;
                        Destroy(anim, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
                    }
                    else
                    {
                        Destroy(anim, 5);
                    }
                }
            }
        }

        PlayDeathSound(playSound, null);
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
    protected void OnBecameInvisible()
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

    private Vector3 defaultScale;
    public void Blink(bool on, Color color = default)
    {
        if (on)
            foreach (var item in renderers)
            {
                item.material.shader = Instances.ShaderGUIMaterial;
                item.material.color = color;
                item.transform.localScale *= 1.1f;
            }
        if (!on)
            foreach (var item in renderers)
            {
                item.material.shader = defaultShader;
                item.material.color = Color.white;
                item.transform.localScale /= 1.1f;
            }
    }
}