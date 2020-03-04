using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjMoveTestStartSetting : MonoBehaviour
{//GameObjMoveTestStartSetting씬 시작세팅 
    private PlayerInfo playerInfo;

    public int tempint;
    void Start()
    {
        //csv 테스트 
      // List<Dictionary<string, object>> data = CSVReader.Read("character(CSV)");
      // for (var i = 0; i < data.Count; i++)
      // {
      //     tempint = (int)data[i]["AP"];
      //     Debug.Log(data[i]["ID"] + " " + tempint);
      // }


        //for (int i=0;i<5;i++)
        //{
        //    playerInfo.SetCharId(i);
        //    playerInfo.SetSlotChar(i, i);
        //    playerInfo.SetCharHP(i, i);
        //    playerInfo.SetCharAP(i, i);
        //}

    }

}
