using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVSet : MonoBehaviour
{
    public class GameObjMoveTestStartSetting : MonoBehaviour
    {//GameObjMoveTestStartSetting씬 시작세팅 
        private PlayerInfo playerInfo;
        public List<Dictionary<string, object>> Data;
        public int tempint;
        void Start()
        {
            playerInfo = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerInfo>();
            //csv 테스트 
            Data = CSVReader.Read("character(CSV)");

        }

        
    }
}
