using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    
    //캐릭터 관련
    #region CharID
    public void SetCharId(int charID)
    {//플레이어 id설정
        PlayerPrefs.SetInt("CharID" + charID, charID);
    }
    public int GetCharId(int charID)
    {//플레이어 id불러오기
        return PlayerPrefs.GetInt("CharID" + charID);
    }
    #endregion
    #region SlotSetting
    public void SetSlotChar(int slotNum,int slotCharID)
    {//슬롯에 캐릭터 넘버 설정
        PlayerPrefs.SetInt(("Slot" + slotNum + "Char"), slotCharID);
    }
    public int GetSlotChar(int slotNum)
    {//해당슬롯에 캐릭터 넘버 가져오기
        return PlayerPrefs.GetInt("Slot" + slotNum + "Char");
    }
    #endregion
    #region HP
    public void SetCharHP(int charID, int charHp)
    {
        PlayerPrefs.SetInt(charID+ "HP", charHp);
    }
    public int GetCharHP(int charID)
    {
        return PlayerPrefs.GetInt(charID + "HP");
    }
    #endregion
    #region AP
    public void SetCharAP(int charID, int charAP)
    {
        PlayerPrefs.SetInt(charID + "AP", charAP);
    }
    public int GetCharAP(int charID)
    {
        return PlayerPrefs.GetInt(charID + "AP");
    }
    #endregion
    //스테이지 관련
    #region Stage
    public void SetStage(int stage)
    {
        PlayerPrefs.SetInt("Stage", stage);
    }
    public int GetStage()
    {
        return PlayerPrefs.GetInt("Stage");
    }
    #endregion


}
