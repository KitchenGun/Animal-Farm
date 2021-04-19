using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Animal
{
    //애니메이션
    [SerializeField]
    private Animator DogAnimator;

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameCombatManager>();
        playerInfo = GM.playerInfo;
        //현재 스크립트의 이름 설정
        AnimalID = 2;
        thisAnimalState = AnimalState.Move;
        isMove = false;
        isDie = false;
        //추후에 csv 파일 편집 완료시 사용
        HP = PlayerPrefs.GetInt(AnimalID + "HP");
        AP = PlayerPrefs.GetInt(AnimalID + "AP");
        MoveSpeed = PlayerPrefs.GetFloat(AnimalID + "MoveSpeed");
        ATKDelay = PlayerPrefs.GetFloat(AnimalID + "ATKDelay");
        ATKRange = PlayerPrefs.GetInt(AnimalID + "ATKRange");
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(DogStateCheck());//개의 상태체크 코루틴 실행
    }

    private void Update()
    {
        if (Physics.Raycast(this.transform.position, new Vector3(ATKRange,0,0), out ATKRay, ATKRange))
        {//레이케스트
            if (thisAnimalState == AnimalState.Retreat)
            {
                if (isRunCo)
                {
                    EnemyObj = null;
                    isRunCo = false;
                    StopCoroutine(DogStateCheck());
                }
                isMove = false;
                //이동 스크립트
                this.gameObject.transform.position -= new Vector3(MoveSpeed * 2, 0, 0) * Time.deltaTime;
            }
            else if (ATKRay.transform.gameObject.tag == "Enemy")
            {
                if (ATKRay.collider.GetComponent<BoxCollider>())
                {
                    EnemyObj = ATKRay.transform.gameObject;
                    MoveContact();
                }
            }
            else
            {
                thisAnimalState = AnimalState.Move;
            }
        }
    }

    
    private void OnTriggerStay(Collider other)
    {
        HPCheck();//체력체크
        if (other.transform.gameObject.tag != "Untagged")
        {
            switch (other.transform.gameObject.tag)
            {
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
    

    private IEnumerator DogStateCheck()
    {//소 상태의 코루틴
        isRunCo = true;
        while (thisAnimalState != AnimalState.Die)
        {//죽을때 까지 계속 
            HPCheck();//체력체크
            MoveContact();//충돌처리
            DogAnimator.SetBool("isMove", isMove);//이동애니메이션 체크
            switch (thisAnimalState)
            {
                case AnimalState.Idle://대기
                    break;
                case AnimalState.Move://이동
                    Move(MoveSpeed);
                    break;
                case AnimalState.Dash://돌진
                    break;
                case AnimalState.Attack://공격
                    Invoke("Attack",0f);
                    yield return new WaitForSeconds(0.5f);
                    DogAnimator.SetBool("isAtk", false);
                    yield return new WaitForSeconds(ATKDelay);
                    break;
                case AnimalState.Stun://기절
                    break;
                case AnimalState.Retreat://후퇴
                    isMove = false;
                    //이동 스크립트
                    this.gameObject.transform.position -= new Vector3(MoveSpeed * 2, 0, 0) * Time.deltaTime;
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
        DogAnimator.Rebind();//모든 애니메이션 다시 시작 내부 변수값 초기화
    }

    #endregion

    #region Move
    private void Move(float MoveSpeed = 1f)
    {
        //이동 
        isMove = true;
        //이동 스크립트
        this.gameObject.transform.position += new Vector3(MoveSpeed, 0, 0) * Time.deltaTime;
    }

    #endregion

    #region Attack
    private void MoveContact()
    {//이동중 충돌 경우
        if (EnemyObj)
        {
            //이동 
            isMove = false;
            thisAnimalState = AnimalState.Attack;
        }
    }
    private void Attack()
    {//공격
        if (EnemyObj == null)
        {
            isMove = true;
            thisAnimalState = AnimalState.Move;
            return;
        }
        else
        {
            //적 오브젝트 접근
            if (EnemyObj.GetComponent<Enemy>())
            {
                EnemyObj.GetComponent<Enemy>().SendMessage("Hit", AP);
            }
            else if (EnemyObj.GetComponent<EnemySpawner>())
            {
                EnemyObj.GetComponent<EnemySpawner>().SendMessage("Hit", AP);
            }

            thisAnimalState = AnimalState.Attack;
            DogAnimator.SetBool("isAtk", true);
        }
    }

    #endregion

    #region Hit
    public void Hit(int EnemyAP)
    {//체력을 깍고 체력을 확인
        HP -= EnemyAP;
        spriteRenderer.color = Color.red;// 색변경
        Invoke("ColorRollback", 0.3f);//변경 복원
        HPCheck();
    }

    private void ColorRollback()
    {
        spriteRenderer.color = Color.white;
    }

    #endregion

    #region Retreat
    public void Retreat()
    {//후퇴버튼 클릭시 실행 함수
        thisAnimalState = AnimalState.Retreat;
        DogAnimator.SetTrigger("isRetreat");
    }
    #endregion

    #region Die
    private void Die(bool isDie)
    {//사망시 적용
        if (isDie == false)
        {
            isDie = true;
            CallDeadToGM(AnimalID);//죽을경우 게임 매니져 호출
            DogAnimator.SetBool("isAtk", false);
            DogAnimator.SetBool("isDie", true);//애니메이션 제어
            Destroy(this.gameObject, 1.2f);
        }
    }

    private void HPCheck()
    {
        if (HP <= 0)
        {
            thisAnimalState = AnimalState.Die;
            Die(isDie);
        }
    }

    #endregion
}
