using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barn : MonoBehaviour
{

    private GameCombatManager GM;


    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameCombatManager>();

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "Enemy")
        {
            GM.Lose();
        }
    }

}
