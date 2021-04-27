using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using System;

public enum SceneName
{
    Farm,
    BattleField
}

public class ButtonClick : MonoBehaviour
{
    [SerializeField]
    private PlayerInfo playerInfo;
    [SerializeField]
    private PlayerCtrl playerCtrl;
    [HideInInspector]
    public SceneName sceneName;

    #region MainButton
    private GameObject house;
    private GameObject barn;
    private GameObject gate;
    private GameObject waterTower;
    #endregion

    #region Panel
    private static GameObject houseUI;//정적 변수 SetActive때문에
    private static GameObject gateUI;
    private static GameObject barnUI;
    private static GameObject WatchTowerUI;
    private static GameObject closePanelButton;
    private static GameObject changePageButton;
    private static GameObject animalCountPanel;
    private static GameObject escUI;
    private bool PanelOn=false;
    #endregion

    private string buttonName;//현재 버튼이름 저장 

    #region house
    public Sprite[] HouseImg;
    #endregion
    #region gate
    private string imageName;//현재 이미지 이름
    private int imageNum=0;//현재 이미지의 숫자
    private GameObject[] animalPanel;//패널들의 배열
    #endregion
    #region gate-slot
    private bool isSlotset = false;//슬롯에 들어갔는지 확인을 위한 bool값
    private GameObject currentSlotObj;//현재 슬롯의 오브젝트
    private int currentSlotID;//현재 슬롯의 id
    #endregion
    #region gate-animalcountpanel
    [HideInInspector]
    public float currentAnimalCount;//현재 동물의 개체수
    [HideInInspector]
    public float combatAnimalCount;//전투에 사용될 개체수
    #endregion
    #region timer
    [SerializeField]
    private Text Timer;//타이머
    private bool timerRunning=true;//시작체크
    private float currentTime;//현재시간
    private float prepareTime=50f;//준비시간
    #endregion
    //Wheat
    //[SerializeField]
    //private Text WheatText;//텍스트 오브젝트
    private int WheatCount;//밀의 수
    #region Script
    private int Phase;
    private int Branch;
    private int Count;
    private ScriptManager sm;
    private CharacterImage charImage;
    #endregion

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (PanelOn == false)//패널이 꺼져있을 경우에 메인씬으로 돌아가기가능
            {/////게이트 예외처리 필요
                if (gateUI.activeSelf != true)
                {
                    ESCUI();
                }
            }
            else
            {
                ClosePanelButton();
            }
        }

        if(barnUI.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                NextScript();
            }
        }
        //시간 관련 업데이트 함수
        if(timerRunning == true)
        {//타이머가 시작될경우
            currentTime += Time.deltaTime;
            Timer.text = Math.Truncate(prepareTime - currentTime).ToString();
            if(currentTime >= prepareTime)
            {//공격 준비 시간이 끝날경우
                Debug.Log("Done");
                timerRunning = false;
                SceneManager.LoadScene("BattleField");
            }
        }
    }

    private void Awake()
    {
        SceneCheck();//현재 씬 확인
        ScriptCheck();//현재 스크립트 단계 확인

       
        if (sceneName==SceneName.Farm)
        {
            //농장씬일 경우
            sm = GameObject.Find("ScriptManager").GetComponent<ScriptManager>();
            
            houseUI = GameObject.FindGameObjectWithTag("HouseUI");
            houseUI.SetActive(false);
            animalCountPanel = GameObject.Find("AnimalCountPanel");
            animalCountPanel.SetActive(false);
            gateUI = GameObject.FindGameObjectWithTag("GateUI");
            gateUI.SetActive(false);
            barnUI = GameObject.FindGameObjectWithTag("BarnUI");
            barnUI.SetActive(false);
            WatchTowerUI = GameObject.FindGameObjectWithTag("WatchTowerUI");
            WatchTowerUI.SetActive(false);
            escUI = GameObject.FindGameObjectWithTag("ESCUI");
            escUI.SetActive(false);
            //스프라이트 패커 사용
            SpriteSheetManager.Load("SlotImage");
            //시간 관련 부분
            currentTime = Time.time;//현재시간
            prepareTime = currentTime + prepareTime; //준비시간 = 현재시간 + 준비시간(3분)
        }
    }
    private void Start()
    {
        Invoke("StoryProgress", 0.1f);//스토리 진행
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
        PanelOn = true;
        houseUI.SetActive(true);//패널 활성화
        playerCtrl.PanelUP();//플레이어 스프라이트 삭제
        closePanelButton = GameObject.FindGameObjectWithTag("ClosePanelButton");//패널 생성시 확인
    }
    #endregion

    #region GateFuncGroup
    public void GateButton()
    {//게이트 버튼을 클릭할 경우 사용할 함수
        PanelOn = true;
        gateUI.SetActive(true);//패널 활성화
        playerCtrl.PanelUP();//플레이어 스프라이트 삭제
        closePanelButton = GameObject.FindGameObjectWithTag("ClosePanelButton");//패널 생성시 확인
        //캐릭터 패널 확인하기   
        animalPanel = GameObject.FindGameObjectsWithTag("AnimalPanel");

        //슬롯 초기화
       for (int slotNum = 0; slotNum < 6; slotNum++) 
       {
            if (PlayerPrefs.GetInt("Slot" + slotNum) == 0)
            {
                currentSlotObj = GameObject.FindGameObjectWithTag("Slot" + slotNum);
                currentSlotObj.GetComponent<Image>().sprite = SpriteSheetManager.GetSpriteByName("SlotImage", "EmptySlot");//리소스의 이미지 가져오기
            }
        }
    }
  
    public void AnimalSelect()
    {//이미지 이름을 이용하여 캐릭터 코드를 추출
        GameObject ImageObj = EventSystem.current.currentSelectedGameObject;
        imageName = ImageObj.GetComponent<Image>().sprite.name;
        imageName.Remove(1,imageName.Length-1);
        imageName = imageName[0].ToString();
        if (int.TryParse(imageName, out imageNum))
        {//string->int화
            imageNum = imageNum;
        }
        //-----------------------------------------------------------------------
        bool sameID = false;
        for (int slotNum = 0; slotNum <= 5; slotNum++)
        {//슬롯 돌리기
            if(imageNum==PlayerPrefs.GetInt("Slot"+slotNum.ToString()))
            {//같은 아이디가 나올경우
                sameID = true;
                break;
            }
        }
        if (sameID == false)
        {//같은 아이디가 없을 경우
            currentAnimalCount = PlayerPrefs.GetInt(imageNum + "Count");//클릭한 동물의 개체수 호출
            animalCountPanel.SetActive(true);//패널 활성화//animalcount패널로 이동
        }
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
            int currentAnimalCombatCount = PlayerPrefs.GetInt(imageNum + "CombatCount");
            PlayerPrefs.SetInt(imageNum + "Count", PlayerPrefs.GetInt(imageNum + "Count") - currentAnimalCombatCount);
            Debug.Log(PlayerPrefs.GetInt(imageNum + "Count"));
            AnimalSlotSetting(imageNum, (int)combatAnimalCount);//슬롯에 집어 넣기
        }
        animalCountPanel.GetComponentInChildren<Scrollbar>().value = 0;
        animalCountPanel.SetActive(false);
    }
    public void CombatCountCancel()
    {
        //취소버튼을 눌렀을 경우
        animalCountPanel.GetComponentInChildren<Scrollbar>().value = 0;
        animalCountPanel.SetActive(false);
    }

    #endregion

    public void AnimalSlotSetting(int charID,int count)
    {//id를 받아서 슬롯에 빈공간에 넣는 코드
        //슬롯 빈 공간 확인
        for (int slotNum = 0; slotNum <= 5; slotNum++)
        {//슬롯 돌리기
            currentSlotID = PlayerPrefs.GetInt("Slot" + slotNum);
            if (currentSlotID == 0)
            {//초기화 된 슬롯의 값은 무조건 0 
                PlayerPrefs.SetInt(("Slot" + slotNum), charID);//슬롯에 값을 저장
                currentSlotObj = GameObject.FindGameObjectWithTag("Slot" + slotNum);//여분의 슬롯에 이미지
                currentSlotObj.GetComponent<Image>().sprite = 
                    SpriteSheetManager.GetSpriteByName("SlotImage", charID.ToString());//리소스의 이미지 가져오기
                currentSlotObj.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = count.ToString();
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
        int slotID = PlayerPrefs.GetInt(SlotName);//해당하는 슬롯의 캐릭터 정보를 파악
        PlayerPrefs.SetInt(slotID + "Count", PlayerPrefs.GetInt(slotID + "Count") + PlayerPrefs.GetInt(slotID + "CombatCount"));//현재 동물의 수 = 기존 동물의 수 + 현재 슬롯에 들어있는 동물 수
        PlayerPrefs.SetInt(SlotName, 0);
        EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = SpriteSheetManager.GetSpriteByName("SlotImage", "EmptySlot"); ;//리소스의 이미지 가져오기//추후에 +"" 로 이름을 수정가능//초기 이미지로 변경
        EventSystem.current.currentSelectedGameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "0";
    }

    public void GameStartButton()
    {//게임 시작 버튼
        SceneManager.LoadScene("BattleField");
        PlayerPrefs.SetInt("Stage", 1);//스테이지 저장 추후에 수정필요
    }
    #endregion

    #region BarnUIFuncGroup
    public void BarnButton()
    {//헛간 버튼을 클릭할 경우 사용할 함수
        PanelOn = true;
        timerRunning = false;//타이머 
        barnUI.SetActive(true);//패널 활성화
        playerCtrl.PanelUP();//플레이어 스프라이트 삭제
        closePanelButton = GameObject.FindGameObjectWithTag("ClosePanelButton");//패널 생성시 확인
        //캐릭터 패널 확인하기
        charImage = barnUI.transform.Find("CharacterImage").GetComponent<CharacterImage>();
        //현재 스크립트의 정보를 불러옴
        ScriptCheck();
        Script tempScript = sm.Find(Phase, Branch, Count);
        //텍스트 출력
        barnUI.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = tempScript.Content;
        barnUI.transform.GetChild(2).GetChild(0).GetComponentInChildren<Text>().text = tempScript.CharID;
        //이미지 선택
        charImage.FaceChange(tempScript.CharID, tempScript.Face);
    }

    private void ScriptCheck()
    {
        Phase = PlayerPrefs.GetInt("Phase");
        Branch = PlayerPrefs.GetInt("Branch");
        Count = PlayerPrefs.GetInt("Count");
    }

    public void NextScript()
    {
        PlayerPrefs.SetInt("Count", ++Count);//다음 대사로 변경
        //갱신
        ScriptCheck();
        Script tempScript = sm.Find(Phase, Branch, Count);
        try
        {
            //텍스트 출력
            barnUI.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = tempScript.Content;
            barnUI.transform.GetChild(2).GetChild(0).GetComponentInChildren<Text>().text = tempScript.CharID;
            //이미지 선택                                                                
            charImage.FaceChange(tempScript.CharID, tempScript.Face);
        }
        catch
        {
            if (Phase == 0)
            {
                //게임오브젝트 접근&해제    대사마다 다르게 해야할듯 
                GameObjectDisable(barn);//헛간 더이상 접근 못하게 비활성화
                GameObjectEnable(waterTower);
                GameObjectEnable(gate);

                GameObject button = GameObject.Find("Canvas").transform.Find("Barn").gameObject;
                button.GetComponent<BoxCollider>().enabled = false;
                button.GetComponentInChildren<Button>().enabled = false;
                ClosePanelButton();
            }
            else if(Phase == 1)
            {
                if (Branch == 0)
                {
                    //게임오브젝트 접근&해제    
                    GameObjectDisable(barn);//헛간 더이상 접근 못하게 비활성화
                    GameObjectEnable(house);
                    GameObjectEnable(waterTower);
                    GameObjectEnable(gate);
                    Branch = 2; 
                    Count = 9;
                    ClosePanelButton();
                }
                else if (Branch == 2)
                {
                    //게임오브젝트 접근&해제    
                    GameObjectDisable(barn);//헛간 더이상 접근 못하게 비활성화
                    GameObjectDisable(house);
                    GameObjectEnable(waterTower);
                    GameObjectEnable(gate);
                }
                GameObject button = GameObject.Find("Canvas").transform.Find("Barn").gameObject;
                button.GetComponent<BoxCollider>().enabled = false;
                button.GetComponentInChildren<Button>().enabled = false;
                ClosePanelButton();
            }
            else if(Phase == 2)
            {

            }
        }
    }
    #endregion

    #region WatchTowerUIFuncGroup
    public void WatchTowerButton()//감시탑 클릭시
    {
        PanelOn = true;
        timerRunning = false;//타이머 
        WatchTowerUI.SetActive(true);//패널 활성화
        playerCtrl.PanelUP();//플레이어 스프라이트 삭제
        closePanelButton = GameObject.FindGameObjectWithTag("ClosePanelButton");//패널 생성시 확인
        Timer.text = Math.Truncate(prepareTime - currentTime).ToString()+"초 뒤에 적이 도착할거 같아요!";
    }

    #endregion

    #region ESCUIFuncGroup
    public void ESCUI()//esc 키 입력시 
    {
        PanelOn = true;
        timerRunning = false;//타이머 
        escUI.SetActive(true);
        playerCtrl.PanelUP();//플레이어 스프라이트 삭제
    }

    public void Confirm()
    {
        SceneManager.LoadScene(0);
    }

    public void Cancel()
    {
        ClosePanelButton();
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
        PanelOn = false;
        playerCtrl.PanelDown();
        houseUI.SetActive(false);
        gateUI.SetActive(false);
        CombatCountCancel();
        barnUI.SetActive(false);
        escUI.SetActive(false);
        timerRunning = true;
        WatchTowerUI.SetActive(false);
    }

    public int NextPage(int page)
    {//다음 페이지
        return ++page;
    }
    public int PreviousPage(int page)
    {//이전 페이지
        return --page;
    }
    
    private void GameObjectDisable(GameObject Target)
    {
        Target.GetComponent<BoxCollider>().enabled=false;
        Target.transform.GetChild(0).GetComponent<Button>().enabled = false;
    }
    private void GameObjectEnable(GameObject Target)
    {
        Target.GetComponent<BoxCollider>().enabled = true;
        Target.transform.GetChild(0).GetComponent<Button>().enabled = true;
    }
    #endregion



    #region StoryProgress

    void StoryProgress()
    {
        ScriptCheck();
        //버튼에 접근 하고 싶으면 하위오브젝트에 접근 필요 
        gate = this.gameObject.transform.Find("Gate").gameObject;
        house = this.gameObject.transform.Find("House").gameObject;
        barn = this.gameObject.transform.Find("Barn").gameObject;
        waterTower = this.gameObject.transform.Find("WatchTower").gameObject;


        if (Phase == 0) 
        {
            house.GetComponent<Image>().sprite = HouseImg[0];
            GameObjectDisable(gate);
            GameObjectDisable(house);
            GameObjectDisable(waterTower);
            BarnButton();
        }
        else if (Phase == 1)
        {
            house.GetComponent<Image>().sprite = HouseImg[1];
            GameObjectDisable(gate);
            GameObjectDisable(house);
            GameObjectDisable(waterTower);
            BarnButton();
        }
        else if (Phase == 2) 
        {
            house.GetComponent<Image>().sprite = HouseImg[2];
        }
    }

    #endregion
}
