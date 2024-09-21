using UnityEngine;
using UnityEngine.SceneManagement;

public class Instances : MonoBehaviour
{
    public static Instances Instance;

    public static Transform Player;
    public static Transform ProjectileHolder;

    public static UIGameOver UIGameOver;
    public static UIEquipedPanel UIEquipedPanel;
    public static CameraShaker CameraShaker;
    public static LevelManager LevelManager;
    public static WavesManager WavesManager;

    public static Shader ShaderGUIMaterial;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) return;

        ProjectileHolder = GameObject.Find("ProjectileHolder").transform;
        ShaderGUIMaterial = Shader.Find("GUI/Text Shader");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}