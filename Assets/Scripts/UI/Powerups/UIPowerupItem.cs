using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIPowerupItem : MonoBehaviour
{
    [HideInInspector] public PowerupScriptableObject powerupScriptableObject;

    private Transform selectedPowerupUIObject;

    public static System.Action<int> OnPowerupSelected;

    private void Start()
    {
        if (!powerupScriptableObject || !powerupScriptableObject.available)
        {
            Debug.LogWarning("Object not assigned!");
            return;
        }
        selectedPowerupUIObject = transform.parent.parent.Find("SelectedPowerup");
        EventTrigger eventTrigger = GetComponent<EventTrigger>();

        // Create a new EventTrigger.Entry
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((eventData) => { OnClick(); });
        eventTrigger.triggers.Add(entry);

        Populate();
    }

    private void OnEnable() => OnPowerupSelected += SelectPowerup;
    private void OnDisable() => OnPowerupSelected -= SelectPowerup;


    private void Populate()
    {
        transform.Find("Container").Find("Name").GetComponent<TMP_Text>().text = powerupScriptableObject.itemName;
        transform.Find("Container").Find("Image").GetComponent<Image>().sprite = powerupScriptableObject.sprite;
        if (!powerupScriptableObject || !powerupScriptableObject.available) transform.Find("Container").Find("Image").GetComponent<Image>().color = new(0.1f, 0.1f, 0.1f, 0.95f);
        else transform.Find("Container").Find("Image").GetComponent<Image>().color = new(1, 1, 1, 1);
    }

    private void OnClick()
    {
        if (PowerupsManager.SelectedPowerup != powerupScriptableObject.id)
        {
            // Broadcasts an event so the previous selected powerup could disable its selector indicator
            OnPowerupSelected?.Invoke(powerupScriptableObject.id);
        }
        else // This unselects the current selected powerup reseting it to a no selected powerup state
        {
            selectedPowerupUIObject.Find("Name").GetComponent<TMP_Text>().text = "";
            selectedPowerupUIObject.Find("Description").GetComponent<TMP_Text>().text = "";
            selectedPowerupUIObject.Find("Image").gameObject.SetActive(false);
            selectedPowerupUIObject.Find("Image").gameObject.SetActive(false);
            transform.Find("Selector").gameObject.SetActive(false);

            PowerupsManager.SelectedPowerup = -1;
        }
    }

    private void SelectPowerup(int id)
    {
        if (id == powerupScriptableObject.id)
        {
            selectedPowerupUIObject.Find("Image").GetComponent<Image>().sprite = powerupScriptableObject.sprite;
            selectedPowerupUIObject.Find("Name").GetComponent<TMP_Text>().text = powerupScriptableObject.itemName;
            selectedPowerupUIObject.Find("Description").GetComponent<TMP_Text>().text = powerupScriptableObject.description;
            selectedPowerupUIObject.Find("Image").gameObject.SetActive(true);
            transform.Find("Selector").gameObject.SetActive(true);

            PowerupsManager.SelectedPowerup = powerupScriptableObject.id;
        }
        else
        {
            transform.Find("Selector").gameObject.SetActive(false);
        }
    }
}
