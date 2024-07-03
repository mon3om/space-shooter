using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetBackground : MonoBehaviour
{
    public int particlesCount = 50;
    public GameObject internetParticle;
    private List<Transform> particles = new();


    void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        for (int i = 0; i < particlesCount; i++)
        {
            var pos = new Vector3(Random.Range(-CameraUtils.CameraRect.xMax, CameraUtils.CameraRect.xMax), Random.Range(-CameraUtils.CameraRect.yMax, CameraUtils.CameraRect.yMax), 0);
            var go = Instantiate(internetParticle, pos, Quaternion.identity, transform);
            particles.Add(go.transform);
        }
    }
}
