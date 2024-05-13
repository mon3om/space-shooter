using UnityEngine;

[RequireComponent(typeof(EnemyDamager))]
[RequireComponent(typeof(DirectionalMover))]
[RequireComponent(typeof(OrientationHandler))]
public class EnemyAIBase : MonoBehaviour
{
    protected OrientationHandler orientationHandler;
    protected DirectionalMover directionalMover;
    protected EnemyDamager enemyDamager;

    public void Start()
    {
        directionalMover = GetComponent<DirectionalMover>();
        orientationHandler = GetComponent<OrientationHandler>();
        enemyDamager = GetComponent<EnemyDamager>();
    }
}