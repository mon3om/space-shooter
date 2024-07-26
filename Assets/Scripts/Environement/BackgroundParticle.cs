using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParticle : MonoBehaviour
{
    public GameObject lineHandlerPrefab;
    public bool moveInStraightLines = false;
    public bool preventTriggers = false;
    public int ID;
    [HideInInspector] public List<int> trackedParticles = new();

    private void Start()
    {
        GetComponent<DirectionalMover>().MoveTowardsPoint(transform.position + Vector3.down * 30);
        StartCoroutine(CheckScreenPositionCoroutine());
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out BackgroundParticle backgroundParticle))
        {
            if (preventTriggers)
            {
                if (ID > backgroundParticle.ID && backgroundParticle.preventTriggers && backgroundParticle.gameObject.name == gameObject.name)
                    UpdatePosition();
            }
            else
            {
                if (ID > backgroundParticle.ID && backgroundParticle.gameObject.name == gameObject.name)
                    TrackParticle(backgroundParticle);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out BackgroundParticle backgroundParticle))
        {
            if (ID > backgroundParticle.ID)
                UntrackParticle(backgroundParticle);
        }
    }

    public void TrackParticle(BackgroundParticle particle)
    {
        if (!trackedParticles.Contains(ID))
        {
            Transform child = transform.Find(particle.ID.ToString());
            if (!child)
            {
                child = Instantiate(lineHandlerPrefab, transform).transform;
                child.name = particle.ID.ToString();
            }
            else
            {
                child.gameObject.SetActive(true);
            }

            child.GetComponent<ParticleLineHandler>().isActive = true;
            child.GetComponent<ParticleLineHandler>().trackedTransform = particle.transform;

            trackedParticles.Add(ID);
        }
    }

    public void UntrackParticle(BackgroundParticle particle)
    {
        Transform child = transform.Find(particle.ID.ToString());
        if (child)
            child.GetComponent<ParticleLineHandler>().isActive = false;

        trackedParticles.Remove(ID);
    }

    private IEnumerator CheckScreenPositionCoroutine()
    {
        var pos = transform.position;
        if (pos.x > CameraUtils.CameraRect.xMax || pos.x < CameraUtils.CameraRect.xMin || pos.y < CameraUtils.CameraRect.yMin)
            UpdatePosition();

        yield return new WaitForSeconds(2);
        StartCoroutine(CheckScreenPositionCoroutine());
    }

    private void UpdatePosition()
    {
        transform.position = new Vector3(Random.Range(-CameraUtils.CameraRect.xMax, CameraUtils.CameraRect.xMax), Random.Range(CameraUtils.CameraRect.yMax + 4, CameraUtils.CameraRect.yMax + 10), 0);
        if (moveInStraightLines)
            GetComponent<DirectionalMover>().MoveTowardsPoint(transform.position + Vector3.down * 30);
    }
}