using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveright : MonoBehaviour
{
    [SerializeField]
    private float speed;
    void Update()
    {
        this.gameObject.transform.position += speed*Vector3.right*Time.deltaTime;
    }
}
