using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCombatManager : MonoBehaviour
{
    [SerializeField]
    private Collider PreviousClickTrans;
    [SerializeField]
    private Collider CurrentClickTrans;
    //현재 클릭 위치

    public void MouseClickInput()
    {//마우스 클릭시 호출 함수
        Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit click;
        if(Physics.Raycast(ray,out click,100f))
        {//충돌 물체로 부터 콜라이더 추출 
            click.transform.GetComponent<BoxCollider>();
        }
    }
    public void MoveChar(Collider afterTrans,GameObject riot)
    {//캐릭터 이동
        riot.transform.position = Vector3.MoveTowards(riot.transform.position, afterTrans.transform.position, Time.deltaTime);
        //이동속도 조절을 timedeltatime에서 조절
    }

}
