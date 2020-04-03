using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveright : MonoBehaviour
{
    void Update()
    {
        this.gameObject.transform.position += Vector3.right*Time.deltaTime;
    }
}
