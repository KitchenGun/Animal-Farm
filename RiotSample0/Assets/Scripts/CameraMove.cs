using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//-13,15
public class CameraMove : MonoBehaviour
{
    float HorizontalSpeed = 0.0f;

    // Update is called once per frame
    void Update()
    {
        HorizontalSpeed=Input.GetAxis("Horizontal");
        if (HorizontalSpeed < 0)
        {
            if (this.gameObject.transform.position.x < -13)
            {
                HorizontalSpeed = 0;
            }
        }
        else
        {
            if (this.gameObject.transform.position.x > 15)
            {
                HorizontalSpeed = 0;
            }
        }

        this.gameObject.transform.position += new Vector3(HorizontalSpeed, 0, 0);
    }
}
