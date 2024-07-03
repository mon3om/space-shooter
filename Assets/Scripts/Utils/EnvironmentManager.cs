using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private float interval = 30;

    [SerializeField] private List<GameObject> layer1;
    [SerializeField] private List<GameObject> layer2;
    [SerializeField] private List<GameObject> layer3;

    [SerializeField] private RandomFloat layer1Speed;
    [SerializeField] private RandomFloat layer2Speed;
    [SerializeField] private RandomFloat layer3Speed;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        // layer 1

        if (Random.Range(0, 3) == 0)
        {
            var go = Instantiate(layer1[Random.Range(0, layer1.Count)], new(Random.Range(-CameraUtils.CameraRect.xMax, CameraUtils.CameraRect.xMax), CameraUtils.CameraRect.yMax + Random.Range(5, 15)), Quaternion.Euler(0, 0, Random.Range(0, 180)), transform);
            go.GetComponent<EnvironmentObject>().speed = layer1Speed.value;
            go.GetComponent<SpriteRenderer>().sortingOrder = -1;

        }
        yield return new WaitForSeconds(interval);
        StartCoroutine(Spawn());
    }
}
