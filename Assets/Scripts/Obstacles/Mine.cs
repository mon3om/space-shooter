using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [HideInInspector] public Vector2 destination;
    [SerializeField] private AudioClip audioClip;

    private DirectionalMover directionalMover;
    private OrientationHandler orientationHandler;
    private SoundPlayer soundPlayer;


    void Start()
    {
        GetComponent<Collider2D>().enabled = false;
        soundPlayer = GetComponent<SoundPlayer>();
        directionalMover = GetComponent<DirectionalMover>();
        orientationHandler = GetComponent<OrientationHandler>();
        directionalMover.MoveTowardsPoint(destination);
        directionalMover.onDestinationReached.AddListener(() =>
        {
            directionalMover.StopMoving();
            GetComponent<Collider2D>().enabled = true;
        });
        orientationHandler.StartRotatingInAngle(Vector3.forward);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.PLAYER_SHIP))
        {
            soundPlayer.PlayStandalone(audioClip);
            other.GetComponent<PlayerMovement>().SlowDown();
            Destroy(gameObject);
        }

        if (other.CompareTag(Tags.PLAYER_BULLET))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
