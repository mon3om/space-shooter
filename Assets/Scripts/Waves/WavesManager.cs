using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaveType
{
    PeacefulVShape, PeacefulLShape, PeacefulWaveShape, PeacefulInversedVShape
}

public class WavesManager : MonoBehaviour
{
    public GameObject tempEnemy;

    void Start()
    {
        Instantiate(tempEnemy, new(0, 10, 0), Quaternion.identity).GetComponent<ShootingAssaultEnemy>().destination = CameraUtils.TopRight;
        Instantiate(tempEnemy, new(0, 10, 0), Quaternion.identity).GetComponent<ShootingAssaultEnemy>().destination = CameraUtils.TopLeft;
    }

}
