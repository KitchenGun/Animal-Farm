﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollbarCount : MonoBehaviour
{
    public GameObject buttonClick;

    private float currentAnimalCount;
    private float combatAnimalCount;

    public GameObject CurrentCount;
    public GameObject MaxCount;

   

    private void Update()
    {
        currentAnimalCount = buttonClick.GetComponent<ButtonClick>().currentAnimalCount;
        int currentAnimalCountInt = (int)currentAnimalCount;
        MaxCount.GetComponent<TextMeshProUGUI>().text = currentAnimalCountInt.ToString();//최대값 표시
        float countScrollCount = this.gameObject.GetComponent<Scrollbar>().value;//스크롤바 수치값 입력

        combatAnimalCount = currentAnimalCount * countScrollCount;//계산

        int combatAnimalCountInt = (int)combatAnimalCount;//int로 변환
        CurrentCount.GetComponent<TextMeshProUGUI>().text = combatAnimalCountInt.ToString();//현재 선택한 수치 표시
    }
}
