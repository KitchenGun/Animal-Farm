using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Reflection;

public class ButtonClick : MonoBehaviour
{
    private static GameObject houseUI;//정적 변수 SetActive때문에


    private string buttonName;//현재 버튼이름 저장 

    private void Awake()
    {
        houseUI = GameObject.FindGameObjectWithTag("HouseUI");
        houseUI.SetActive(false);
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
    }



}
