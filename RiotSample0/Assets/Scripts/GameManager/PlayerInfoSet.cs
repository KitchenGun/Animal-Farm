using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoSet : MonoBehaviour
{
    public PlayerInfo playerInfo;//플레이어정보 
    public List<Dictionary<string, object>> Data;
    public int tempint;


    // Start is called before the first frame update
    void Start()
    {

        Data = CSVReader.Read("characterCSV");
        SetHP();
        SetAP();
        SetATKSP();
        SetCount();//개체수 값 불러오기
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
            Debug.Log(Data[i]["ID"] + " " + tempValue);
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
            Debug.Log(Data[i]["ID"] + " " + tempValue);
        }

    }
    #endregion

    #region 공격 속도 가져오기
    private void SetATKSP()
    {  //atksp 세팅
        for (var i = 0; i < Data.Count; i++)
        {//cout
            int tempID = (int)Data[i]["ID"];//id  불러오기
            float tempValue;
            float.TryParse(Data[i]["ATKSP"].ToString(),out tempValue);//개체수 값 불러오기
            Debug.Log(tempValue);
            playerInfo.SetCharDps(tempID, tempValue);
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
            Debug.Log(Data[i]["ID"] + " " + tempValue);
        }
    }
    #endregion

   
}
