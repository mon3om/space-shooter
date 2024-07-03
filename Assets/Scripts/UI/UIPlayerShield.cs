using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum ShieldState { Charging, Active, Activating, Disabled }
public class UIPlayerShield : MonoBehaviour
{
    public float fillingPPS;
    public float draningPPS;
    [SerializeField] private float activationDelay = 2f;
    private Image imageBar, imageIcon;
    private ShieldState shieldState = ShieldState.Charging;
    public static System.Action<ShieldState> OnShielStateChanged;

    private void OnEnable()
    {
        PowerupBase.OnPowerupReady += OnPowerupShieldReady;
    }

    private void OnDisable()
    {
        PowerupBase.OnPowerupReady -= OnPowerupShieldReady;
    }

    private void OnPowerupShieldReady(PowerupBase powerupShield)
    {
        if (powerupShield.GetType() == typeof(PowerupShield))
        {
            ((PowerupShield)powerupShield).uIPlayerShield = this;
        }
    }

    private void Start()
    {
        imageBar = transform.Find("Image/Bar").GetComponent<Image>();
        Debug.Log(imageBar);
        imageIcon = transform.Find("Icon").GetComponent<Image>();
        imageBar.fillAmount = 0;
        ChangeState(ShieldState.Disabled);
    }

    private void Update()
    {
        if (shieldState == ShieldState.Disabled)
        {
            return;
        }
        else if (shieldState == ShieldState.Charging)
        {
            imageBar.fillAmount += fillingPPS / 100 * Time.deltaTime;
            if (imageBar.fillAmount >= 1)
            {
                ChangeState(ShieldState.Activating);
                StartCoroutine(ActivationCoroutine());
            }
        }
        else if (shieldState == ShieldState.Active)
        {
            imageBar.fillAmount -= draningPPS / 100 * Time.deltaTime;
            if (imageBar.fillAmount <= 0)
                ChangeState(ShieldState.Charging);
        }

        imageBar.fillAmount = Mathf.Clamp(imageBar.fillAmount, 0, 1);
    }

    public void ChangeState(ShieldState newState)
    {
        shieldState = newState;
        OnShielStateChanged?.Invoke(newState);

        switch (newState)
        {
            case ShieldState.Charging:
                DisplayUI(true);
                imageBar.fillAmount = 0;
                break;
            case ShieldState.Active:
                break;
            case ShieldState.Activating:
                break;
            case ShieldState.Disabled:
                StopAllCoroutines();
                DisplayUI(false);
                break;
            default:
                break;
        }
    }

    private IEnumerator ActivationCoroutine()
    {
        yield return new WaitForSeconds(activationDelay);
        ChangeState(ShieldState.Active);
    }

    public void Reset()
    {
        ChangeState(ShieldState.Charging);
    }

    private void DisplayUI(bool display)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(display);
    }
}
