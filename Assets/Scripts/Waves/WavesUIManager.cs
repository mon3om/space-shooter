using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class WavesUIManager : MonoBehaviour
{
    public TextMeshProUGUI bossAlertText;

    public IEnumerator UpdateUI(int currentLevelIndex)
    {
        if (currentLevelIndex > 1)
        {
            bossAlertText.DOText("LEVEL CLEARED", 1);
            yield return new WaitForSeconds(3);
        }

        bossAlertText.DOText("LEVEL " + currentLevelIndex, 1f);
        yield return new WaitForSeconds(1.5f);
        bossAlertText.DOText("START", 1);
        yield return new WaitForSeconds(1f);
        bossAlertText.text = "";
    }
}