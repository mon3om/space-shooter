using System.Collections.Generic;
#if UNITY_EDITOR
using System.Diagnostics;
#endif
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace JsonWaves
{
    public class JsonWavesManager : MonoBehaviour
    {
        public async Task StartNode()
        {
#if UNITY_EDITOR
            string nodePath = @"C:\Program Files\nodejs\node.exe";
            string scriptPath = Application.dataPath + "/Scripts/Enemies/Waves/Helpers/json/wavesBuilder.js";
            ProcessStartInfo startInfo = new()
            {
                FileName = nodePath,
                Arguments = "\"" + scriptPath + "\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            Process process = new()
            {
                StartInfo = startInfo
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            await Task.Run(() => process.WaitForExit());

            UnityEngine.Debug.Log("Node.js Output: " + output);
            if (!string.IsNullOrEmpty(error))
            {
                UnityEngine.Debug.LogError("Node.js Error: " + error);
            }
#endif
        }

        public List<WaveEnemy> GetWaves()
        {
            List<WaveEnemy> waveEnemies = new();

            string json = Resources.Load<TextAsset>("waves").text;
            WavesData wavesData = JsonUtility.FromJson<WavesData>(json);

            foreach (var item in wavesData.data)
            {
                WaveEnemy waveEnemy = new();
                waveEnemy.enemyPrefab = (GameObject)Resources.Load(item.enemyPrefab);
                WaveEnemyDifficulty waveEnemyDifficulty = (WaveEnemyDifficulty)System.Enum.Parse(typeof(WaveEnemyDifficulty), item.difficulty);
                UnityEngine.Debug.Log(waveEnemyDifficulty);
                waveEnemy.enemyDifficulty = waveEnemyDifficulty;

                waveEnemy.count = new IntRange(item.countMin, item.countMax, item.countStep);

                List<WaveEnteringPosition> waveEnteringPositions = new();
                foreach (var enteringPosition in item.enterPositions)
                {
                    waveEnteringPositions.Add((WaveEnteringPosition)System.Enum.Parse(typeof(WaveEnteringPosition), enteringPosition));
                }
                waveEnemy.waveEnteringPositions = waveEnteringPositions.ToArray();

                List<WaveShape> waveShapes = new();
                foreach (var shape in item.shapes)
                {
                    waveShapes.Add((WaveShape)System.Enum.Parse(typeof(WaveShape), shape));
                }
                waveEnemy.waveShapes = waveShapes.ToArray();

                waveEnemy.maxEnemiesPerLine = item.maxEnemiesPerLine;

                waveEnemies.Add(waveEnemy);
            }


            return waveEnemies;
        }

    }

    [System.Serializable]
    public class WavesData
    {
        public WaveData[] data;
    }

    [System.Serializable]
    public class WaveData
    {
        public string enemyPrefab;
        public string[] enterPositions;
        public string[] shapes;
        public int maxEnemiesPerLine;
        public string difficulty;
        public int countMin;
        public int countMax;
        public int countStep;
    }

}