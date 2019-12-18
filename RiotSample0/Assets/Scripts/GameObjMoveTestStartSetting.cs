using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjMoveTestStartSetting : MonoBehaviour
{//GameObjMoveTestStartSetting씬 시작세팅 
    private PlayerInfo playerInfo;
    void Start()
    {
        for(int i=0;i<5;i++)
        {
            playerInfo.SetCharId(i);
            playerInfo.SetSlotChar(i, i);
            playerInfo.SetCharHP(i, i);
            playerInfo.SetCharArm(i, i);
        }
        
    }

}
