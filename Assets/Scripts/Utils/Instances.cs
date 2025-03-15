using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
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
    public static PostProcessVolume PostProcessVolume;
    public static StoryManager StoryManager;
    public static AuthInfo AuthInfo;

    public static Shader ShaderGUIMaterial;

    [Space]
    public float menuBloomValue = 35;
    public float gameBloomValue = 8;

    [HideInInspector] public SaveSlotData saveSlotData = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            saveSlotData = null;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightControl))
            if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene(0);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) return;

        ProjectileHolder = GameObject.Find("ProjectileHolder").transform;
        ShaderGUIMaterial = Shader.Find("GUI/Text Shader");
        PostProcessVolume = GameObject.Find("Post").GetComponent<PostProcessVolume>();
        if (PostProcessVolume.profile.TryGetSettings(out Bloom bloom))
            if (scene.name == "Menu")
                bloom.intensity.Interp(gameBloomValue, menuBloomValue, 1);
            else
                bloom.intensity.Interp(menuBloomValue, gameBloomValue, 1);

        if (saveSlotData != null && !string.IsNullOrEmpty(saveSlotData._id))
            StartCoroutine(ApplySavedDataCoroutine());
    }

    private IEnumerator ApplySavedDataCoroutine()
    {
        Debug.Log("Applying save data");
        yield return new WaitForEndOfFrame();
        LevelManager.SetLevelFromSavedData(saveSlotData);
        WavesManager.waveId = saveSlotData.wave;
        Player.GetComponent<PlayerDamager>().SetHealth(saveSlotData.health, saveSlotData.initHealth);
        PowerupsManager.Instance.EquipPowerupsByString(saveSlotData.powerups);
        UIScoreManager.UpdateScore(saveSlotData.score);
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