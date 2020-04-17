using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimpleTextChanger : MonoBehaviour
{
    private string[] Temptext = { "1", "2", "3" };
    public GameObject TempTextObj;
    private TextMeshProUGUI temptext;
    private int tempTextNum=0;

    void Start()
    {
        temptext = TempTextObj.GetComponent<TextMeshProUGUI>();
        temptext.text = Temptext[tempTextNum];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            temptext.text = Temptext[tempTextNum];
            tempTextNum++;
        }
    }
}
