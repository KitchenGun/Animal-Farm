using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Reflection;

public class ButtonClick : MonoBehaviour
{
    private static GameObject houseUI;//정적 변수 SetActive때문에
    private static GameObject gateUI;
    private static GameObject barnUI;
    private static GameObject closePanelButton;
    private static GameObject changePageButton;


    private int AnimalPanelPage = 0;//동물 선택 페이지
    private string buttonName;//현재 버튼이름 저장 
    private GameObject[] AnimalPanel;


    private void Awake()
    {
        houseUI = GameObject.FindGameObjectWithTag("HouseUI");
        houseUI.SetActive(false);
        gateUI = GameObject.FindGameObjectWithTag("GateUI");
        gateUI.SetActive(false);
        barnUI= GameObject.FindGameObjectWithTag("BarnUI");
        barnUI.SetActive(false);

    }


    public void UIButtonClick()
    {//버튼 클릭시 실행함수
        buttonName = EventSystem.current.currentSelectedGameObject.name;//현재 클릭한 오브젝트 이름
        //string 문으로 함수 실행 
        SendMessage(buttonName);
    }
    public void HouseButton()
    {//집 버튼을 클릭할 경우 사용할 함수 
        houseUI.SetActive(true);//패널 활성화
        closePanelButton= GameObject.FindGameObjectWithTag("ClosePanelButton");//패널 생성시 확인
    }

    public void GateButton()
    {//게이트 버튼을 클릭할 경우 사용할 함수
        gateUI.SetActive(true);//패널 활성화
        closePanelButton = GameObject.FindGameObjectWithTag("ClosePanelButton");//패널 생성시 확인
        changePageButton = GameObject.FindGameObjectWithTag("ChangePageButton");//패널 생성시 확인
        
    }
    //animalpanel의 변환 방법 버튼문제해결필요
    //public int ListGameObjectSort(GameObject a, GameObject b)
    //{
    //    string aS;
    //    string bS;
    //    //이름 임시변수에넣기
    //    aS = a.name;
    //    bS = b.name;
    //    return aS.CompareTo(bS);
    //}

    public void BarnButton()
    {//헛간 버튼을 클릭할 경우 사용할 함수

    }

    public void ClosePanelButton()
    {//모든 패널 비활성화
        houseUI.SetActive(false);
        gateUI.SetActive(false);
        barnUI.SetActive(false);
    }

    public void ChangePageButton()
    {
        if(AnimalPanelPage==0)
        {
            
        }
        else if(AnimalPanelPage==1)
        {

        }
        else
        {//페이지 범위를 초과함
            Debug.Log("bug");
        }
    }

}
