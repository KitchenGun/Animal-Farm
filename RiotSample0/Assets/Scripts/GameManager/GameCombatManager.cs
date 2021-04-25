using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//public class RetreatArrayDic
//{//후퇴한 동물 정보 저장용 클래스
//    string CharID;
//    int ArrayNum=0;
//    int HP;
//    public RetreatArrayDic(string charID,int arrayNum,int hp)
//    {
//
//    }
//}


public enum PlayerState
{
    Hold,//아무것도 못하고 대화만 보는 정지 상태 적 또한 정지한다
    Select,//슬롯의 버튼을 눌렀거나  유닛 콜라이더를 선택하는 상황
    Deploy,//배치를 할 경우에 콜라이더를 선택하는 상황
    Fire,//발사를 위해서 콜라이더를 선택하는 상황
    Play//정상적인 플레이 상태
}

public class GameCombatManager : MonoBehaviour
{
    public PlayerInfo playerInfo;//플레이어 정보
    public GameLoadManager gameLoadManager;
    public PlayerState playerState;//플레이어상태

    //public static GameObject[] Line;//배치 라인 오브젝트 
    public static GameCombatManager GM;
    public static List<GameObject> Line = new List<GameObject>();
    private GameObject mainLine;
    private GameObject compareLine;
    private float VerticalInput;

    //포격 관련 변수
    public static List<GameObject> ArtyPos = new List<GameObject>();//포격위치 
    private GameObject mainArtyPos;
    private GameObject compareArtyPos;
    private bool isArtyCoroutinRunning = false;
    public int pressSpaceBar=1;//스페이스바를 누른시간

    //공습 관련 변수
    private GameObject airStrikeBird;//폭탄을 떨굴 새 오브젝트 변수

    //배치 관련 변수
    private string buttonName;//버튼의 이름
    private int charID;//캐릭터 id
    private string slotNum;//슬롯 숫자
    private int tempSlotNum = 0;//임시로 슬롯넘버가 들어가는 변수
    private int[] slotNumID=new int[6];//슬롯에 저장되어있는 id
    private int selectLine;//돼지가 있는 라인
    private bool isDeployCoroutinRunning = false;//입력 중복을 막기 위한 변수

    //후퇴관련
    //private Dictionary<string,int> Animal

   
        
    //현재 입력된 키
    private KeyCode inputValue;

    //outline관련변수
    private bool outlineSizeUP;

    
    //돼지 이동 관련 변수
    private GameObject PigObj;
    private GameObject selectLineObj;

    //과거의 선택 위치
    [SerializeField]
    private Collider PreviousClickTrans;
    
    //현재 클릭 위치
    [SerializeField]
    private Collider CurrentClickTrans;
    
    //클릭한 위치의 게임 오브젝트
    private GameObject previousSelectGameObj;
    private GameObject selectGameObj;

    //땅에 파붙히지 않기 위한 변수
    private Vector3 BasicPos = new Vector3(0, 1.2f, 0);

    //움직일수 있는지 확인용
    public bool isMove;
    private bool reverse;

    //일시정지
    private bool isPause;
    [SerializeField]
    private GameObject PausePanel;

    //승리 패배 패널
    [SerializeField]
    private GameObject WinPanel;
    [SerializeField]
    private GameObject LosePanel;

    //죽은 동물 수 관련 변수
    private int[] deadAnimalCount=new int[10];
    private int[] SpawnCount = new int[10];


    public void Start()
    {
        Cursor.visible = false;
        playerState = PlayerState.Play;/////////////////////임시 나중에 Hold로 교체가 필요함
        StartCoroutine(GameTimeSet());//게임 타임 설정
        GM = this.gameObject.GetComponent<GameCombatManager>();
        PausePanel.SetActive(false);//게임 정지 패널
        WinPanel.SetActive(false);//게임 승리 패널
        LosePanel.SetActive(false);//게임 패배 패널
        #region lineFindCode
        Line = new List<GameObject> (GameObject.FindGameObjectsWithTag("Line"));
        foreach (GameObject LineGameObj in Line)
        {//순서정리
            compareLine = LineGameObj;
            if (mainLine != null)
            {//정렬실행
                Line.Sort(ListGameObjectSort);
            }
            mainLine = LineGameObj;
        }
        foreach (GameObject line in Line)
        {//라인 오브젝트 끄기
            line.SetActive(false);
        }
        selectLineCheck();//돼지의 시작 위치확인
        #endregion
        #region
        airStrikeBird = Resources.Load<GameObject>("Bird");
        #endregion
        #region ArtyPosFindCode
        ArtyPos = new List<GameObject>(GameObject.FindGameObjectsWithTag("ArtyPos"));
        foreach (GameObject ArtyPosObj in ArtyPos)
        {//순서정리
            compareArtyPos = ArtyPosObj;
            if (mainArtyPos != null)
            {//정렬실행
                ArtyPos.Sort(ListGameObjectSort);
            }
            mainArtyPos = ArtyPosObj;
        }
        foreach (GameObject ArtyPosObj in ArtyPos)
        {//라인 오브젝트 끄기
            ArtyPosObj.SetActive(false);
        }


        #endregion
    }

    private void Update()//플레이어의 입력을 받기위한 업데이트
    {
        #region PauseInput
        if(Input.GetKeyDown(KeyCode.Escape)&&!isPause)
        {//정지인지 확인
            isPause = true;
            Cursor.visible = true;//커서 
            Time.timeScale = 0f;//시간
            //패널
            PausePanel.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPause)
        {
            isPause = false;
            Cursor.visible = false;
            Time.timeScale = 1f;
            PausePanel.SetActive(false);
        }
        #endregion
        #region MoveInput
        VerticalInput = Input.GetAxisRaw("Vertical");//위 아래키 입력값
        if (VerticalInput >= 1 && isMove==false)
        {
            pigMoveUp();
        }
        if (VerticalInput <= -1 && isMove==false)
        {
            pigMoveDown();
        }
        #endregion
        #region SlotInput
        foreach (KeyCode keyname in Enum.GetValues(typeof(KeyCode)))
        {
            if (KeyCode.Alpha0 <= keyname && keyname <= KeyCode.Alpha6)
            {
                if (Input.GetKey(keyname))
                {
                    inputValue = keyname;
                    if (inputValue >= KeyCode.Alpha1 && inputValue <= KeyCode.Alpha6)
                    {//키값이 숫자1~6까지일 경우 실행
                        StartCoroutine(InputCoolTime());
                    }
                }
            }
        }
        #endregion
        #region Artyinput
        //포격 키입력
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ArtyPos[selectLine].SetActive(true);
            ArtyPos[selectLine].transform.parent.GetComponent<Animator>().SetTrigger("isPress");
        }
        if (Input.GetKey(KeyCode.Space))
        {

            if (pressSpaceBar <= 100 && !isArtyCoroutinRunning)
            {
                pressSpaceBar++;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ArtyPos[selectLine].transform.parent.GetComponent<Animator>().SetTrigger("isFire");
            StartCoroutine(InputFireCoolTime());
        }
        #endregion
        #region ClickMove
        if (Input.GetMouseButtonDown(0))
        {//마우스 클릭시
            if (!EventSystem.current.currentSelectedGameObject)
            {//클릭한 오브젝트가 버튼 같은 이벤트 입력이 아닐 경우 아래 구문 실행
                switch (playerState)
                {
                    case PlayerState.Hold:
                        break;
                    case PlayerState.Select://이동
                        selectColiderFind();
                        break;
                    case PlayerState.Deploy://배치
                        DeploySelectColiderFind();
                        break;
                    case PlayerState.Fire:
                        fireSelectColiderFind();//사격 지점 선택
                        break;
                    case PlayerState.Play://게임 플레이중
                        playColiderFind();
                        break;
                }
            }
        }
        if (isMove)
        {//움직이는 코드
            if (selectGameObj != null)
            {
                selectGameObj.GetComponent<SpriteOutline>().outlineSize = 0;//아웃 라인 제거
            }
            if (playerState == PlayerState.Select)
            {//선택 상태에서 움직일 경우 플레이상태로 변경
                playerState = PlayerState.Play;
            }
            if (CurrentClickTrans.gameObject.transform.childCount > 0)
            {//이동할 곳에 오브젝트가 있으면
                reverse = true;
                MoveChar(PreviousClickTrans, CurrentClickTrans.transform.GetChild(0).gameObject, reverse);
            }
            reverse = false;
            MoveChar(CurrentClickTrans, PreviousClickTrans.transform.GetChild(0).gameObject, reverse);
        }
        #endregion
        #region Retreat
        if (Input.GetKeyDown(KeyCode.T))
        {
            Retreat();
        }
        #endregion
    }

    public int ListGameObjectSort(GameObject a, GameObject b)
    {//정렬함수
        string aS;
        string bS;
        //이름 임시변수에넣기
        aS = a.name;
        bS = b.name;
        return aS.CompareTo(bS);
    }
    public void UIButtonClick()
    {//이벤트 버튼 클릭시 실행함수
        switch (playerState)
        {
            case PlayerState.Hold://정지
                buttonName = EventSystem.current.currentSelectedGameObject.name;//현재 클릭한 오브젝트 이름
                //string 문으로 함수 실행 
                SendMessage(buttonName);
                break;
            case PlayerState.Select://이동
                buttonName = EventSystem.current.currentSelectedGameObject.name;//현재 클릭한 오브젝트 이름
                break;
            case PlayerState.Deploy://배치
                buttonName = EventSystem.current.currentSelectedGameObject.name;//현재 클릭한 오브젝트 이름
                break;
            case PlayerState.Fire:
                buttonName = EventSystem.current.currentSelectedGameObject.name;//현재 클릭한 오브젝트 이름
                break;
            case PlayerState.Play://플레이중
                buttonName = EventSystem.current.currentSelectedGameObject.name;
                SendMessage(buttonName);
                //string 문으로 함수 실행 
                break;
        }
    }

    #region Artillery

    private IEnumerator InputFireCoolTime()
    {//사격 쿨타임
        if (isArtyCoroutinRunning == false)
        {
            isArtyCoroutinRunning = true;
            InputArtilleryButton();
            yield return new WaitForSeconds(3.0f);
            isArtyCoroutinRunning = false;
        }
        StopCoroutine(this.InputFireCoolTime());
        yield return null;
    }

    public void InputArtilleryButton()
    {//발사장소 확보
        switch (selectLine)
        {
            case 0:
                ArtyPos[1].SetActive(false);
                ArtyPos[2].SetActive(false);
                if (Input.GetKeyUp(KeyCode.Space))
                {//탄두 발사
                    ArtillerySpawn(charID, 0);
                    pressSpaceBar = 0;//게이지 초기화
                }
                Debug.Log("1번라인 포격");
                break;
            case 1:
                ArtyPos[0].SetActive(false);
                ArtyPos[2].SetActive(false);
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    ArtillerySpawn(charID, 1);
                    pressSpaceBar = 0;//게이지 초기화
                }
                Debug.Log("2번라인 포격");
                break;
            case 2:
                ArtyPos[0].SetActive(false);
                ArtyPos[1].SetActive(false);
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    ArtillerySpawn(charID, 2);
                    pressSpaceBar = 0;//게이지 초기화
                }
                Debug.Log("3번라인 포격");
                break;
            default:
                break;
        }
    }

    private void ArtillerySpawn(int charID, int artyPosnum)//charid 현재는 더미값이 들어가고 있지만 추후에 탄두교체시 사용될 예정
    {
        GameObject TempArtyPos;
        //포격 위치 
        TempArtyPos = Instantiate(Resources.Load<GameObject>("Egg"), ArtyPos[artyPosnum].transform.position+new Vector3(0,3,0), Quaternion.identity);// 리소스 안에 넣어둬야함//인스턴스를 이용해서 필드에 배치

        TempArtyPos.GetComponent<Rigidbody>().AddForce(new Vector3(pressSpaceBar/10.0f,pressSpaceBar/10.0f,0),ForceMode.Impulse);//누른 시간만큼 힘증가
        ArtyPos[artyPosnum].SetActive(false);
    }

    private void fireSelectColiderFind()
    {//사격할 라인 선택
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit click;
        if (Physics.Raycast(ray, out click, 100f))
        {//충돌 물체로 부터 콜라이더 추출 하여 현재 콜라이더에 대입
            if (click.transform.GetComponent<BoxCollider>())
            {
                switch (click.transform.gameObject.name)
                {
                    case "ArtyPos1":
                        //소환 끝나고 상태 플레이상태로 변화
                        playerState = PlayerState.Play;
                        ArtillerySpawn(charID, 0);
                        Debug.Log("1번라인 포격");
                        break;
                    case "ArtyPos2":
                        //소환 끝나고 상태 플레이상태로 변화
                        playerState = PlayerState.Play;
                        ArtillerySpawn(charID, 1);
                        Debug.Log("2번라인 포격");
                        break;
                    case "ArtyPos3":
                        //소환 끝나고 상태 플레이상태로 변화
                        playerState = PlayerState.Play;
                        ArtillerySpawn(charID, 2);
                        Debug.Log("3번라인 포격");
                        break;
                }

            }
        }
    }

    #endregion

    #region AirStrike
    public void AirStrike()
    {
        if (GameObject.FindGameObjectWithTag("AirStrikeBird")==null)
        {
            ////////////코루틴문으로 쿨타임 제작 현재는 쿨타임 없음
            foreach (GameObject pos in ArtyPos)
            {
                Vector3 flyPos = pos.transform.position + new Vector3(-10, 5, 0);
                Instantiate(airStrikeBird, flyPos, Quaternion.Euler(0, 0, 90));
            }
        }
    }

    #endregion

    #region OutlineCtrl
    private IEnumerator OutlineCtrl()
    {
        while (playerState == PlayerState.Select)
        {//사이즈 제어하는 반복문
            if (selectGameObj.GetComponent<SpriteOutline>().outlineSize == 10)
            {
                outlineSizeUP = false;
            }
            else if (selectGameObj.GetComponent<SpriteOutline>().outlineSize == 0)
            {
                outlineSizeUP = true;
            }

            if (outlineSizeUP)
            {
                selectGameObj.GetComponent<SpriteOutline>().outlineSize = 10;
            }
            else
            {
                selectGameObj.GetComponent<SpriteOutline>().outlineSize = 0;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
    #endregion

    #region AnimalMoveFuncGroup

    private void playColiderFind()
    {//배치 상태 버튼 클릭
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit click;
        if (Physics.Raycast(ray, out click, 100f))
        {//충돌 물체로 부터 콜라이더 추출 하여 현재 콜라이더에 대입
            if (click.transform.GetComponent<BoxCollider>())
            {//box콜라이더 확인
                if (click.transform.GetComponentInChildren<BoxCollider>().gameObject.transform.childCount != 0)
                {//자식 오브젝트 확인
                    selectGameObj = click.transform.GetComponentInChildren<BoxCollider>().gameObject.transform.GetChild(0).gameObject;//자식으로 되어 있는 오브젝트 연결
               
                }
                ColiderCheck(click.transform.GetComponent<BoxCollider>());//콜라이더 상태 체크
            }
        }
    }
    private void selectColiderFind()
    {//선택했을 경우 실행하는 함수
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit click;
        if (Physics.Raycast(ray, out click, 100f))
        {//충돌 물체로 부터 콜라이더 추출 하여 현재 콜라이더에 대입
            if (click.transform.GetComponent<BoxCollider>())
            {//box콜라이더 확인
                if (click.transform.GetComponentInChildren<BoxCollider>().gameObject.transform.childCount != 0)
                {//자식 오브젝트 확인
                    previousSelectGameObj = selectGameObj;
                    selectGameObj = click.transform.GetComponentInChildren<BoxCollider>().gameObject.transform.GetChild(0).gameObject;//자식으로 되어 있는 오브젝트 연결
                    
                }
                ColiderCheck(click.transform.GetComponent<BoxCollider>());//콜라이더 상태 체크
            }
        }
    }

    public void ColiderCheck(Collider clickObj)
    {
        if (CurrentClickTrans == null)
        {//현재 콜라이더로 선택 된것이 없을 경우
            CurrentClickTrans = clickObj.transform.GetComponent<BoxCollider>();
        }
        else if (CurrentClickTrans != null)
        {
            Debug.Log("현재콜라이더 교체");
            if (CurrentClickTrans.transform.childCount != 0)
            {//이전에 선택한 위치가 캐릭터가 존재할 경우
                PreviousClickTrans = CurrentClickTrans;
                isMove = true;
            }
            CurrentClickTrans = clickObj.transform.GetComponent<BoxCollider>();
        }
    }

    public void MoveChar(Collider afterTrans, GameObject Animal, bool reverse)
    {//캐릭터 이동
        Animal.transform.position = 
            Vector3.MoveTowards(Animal.transform.position, afterTrans.transform.position, 20 * Time.deltaTime);
        //이동속도 조절을 timedeltatime에서 조절
        if (Animal.transform.position == afterTrans.transform.position)
        {
            if (!reverse)
            {//반대로 움직이는 것이 아닐경우 이동하는 위치 초기화
                isMove = false;
                ResetClickTrans();
            }
            //부모변경
            Animal.transform.parent = afterTrans.transform;
            selectLineCheck();
        }
    }
    public void ResetClickTrans()
    {//콜라이더 초기화
        PreviousClickTrans = null;
        CurrentClickTrans = null;
    }

    private void selectLineCheck()
    {//돼지가 배치된 라인 확인용
        PigObj = GameObject.FindGameObjectWithTag("Pig");
        selectLineObj = PigObj.transform.parent.gameObject;
        switch (selectLineObj.name)
        {
            case "Pig0*1":
                selectLine = 0;
                Line[0].SetActive(true);
                Line[1].SetActive(false);
                Line[2].SetActive(false);
                break;
            case "Pig1*1":
                selectLine = 1;
                Line[0].SetActive(false);
                Line[1].SetActive(true);
                Line[2].SetActive(false);
                break;
            case "Pig2*1":
                selectLine = 2;
                Line[0].SetActive(false);
                Line[1].SetActive(false);
                Line[2].SetActive(true);
                break;
        }
    }

    private void pigMoveUp()
    {
        if(selectLine<=2&&selectLine>0)//올라가는 함수
        {
            selectLineCheck();
            string currentPosObjName = "Pig"+(selectLine-1)+"*1";
            string previousPosObjName = "Pig" + (selectLine) + "*1";
            GameObject currentPosObj = GameObject.Find(currentPosObjName);
            GameObject previousPosObj= GameObject.Find(previousPosObjName);
            PreviousClickTrans = previousPosObj.GetComponent<BoxCollider>();
            CurrentClickTrans = currentPosObj.GetComponent<BoxCollider>();
            isMove = true;

        }
    }
    private void pigMoveDown()
    {
        if(selectLine < 2 && selectLine >= 0)
        {
            selectLineCheck();
            string currentPosObjName = "Pig" + (selectLine+1) + "*1";
            string previousPosObjName = "Pig" + (selectLine) + "*1";
            GameObject currentPosObj = GameObject.Find(currentPosObjName);
            GameObject previousPosObj = GameObject.Find(previousPosObjName);
            PreviousClickTrans = previousPosObj.GetComponent<BoxCollider>();
            CurrentClickTrans = currentPosObj.GetComponent<BoxCollider>();
            isMove = true;
        }
    }

    private void pigMove()
    {//애니메이션과 같은 돼지가 움직일 경우 실행할 코드들 
        StartCoroutine(OutlineCtrl());//사이즈 제어 코루틴 실행
    }
    private void pigStop()
    {//애니메이션과 같은 돼지가 멈출 경우 실행할 코드들 
        previousSelectGameObj.GetComponent<SpriteOutline>().outlineSize = 0;
        selectGameObj.GetComponent<SpriteOutline>().outlineSize = 0;
        StopCoroutine(OutlineCtrl());//사이즈 제어 코루틴 정지
    }



    #endregion

    #region AnimalRetreat
    public void Retreat()
    {//후퇴 명령버튼 클릭시 실행
        GameObject[] retreatAnimal = GameObject.FindGameObjectsWithTag("Friendly");
        foreach(GameObject animal in retreatAnimal)
        {
            animal.GetComponent<Animator>().SetBool("",true);//애니메이션 접근 해서 후퇴기능 키기//아직 없음
            animal.SendMessage("Retreat");
        }
    }
    public void AnimalRelocation(int charID)
    {
        //슬롯에 재배치
        int combatCount = playerInfo.GetCombatCount(charID);
        combatCount++;
        playerInfo.SetCombatCount(charID, combatCount);//객체수 수정
        gameLoadManager.CombatCount = combatCount;
        gameLoadManager.SendMessage("RelocationCharCountSet", charID);//아이디 보내서 슬롯 메뉴에서 숫자 늘리기
    }
    
    
    #endregion

    #region DeployFuncGroup
    //개체를 배치하는 코드 그룹
    public void CombatSlot()
    {//슬롯클릭시 실행
        string imageName = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite.name;
        slotNum = EventSystem.current.currentSelectedGameObject.tag;
        charID = int.Parse(imageName);
        playerState = PlayerState.Deploy;//배치위치 선택하는 상태
        foreach (GameObject line in Line)
        {//활성화 라인 오브젝트 
            line.SetActive(true);
        }

    }

    public void InputSlotNum()
    {//숫자 입력으로 소환
        var tempID=0;
        switch (inputValue)
        {//입력값으로 소환객체 정하고 소환 돼지가 있는 위치에 소환
            case KeyCode.Alpha1:
                tempID = slotNumID[0];//불러온 id 배열에서 지정
                slotNum = "Slot0";//현재 슬롯태그 설정
                charID = tempID;//id에 gameload로 부터 받아온 값 넣기
                AnimalSpawn(charID, selectLine);
                break;
            case KeyCode.Alpha2:
                tempID = slotNumID[1];
                slotNum = "Slot1";//현재 슬롯태그 설정
                charID = tempID;//id에 gameload로 부터 받아온 값 넣기
                AnimalSpawn(charID, selectLine);
                break;
            case KeyCode.Alpha3:
                tempID = slotNumID[2];
                slotNum = "Slot2";//현재 슬롯태그 설정
                charID = tempID;//id에 gameload로 부터 받아온 값 넣기
                AnimalSpawn(charID, selectLine);
                break;
            case KeyCode.Alpha4:
                tempID = slotNumID[3];
                slotNum = "Slot3";//현재 슬롯태그 설정
                charID = tempID;//id에 gameload로 부터 받아온 값 넣기
                AnimalSpawn(charID, selectLine);
                break;
            case KeyCode.Alpha5:
                tempID = slotNumID[4];
                slotNum = "Slot4";//현재 슬롯태그 설정
                charID = tempID;//id에 gameload로 부터 받아온 값 넣기
                AnimalSpawn(charID, selectLine);
                break;
            case KeyCode.Alpha6:
                tempID = slotNumID[5];
                slotNum = "Slot5";//현재 슬롯태그 설정
                charID = tempID;//id에 gameload로 부터 받아온 값 넣기
                AnimalSpawn(charID, selectLine);
                break;
              default:
                  break;
        }
        tempID = 0;//한마리 소환하고 초기화
    }

    private IEnumerator InputCoolTime()
    {
        if (isDeployCoroutinRunning == false)
        {
            isDeployCoroutinRunning = true;
            InputSlotNum();
            yield return new WaitForSeconds(1.5f);
            isDeployCoroutinRunning = false;
        }
        StopCoroutine(this.InputCoolTime());
        yield return null;
    }

    public void AnimalSpawn(int charID, int lineNum)
    {//동물 스폰
        if (charID != 0)
        {//캐릭터id가 존재할경우
            int combatCount = PlayerPrefs.GetInt(charID + "CombatCount");
            #region Random
            //랜덤위치용 숫자 받아오기
            int randomPosValue = RandomNum()/3;
            int plusminus = RandomNum() % 2;
            if(plusminus==0)
            {
                plusminus = -1;
            }
            //랜덤위치 지정
            Vector3 randomPos = new Vector3(0, 0, plusminus * randomPosValue * 0.2f);
            randomPos = Line[lineNum].transform.position + randomPos;
            #endregion
            if (combatCount != 0)
            {//개체수가 존재할 경우
                Debug.LogFormat("spawn{0}", charID);
                PigObj.GetComponent<Animator>().SetTrigger("isOrder");
                Instantiate(Resources.Load<GameObject>(charID.ToString() + "GameObj"), randomPos, Quaternion.identity);//id+GameObj를 리소스 안에 넣어둬야함//인스턴스를 이용해서 필드에 배치
                //소환하고 적용할 코드
                combatCount--;//한번 클릭마다 개체 하나식 제거
                SpawnCount[charID]++;//스폰한 동물 확인용
                PlayerPrefs.SetInt(charID + "CombatCount", combatCount);//제거한 후 전투가능 개체수를 수정
                gameLoadManager.CombatCount = combatCount;
                gameLoadManager.SendMessage("SlotCharCountSet", slotNum);
            }
            else if (combatCount != 0)
            {
                Debug.Log("동물부족함");
            }
        }
    }

    public void DeploySelectColiderFind()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit click;
        if (Physics.Raycast(ray, out click, 100f))
        {//충돌 물체로 부터 콜라이더 추출 하여 현재 콜라이더에 대입
            if (click.transform.GetComponent<BoxCollider>())
            {
                switch (click.transform.gameObject.name)
                {
                    case "Line1":
                        //소환 끝나고 상태 플레이상태로 변화
                        playerState = PlayerState.Play;
                        AnimalSpawn(charID, 0);
                        Debug.Log("1번라인 배치");
                        break;
                    case "Line2":
                        //소환 끝나고 상태 플레이상태로 변화
                        playerState = PlayerState.Play;
                        AnimalSpawn(charID, 1);
                        Debug.Log("2번라인 배치");
                        break;
                    case "Line3":
                        //소환 끝나고 상태 플레이상태로 변화
                        playerState = PlayerState.Play;
                        AnimalSpawn(charID, 2);
                        Debug.Log("3번라인 배치");
                        break;
                }

            }
        }
    }

    public void SetSlotNumID(int id = 0)
    {//id를 받아서 슬롯에 넣는 역할
        slotNumID[tempSlotNum] = id;
        tempSlotNum++;
    }

    public int RandomNum()//0부터 10까지 랜덤 수 나옴 
    {
        int randomNum = UnityEngine.Random.Range(0, 10);
        return randomNum;
    }
    #endregion

    #region Time
    //시간을 조작하는 데 관련된 코드 모음
    private IEnumerator GameTimeSet()
    {

        switch (playerState)
        {
            case PlayerState.Hold:
                Time.timeScale = 0f;//시간이 멈춤
                break;
            case PlayerState.Select:
                Time.timeScale = 0.5f;//유닛 선택시 시간이 느리게 흐름
                break;
            case PlayerState.Deploy:
                Time.timeScale = 0.5f;//배치 위치를 고르는 시간이 느리게 흐름
                break;
            case PlayerState.Play:
                Time.timeScale = 1.0f;//시간 정상
                break;
        }
        yield return null;
    }

    private void GameExit()
    {
        Destroy(EventSystem.current.currentSelectedGameObject);////검정색으로 변하는화면용 코드
        Vector4 panelColor = PausePanel.GetComponent<Image>().color;
        panelColor += new Vector4(0, 0, 0, 20);
        PausePanel.GetComponent<Image>().color = panelColor;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    private void GameExitCancel()
    {
        Time.timeScale = 1f;
        PausePanel.SetActive(false);
    }
    #endregion

    #region DeadAnimalCount

    public void DeadAnimalCountAdd(int deadAnimalID)
    {
        deadAnimalCount[deadAnimalID]++;
        Debug.Log("die");
        Debug.Log("deadAnimalID");
    }
    
    #endregion

    #region ResultFuncGroup

    public void Win()
    {
        isPause = true;
        Cursor.visible = true;//커서 
        //Time.timeScale = 0f;//시간
        Debug.Log(PlayerPrefs.GetInt("Phase"));
        PlayerPrefs.SetInt("Phase", PlayerPrefs.GetInt("Phase") + 1);
        PlayerPrefs.SetInt("Branch", 0);
        PlayerPrefs.SetInt("Count", 1);
        //전투 결과값 반영
        /*//생산
        //자원
        //float ResourceProductionValue = 10f;
        //int[] ResourceProductionAnimalCount = new int[11];
        //for (int id=0;id<=10;id++)//수정필요
        //{
        //    if(ResourceProductionAnimalCount[id]!=0)
        //    {
        //
        //    }
        //}
        //PlayerPrefs.SetInt("Wheat",PlayerPrefs.GetInt("Wheat")+)
        //전투결과 동물 수 변경 적용
        */

        for (int id = 2; id < deadAnimalCount.Length ; id++)
        {
            int animalCombatCount = PlayerPrefs.GetInt(id + "CombatCount")+SpawnCount[id];//동물이 전투용으로 데려온 수
            if (deadAnimalCount[id] != 0)
            {
                /////animalCombatCount 값 이상함 수정 필요
                animalCombatCount = animalCombatCount - deadAnimalCount[id];
                PlayerPrefs.SetInt(id + "CombatCount", 0);//전투용으로 빼놓은 자리-사망한 동물수 
            }
            animalCombatCount = animalCombatCount + PlayerPrefs.GetInt(id + "Count");
            PlayerPrefs.SetInt(id + "Count", 0);
            PlayerPrefs.SetInt(id + "Count",animalCombatCount);//해당하는 동물의 숫자+동물의 전투용으로 빼놓은 자리=동물 숫자
            Debug.LogFormat("{0} {1}마리가 남았습니다.", id, PlayerPrefs.GetInt(id + "Count"));
        }
        //패널
        WinPanel.SetActive(true);
        GameObject.Find("Audio").GetComponent<AudioSource>().Stop();
        Invoke("LoadScene1", 5);
    }

    public void Lose()
    {
        isPause = true;
        Cursor.visible = true;//커서 
        //Time.timeScale = 0f;//시간
        //전투 이전 상황으로 속성값 변경
        //패널
        LosePanel.SetActive(true);
        GameObject.Find("Audio").GetComponent<AudioSource>().Stop();
        Invoke("LoadScene0", 5);
    }


    void LoadScene1()
    {
        SceneManager.LoadScene(1);
    }
    void LoadScene0()
    {
        SceneManager.LoadScene(0);
    }
    #endregion
}