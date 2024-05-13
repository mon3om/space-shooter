using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITemp : MonoBehaviour
{
    public Slider fireRateSlider, bulletsSlider, angleSlider;
    public TMPro.TMP_Text fireRateText, bulletsText, angleText;
    public PlayerAttack playerAttack;

    private void Start()
    {
        fireRateSlider.onValueChanged.AddListener(OnRateChange);
        bulletsSlider.onValueChanged.AddListener(OnBulletsChange);
        angleSlider.onValueChanged.AddListener(OnAngleChange);

        fireRateText.text = "Fire rate = " + fireRateSlider.value;
        playerAttack.SetFireRate(fireRateSlider.value);

        bulletsText.text = "Bullets per shot = " + bulletsSlider.value;
        playerAttack.SetBulletsPerShot((int)bulletsSlider.value);

        angleText.text = "Angle between bullets = " + angleSlider.value;
        playerAttack.SetAngleBetweenBullets((int)angleSlider.value);
    }

    public void OnRateChange(float value)
    {
        fireRateText.text = "Fire rate = " + value;
        playerAttack.SetFireRate(value);
    }
    public void OnBulletsChange(float value)
    {
        bulletsText.text = "Bullets per shot = " + value;
        playerAttack.SetBulletsPerShot((int)value);
    }
    public void OnAngleChange(float value)
    {
        angleText.text = "Angle between bullets = " + value;
        playerAttack.SetAngleBetweenBullets((int)value);
    }
}
