using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoadManager : MonoBehaviour
{
    private ButtonClick buttonClick;
    private GameCombatManager gameCombatManager;

    private GameObject slot;
    private int slotNum;//슬롯의 숫자
    private int slotID;//슬롯의 id
    public int CombatCount;//슬롯의 캐릭터 전투가능 개체수
    private int[] SlotInfo = new int[6];//슬롯의 id를 저장하는 배열
    
    // Start is called before the first frame update
    private void Awake()
    {
        gameCombatManager = this.gameObject.GetComponent<GameCombatManager>();
        InGameSlotSetting();//슬롯에 이미지와 전투가능 개체수 띄우기
    }


    public void InGameSlotSetting()
    {
        for (slotNum = 0; slotNum < 6; slotNum++) 
        {
            slotID = PlayerPrefs.GetInt("Slot" + slotNum);//id에 playerinfo에서 가져온 정보 넣기
            SlotInfo[slotNum] = slotID;//슬롯에 맞는 id 정보를 저장
            CombatCount = PlayerPrefs.GetInt(slotID + "CombatCount");//id를 이용하여 전투 가능 개체 정보를 불러오기
            slot = GameObject.FindGameObjectWithTag("Slot"+slotNum);//태그를 이용해서 물건 찾기
            if(slotID==0)
            {//슬롯이 빈 공간임으로 비어있게 변경
                slot.GetComponent<Image>().sprite = SpriteSheetManager.GetSpriteByName("SlotImage", "EmptySlot");//이미지변경
            }
            else
            {//슬롯안에 저장된 값이 있을 경우
                slot.GetComponent<Image>().sprite = SpriteSheetManager.GetSpriteByName("SlotImage", slotID.ToString());//이미지변경
                slot.GetComponentInChildren<Text>().text = CombatCount.ToString();//전투가능 개체 띄우기
                gameCombatManager.SendMessage("SetSlotNumID", slotID);
            }
        }
    }


    public void SlotCharCountSet(string slotNum)
    {
        slot = GameObject.FindGameObjectWithTag(slotNum);
        slot.GetComponentInChildren<Text>().text = CombatCount.ToString();//전투가능 개체 띄우기
    }

    public void RelocationCharCountSet(int charID)
    {//후퇴한 캐릭터 받으면 슬롯 찾아서 숫자 증가
        for (slotNum = 0; slotNum < 6; slotNum++)
        {
            if(SlotInfo[slotNum]==charID)
            {
                slot = GameObject.FindGameObjectWithTag("Slot"+slotNum.ToString());
                slot.GetComponentInChildren<Text>().text = CombatCount.ToString();//전투가능 개체 띄우기
                break;
            }
        }
    }
}
