using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    float HorizontalSpeed = 0.0f;

    // Update is called once per frame
    void Update()
    {
        HorizontalSpeed=Input.GetAxis("Horizontal");

        this.gameObject.transform.position += new Vector3(HorizontalSpeed, 0, 0);
    }
}
