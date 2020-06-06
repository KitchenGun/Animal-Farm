using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alpaca : Animal
{
    private GameCombatManager GM;
    private PlayerInfo playerInfo;
    private float HP = 5f;
    //애니메이션
    [SerializeField]
    private Animator AlpacaAnimator;
    //이동체크
    private bool isMove;
    //체력
    private bool isDie;
    //충돌 관련 변수
    private GameObject EnemyObj;

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameCombatManager>();
        playerInfo = GM.playerInfo;
        //현재 스크립트의 이름 설정
        AnimalID = 9;
        thisAnimalState = AnimalState.Move;
        isMove = false;
        isDie = false;
        //추후에 csv 파일 편집 완료시 사용
        //HP = playerInfo.GetCharHP(AnimalID);
        Debug.Log(HP);
        StartCoroutine(AlpacaStateCheck());//개의 상태체크 코루틴 실행
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
                    EnemyObj = other.transform.gameObject;
                    switch (thisAnimalState)
                    {
                        case AnimalState.Move://이동
                            MoveContact();
                            break;
                        case AnimalState.Dash://돌진
                            break;
                        case AnimalState.Attack://공격
                            Attack();
                            break;
                        case AnimalState.Stun://기절
                            break;
                        case AnimalState.Retreat://후퇴
                            break;
                        case AnimalState.Die://사망
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
                            Die(isDie);
                            break;
                    }
                    break;
            }
        }
    }

    private IEnumerator AlpacaStateCheck()
    {//소 상태의 코루틴
        while (thisAnimalState != AnimalState.Die)
        {//죽을때 까지 계속 
            HPCheck();//체력체크
            AlpacaAnimator.SetBool("isMove", isMove);//이동애니메이션 체크
            switch (thisAnimalState)
            {
                case AnimalState.Idle://대기
                    break;
                case AnimalState.Move://이동
                    Move();
                    break;
                case AnimalState.Dash://돌진
                    break;
                case AnimalState.Attack://공격
                    AlpacaAnimator.SetBool("isAtk", false);
                    yield return new WaitForSeconds(1.0f);
                    Invoke("Attack", 0f);
                    yield return new WaitForSeconds(0.5f);
                    Debug.Log("atk");
                    break;
                case AnimalState.Stun://기절
                    break;
                case AnimalState.Retreat://후퇴
                    Move(-5);
                    break;
                case AnimalState.Die://사망
                    Die(isDie);
                    yield return new WaitForSeconds(2.0f);//중복 실행 방지용
                    break;

            }
            yield return null;
        }
    }


    #region Idle
    private void Idle()
    {
        AlpacaAnimator.Rebind();//모든 애니메이션 다시 시작 내부 변수값 초기화

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

    private void MoveContact()
    {//이동중 충돌 경우
        if (EnemyObj)
        {
            //이동 
            isMove = false;
            thisAnimalState = AnimalState.Attack;
        }
    }
    #endregion

    #region Attack
    private void Attack()
    {
        if (EnemyObj == null)
        {
            this.GetComponent<SphereCollider>().enabled = true;
            thisAnimalState = AnimalState.Move;
            return;
        }
        else
        {
            //적 오브젝트 접근
            //EnemyObj.
            thisAnimalState = AnimalState.Attack;
            AlpacaAnimator.SetBool("isAtk", true);
        }

    }
    #endregion

    #region Retreat
    public void Retreat()
    {//후퇴버튼 클릭시 실행 함수
        thisAnimalState = AnimalState.Retreat;
        //스프라이트 뒤집기
        //this.transform.GetComponent<Renderer>().material.mainTextureScale = new Vector2(-1, 1);
    }
    #endregion

    #region Die
    private void Die(bool isDie)
    {//사망시 적용
        if (isDie == false)
        {
            Debug.Log("Die");
            AlpacaAnimator.SetBool("isDie", true);//애니메이션 제어
            Destroy(this.gameObject, 2.0f);
            isDie = true;
        }
    }

    private void HPCheck()
    {
        if (HP <= 0)
        {
            thisAnimalState = AnimalState.Die;
        }
    }

    #endregion

}
