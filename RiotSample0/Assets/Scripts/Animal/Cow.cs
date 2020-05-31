using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class Cow : Animal
{
    private GameCombatManager GM;

    private float HP=5f;
    //애니메이션
    [SerializeField]
    private Animator CowAnimator;
    //이동체크
    private bool isMove;
    private bool isDash;
    //충돌 관련 변수
    private GameObject EnemyObj;
    


    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameCombatManager>();
        //현재 스크립트의 이름 설정
        AnimalID = 3;
        thisAnimalState = AnimalState.Dash;
        isMove = false;
        isDash = true;
        StartCoroutine(CowStateCheck());//소의 상태체크 코루틴 실행
    }

    private void OnTriggerStay(Collider other)
    {
        HPCheck();//체력체크
        if (other.transform.gameObject.tag != "Untagged")
        {
            switch (other.transform.gameObject.tag)
            {
                case "Enemy"://적과 충돌시
                    //이동 멈춤
                    isMove = false;
                    isDash = false;
                    EnemyObj = other.transform.gameObject;
                    switch (thisAnimalState)
                    {
                        case AnimalState.Move://이동
                            break;
                        case AnimalState.Dash://돌진
                            DashHit();
                            //EnemyObj.sendmessage();
                            break;
                        case AnimalState.Attack://공격
                            break;
                        case AnimalState.Stun://기절
                            break;
                        case AnimalState.Retreat://후퇴
                            break;
                        case AnimalState.Die://사망
                            //Destroy(this.gameObject.GetComponent<SphereCollider>());//충돌체 제거
                            break;
                    }
                    break;
                case "RetreatPoint": //후퇴위치
                    switch (thisAnimalState)
                    {
                        case AnimalState.Idle://대기
                            break;
                        case AnimalState.Move://이동
                            break;
                        case AnimalState.Dash://돌진
                            break;
                        case AnimalState.Attack://공격
                            break;
                        case AnimalState.Stun://기절
                            break;
                        case AnimalState.Retreat://후퇴
                            GM.AnimalRelocation(AnimalID);
                            Destroy(this.gameObject);
                            break;
                        case AnimalState.Die://사망
                            Die();/////////////?스크립트 점검필요
                            break;
                    }
                    break;
            }
        }
    }

    private IEnumerator CowStateCheck()
    {//소 상태의 코루틴
        while (thisAnimalState != AnimalState.Die)
        {//죽을때 까지 계속 
            HPCheck();//체력체크
            CowAnimator.SetBool("isMove", isMove);//이동애니메이션 체크
            CowAnimator.SetBool("isDash", isDash);//이동애니메이션 체크
            switch (thisAnimalState)
            {
                case AnimalState.Idle://대기
                    break;
                case AnimalState.Move://이동
                    break;
                case AnimalState.Dash://돌진
                    Dash();
                    break;
                case AnimalState.Attack://공격
                    Debug.Log("atk");
                    HP = 0;
                    break;
                case AnimalState.Stun://기절
                    break;
                case AnimalState.Retreat://후퇴
                    Move(-5);
                    break;
                case AnimalState.Die://사망
                    Die();
                    yield return new WaitForSeconds(2.0f);//중복 실행 방지용
                    break;

            }
            yield return null;
        }
    }


    #region Idle
    private void Idle()
    {
        CowAnimator.Rebind();//모든 애니메이션 다시 시작 내부 변수값 초기화

    }

    #endregion

    #region Move
    private void Move(float MoveSpeed = 1f)
    {
        //임시 변수 추후에 교체해야함
        //
        //이동 
        isMove = true;
        //이동 스크립트
        this.gameObject.transform.position += new Vector3(MoveSpeed, 0, 0) * Time.deltaTime;
    }
    #endregion

    #region Dash
    private void Dash()
    {
        //이동 
        isDash = true;
        //임시 변수 추후에 교체해야함
        float MoveSpeed=1f;
        float DashSpeed=10f;
        //
        //돌진 스크립트
        this.gameObject.transform.position += new Vector3(MoveSpeed * DashSpeed,0,0) * Time.deltaTime;//이동 
    }

    private void DashHit()
    {
        if (EnemyObj)
        {
            //이동 
            isDash = false;
            thisAnimalState = AnimalState.Attack;
        }
    }
    #endregion

    #region Retreat
    public void Retreat()
    {//후퇴버튼 클릭시 실행 함수
        thisAnimalState=AnimalState.Retreat;
        //스프라이트 뒤집기
        //this.transform.GetComponent<Renderer>().material.mainTextureScale = new Vector2(-1, 1);
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
            thisAnimalState = AnimalState.Die;
        }
    }

    #endregion
}
