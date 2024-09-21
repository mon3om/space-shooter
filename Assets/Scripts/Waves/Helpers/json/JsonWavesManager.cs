using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
using System.Diagnostics;
#endif

namespace JsonWaves
{
    public class JsonWavesManager : MonoBehaviour
    {
        private static List<WaveEnemy> WaveEnemies = new();
        public static async Task StartNode()
        {
#if UNITY_EDITOR
            string nodePath = @"C:\Program Files\nodejs\node.exe";
            string scriptPath = Application.dataPath + "/Scripts/Waves/Helpers/json/wavesBuilder.js";
            scriptPath = "\"" + scriptPath + "\"";
            ProcessStartInfo startInfo = new()
            {
                FileName = nodePath,
                Arguments = scriptPath,
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

        public static List<WaveEnemy> GetWaves()
        {
            if (WaveEnemies.Count > 0) return WaveEnemies;

            List<WaveEnemy> waveEnemies = new();

            string json = Resources.Load<TextAsset>("waves").text;
            WavesData wavesData = JsonUtility.FromJson<WavesData>(json);

            foreach (var item in wavesData.data)
            {
                WaveEnemy waveEnemy = new();
                waveEnemy.enemyPrefab = (GameObject)Resources.Load(item.enemyPrefab);
                WaveEnemyDifficulty waveEnemyDifficulty = (WaveEnemyDifficulty)System.Enum.Parse(typeof(WaveEnemyDifficulty), item.difficulty);
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

            WaveEnemies = waveEnemies;
            return waveEnemies;
        }

        public static WaveEnemy GetEnemyByName(string name)
        {
            var enemies = GetWaves();
            var temp = "";
            enemies.ForEach(el => temp += el.enemyPrefab.name + " - ");
            UnityEngine.Debug.Log(temp);
            var enemy = enemies.First(x => x.enemyPrefab.name == name) ?? throw new System.Exception("Enemy not found. Enemy's name = " + name);
            return enemy;
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