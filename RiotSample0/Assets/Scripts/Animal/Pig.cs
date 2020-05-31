using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField]
    private GameCombatManager gameCombatManager;

    private Animator animator;
    private bool isMove;

    private GameObject pig;

    private void Start()
    {
        //초기화
        pig = this.gameObject;
        animator = pig.GetComponent<Animator>();
        gameCombatManager = GameObject.Find("GM").GetComponent<GameCombatManager>();
    }

    private void Update()
    {
        isMove=gameCombatManager.isMove;
        animator.SetBool("isMove",isMove);
    }


}
