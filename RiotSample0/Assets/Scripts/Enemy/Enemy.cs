using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected GameCombatManager GM;
    protected PlayerInfo playerInfo;
    public enum EnemyState
    {
        Idle,//기본상태
        Move,//이동
        Dash,//돌진
        Attack,//공격
        Stun,//충격
        Die//사망

    }
    //상태
    protected EnemyState thisEnemyState;
    //동물이름
    protected int EnemyID;
    protected float HP;
    protected int AP;
    protected float MoveSpeed;
    protected float ATKDelay;
    protected int ATKRange;

    //이동체크
    protected bool isMove;
    //체력
    protected bool isDie;
    //충돌 관련 변수
    protected RaycastHit ATKRay;
    protected GameObject AnimalObj;

}

