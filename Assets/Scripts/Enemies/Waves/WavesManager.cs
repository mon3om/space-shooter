using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WavesManager : MonoBehaviour
{
    private WaverBase[] waverBases;
    private float delaysSum = 0;

    private void Awake()
    {
        waverBases = GetComponents<WaverBase>();
        for (int i = 0; i < waverBases.Length; i++)
        {
            var waver = waverBases[i];
            waver.enabled = false;
            delaysSum += waver.delayAfterLastWave;
            StartCoroutine(StartWaveCoroutine(delaysSum, waver));
        }


    }

    private IEnumerator StartWaveCoroutine(float delay, WaverBase waverBase)
    {
        yield return new WaitForSeconds(delay);
        if (waverBase != null)
            waverBase.enabled = true;
    }
}