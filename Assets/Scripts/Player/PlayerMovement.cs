using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Public variables
    public float movementSpeed;

    // Private variables
    private float xInput = 0, yInput = 0;
    // Mouse movement
    private Vector3 mousePosition = Vector3.zero;
    private Vector3 lastMousePosition = Vector3.zero;
    private Camera cam;
    private bool isMouseMovement = false;
    private Rect cameraRect;

    private void Awake()
    {
        Instances.Player = transform;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        cam = Camera.main;
        CalculateScreenBoundaries();
    }

    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        if (xInput != 0 || yInput != 0)
            isMouseMovement = false;

        // Mouse movement
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
        if (lastMousePosition != mousePosition && xInput == 0 && yInput == 0)
        {
            lastMousePosition = mousePosition;
            isMouseMovement = true;
        }

        // if (isMouseMovement)
        //     transform.position = Vector3.MoveTowards(transform.position, mousePosition, movementSpeed * Time.deltaTime);

        PreventLeavingScreen();
    }

    private void FixedUpdate()
    {
        transform.position = transform.position + movementSpeed * Time.fixedDeltaTime * new Vector3(xInput, yInput, 0);
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

}
