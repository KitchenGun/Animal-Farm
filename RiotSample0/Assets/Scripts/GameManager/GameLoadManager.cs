using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoadManager : MonoBehaviour
{
    private ButtonClick buttonClick;

    private GameObject slot;
    private int slotNum;//슬롯의 숫자
    private int slotID;//슬롯의 id
    
    // Start is called before the first frame update
    private void Awake()
    {
        InGameSlotSetting();
       
    }

    public void InGameSlotSetting()
    {
        for (slotNum = 0; slotNum < 6; slotNum++) 
        {
            slotID= PlayerPrefs.GetInt("Slot" + slotNum);//id에 playerinfo에서 가져온 정보 넣기
            Debug.Log(slotNum.ToString()+slotID.ToString());
            slot = GameObject.FindGameObjectWithTag("Slot"+slotNum);//태그를 이용해서 물건 찾기
            if(slotID==0)
            {//슬롯이 빈 공간임으로 비어있게 변경
                slot.GetComponent<Image>().sprite = null;//이미지변경
            }
            else
            {//슬롯안에 저장된 값이 있을 경우
                slot.GetComponent<Image>().sprite = Resources.Load<Sprite>(slotID.ToString());//이미지변경
            }
        }
    }


}
