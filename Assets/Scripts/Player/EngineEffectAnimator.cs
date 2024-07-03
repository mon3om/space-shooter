using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineEffectAnimator : MonoBehaviour
{
    [SerializeField] private float animationSpeed = 5;

    [Space]

    [SerializeField] private List<EngineAnimationRotator> rotationParts;
    [SerializeField] private List<EngineAnimationScaler> scaleParts;

    [HideInInspector] public Vector2 velocity;

    private void Start()
    {

    }

    void Update()
    {
        foreach (var item in rotationParts)
        {
            item.Rotate(velocity, animationSpeed);
        }

        foreach (var item in scaleParts)
        {
            if (velocity.y > 0)
                item.Scale(animationSpeed);
            else
                item.Unscale(animationSpeed);
        }
    }
}

[System.Serializable]
public class EngineAnimationScaler
{
    public Transform scalePart;
    public Vector3 scaleAmount;
    public Vector3 positionRegulation;

    private Vector3 initScale = Vector3.zero;
    private Vector3 initPosition = Vector3.zero;

    public void Scale(float speed)
    {
        if (initScale == Vector3.zero) initScale = scalePart.localScale;
        if (initPosition == Vector3.zero) initPosition = scalePart.localPosition;
        scalePart.localScale = Vector3.Lerp(scalePart.localScale, new(scaleAmount.x != 0 ? scaleAmount.x : initScale.x, scaleAmount.y != 0 ? scaleAmount.y : initScale.y, 1), speed * Time.deltaTime);

        scalePart.localPosition = Vector3.Lerp(scalePart.localPosition, positionRegulation, speed * Time.deltaTime);
    }

    public void Unscale(float speed)
    {
        if (initScale == Vector3.zero) initScale = scalePart.localScale;
        if (initPosition == Vector3.zero) initPosition = scalePart.localPosition;
        scalePart.localScale = Vector3.Lerp(scalePart.localScale, initScale, speed * Time.deltaTime);

        scalePart.localPosition = Vector3.Lerp(scalePart.localPosition, initPosition, speed * Time.deltaTime);
    }
}

[System.Serializable]
public class EngineAnimationRotator
{
    public Transform rotationPart;
    public float rotationAmount;


    public void Rotate(Vector2 velocity, float speed)
    {
        if (velocity.x != 0)
        {
            if (velocity.x > 0) // Positive velocity
            {
                if (rotationPart.localRotation.z > -rotationAmount)
                    rotationPart.localRotation = Quaternion.Lerp(rotationPart.localRotation, Quaternion.Euler(0, 0, -rotationAmount), speed * Time.deltaTime);
            }
            else // Negative velocity
            {
                if (rotationPart.localRotation.z < rotationAmount)
                    rotationPart.localRotation = Quaternion.Lerp(rotationPart.localRotation, Quaternion.Euler(0, 0, rotationAmount), speed * Time.deltaTime);
            }
        }
        else
        {
            if (rotationPart.localRotation.z != 0)
                rotationPart.localRotation = Quaternion.Lerp(rotationPart.localRotation, Quaternion.Euler(0, 0, 0), speed * Time.deltaTime);
        }
    }
}
