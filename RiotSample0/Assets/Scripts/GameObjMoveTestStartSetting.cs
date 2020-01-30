using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjMoveTestStartSetting : MonoBehaviour
{//GameObjMoveTestStartSetting씬 시작세팅 
    private PlayerInfo playerInfo;

    public int tempint;
    void Start()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("TempData");
        for (var i = 0; i < data.Count; i++)
        {
            tempint = (int)data[i]["ID"];
            Debug.Log(data[i]["name"] + " " + tempint);
        }


        //for (int i=0;i<5;i++)
        //{
        //    playerInfo.SetCharId(i);
        //    playerInfo.SetSlotChar(i, i);
        //    playerInfo.SetCharHP(i, i);
        //    playerInfo.SetCharAP(i, i);
        //}
        
    }

}
