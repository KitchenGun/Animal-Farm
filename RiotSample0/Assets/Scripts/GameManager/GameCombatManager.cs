using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCombatManager : MonoBehaviour
{
    //이전 클릭 위치
    [SerializeField]
    private Collider PreviousClickTrans;
    //현재 클릭 위치
    [SerializeField]
    private Collider CurrentClickTrans;
    //움직일수 있는지 확인용
    private bool isMove;

    private void Start()
    {
        //초기화
        isMove = false;
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("클릭됨");
            CombatClickInput();
        }
        if (isMove)
        {
            MoveChar(CurrentClickTrans, PreviousClickTrans.transform.GetChild(0).gameObject);
        }
    }
    public void CombatClickInput()
    {//전투상황시 클릭 호출 함수
        Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit click;
        if(Physics.Raycast(ray,out click,100f))
        {//충돌 물체로 부터 콜라이더 추출 하여 현재 콜라이더에 대입
            if(click.transform.GetComponent<BoxCollider>())
            {
                if (CurrentClickTrans == null)
                {//현재 콜라이더로 선택 된것이 없을 경우
                    Debug.Log("현재 콜라이더 비어있음");
                    CurrentClickTrans = click.transform.GetComponent<BoxCollider>();
                }
                else if (CurrentClickTrans != null)
                {
                    Debug.Log("현재콜라이더 교체");
                    if (CurrentClickTrans.transform.childCount!=0)
                    {//이전에 선택한 위치가 캐릭터가 존재할 경우
                        Debug.Log("이전에 선택한 위치에 캐릭터 존재 교체 가능");
                        PreviousClickTrans = CurrentClickTrans;
                        isMove = true;
                    }
                    CurrentClickTrans = click.transform.GetComponent<BoxCollider>();
                }
            }

        }
    }
    public void MoveChar(Collider afterTrans,GameObject riot)
    {//캐릭터 이동
        Debug.Log("움직이는중");
        riot.transform.position = Vector3.MoveTowards(riot.transform.position, afterTrans.transform.position, 20*Time.deltaTime);
        //이동속도 조절을 timedeltatime에서 조절
        if(riot.transform.position==afterTrans.transform.position)
        {
            isMove = false;
            riot.transform.parent = afterTrans.transform;
        }
    }

}
