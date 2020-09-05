using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{

    protected GameCombatManager GM;
    protected PlayerInfo playerInfo;
    public enum AnimalState
    {
        Idle,//기본상태
        Move,//이동
        Dash,//돌진
        Attack,//공격
        Stun,//충격
        Retreat,//후퇴
        Die//사망

    }
    //상태
    protected AnimalState thisAnimalState;
    //동물이름
    protected int AnimalID;
    protected float HP;
    protected int AP;
    protected float MoveSpeed;
    protected float ATKDelay;
    protected int ATKRange;

    //이동체크
    protected bool isMove;
    //체력
    protected bool isDie;
    protected SpriteRenderer spriteRenderer;
    //충돌 관련 변수
    protected RaycastHit ATKRay;
    protected GameObject EnemyObj;

    protected void CallDeadToGM(int ID)
    {// 동물이 죽었을 경우 동물의 id를 보내기 위해서
        GM.DeadAnimalCountAdd(ID);
        Debug.Log(ID + "죽음");
    }
    
}
