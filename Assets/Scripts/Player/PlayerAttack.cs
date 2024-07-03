using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Public variables
    public ShootingSettings shootingSettings;

    // Private variables
    private bool attackTriggered = false;
    private bool isShooting = false;
    private ShootingBase shootingPlugin;
    private float timeSinceLastBurst = 0;

    private SoundManager soundManager;

    // Components
    private RecoilHandler recoilHandler;

    private void Start()
    {
        recoilHandler = GetComponent<RecoilHandler>();
        soundManager = GetComponent<SoundManager>();
        shootingPlugin = gameObject.AddComponent<ShootingBulletsPlayer>();
        shootingPlugin.shootingSettings = shootingSettings;
    }

    void Update()
    {
        attackTriggered = Input.GetKey(KeyCode.Space);// || Input.GetMouseButton(0);

        // Exit if burst mode and shooting button is held down
        if (shootingPlugin.shootingSettings.shootingMode == ShootingMode.Burst)
        {
            if (!attackTriggered)
                isShooting = false;

            if (isShooting)
                return;
            if (Time.time < timeSinceLastBurst + (1 / shootingPlugin.shootingSettings.fireRate))
                return;
        }

        //
        if (attackTriggered && !isShooting)
        {
            StartCoroutine(ShootingCoroutine());
            isShooting = true;
            timeSinceLastBurst = Time.time;
        }
    }

    private void Shoot()
    {
        if (shootingPlugin != null)
        {
            (shootingPlugin as ShootingBulletsPlayer).FireVariation(gameObject, new(0, 1, 0));
            if (recoilHandler) recoilHandler.PlayRecoilEffect(Vector2.up);
            soundManager.PlaySound();
        }
        else
        {
            throw new Exception("No ShootingBase found");
        }
    }

    private IEnumerator ShootingCoroutine()
    {
        Shoot();

        // Exit if Burst mode
        if (shootingPlugin.shootingSettings.shootingMode == ShootingMode.Burst)
            yield break;

        yield return new WaitForSeconds(1 / shootingPlugin.shootingSettings.fireRate);

        if (attackTriggered)
            StartCoroutine(ShootingCoroutine());
        else
            isShooting = false;
    }

    public void SetBulletsPerShot(int value)
    {
        shootingPlugin.shootingSettings.shotsCount = value;
    }

    public void SetAngleBetweenBullets(int value)
    {
        shootingPlugin.shootingSettings.AngleBetweenShots = value;
    }

    public void SetFireRate(float value)
    {
        shootingPlugin.shootingSettings.fireRate = value;
    }

    public void UpdateShootingPlugin(ShootingBase plugin)
    {
        shootingPlugin = plugin;
    }
}
