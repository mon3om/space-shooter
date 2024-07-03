using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealth : MonoBehaviour
{
    [SerializeField] private Image mainHealthSprite;
    [SerializeField] private Image tempHealthSprite;

    private GameObject player;

    [Space]
    [SerializeField] private float tempBarReductionSpeed = 0.1f;
    [SerializeField] private float tempBarReductionDelay = 1.5f;
    private bool willReduceTempBar = false;

    void Start()
    {
        player = GameObject.FindWithTag(Tags.PLAYER_SHIP);
        if (player != null)
        {
            if (player.TryGetComponent(out PlayerDamager playerDamager))
            {
                playerDamager.onDamageTaken += OnDamageTaken;
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
    }

    private void Update()
    {
        if (willReduceTempBar)
        {
            if (mainHealthSprite.fillAmount * 100 / tempHealthSprite.fillAmount < 99)
                tempHealthSprite.fillAmount = Mathf.Lerp(tempHealthSprite.fillAmount, mainHealthSprite.fillAmount, tempBarReductionSpeed * Time.deltaTime);
            else
            {
                willReduceTempBar = false;
                tempHealthSprite.fillAmount = mainHealthSprite.fillAmount;
            }
        }
    }

    private void OnDamageTaken(DamageData damageData)
    {
        mainHealthSprite.fillAmount = damageData.currentHealth / damageData.initialHealth;
        StartCoroutine(SyncTempBarCoroutine());
    }

    private IEnumerator SyncTempBarCoroutine()
    {
        yield return new WaitForSeconds(tempBarReductionDelay);
        willReduceTempBar = true;
    }
}
