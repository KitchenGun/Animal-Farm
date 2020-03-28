using System.Collections;
using System.Collections.Generic;
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
    public PlayerState playerState;//플레이어상태

    public static GameObject[] Line;//라인오브젝트 배열

    private string buttonName;//버튼의 이름
    private int charID;//캐릭터 id

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
    //움직일수 있는지 확인용
    private bool isMove;
    private bool reverse;
    //outline관련변수
    private bool outlineSizeUP;

    public void Awake()
    {
        playerState = PlayerState.Play;/////////////////////임시 나중에 Hold로 교체가 필요함
        StartCoroutine(GameTimeSet());//게임 타임 설정
        #region lineCodeFind
        Line = GameObject.FindGameObjectsWithTag("Line");//라인 찾기
        for(int lineNum=0;lineNum<2;lineNum++)
        {//라인 오브젝트 배열 정렬
            if (lineNum >= 1)
            {
                ListGameObjectSort(Line[lineNum - 1], Line[lineNum]);
            }
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
        if(Input.GetMouseButtonDown(0))
        {//마우스 클릭시
            switch (playerState)
            {
                case PlayerState.Hold:

                    break;
                case PlayerState.Select:
                    selectColiderFind();
                    break;
                case PlayerState.Deploy:
                    DeploySelect();
                    break;
                case PlayerState.Play:
                    playColiderFind();
                    break;
            }
        }
        if (isMove)
        {//움직이는 코드
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
    {//배열 정렬
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
            case PlayerState.Hold:
                //스토리 넘기기
                break;
            case PlayerState.Select:
                buttonName = EventSystem.current.currentSelectedGameObject.name;//현재 클릭한 오브젝트 이름
                break;
            case PlayerState.Deploy:
                buttonName = EventSystem.current.currentSelectedGameObject.name;//현재 클릭한 오브젝트 이름
                //string 문으로 함수 실행 
                SendMessage(buttonName);
                break;
        }       
    }

    #region OutlineCtrl
    private IEnumerator OutlineCtrl()
    {
        while (playerState==PlayerState.Select)
        {
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
                Debug.Log("sizeup");
                selectGameObj.GetComponent<SpriteOutline>().outlineSize = 10;
            }
            else
            {
                Debug.Log("sizedown");
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
    {
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
                        case"Line1":
                            break;
                        case "Line2":
                            break;
                        case "Line3":
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
        StopCoroutine(OutlineCtrl());//사이즈 제어 코루틴 정지
        selectGameObj.GetComponent<SpriteOutline>().outlineSize = 0;
    }
    #endregion

    #region DeployFuncGroup
    //개체를 배치하는 코드 그룹
    public void CombatSlot()
    {//슬롯클릭시 실행
        string imageName = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite.name;
        charID = int.Parse(imageName);
        playerState = PlayerState.Select;//배치위치 선택하는 상태
        foreach (GameObject line in Line)
        {//활성화 라인 오브젝트 
            line.SetActive(true);
        }

    }

    public void AnimalSpawn(int charID, int lineNum)
    {//동물 스폰
        ///AnimalSpawn(charID);//////소환지점을 클릭했을때 호출이 필요
        int combatCount = playerInfo.GetCombatCount(charID);
        if (combatCount != 0)
        {//개체수가 존재할 경우
            Instantiate(Resources.Load<GameObject>(charID + "GameObj"), Line[lineNum].transform.position, Quaternion.identity);//id+GameObj를 리소스 안에 넣어둬야함//인스턴스를 이용해서 필드에 배치
            //소환하고 적용할 코드
            combatCount--;//한번 클릭마다 개체 하나식 제거
            playerInfo.SetCombatCount(charID, combatCount);//제거한 후 전투가능 개체수를 수정
        }
        foreach (GameObject line in Line)
        {//라인 오브젝트 끄기
            line.SetActive(false);
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
                switch (click.transform.gameObject.tag)
                {
                    case "Line1":
                        AnimalSpawn(charID, 0);
                        break;
                    case "Line2":
                        AnimalSpawn(charID, 1);
                        break;
                    case "Line3":
                        AnimalSpawn(charID, 2);
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


    ////이전 클릭 위치
    //[SerializeField]
    //private Collider PreviousClickTrans;
    ////현재 클릭 위치
    //[SerializeField]
    //private Collider CurrentClickTrans;
    ////움직일수 있는지 확인용
    //private bool isMove;
    //private bool reverse;
    //private void Start()
    //{
    //    //초기화
    //    isMove = false;
    //    reverse = false;
    //}
    //private void Update()
    //{
    //    if(Input.GetMouseButtonDown(0))
    //    {
    //        Debug.Log("클릭됨");
    //        CombatClickInput();
    //    }
    //    if (isMove)
    //    {
    //        if (CurrentClickTrans.gameObject.transform.childCount > 0)
    //        {//이동할 곳에 오브젝트가 있으면
    //            reverse = true;
    //            MoveChar(PreviousClickTrans, CurrentClickTrans.transform.GetChild(0).gameObject,reverse);
    //        }
    //        reverse = false;
    //        MoveChar(CurrentClickTrans, PreviousClickTrans.transform.GetChild(0).gameObject,reverse);
    //    }
    //}
    //public void CombatClickInput()
    //{//전투상황시 클릭 호출 함수
    //    Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit click;
    //    if(Physics.Raycast(ray,out click,100f))
    //    {//충돌 물체로 부터 콜라이더 추출 하여 현재 콜라이더에 대입
    //        if(click.transform.GetComponent<BoxCollider>())
    //        {
    //            if (CurrentClickTrans == null)
    //            {//현재 콜라이더로 선택 된것이 없을 경우
    //                Debug.Log("현재 콜라이더 비어있음");
    //                CurrentClickTrans = click.transform.GetComponent<BoxCollider>();
    //            }
    //            else if (CurrentClickTrans != null)
    //            {
    //                Debug.Log("현재콜라이더 교체");
    //                if (CurrentClickTrans.transform.childCount!=0)
    //                {//이전에 선택한 위치가 캐릭터가 존재할 경우
    //                    Debug.Log("이전에 선택한 위치에 캐릭터 존재 교체 가능");
    //                    PreviousClickTrans = CurrentClickTrans;
    //                    isMove = true;
    //                }
    //                CurrentClickTrans = click.transform.GetComponent<BoxCollider>();
    //            }
    //        }
    //
    //    }
    //}
    //public void ResetClickTrans()
    //{//콜라이더 초기화
    //    PreviousClickTrans = null;
    //    CurrentClickTrans = null;
    //}
    //
    //public void MoveChar(Collider afterTrans,GameObject riot,bool reverse)
    //{//캐릭터 이동
    //    Debug.Log("움직이는중");
    //    riot.transform.position = Vector3.MoveTowards(riot.transform.position, afterTrans.transform.position, 20*Time.deltaTime);
    //    //이동속도 조절을 timedeltatime에서 조절
    //    if(riot.transform.position==afterTrans.transform.position)
    //    {
    //        if(!reverse)
    //        {//반대로 움직이는 것이 아닐경우 이동하는 위치 초기화
    //            isMove = false;
    //            ResetClickTrans();
    //        }
    //        //부모변경
    //        riot.transform.parent = afterTrans.transform;
    //    }
    //}
    //
}
