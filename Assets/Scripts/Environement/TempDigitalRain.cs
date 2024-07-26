using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class TempDigitalRain : MonoBehaviour
{
    public int min, max;
    public float changeRate = 0.5f;

    private TMP_Text tMP_Text;

    private string str = "012345789Z=*+-<>_çéàè";

    private void Start()
    {
        tMP_Text = GetComponent<TMP_Text>();
        StartCoroutine(Test());
    }

    private IEnumerator Test()
    {
        var newTxt = tMP_Text.text;
        newTxt = Regex.Replace(newTxt, "\n", "");
        for (int i = 0; i < newTxt.Length; i++)
        {
            if (Random.value > 0.5f)
            {
                int randomIndex = Random.Range(0, str.Length);
                newTxt = newTxt.Remove(i, 1).Insert(i, str[randomIndex].ToString());
            }
        }
        tMP_Text.text = string.Join("", newTxt.ToCharArray());
        yield return new WaitForSeconds(changeRate);
        StartCoroutine(Test());
    }
}