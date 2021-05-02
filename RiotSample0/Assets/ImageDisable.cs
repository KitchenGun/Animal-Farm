using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDisable : MonoBehaviour
{

    public void DestroyImg()
    {
        Destroy(this.gameObject);
    }

    public void SetActiveFalse()
    {
        this.gameObject.GetComponent<RectTransform>().transform.position = new Vector3(0, 5000f, 0);
    }
}
