using System.Collections;
using UnityEngine;

public class CircuitPoint : MonoBehaviour
{
    public RandomFloat length;
    [HideInInspector] public RandomFloat speed;
    [HideInInspector] public Vector2 direction = Vector2.up;
    [Space]
    public int maxIterations = 5;
    public float angleVariation;

    private int activePointIndex = 1;
    private float traveledDistance = 0;
    private bool isDestroying = false;
    private Vector2 previousDirection;

    public System.Action<CircuitPoint> onCircuitPointDestroy;

    void FixedUpdate()
    {
        if (activePointIndex >= maxIterations && !isDestroying)
        {
            isDestroying = true;
            StartCoroutine(DestroyCoroutine());
            return;
        }

        float distance = speed.FixedValue * Time.deltaTime;
        transform.position -= (Vector3)direction.normalized * distance;
        traveledDistance += distance;

        if (traveledDistance >= length.FixedValue)
        {
            activePointIndex++;
            length.NewFixedValue();
            speed.NewFixedValue();
            traveledDistance = 0;
            if (Mathf.Abs(direction.y) == 1 && direction.x == 0)
            {
                previousDirection = direction;
                direction = new(Random.Range(0, 2) == 0 ? angleVariation : -angleVariation, direction.y);
            }
            else if (Mathf.Abs(direction.x) == 1 && direction.y == 0)
            {
                previousDirection = direction;
                direction = new(direction.x, Random.Range(0, 2) == 0 ? angleVariation : -angleVariation);
            }
            else
            {
                direction = previousDirection;
            }
        }
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(5);

        onCircuitPointDestroy?.Invoke(this);
        Destroy(gameObject);
    }
}
