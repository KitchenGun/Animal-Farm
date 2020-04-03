using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoSet : MonoBehaviour
{
    public PlayerInfo playerInfo;//플레이어정보 

    

    // Start is called before the first frame update
    void Start()
    {
        SetCount();//개체수 값 불러오기

    }

    private void SetCount()
    {//개체수 세팅
        List<Dictionary<string, object>> data = CSVReader.Read("characterCSV");
        for (var i = 0; i < data.Count; i++)
        {
            int tempID = (int)data[i]["ID"];//id  불러오기
            int tempValue = (int)data[i]["Count"];//개체수 값 불러오기
            playerInfo.SetCharCount(tempID, tempValue);
            Debug.Log(data[i]["ID"] + " " + tempValue);
        }
    }

}
