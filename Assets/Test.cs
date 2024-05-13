using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float angle = Vector3.Angle(transform.up, transform.position - Instances.Player.position);
        Debug.Log(angle);
        if (Mathf.Abs(angle) > 3)
            transform.Rotate(new(0, 0, 1), angle > 0 ? 1 : -1);
        // transform.rotation = Quaternion.AngleAxis(angle, new(0, 0, 1));
    }
}
