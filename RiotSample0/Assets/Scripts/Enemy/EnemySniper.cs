using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SniperState
{//저격수 상태
    Idle,
    Ready,
    Aim,
    Fire,
    Wait
}
public class EnemySniper : MonoBehaviour
{
    private SniperState enemySniperState;
    //스나이퍼 상태 변수
    public GameObject[] SniperNest=new GameObject[2];
    //스나이퍼 위치 세팅
    private GameObject currentSniperNest;
    //현재 위치
    private int sniperNestNum;
    //스나이퍼 배열 변수
    public GameObject Bullet;
    //총알

    private void Start()
    {
        sniperRePosition();
        StartCoroutine(sniperState());
    }

    private IEnumerator sniperState()
    {
        while (enemySniperState != SniperState.Wait)
        {
            switch (enemySniperState)
            {
                case SniperState.Idle:
                    {//기본상태
                        Debug.Log("idle enemysniper");
                        yield return new WaitForSeconds(0.5f);
                        enemySniperState = SniperState.Ready;
                        break;
                    }
                case SniperState.Ready:
                    {//위치 세팅
                        Debug.Log("Ready enemysniper");
                        yield return new WaitForSeconds(0.5f);
                        enemySniperState = SniperState.Aim;
                        break;
                    }
                case SniperState.Aim:
                    {//공격 준비
                        Debug.Log("Aim enemysniper");
                        warningLine();
                        yield return new WaitForSeconds(0.1f);
                        break;
                    }
                case SniperState.Fire:
                    {//공격
                        Debug.Log("Fire enemysniper");
                        break;
                    }
                case SniperState.Wait:
                    {//대기 상태
                        Debug.Log("Wait enemysniper");
                        sniperRePosition();
                        break;
                    }
            }
            yield return null;
        }
        yield return null;
    }

    private void sniperRePosition()
    {//위치 선정
        int tempNestNum = Random.Range(0,2);
        sniperNestNum = tempNestNum;
        Debug.Log(sniperNestNum);
        currentSniperNest = SniperNest[sniperNestNum];
    }

    private void warningLine()
    {//선표시
        Debug.DrawLine(currentSniperNest.transform.position, currentSniperNest.transform.position + new Vector3(-50, 0, 0), Color.red);
    }
}
