using UnityEngine;

public class LaserWave : MonoBehaviour
{
    public GameObject laserSource, laserDeflector;

    private void LaunchSimpleWave()
    {
        var s1Position = CameraUtils.TopRight;
        var s2Position = CameraUtils.BottomRight;
        var source1 = Instantiate(laserSource, s1Position + Vector3.right * 3, Quaternion.identity).GetComponent<EnemyAIBase>();
        // var source2 = Instantiate(laserSource, s2Position + Vector3.right * 3, Quaternion.identity).GetComponent<EnemyAIBase>();
        source1.enteringTargetPosition = s1Position;
        // source2.enteringTargetPosition = s2Position;

        var deflector1 = Instantiate(laserDeflector, CameraUtils.TopCenter + Vector3.up * 3, Quaternion.identity);
        var deflector2 = Instantiate(laserDeflector, CameraUtils.TopLeft + Vector3.up * 3, Quaternion.identity);
        var deflector3 = Instantiate(laserDeflector, CameraUtils.MiddleLeft + Vector3.up * 3, Quaternion.identity);
        var deflector4 = Instantiate(laserDeflector, CameraUtils.MiddleCenter + Vector3.up * 3, Quaternion.identity);
    }
}