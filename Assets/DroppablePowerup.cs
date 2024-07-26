using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DroppablePowerup : MonoBehaviour
{
    private bool playerHovering = false;
    private PowerupScriptableObject selectedPowerup;
    private PowerupScriptableObject[] allPowerups;

    [SerializeField] private Image powerupImage;
    [SerializeField] private TMP_Text powerupName;
    [SerializeField] private TMP_Text powerupDescription;
    [SerializeField] private GameObject textPanel;

    [Space]
    [Header("Debug")]
    public int selectedIndex = -1;

    void Start()
    {
        allPowerups = Resources.LoadAll<PowerupScriptableObject>("ScriptableObjects/Powerups");
        allPowerups = allPowerups.Where(el => el.available).ToArray();
        int selected = Random.Range(0, allPowerups.Length);
        selectedPowerup = allPowerups[selected];

        if (selectedIndex != -1)
            selectedPowerup = allPowerups[selectedIndex];

        powerupImage.sprite = selectedPowerup.sprite;
        powerupName.text = selectedPowerup.itemName;
        powerupDescription.text = selectedPowerup.description;

        if (selectedPowerup.animationClip != null)
        {
            powerupImage.GetComponent<Animator>().enabled = true;
            powerupImage.GetComponent<Animator>().Play(selectedPowerup.animationClip.name);
        }
    }

    void Update()
    {
        ActivatePowerup();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.PLAYER_SHIP))
        {
            playerHovering = true;
            textPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.PLAYER_SHIP))
        {
            playerHovering = false;
            textPanel.SetActive(false);
        }
    }

    private void ActivatePowerup()
    {
        if (playerHovering && Input.GetKey(KeyCode.Space))
        {
            var powerupScript = selectedPowerup.GetPowerupType();
            Debug.Log("SELECTED POWERUP = " + powerupScript.ToString());
            var pus = gameObject.AddComponent(powerupScript) as PowerupBase;
            pus.Activate();

            Destroy(gameObject);
        }
    }
}
