using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class PowerupItem : MonoBehaviour
{
    public PowerupScriptableObject powerupScriptableObject;

    private Transform selectedPowerup;

    private void Start()
    {
        if (!powerupScriptableObject || !powerupScriptableObject.available) return;
        selectedPowerup = transform.parent.parent.Find("SelectedPowerup");
        EventTrigger eventTrigger = GetComponent<EventTrigger>();

        // Create a new EventTrigger.Entry
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((eventData) => { OnClick(); });
        eventTrigger.triggers.Add(entry);
    }

    private void OnValidate()
    {
        transform.Find("Name").GetComponent<TMP_Text>().text = powerupScriptableObject.itemName;
        transform.Find("Image").GetComponent<Image>().sprite = powerupScriptableObject.sprite;
        if (!powerupScriptableObject || !powerupScriptableObject.available) transform.Find("Image").GetComponent<Image>().color = new(0.1f, 0.1f, 0.1f, 0.95f);
        else transform.Find("Image").GetComponent<Image>().color = new(1, 1, 1, 1);
    }

    private void OnClick()
    {
        if (selectedPowerup)
        {
            if (PowerupsManager.SelectedPowerup != powerupScriptableObject.id)
            {
                selectedPowerup.Find("Image").GetComponent<Image>().sprite = powerupScriptableObject.sprite;
                selectedPowerup.Find("Name").GetComponent<TMP_Text>().text = powerupScriptableObject.itemName;
                selectedPowerup.Find("Description").GetComponent<TMP_Text>().text = powerupScriptableObject.description;

                PowerupsManager.SelectedPowerup = powerupScriptableObject.id;
            }
            else
            {
                selectedPowerup.Find("Name").GetComponent<TMP_Text>().text = "";
                selectedPowerup.Find("Description").GetComponent<TMP_Text>().text = "";

                PowerupsManager.SelectedPowerup = -1;
            }
        }

        selectedPowerup.Find("Image").gameObject.SetActive(PowerupsManager.SelectedPowerup != -1);
    }
}
