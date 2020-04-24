using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum CowState
{
    Idle,//기본상태
    Move,//이동
    Dash,//돌진
    Attack,//공격
    Stun,//충격
    Retreat,//후퇴
    Die//사망

}


public class Cow : MonoBehaviour
{
    //상태
    private CowState thisCowState;
    private float HP=5f;
    //애니메이션
    [SerializeField]
    private Animator CowAnimator;
    //충돌 관련 변수
    private GameObject EnemyObj;
    void Start()
    {
        thisCowState = CowState.Dash;
        StartCoroutine(CowStateCheck());//소의 상태체크 코루틴 실행
    }

    private void OnTriggerEnter(Collider other)
    {
        HPCheck();//체력체크
        if (other.transform.gameObject.tag == "Enemy")
        {//충돌 오브젝트의 태그가 enemy일 경우
            Debug.Log("hit");
            EnemyObj = other.transform.gameObject;
            switch (thisCowState)
            {
                case CowState.Idle://대기
                    break;
                case CowState.Move://이동
                    break;
                case CowState.Dash://돌진
                    DashHit();
                    //EnemyObj.sendmessage();
                    break;
                case CowState.Attack://공격
                    
                    break;
                case CowState.Stun://기절
                    break;
                case CowState.Retreat://후퇴
                    break;
                case CowState.Die://사망
                    Die();
                    break;
            }

        }
    }

    private IEnumerator CowStateCheck()
    {//소 상태의 코루틴
        while (thisCowState!=CowState.Die)
        {//죽을때 까지 계속 
            HPCheck();//체력체크
            Debug.Log(thisCowState);
            switch (thisCowState)
            {
                case CowState.Idle://대기
                    break;
                case CowState.Move://이동
                    break;
                case CowState.Dash://돌진
                    Dash();
                    break;
                case CowState.Attack://공격
                    Debug.Log("atk");
                    HP = 0;
                    break;
                case CowState.Stun://기절
                    break;
                case CowState.Retreat://후퇴
                    break;
                case CowState.Die://사망
                    Die();
                    break;

            }

            yield return new WaitForSeconds(0.1f);
        }
    }


    #region Idle
    private void Idle()
    {
        CowAnimator.Rebind();//모든 애니메이션 다시 시작 내부 변수값 초기화

    }

    #endregion

    #region Move
    private void Move()
    {
        //임시 변수 추후에 교체해야함
        float MoveSpeed = 1f;
        //
        //이동 스크립트
        this.gameObject.transform.position += new Vector3(MoveSpeed, 0, 0) * Time.deltaTime;//이동 

    }
    #endregion

    #region Dash
    private void Dash()
    {
        //임시 변수 추후에 교체해야함
        float MoveSpeed=1f;
        float DashSpeed=20f;
        //
        //돌진 스크립트
        this.gameObject.transform.position += new Vector3(MoveSpeed * DashSpeed,0,0) * Time.deltaTime;//이동 

    }

    private void DashHit()
    {
        if (EnemyObj)
        {
            thisCowState = CowState.Attack;
        }
    }
    #endregion

    #region Die
    private void Die()
    {//사망시 적용
        Debug.Log("Die");
        Instantiate(Resources.Load<GameObject>("8"+"GameObj"), this.gameObject.transform.position - Vector3.right, Quaternion.identity);
        Destroy(this.gameObject,2.0f);
    }

    private void HPCheck()
    {
        if(HP<=0)
        {
            thisCowState = CowState.Die;
        }
    }

    #endregion
}
