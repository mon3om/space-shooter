using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetBackground : MonoBehaviour
{
    public int particlesCount = 50;
    public GameObject internetParticle, roadParticle, roadParticle1;
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
            go.GetComponent<BackgroundParticle>().ID = i;
        }
        for (int i = 0; i < particlesCount; i++)
        {
            var pos = new Vector3(Random.Range(-CameraUtils.CameraRect.xMax, CameraUtils.CameraRect.xMax), Random.Range(-CameraUtils.CameraRect.yMax, CameraUtils.CameraRect.yMax), 0);
            var go = Instantiate(roadParticle, pos, Quaternion.identity, transform);
            go.GetComponent<BackgroundParticle>().ID = i;

            float scaleX = Random.Range(0.04f, 0.1f);
            go.transform.localScale = new Vector3(scaleX, scaleX, 1);
            go.GetComponent<OrientationHandler>().StartRotatingInAngle(new Vector3(0, 0, Random.Range(-10f, 10f)));
        }
        for (int i = 0; i < particlesCount; i++)
        {
            var pos = new Vector3(Random.Range(-CameraUtils.CameraRect.xMax, CameraUtils.CameraRect.xMax), Random.Range(-CameraUtils.CameraRect.yMax, CameraUtils.CameraRect.yMax), 0);
            var go = Instantiate(roadParticle1, pos, Quaternion.identity, transform);
            go.GetComponent<BackgroundParticle>().ID = i;

            float scaleX = Random.Range(.1f, .3f);
            go.transform.localScale = new Vector3(scaleX, scaleX * Random.Range(2.5f, 5f), 1);
        }
    }
}
