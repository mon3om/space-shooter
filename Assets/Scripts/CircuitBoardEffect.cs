using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitBoardEffect : MonoBehaviour
{
    public GameObject circuitPointPrefab;
    public float spawnInterval = 0.5f;
    public RandomFloat speed;
    public int maxParticlesCount = 20;
    [Space]
    public Color startColor;
    public Color endColor;
    public List<Vector2> alphas = new();

    private List<CircuitPoint> points = new();

    [Space]
    public bool randomFromPoint = false;
    public Vector2 randomSpawnPoint;
    public Vector2 randomSpawnPointVariation = Vector2.zero;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        if (points.Count < maxParticlesCount)
        {
            var x = Random.Range(CameraUtils.CameraRect.xMin, CameraUtils.CameraRect.xMax);
            var circuitPoint = Instantiate(circuitPointPrefab, new(x, CameraUtils.CameraRect.yMax + 0.2f, 0), Quaternion.identity, transform).GetComponent<CircuitPoint>();
            if (randomFromPoint)
            {
                var randomPoint = new Vector2(Random.Range(-randomSpawnPointVariation.x, randomSpawnPointVariation.x), Random.Range(-randomSpawnPointVariation.y, randomSpawnPointVariation.y));
                circuitPoint.transform.position = randomSpawnPoint + randomPoint;
                circuitPoint.direction = new List<Vector2> { Vector2.down, Vector2.left, Vector2.right, Vector2.up }[Random.Range(0, 4)];
            }
            points.Add(circuitPoint);
            circuitPoint.name = points.Count.ToString();
            circuitPoint.onCircuitPointDestroy += (p) => points.Remove(p);
            circuitPoint.speed = speed;
            circuitPoint.GetComponent<SpriteRenderer>().color = startColor;

            Gradient gradient = new();
            var colors = new GradientColorKey[2];
            colors[0] = new GradientColorKey(startColor, 0);
            colors[1] = new GradientColorKey(endColor, 1);
            var _alphas = new GradientAlphaKey[alphas.Count];
            for (int i = 0; i < alphas.Count; i++)
                _alphas[i] = new GradientAlphaKey(alphas[i].x, alphas[i].y);

            gradient.SetKeys(colors, _alphas);
            circuitPoint.GetComponent<TrailRenderer>().colorGradient = gradient;
        }
        yield return new WaitForSeconds(spawnInterval);
        StartCoroutine(Spawn());
    }
}
