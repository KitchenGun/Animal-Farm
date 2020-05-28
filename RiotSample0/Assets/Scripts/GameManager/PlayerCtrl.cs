using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FarmPlayerState
{//플레이어 상태
    Idle,//입력 없을때
    Move,
    PanelUP//입력 값 나올때
}
public class PlayerCtrl : MonoBehaviour
{
    //플레이어 상태
    private FarmPlayerState farmPlayerState;
    //패널이 켜졋는지 확인
    private bool isPanelUP;
    //속도
    [SerializeField]
    private float Speed = 5f;
    //이동 값
    private float vValue;
    private float hValue;
    public bool isMove=false;

    private void Start()
    {
        //초기화
        farmPlayerState = FarmPlayerState.Idle;        
    }


    private void Update()
    {

        switch (farmPlayerState)
        {
            case FarmPlayerState.Idle://대기상태
                vValue = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
                hValue = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
                //이동확인
                isMove = inputCheck(vValue, hValue);
                break;
            case FarmPlayerState.Move://이동중
                vValue = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
                hValue = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
                //이동확인
                isMove = inputCheck(vValue, hValue);
                break;
            case FarmPlayerState.PanelUP://패널켜졌을대
                //이동 제한
                vValue = 0;
                hValue = 0;
                isMove = inputCheck(vValue, hValue);
                break;
        }



    }

    #region MoveInput
    private bool inputCheck(float vValue , float hValue)
    {
        if (vValue == 0 && hValue == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

}
