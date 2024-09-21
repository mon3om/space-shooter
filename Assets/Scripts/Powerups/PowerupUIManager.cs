using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUIManager : MonoBehaviour
{
    [SerializeField] private Image powerupImage;
    [SerializeField] private TMP_Text powerupName;
    [SerializeField] private TMP_Text powerupDescription;
    [SerializeField] private RectTransform textPanel;

    private bool playerHovering = false;

    void Update()
    {
        if (playerHovering)
        {
            CorrectTextPosition();

            if (Input.GetKey(KeyCode.Space))
            {
                GetComponent<PowerupBase>().Activate();
                Destroy(transform.parent.gameObject);
            }
        }
    }

    public void SetData(PowerupScriptableObject powerupScriptableObject)
    {
        powerupImage.sprite = powerupScriptableObject.sprite;
        powerupName.text = powerupScriptableObject.itemName;
        powerupDescription.text = powerupScriptableObject.description;

        if (powerupScriptableObject.animationClip != null)
        {
            powerupImage.GetComponent<Animator>().enabled = true;
            powerupImage.GetComponent<Animator>().Play(powerupScriptableObject.animationClip.name);
        }
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

    private void CorrectTextPosition()
    {
        if (!textPanel) return;

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