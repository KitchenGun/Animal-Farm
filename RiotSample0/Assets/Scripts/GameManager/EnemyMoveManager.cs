using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{//적의 상태
    Hold,
    Move,
    Attack,
    Die
}
public class EnemyMoveManager : MonoBehaviour
{
    private EnemyState enemyState;

    private RaycastHit rayHit;


    private void Start()
    {
        enemyState = EnemyState.Hold;//게임 시작시 정지 상태
        StartCoroutine(enemyObjState());
    }

    private IEnumerator enemyObjState()
    {
        while(enemyState!=EnemyState.Die)
        {//죽으면 끝
            
            rayCheck(2.0f);
            //임시로 넣은 수치값
            switch(enemyState)
            {
                case EnemyState.Hold:
                    {//정지상태
                        Debug.Log("hold");
                        break;
                    }
                case EnemyState.Move:
                    {//움직이는상태
                        Debug.Log("Move");
                        enemyMove(5);
                        break;
                    }
                case EnemyState.Attack:
                    {//공격상태
                        break;
                    }
            }
            yield return null;
        }
        yield break;
    }

    private void rayCheck(float attackRange)
    {
        if (Physics.Raycast(this.gameObject.transform.position, Vector3.left, out rayHit, 20)) 
        {//레이를 생성
            Debug.Log("rayhit");
            if (attackRange>=Vector3.Distance(this.gameObject.transform.position, rayHit.transform.position)&&rayHit.transform.childCount > 0)
            {//공격범위 안에 있을 경우 정지
                if (rayHit.transform.GetChild(0))
                {
                    enemyState = EnemyState.Hold;
                }
            }
            else
            {//공격범위 밖에 있을 경우 움직이기
                enemyState = EnemyState.Move;
            }
        }
    }

    private void enemyMove(float Speed)
    {//앞으로 등속운동하게 만드는 함수
        this.gameObject.transform.position-=Vector3.right*Speed*Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        {//충돌시 호출됨
            if (other.transform.childCount>0)
            {//충돌한 오브젝트에 자식이 있을 경우
                if (other.transform.GetChild(0).gameObject.tag == "Player")
                {//충돌한오브젝트의 자식의 테그가 플레이어일 경우 정지
                    enemyState = EnemyState.Hold;
                    Debug.Log("앞에 뭐있다");
                }
            }
            else
            {//충돌한 오브젝트에 자식이 없다?
                Debug.Log("이상무 패스");
            }
        }
    }
}
