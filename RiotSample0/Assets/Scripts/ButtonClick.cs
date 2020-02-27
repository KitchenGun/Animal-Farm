using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Reflection;

public class ButtonClick : MonoBehaviour
{
    private PlayerInfo playerInfo;

    private static GameObject houseUI;//정적 변수 SetActive때문에
    private static GameObject gateUI;
    private static GameObject barnUI;
    private static GameObject closePanelButton;
    private static GameObject changePageButton;

    private string buttonName;//현재 버튼이름 저장 
    private string ImageName;//현재 이미지 이름
    private GameObject[] animalPanel;//패널들의 배열
    private int animalPanelPage = 0;//동물 선택 페이지  
    bool isSlotset = false;//슬롯에 들어갔는지 확인을 위한 bool값



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

    #region HouseUIFuncGroup
    public void HouseButton()
    {//집 버튼을 클릭할 경우 사용할 함수 
        houseUI.SetActive(true);//패널 활성화
        closePanelButton= GameObject.FindGameObjectWithTag("ClosePanelButton");//패널 생성시 확인
    }
    #endregion

    #region GateFuncGroup
    public void GateButton()
    {//게이트 버튼을 클릭할 경우 사용할 함수
        gateUI.SetActive(true);//패널 활성화
        closePanelButton = GameObject.FindGameObjectWithTag("ClosePanelButton");//패널 생성시 확인
        //캐릭터 패널 확인하기   
        animalPanel = GameObject.FindGameObjectsWithTag("AnimalPanel");
       //캐릭터 패널 배열 정리
       for(int compareMainNum=0;compareMainNum==animalPanel.Length;compareMainNum++)
       {//compare(a,b)의 a에 해당
            for(int compareNum=1;compareNum==animalPanel.Length;compareNum++)
            {//compare(a,b)의 b에 해당
                ListGameObjectSort(animalPanel[compareMainNum], animalPanel[compareNum]);
            }
       }
       for(int panelNum =0;panelNum<animalPanel.Length;panelNum++)
       {//패널 위치 움직이기
            if (panelNum != animalPanelPage)
            {//현재일치하는 패널이 아니라면
                animalPanel[panelNum].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 400, 0);
            }
            else if(panelNum==animalPanelPage)
            {
                animalPanel[panelNum].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -400, 0);
            }
       }
       
    }
    public void AnimalPanelNextPage()
    {
        if (animalPanelPage != animalPanel.Length)
        { 
            //패널의 위치를 옮겨서 위치를 조정한다
            animalPanel[animalPanelPage].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 400, 0);
            animalPanelPage=NextPage(animalPanelPage);
            animalPanel[animalPanelPage].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -400, 0);
            Debug.Log(animalPanelPage);
        }
    }
    public void AnimalPanelPreviousPage()
    {
        if (animalPanelPage != 0)
        {
            animalPanel[animalPanelPage].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 400, 0);
            animalPanelPage=PreviousPage(animalPanelPage);
            animalPanel[animalPanelPage].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -400, 0);
        }
    }
    
    public void AnimalSlotSetting(int charID)
    {//id를 받아서 슬롯에 빈공간에 넣는 코드
        //슬롯 빈 공간 확인
        for (int slotNum = 0; slotNum <= 5; slotNum++)
        {//슬롯 돌리기 
            if (playerInfo.GetSlotChar(slotNum) == 0)
            {//초기화 된 슬롯의 값은 무조건 0
                playerInfo.SetSlotChar(slotNum, charID);
                isSlotset = true;
                break;
            }
            else
            {
                Debug.Log(slotNum + "번째 슬롯은 현재 사용중");
            }

        }
        if(isSlotset==false)
        {//슬롯에 값이 들어가지 않을 경우
            Debug.Log("슬롯 초기화가 필요함");
        }
    }

    public void AnimalSelect()
    {//이미지 이름을 이용하여 캐릭터 코드를 추출
        ImageName=EventSystem.current.currentSelectedGameObject.GetComponent<Image>().name;
        AnimalSlotSetting(int.Parse(ImageName));//int형으로 변경
    }

    public void Slot()
    {//슬롯 버튼을 클릭하였을 경우
        string slotName =EventSystem.current.currentSelectedGameObject.tag;
        SlotCleanUP(slotName);
    }
    public void SlotCleanUP(string SlotName)
    {//슬롯 초기화
        switch (SlotName)
        {//tag를 이용하여서 슬롯찾고 초기화
            case "Slot0":
                playerInfo.SetSlotChar(0, 0);
                break;
            case "Slot1":
                playerInfo.SetSlotChar(1, 0);
                break;
            case "Slot2":
                playerInfo.SetSlotChar(2, 0);
                break;
            case "Slot3":
                playerInfo.SetSlotChar(3, 0);
                break;
            case "Slot4":
                playerInfo.SetSlotChar(4, 0);
                break;
            case "Slot5":
                playerInfo.SetSlotChar(5, 0);
                break;
        }

    }

    #endregion


    #region BarnUIFuncGroup
    public void BarnButton()
    {//헛간 버튼을 클릭할 경우 사용할 함수

    }
    #endregion

    #region CommonUIGroup

    //animalpanel의 변환 방법 버튼문제해결필요
    public int ListGameObjectSort(GameObject compareMainObj, GameObject compareSubObject)
    {
        string compareMainObjName;
        string compareSubObjectName;
        //이름 임시변수에넣기
        compareMainObjName = compareMainObj.name;
        compareSubObjectName = compareSubObject.name;
        return compareMainObjName.CompareTo(compareSubObjectName);
    }

    public void ClosePanelButton()
    {//모든 패널 비활성화
        houseUI.SetActive(false);
        gateUI.SetActive(false);
        barnUI.SetActive(false);
    }

    public int NextPage(int page)
    {//다음 페이지
        return ++page;
    }
    public int PreviousPage(int page)
    {//이전 페이지
        return --page;
    }
    #endregion
}
