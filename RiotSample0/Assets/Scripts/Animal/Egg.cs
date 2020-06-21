using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{

    private void Start()
    {
        Invoke("selfDestroy", 3f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }

    private void selfDestroy()
    {
        Destroy(this.gameObject);
    }
}
