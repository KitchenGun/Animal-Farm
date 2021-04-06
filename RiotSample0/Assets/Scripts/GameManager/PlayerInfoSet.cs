using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoSet : MonoBehaviour
{
    public PlayerInfo playerInfo;//플레이어정보 
    public List<Dictionary<string, object>> Data;
    public List<Dictionary<string, object>> GameScript;
    private ScriptManager scriptManager;
    void Start()
    {
        scriptManager = GameObject.Find("ScriptManager").GetComponent<ScriptManager>();
        PlayerPrefs.SetInt("Start", 0);
        if (PlayerPrefs.GetInt("Start")!=1)
        {//프로그램 실행 처음에만 실행
            ResetInfo();
        }
        else
        {
            Debug.Log(PlayerPrefs.GetInt("Start"));
        }
    }

    public void ResetInfo()
    {
        Data = CSVReader.Read("characterCSV");
        GameScript = CSVReader.Read("textlist");
        //ProductionData = CSVReader.Read("ProductionCSV");
        SetHP();
        SetAP();
        SetMoveSpeed();
        SetATKDelay();
        SetATKRange();
        SetCount();//개체수 값 불러오기
        //SetResource();//자원 고정값 불러오기
        GetScript();//스크립트 가져오기
        PlayerPrefs.SetInt("Start", 1);//숫자를 올려서 다시 실행 안하게 만듬
        Debug.Log(PlayerPrefs.GetInt("Start"));
        Debug.Log("infoSet");
    }

    #region 체력 가져오기
    private void SetHP()
    {
        //HP 세팅
        for (var i = 0; i < Data.Count; i++)
        {//cout
            int tempID = (int)Data[i]["ID"];//id  불러오기
            int tempValue = (int)Data[i]["HP"];//개체수 값 불러오기
            playerInfo.SetCharHP(tempID, tempValue);
        }
    }

    #endregion

    #region 공격력 가져오기
    private void SetAP()
    {
        //HP 세팅
        for (var i = 0; i < Data.Count; i++)
        {//cout
            int tempID = (int)Data[i]["ID"];//id  불러오기
            int tempValue = (int)Data[i]["AP"];//개체수 값 불러오기
            playerInfo.SetCharAP(tempID, tempValue);
        }

    }
    #endregion

    #region 이동 속도 가져오기
    private void SetMoveSpeed()
    {
        for (var i = 0; i < Data.Count; i++)
        {//cout
            int tempID = (int)Data[i]["ID"];//id  불러오기
            float tempValue;
            float.TryParse(Data[i]["MoveSpeed"].ToString(), out tempValue);//개체수 값 불러오기
            PlayerPrefs.SetFloat(tempID + "MoveSpeed", tempValue);
        }
    }

    #endregion

    #region 공격 속도 가져오기
    private void SetATKDelay()
    {  //atksp 세팅
        for (var i = 0; i < Data.Count; i++)
        {//cout
            int tempID = (int)Data[i]["ID"];//id  불러오기
            float tempValue;
            float.TryParse(Data[i]["ATKDelay"].ToString(),out tempValue);//개체수 값 불러오기
            Debug.Log(tempValue);
            playerInfo.SetCharATKDelay(tempID, tempValue);
            Debug.Log(Data[i]["ID"] + " " + tempValue);
        }
    }
    #endregion

    #region 개체수 가져오기
    private void SetCount()
    {//개체수 세팅
        for (var i = 0; i < Data.Count; i++)
        {//cout
            int tempID = (int)Data[i]["ID"];//id  불러오기
            int tempValue = (int)Data[i]["Count"];//개체수 값 불러오기
            playerInfo.SetCharCount(tempID, tempValue);

        }
    }
    #endregion

    #region 사거리 가져오기
    private void SetATKRange()
    {//개체수 세팅
        for (var i = 0; i < Data.Count; i++)
        {//cout
            int tempID = (int)Data[i]["ID"];//id  불러오기
            int tempValue = (int)Data[i]["ATKRange"];//개체수 값 불러오기
            PlayerPrefs.SetInt(tempID + "ATKRange", tempValue);
        }
    }
    #endregion

    #region 자원 관련 가져오기
    private void SetResource()
    {//정해진 고정 수치값으로 세팅
        PlayerPrefs.SetInt("Wheat", 100);
    }

    private void SetAnimalProductionValue()
    {
        for (var i = 0; i < Data.Count; i++)
        {//cout
            int tempID = (int)Data[i]["ID"];//id  불러오기
            int tempValue = (int)Data[i]["AnimalProductionValue"];//동물 생산값
            PlayerPrefs.SetInt(tempID + "AnimalProductionValue", tempValue);
        }
    }

    private void SetResourceProductionValue()
    {
        for (var i = 0; i < Data.Count; i++)
        {//cout
            int tempID = (int)Data[i]["ID"];//id  불러오기
            int tempValue = (int)Data[i]["ResourceProductionValue"];//자원 생산값
            PlayerPrefs.SetInt(tempID + "ResourceProductionValue", tempValue);
        }
    }
    private void SetAnimalProductionRequireResource()
    {
        for (var i = 0; i < Data.Count; i++)
        {//cout
            int tempID = (int)Data[i]["ID"];//id  불러오기
            int tempValue = (int)Data[i]["AnimalProductionRequireResource"];//동물 2마리당  생산 필요 자원
            PlayerPrefs.SetInt(tempID + "AnimalProductionRequireResource", tempValue);
        }
    }
    #endregion

    #region 스크립트 가져오기
    private void GetScript()
    {//개체수 세팅
        for (var i = 0; i < GameScript.Count; i++)
        {//cout

            Script tempGameScript = new Script();

            tempGameScript.CharID = GameScript[i]["CharID"].ToString();//id  불러오기
            tempGameScript.Phase = (int)GameScript[i]["Phase"];
            tempGameScript.Branch = (int)GameScript[i]["Branch"];
            tempGameScript.Count = (int)GameScript[i]["Count"];
            tempGameScript.Face = (int)GameScript[i]["Face"];
            tempGameScript.Content = GameScript[i]["Content"].ToString();


            scriptManager.SetGameScript(tempGameScript);
        }
    }
    #endregion
}
