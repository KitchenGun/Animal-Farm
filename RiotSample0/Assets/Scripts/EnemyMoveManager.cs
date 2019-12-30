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

    private void Start()
    {
        enemyState = EnemyState.Hold;//게임 시작시 정지 상태
    }

    private void Update()
    {//임시
        EnemyMove(5);
    }

    private void EnemyMove(float Speed)
    {
        this.gameObject.transform.position-=Vector3.right*Speed*Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        {//충돌시 호출됨
            if (other.transform.childCount > 0)
            {//충돌한 오브젝트에 자식이 있을 경우
                Debug.Log("앞에 뭐있다");
            }
            else
            {//충돌한 오브젝트에 자식이 없다?
                Debug.Log("이상무 패스");
            }
        }
    }
}
