using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
public enum SceneName
{
    Farm,
    BattleField
}

public class ButtonClick : MonoBehaviour
{
    public PlayerInfo playerInfo;
    public SceneName sceneName;

    private static GameObject houseUI;//정적 변수 SetActive때문에
    private static GameObject gateUI;
    private static GameObject barnUI;
    private static GameObject closePanelButton;
    private static GameObject changePageButton;
    private static GameObject animalCountPanel;

    
    private string buttonName;//현재 버튼이름 저장 
    //gate
    private string imageName;//현재 이미지 이름
    private int imageNum=0;//현재 이미지의 숫자
    private GameObject[] animalPanel;//패널들의 배열
    private int animalPanelPage = 0;//동물 선택 페이지  
    //gate-slot
    private bool isSlotset = false;//슬롯에 들어갔는지 확인을 위한 bool값
    private GameObject currentSlotObj;//현재 슬롯의 오브젝트
    private int currentSlotID;//현재 슬롯의 id
    //gate-animalcountpanel
    public float currentAnimalCount;//현재 동물의 개체수
    public float combatAnimalCount;//전투에 사용될 개체수

    

    private void Awake()
    {
        SceneCheck();//현재 씬 확인
        if (sceneName==SceneName.Farm)
        {//농장씬일 경우
            houseUI = GameObject.FindGameObjectWithTag("HouseUI");
            houseUI.SetActive(false);
            animalCountPanel = GameObject.Find("AnimalCountPanel");
            animalCountPanel.SetActive(false);
            gateUI = GameObject.FindGameObjectWithTag("GateUI");
            gateUI.SetActive(false);
            barnUI = GameObject.FindGameObjectWithTag("BarnUI");
            barnUI.SetActive(false);
        }
    }

    private void SceneCheck()
    {//switch를 이용하여 씬을 확인
        switch(SceneManager.GetActiveScene().name)
        {
            case "Farm":
                sceneName = SceneName.Farm;
                break;
            case "BattleField":
                sceneName = SceneName.BattleField;
                break;
        }
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
       //슬롯 초기화
       for(int slotNum=0;slotNum<6;slotNum++)
       {
            PlayerPrefs.SetInt(("Slot" + slotNum), 0);
            currentSlotObj = GameObject.FindGameObjectWithTag("Slot" + slotNum);
            currentSlotObj.GetComponent<Image>().sprite = null;//리소스의 이미지 가져오기//추후에 +"" 로 이름을 수정가능//초기 이미지로 변경
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
    public void AnimalSelect()
    {//이미지 이름을 이용하여 캐릭터 코드를 추출
        imageName = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite.name;
        if (int.TryParse(imageName, out imageNum))
        {//string->int화
            imageNum = imageNum;
        }
        currentAnimalCount=PlayerPrefs.GetInt(imageNum + "Count");//클릭한 동물의 개체수 호출
        animalCountPanel.SetActive(true);//패널 활성화//animalcount패널로 이동
    }
    #region AnimalCountFuncGroup
    public void CombatCountConfirm()
    {
        //확인버튼을 눌렀을 경우
        animalCountPanel.GetComponentInChildren<Scrollbar>().numberOfSteps = (int)currentAnimalCount;
        float countScrollCount = animalCountPanel.GetComponentInChildren<Scrollbar>().value;
        combatAnimalCount = currentAnimalCount * countScrollCount;//숫자 확인
        if(countScrollCount!=0)
        {// 스크롤바가 0이 아닐 경우 실행되는 코드
            PlayerPrefs.SetInt(imageNum + "CombatCount", (int)combatAnimalCount);//전투에 사용하는 개체 숫자에 입력
            AnimalSlotSetting(imageNum);//슬롯에 집어 넣기
        }
        animalCountPanel.SetActive(false);
    }
    public void CombatCountCancel()
    {
        //취소버튼을 눌렀을 경우
        animalCountPanel.SetActive(false);
    }

    #endregion

    public void AnimalSlotSetting(int charID)
    {//id를 받아서 슬롯에 빈공간에 넣는 코드
        //슬롯 빈 공간 확인
        for (int slotNum = 0; slotNum <= 5; slotNum++)
        {//슬롯 돌리기
            currentSlotID = PlayerPrefs.GetInt("Slot" + slotNum);
            if (currentSlotID == 0)
            {//초기화 된 슬롯의 값은 무조건 0
                //playerInfo.SetSlotChar(slotNum, charID);                            문제를 모르겠음//
                PlayerPrefs.SetInt(("Slot" + slotNum), charID);//슬롯에 값을 저장
                Debug.Log(PlayerPrefs.GetInt("Slot" + slotNum));
                currentSlotObj = GameObject.FindGameObjectWithTag("Slot" + slotNum);//여분의 슬롯에 이미지
                currentSlotObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(imageName);//리소스의 이미지 가져오기//추후에 +"" 로 이름을 수정가능
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
                PlayerPrefs.SetInt(("Slot" + 0), 0);
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = null;//리소스의 이미지 가져오기//추후에 +"" 로 이름을 수정가능//초기 이미지로 변경
                break;
            case "Slot1":
                PlayerPrefs.SetInt(("Slot" + 1), 0);
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = null;
                break;
            case "Slot2":
                PlayerPrefs.SetInt(("Slot" + 2), 0);
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = null;
                break;
            case "Slot3":
                PlayerPrefs.SetInt(("Slot" + 3), 0);
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = null;
                break;
            case "Slot4":
                PlayerPrefs.SetInt(("Slot" + 4), 0);
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = null;
                break;
            case "Slot5":
                PlayerPrefs.SetInt(("Slot" + 5), 0);
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = null;
                break;
        }

    }

    public void GameStartButton()
    {//게임 시작 버튼
        SceneManager.LoadScene("BattleField");
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
