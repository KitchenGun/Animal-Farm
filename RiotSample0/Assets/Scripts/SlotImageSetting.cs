using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotImageSetting : MonoBehaviour
{
    private PlayerInfo playerInfo;
    private string slotName;
    private int slotNum;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.name = slotName;
        switch(slotName)
        {
            case "Slot0":
                slotNum=playerInfo.GetSlotChar(0);
                break;
            case "Slot1":
                slotNum = playerInfo.GetSlotChar(1);
                break;
            case "Slot2":
                slotNum = playerInfo.GetSlotChar(2);
                break;
            case "Slot3":
                slotNum = playerInfo.GetSlotChar(3);
                break;
            case "Slot4":
                slotNum = playerInfo.GetSlotChar(4);
                break;
        }
    }

}
