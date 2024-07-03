using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Public variables
    public float movementSpeed;
    [Space]
    [SerializeField] private float slowDownTime = 10;
    [SerializeField] private float slowDownMultiplier = 3;

    // Private variables
    private float xInput = 0, yInput = 0;
    private Camera cam;
    private Rect cameraRect;

    private float initSpeed;
    private float _slowDownTime = 0;


    private EngineEffectAnimator engineEffectAnimator;

    private void OnEnable() => PowerupBase.OnPowerupReady += OnPowerupReady;
    private void OnDisable() => PowerupBase.OnPowerupReady -= OnPowerupReady;
    private void OnPowerupReady(PowerupBase powerupBase) => powerupBase.playerInstance = transform;

    private void Awake()
    {
        Instances.Player = transform;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        initSpeed = movementSpeed;

        cam = Camera.main;
        engineEffectAnimator = GetComponent<EngineEffectAnimator>();
        CalculateScreenBoundaries();
    }

    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(KeyCode.R))
            SceneManager.LoadScene(0);

        engineEffectAnimator.velocity = new(xInput, yInput);

        PreventLeavingScreen();

        // Handle slowing down
        if (_slowDownTime > 0)
        {
            _slowDownTime -= Time.deltaTime;
            if (_slowDownTime <= 0)
            {
                movementSpeed = initSpeed;
                _slowDownTime = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        transform.position = transform.position + movementSpeed * Time.fixedDeltaTime * new Vector3(xInput, yInput, 0).normalized;

        // MovementRotaionHandler();
    }

    private void PreventLeavingScreen()
    {
        transform.position = new Vector3(
        Mathf.Clamp(transform.position.x, cameraRect.xMin, cameraRect.xMax),
        Mathf.Clamp(transform.position.y, cameraRect.yMin, cameraRect.yMax),
        transform.position.z);
    }

    private void CalculateScreenBoundaries()
    {
        var bottomLeft = cam.ScreenToWorldPoint(Vector3.zero);
        var topRight = cam.ScreenToWorldPoint(new Vector3(
            cam.pixelWidth, cam.pixelHeight));

        cameraRect = new Rect(
           bottomLeft.x,
           bottomLeft.y,
           topRight.x - bottomLeft.x,
           topRight.y - bottomLeft.y);
    }

    public void SlowDown()
    {
        movementSpeed = initSpeed / slowDownMultiplier;
        _slowDownTime = slowDownTime;
    }
}
