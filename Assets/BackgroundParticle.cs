using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParticle : MonoBehaviour
{
    private List<BackgroundParticle> connectedParticles = new();
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        GetComponent<DirectionalMover>().MoveInDirection(new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), 0) - transform.position);
    }

    private void FixedUpdate()
    {
        lineRenderer.SetPositions(new Vector3[] { });
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < connectedParticles.Count; i++)
        {
            positions.Add(connectedParticles[i].transform.position);
        }
        lineRenderer.SetPositions(positions.ToArray());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out BackgroundParticle backgroundParticle))
            if (!connectedParticles.Contains(backgroundParticle))
                connectedParticles.Add(backgroundParticle);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out BackgroundParticle backgroundParticle))
            if (connectedParticles.Contains(backgroundParticle))
                connectedParticles.Remove(backgroundParticle);
    }

}