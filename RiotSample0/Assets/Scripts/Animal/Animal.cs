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
    protected float ATKSP;

    //이동체크
    protected bool isMove;
    //체력
    protected bool isDie;
    //충돌 관련 변수
    protected GameObject EnemyObj;
}
