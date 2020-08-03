using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barn : MonoBehaviour
{

    private GameCombatManager GM;
    private Animator BarnAnimator;

    private void Start()
    {//변수 찾기
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameCombatManager>();
        BarnAnimator = this.GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {//충돌시 파괴함수 실행 추후에 변경가능
        if (other.transform.gameObject.tag == "Enemy")
        {
            StartCoroutine(BarnDestroy());
        }
    }

    private IEnumerator BarnDestroy()
    {//헛간 파괴
        BarnAnimator.SetTrigger("Destroy");
        yield return new WaitForSeconds(2f);
        GM.Lose();
        StopCoroutine(BarnDestroy());
    }

}
