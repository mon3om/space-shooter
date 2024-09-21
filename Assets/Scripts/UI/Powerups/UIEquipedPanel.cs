using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipedPanel : MonoBehaviour
{
    public GameObject powerupIconPrefab;
    public RectTransform container;
    public float imageWidth = 30, spacing;

    public static UIEquipedPanel Instance;

    private void Awake()
    {
        Instances.UIEquipedPanel = this;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        DisplayEquiped();
    }

    public void DisplayEquiped()
    {
        Debug.Log("Displaying equipped powerups " + PowerupsManager.equippedPowerups.Count);
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);

        container.sizeDelta = new(PowerupsManager.equippedPowerups.Count * imageWidth + spacing, imageWidth + spacing * 2);
        foreach (var item in PowerupsManager.equippedPowerups)
        {
            var go = Instantiate(powerupIconPrefab, transform);
            go.GetComponent<RectTransform>().sizeDelta = new(imageWidth, imageWidth);
            go.GetComponent<Image>().sprite = item.sprite;
            go.transform.GetChild(0).GetComponentInChildren<TMPro.TMP_Text>().text = item.description;
        }
    }
}