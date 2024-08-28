using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealth : MonoBehaviour
{
    [SerializeField] private Image healthImage;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color disabledColor;

    [Space]
    [SerializeField] private float spacing;

    private GameObject player;
    private List<Image> healthImages = new();

    void Start()
    {
        player = GameObject.FindWithTag(Tags.PLAYER_SHIP);
        if (player != null)
        {
            if (player.TryGetComponent(out PlayerDamager playerDamager))
            {
                playerDamager.onHealthModified += OnHealthModified;
            }
            else
            {
                throw new System.Exception("Error fetching PlayerDamager");
            }
        }
        else
        {
            throw new System.Exception("Error fetching player");
        }

        healthImage.color = activeColor;
        healthImages.Add(healthImage);
        InstantiateHealthImages();
    }

    private void InstantiateHealthImages()
    {
        for (int i = 0; i < PowerupsManager.playerHealth - 1; i++)
        {
            var go = Instantiate(healthImage.gameObject, healthImage.transform.parent);
            go.TryGetComponent(out RectTransform rectTransform);
            rectTransform.anchoredPosition = healthImage.GetComponent<RectTransform>().anchoredPosition + Vector2.right * (rectTransform.rect.size.x + spacing) * (i + 1);
            go.TryGetComponent<Image>(out var image);
            image.color = activeColor;
            healthImages.Add(image);
        }
    }

    private void OnHealthModified(float current, float initial)
    {
        for (int i = 0; i < healthImages.Count; i++)
            healthImages[i].color = i < current ? activeColor : disabledColor;
    }
}
