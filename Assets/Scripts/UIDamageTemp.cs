using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDamageTemp : MonoBehaviour
{
    [SerializeField] private ShootingSettings bullets;
    [SerializeField] private ShootingSettings missile;

    [SerializeField] private TMPro.TMP_InputField bulletsInput;
    [SerializeField] private TMPro.TMP_InputField missilesInput;

    private static UIDamageTemp Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        bulletsInput.text = bullets.damage.ToString();
        missilesInput.text = missile.damage.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            HandleSettingsOpen();
        }
    }

    float previousTimeScale = -1;
    public void HandleSettingsOpen()
    {
        var canvas = transform.Find("Canvas").gameObject;
        canvas.SetActive(!canvas.activeSelf);
        if (canvas.activeSelf)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;

            transform.GetComponentsInChildren<UnityEngine.UI.Button>().First(el => el.name.ToLower().Contains("mainmenu")).interactable = SceneManager.GetActiveScene().buildIndex != 0;
        }
        else
        {
            Time.timeScale = previousTimeScale != -1 ? previousTimeScale : 1;
        }

    }

    public void MainMenu()
    {
        transform.Find("Canvas").gameObject.SetActive(false);
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(bulletsInput.text))
            bulletsInput.text = bullets.damage.ToString();
        if (string.IsNullOrEmpty(missilesInput.text))
            missilesInput.text = missile.damage.ToString();
    }

    public void OnValueChanged()
    {
        if (int.TryParse(bulletsInput.text, out int bulletsDamage))
            bullets.damage = bulletsDamage;
        else
            bullets.damage = 8;

        if (int.TryParse(missilesInput.text, out int missileDamage))
            missile.damage = missileDamage;
        else
            missile.damage = 10;

        if (bullets.damage <= 0)
            bullets.damage = 8;
        if (missile.damage <= 0)
            missile.damage = 10;
    }
}
