using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : Enemy
{
    //애니메이션
    [SerializeField]
    private Animator WhipAnimator;

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameCombatManager>();
        playerInfo = GM.playerInfo;
        //현재 스크립트의 이름 설정
        EnemyID = 102;
        thisEnemyState = EnemyState.Move;
        isMove = false;
        isDie = false;
        //csv 파일에서 불러오기 
        HP = PlayerPrefs.GetInt(EnemyID + "HP");
        AP = PlayerPrefs.GetInt(EnemyID + "AP");
        MoveSpeed = PlayerPrefs.GetFloat(EnemyID + "MoveSpeed");
        ATKDelay = PlayerPrefs.GetFloat(EnemyID + "ATKDelay");
        ATKRange = PlayerPrefs.GetInt(EnemyID + "ATKRange");
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(SpearStateCheck());//개의 상태체크 코루틴 실행
    }
    private void Update()
    {
        if (Physics.Raycast(this.transform.position + new Vector3(0, 0.5f, 0), new Vector3(-1, 0, 0), out ATKRay, ATKRange))
        {//레이케스트
            if (ATKRay.transform.gameObject.tag == "Friendly")
            {//적확인
                if (ATKRay.collider.GetComponent<BoxCollider>())
                {//충돌체 확인
                    AnimalObj = ATKRay.transform.gameObject;
                    MoveContact();
                }
            }
            else
            {
                thisEnemyState = EnemyState.Move;
            }
        }
    }



    private IEnumerator SpearStateCheck()
    {//소 상태의 코루틴
        while (thisEnemyState != EnemyState.Die)
        {//죽을때 까지 계속 
            HPCheck();//체력체크
            MoveContact();//충돌처리
            WhipAnimator.SetBool("isMove", isMove);//이동애니메이션 체크
            switch (thisEnemyState)
            {
                case EnemyState.Idle://대기
                    break;
                case EnemyState.Move://이동
                    Move(MoveSpeed);
                    break;
                case EnemyState.Dash://돌진
                    break;
                case EnemyState.Attack://공격
                    Invoke("Attack", 0f);
                    yield return new WaitForSeconds(0.5f);
                    WhipAnimator.SetBool("isAtk", false);
                    yield return new WaitForSeconds(ATKDelay);
                    break;
                case EnemyState.Stun://기절
                    break;
                case EnemyState.Die://사망
                    Die(isDie);
                    break;
            }
            yield return null;
        }
    }


    #region Idle
    private void Idle()
    {
        WhipAnimator.Rebind();//모든 애니메이션 다시 시작 내부 변수값 초기화

    }

    #endregion

    #region Move
    private void Move(float MoveSpeed = 1f)
    {
        //이동 
        isMove = true;
        //이동 스크립트
        this.gameObject.transform.position -= new Vector3(MoveSpeed, 0, 0) * Time.deltaTime;
    }

    #endregion

    #region Attack
    private void MoveContact()
    {//이동중 충돌 경우
        if (AnimalObj)
        {
            //이동 
            isMove = false;
            thisEnemyState = EnemyState.Attack;
        }
    }
    private void Attack()
    {
        if (AnimalObj == null)
        {
            isMove = true;
            thisEnemyState = EnemyState.Move;
            return;
        }
        else
        {
            if (AnimalObj.GetComponent<Animal>().GetAnimalState() == Animal.AnimalState.Retreat)
            {
                AnimalObj = null;
            }
            else
            {
                //적 오브젝트 접근
                AnimalObj.SendMessage("Hit", AP);
                thisEnemyState = EnemyState.Attack;
                WhipAnimator.SetBool("isAtk", true);
            }
        }

    }

    #endregion

    #region Hit
    public override void Hit(int AnimalAP)
    {//체력을 깍고 체력을 확인
        HP -= AnimalAP;
        spriteRenderer.color = Color.red;// 색변경
        Invoke("ColorRollback", 0.3f);//변경 복원
        HPCheck();
    }
    #endregion

    #region Die
    private void Die(bool isDie)
    {//사망시 적용
        if (isDie == false)
        {
            isDie = true;
            WhipAnimator.SetBool("isAtk", true);
            WhipAnimator.SetBool("isDie", true);//애니메이션 제어
            Destroy(this.gameObject, 1f);
        }
    }

    private void HPCheck()
    {
        if (HP <= 0)
        {
            thisEnemyState = EnemyState.Die;
            Die(isDie);
        }
    }

    #endregion
}
