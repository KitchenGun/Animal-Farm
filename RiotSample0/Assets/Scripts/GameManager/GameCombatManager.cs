using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public enum PlayerState
{
    Hold,//아무것도 못하고 대화만 보는 정지 상태 적 또한 정지한다
    Select,//슬롯의 버튼을 눌렀거나  유닛 선택상태
    Deploy,//배치를 할 경우에 사용되는 상태이다.
    Play//정상적인 플레이 상태
}

public class GameCombatManager : MonoBehaviour
{
    public PlayerInfo playerInfo;//플레이어 정보
    public GameLoadManager gameLoadManager;
    public PlayerState playerState;//플레이어상태

    //public static GameObject[] Line;//배치 라인 오브젝트 
    public static List<GameObject> Line = new List<GameObject>();
    private GameObject mainLine;
    private GameObject compareLine;

    private string buttonName;//버튼의 이름
    private int charID;//캐릭터 id
    private string slotNum;//슬롯 숫자

    //outline관련변수
    private bool outlineSizeUP;

    /// <summary>
    /// 이동에 관련된 변수들
    //과거의 선택 위치
    [SerializeField]
    private Collider PreviousClickTrans;
    //현재 클릭 위치
    [SerializeField]
    private Collider CurrentClickTrans;
    //클릭한 위치의 게임 오브젝트
    private GameObject selectGameObj;
    //땅에 파붙히지 않기 위한 변수
    private Vector3 BasicPos = new Vector3(0, 1.2f, 0);

    //움직일수 있는지 확인용
    private bool isMove;
    private bool reverse;

    public void Start()
    {
        playerState = PlayerState.Play;/////////////////////임시 나중에 Hold로 교체가 필요함
        StartCoroutine(GameTimeSet());//게임 타임 설정
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
        #endregion
    }

    private void Update()//플레이어의 입력을 받기위한 업데이트
    {
        Debug.Log(playerState);
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
                        DeploySelect();
                        break;
                    case PlayerState.Play://게임 플레이중
                        playColiderFind();
                        break;
                }
            }
        }
        if (isMove)
        {//움직이는 코드
            selectGameObj.GetComponent<SpriteOutline>().outlineSize = 0;//아웃 라인 제거
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
            case PlayerState.Play://플레이중
                buttonName = EventSystem.current.currentSelectedGameObject.name;
                //string 문으로 함수 실행 
                SendMessage(buttonName);
                break;
        }
    }

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
                    switch (selectGameObj.tag)
                    {
                        case "Pig":
                            pigMove();
                            break;
                    }
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
                    selectGameObj = click.transform.GetComponentInChildren<BoxCollider>().gameObject.transform.GetChild(0).gameObject;//자식으로 되어 있는 오브젝트 연결
                    switch (selectGameObj.tag)
                    {
                        case "Pig":
                            pigStop();
                            break;

                    }
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
        Animal.transform.position = Vector3.MoveTowards(Animal.transform.position, afterTrans.transform.position, 20 * Time.deltaTime);
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
        }
    }
    public void ResetClickTrans()
    {//콜라이더 초기화
        PreviousClickTrans = null;
        CurrentClickTrans = null;
    }


    private void pigMove()
    {//애니메이션과 같은 돼지가 움직일 경우 실행할 코드들 
        playerState = PlayerState.Select;//선택상태로 만듬
        StartCoroutine(OutlineCtrl());//사이즈 제어 코루틴 실행
    }
    private void pigStop()
    {//애니메이션과 같은 돼지가 멈출 경우 실행할 코드들 
        playerState = PlayerState.Play;//플레이상태로 만듬
        selectGameObj.GetComponent<SpriteOutline>().outlineSize = 0;
        StopCoroutine(OutlineCtrl());//사이즈 제어 코루틴 정지
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

    public void AnimalSpawn(int charID, int lineNum)
    {//동물 스폰
        int combatCount = playerInfo.GetCombatCount(charID);
        if (combatCount != 0)
        {//개체수가 존재할 경우
            Instantiate(Resources.Load<GameObject>(charID + "GameObj"), Line[lineNum].transform.position, Quaternion.identity);//id+GameObj를 리소스 안에 넣어둬야함//인스턴스를 이용해서 필드에 배치
            //소환하고 적용할 코드
            combatCount--;//한번 클릭마다 개체 하나식 제거
            playerInfo.SetCombatCount(charID, combatCount);//제거한 후 전투가능 개체수를 수정
            gameLoadManager.CombatCount = combatCount;
            gameLoadManager.SendMessage("SlotCharCountSet",slotNum);
        }
        else if (combatCount != 0)
        {
            Debug.Log("동물부족함");
        }
    
    }

    public void DeploySelect()
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
    #endregion
}