using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotsManager : MonoBehaviour
{
    [SerializeField] private Transform[] slots;
    [SerializeField] private Button startButton;
    private SaveSlotData[] saveSessionDatas;
    private int selectedSlot = -1;

    public static System.Action<SaveSlotData[]> OnSlotsLoaded;

    private void Start()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Find("Info").GetComponent<TMP_Text>().text = "Empty Slot";
        }
    }

    private void OnEnable()
    {
        OnSlotsLoaded += PopulateSlots;
    }
    private void OnDisable()
    {
        OnSlotsLoaded -= PopulateSlots;
    }

    private void PopulateSlots(SaveSlotData[] saves)
    {
        if (saves.Length == 0) return;

        for (int i = 0; i < saves.Length; i++)
        {
            Transform slot = transform.GetChild(0).Find("Slots").GetChild(saves[i].slotIndex);
            SaveSlotData saveSlotData = saves[i];
            slot.Find("Info").GetComponent<TMP_Text>().text = "Score: " + saveSlotData.score + "\nLevel: " + saveSlotData.level + "\nHealth: " + saveSlotData.health;

            // If delete functionality is nedded, uncomment this
            // slotIndex.Find("DeleteButton").gameObject.SetActive(true);
        }

        saveSessionDatas = saves;
    }


    public void SelectSlot(int index)
    {
        selectedSlot = index;
        startButton.interactable = true;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i == selectedSlot)
            {
                slots[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                slots[i].Find("Selector").gameObject.SetActive(true);
            }
            else
            {
                slots[i].GetComponent<Image>().color = new Color(1, 1, 1, 100f / 255f);
                slots[i].Find("Selector").gameObject.SetActive(false);
            }
        }

        SaveSlotData selectedSession;
        if (saveSessionDatas != null && saveSessionDatas.Length > 0 && saveSessionDatas.ToList().Any(el => el.slotIndex == index))
            selectedSession = saveSessionDatas[index];
        else
            selectedSession = new() { slotIndex = index };

        Instances.Instance.saveSlotData = selectedSession;
    }
}
