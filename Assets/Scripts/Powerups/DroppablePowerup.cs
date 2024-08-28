using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DroppablePowerup : MonoBehaviour
{
    private bool playerHovering = false;
    private PowerupScriptableObject selectedPowerup;
    private List<PowerupScriptableObject> allPowerups;

    [SerializeField] private Image powerupImage;
    [SerializeField] private TMP_Text powerupName;
    [SerializeField] private TMP_Text powerupDescription;
    [SerializeField] private RectTransform textPanel;

    [Space]
    [Header("Debug")]
    public int manualSelectedID = -1;

    private void Start()
    {
        if (manualSelectedID != -1)
            PickRandomPowerup(new());
    }

    public PowerupScriptableObject PickRandomPowerup(List<PowerupScriptableObject> filter)
    {
        allPowerups = Resources.LoadAll<PowerupScriptableObject>("ScriptableObjects/Powerups").ToList();
        allPowerups = allPowerups.Where(el => el.available).ToList();
        var filteredPowerups = allPowerups.Where(el => filter.Any(el1 => el1.id == el.id) == false).ToList();

        if (allPowerups.Count == 0)
            throw new System.Exception("Powerups count is 0 after filtering the whole list of size " + allPowerups.Count + " with a filter of size " + filter.Count + "");

        int selected = Random.Range(0, filteredPowerups.Count);
        selectedPowerup = filteredPowerups[selected];

        if (manualSelectedID != -1)
            selectedPowerup = filteredPowerups.First(el => el.id == manualSelectedID);

        powerupImage.sprite = selectedPowerup.sprite;
        powerupName.text = selectedPowerup.itemName;
        powerupDescription.text = selectedPowerup.description;

        if (selectedPowerup.animationClip != null)
        {
            powerupImage.GetComponent<Animator>().enabled = true;
            powerupImage.GetComponent<Animator>().Play(selectedPowerup.animationClip.name);
        }

        return selectedPowerup;
    }

    void Update()
    {
        ActivatePowerup();
        CorrectTextPosition();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.PLAYER_SHIP))
        {
            playerHovering = true;
            textPanel.gameObject.SetActive(true);
            powerupImage.transform.parent.TryGetComponent<Canvas>(out var canvas);
            canvas.sortingOrder += 1;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.PLAYER_SHIP))
        {
            playerHovering = false;
            textPanel.gameObject.SetActive(false);
            powerupImage.transform.parent.TryGetComponent<Canvas>(out var canvas);
            canvas.sortingOrder -= 1;
        }
    }

    private void ActivatePowerup()
    {
        if (playerHovering && Input.GetKey(KeyCode.Space))
        {
            var powerupScript = selectedPowerup.GetPowerupType();
            Debug.Log("SELECTED");
            var pus = gameObject.AddComponent(powerupScript) as PowerupBase;
            Debug.Log("ADDED");
            pus.Activate(selectedPowerup);
            Debug.Log("ACTIVATED");
            Destroy(gameObject);
            Debug.Log("DESTROYED");
        }
    }

    private void CorrectTextPosition()
    {
        Vector3 position = textPanel.anchoredPosition;
        float width = textPanel.rect.width;
        float height = textPanel.rect.height;

        if (textPanel.transform.position.y + height > CameraUtils.CameraRect.yMax)
            position.y = -Mathf.Abs(position.y);

        if (textPanel.transform.position.y - height < CameraUtils.CameraRect.yMin)
            position.y = Mathf.Abs(position.y);

        if (textPanel.transform.position.x + width > CameraUtils.CameraRect.xMax)
            position.x = -width / 2f;

        if (textPanel.transform.position.x - width < CameraUtils.CameraRect.xMin)
            position.x = width / 2f;


        textPanel.anchoredPosition = position;
    }
}
