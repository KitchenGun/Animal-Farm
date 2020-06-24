using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField]
    private Animator EggAni;

    private void Start()
    {
        Invoke("selfDestroy", 3f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.gameObject.name=="Plane")
        {
            selfDestroy();
        }
        if (collision.transform.gameObject.tag == "Enemy")
        {
            collision.transform.gameObject.GetComponent<Enemy>().SendMessage("Hit", 10);
            selfDestroy();
        }
    }

    private void selfDestroy()
    {
        EggAni.SetTrigger("Hit");
        Destroy(this.gameObject, 0.5f);
    }
}
