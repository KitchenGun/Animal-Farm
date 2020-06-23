using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagMovement : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //깃발이 움직이게 만듬
        this.gameObject.transform.position += (Vector3.left*1.5f) * Time.deltaTime;
        
    }
}
