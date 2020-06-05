using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;



public enum FarmPlayerState
{//플레이어 상태
    Idle,//입력 없을때
    Move,
    PanelUP//입력 값 나올때
}

public class PlayerCtrl : MonoBehaviour
{

    //애니메이션
    [SerializeField]
    private Animator pigAniCtrl;
    //플레이어 상태
    private FarmPlayerState farmPlayerState;
    //패널 확인
    [SerializeField]
    private ButtonClick buttonClick;
    //플레이어 스프라이트 제어
    private SpriteRenderer sprite;
    //속도
    private float Speed = 12f;
    //이동 값
    private float vValue;
    private float hValue;
    private bool isMove=false;

    private void Start()
    {
        //초기화
        farmPlayerState = FarmPlayerState.Idle;
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {//충돌
        string colliderName = other.gameObject.name;
        switch (colliderName)
        {//충돌체 이름에 따른 호출
            case "HouseCollider":
                buttonClick.HouseButton();
                break;
            case "GateCollider":
                buttonClick.GateButton();
                break;
            case "BarnCollider":
                buttonClick.BarnButton();
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        switch (farmPlayerState)
        {
            case FarmPlayerState.Idle://대기상태
                //sprite.enabled = true;//스프라이트 켜기
                vValue = Input.GetAxis("Vertical") * Time.deltaTime;
                hValue = Input.GetAxis("Horizontal") * Time.deltaTime;
                //이동확인
                isMove = inputCheck(vValue, hValue);
                //애니메이션
                pigAniCtrl.SetBool("isMove", isMove);
                //이동
                PlayerMove(vValue, hValue);
                    break;
            case FarmPlayerState.Move://이동중
                vValue = Input.GetAxis("Vertical") * Time.deltaTime;
                hValue = Input.GetAxis("Horizontal") * Time.deltaTime;
                //이동확인
                isMove = inputCheck(vValue, hValue);
                //애니메이션
                pigAniCtrl.SetBool("isMove", isMove);
                //이동
                PlayerMove(vValue, hValue);
                break;
            case FarmPlayerState.PanelUP://패널켜졌을대
                //이동 제한
                vValue = 0;
                hValue = 0;
                isMove = inputCheck(vValue, hValue);
                //애니메이션
                pigAniCtrl.SetBool("isMove", isMove);
                //스프라이트 투명화
                sprite.color = new Vector4(255, 255, 255, 0);
                break;
        }

    }


    #region Panel

    public void PanelUP()
    {//패널이 올라갈 경우 실행
        //상태
        farmPlayerState = FarmPlayerState.PanelUP;
        //이미지 투명화
        sprite.color = new Vector4(255, 255, 255, 0);
    }
    public void PanelDown()
    {//패널이 내려갈 경우 실행
        //상태
        farmPlayerState = FarmPlayerState.Idle;
        //이미지 투명화
        sprite.color = new Vector4(255, 255, 255, 255);
    }
    #endregion

    #region MoveInput



    private bool inputCheck(float vValue , float hValue)
    {
        if (vValue == 0 && hValue == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void PlayerMove(float vValue , float hValue)
    {
        //방향 저장
        Quaternion rightDir = Quaternion.Euler(0, 0, 0);
        Quaternion leftDir = Quaternion.Euler(0, 180, 0);
       
        //움직임 확인
        if (vValue == 0 && hValue == 0)
        {//움직이지 않을경우
            isMove = false;
            farmPlayerState = FarmPlayerState.Idle;
            return;
        }
        else
        {
            //움직이면
            isMove = true;
            farmPlayerState = FarmPlayerState.Move;
            //스프라이트 방향전환
            if (hValue < 0)
            {
                this.gameObject.transform.rotation = leftDir;
            }
            else if(hValue > 0)
            {

                this.gameObject.transform.rotation = rightDir;
            }
            //화면 밖으로 나가는지 체크
            positionCheck();
            //이동
            this.gameObject.transform.position += new Vector3(hValue * Speed, vValue * Speed, 0);
                
        }
    }

    
    public void positionCheck()
    {//화면 밖으로 나가는지 체크

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.x < 0.03f) pos.x = 0.03f;

        if (pos.x > 0.97f) pos.x = 0.97f;

        if (pos.y < 0.05f) pos.y = 0.05f;

        if (pos.y > 0.95f) pos.y = 0.95f;

        this.transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    #endregion

}
