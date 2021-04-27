using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : Animal
{
    [SerializeField]
    private Animator EggAni;
    
    private List<GameObject> hitObj = new List<GameObject>();

    private void Start()
    {
        AnimalID = 1001;
        AP =PlayerPrefs.GetInt(AnimalID + "AP");
        ATKRange = PlayerPrefs.GetInt(AnimalID + "ATKRange");
        Invoke("selfDestroy", 3f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.transform.gameObject.name=="Plane")
        {
            EggAni.SetTrigger("Hit");//애니메이션
            selfDestroy();
        }
        else if(collision.transform.gameObject.CompareTag("Friendly"))
        {
            EggAni.SetTrigger("Hit");//애니메이션
            selfDestroy();
        }
        if (collision.transform.gameObject.tag == "Enemy")
        {
            this.GetComponent<SphereCollider>().radius = ATKRange;
            hitObj.Add(collision.gameObject);
            this.GetComponent<Rigidbody>().isKinematic = true;
            EggAni.SetTrigger("Hit");//애니메이션
            //대미지주기
            Invoke("SetDamage",0.3f);
        }

    }

    private void SetDamage()
    {
        foreach (GameObject enemy in hitObj)
        {
            if (enemy != null)
            {//타겟이 죽는 경우는 무시한다
                enemy.GetComponent<Enemy>().SendMessage("Hit", AP);
            }
        }
    }

    private void selfDestroy()
    {
        Destroy(this.gameObject, 0.5f);
    }
}
