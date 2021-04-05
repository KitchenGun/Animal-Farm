using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterImage : MonoBehaviour
{
    private int Phase;
    private int Branch;
    private int Count;

    private ScriptManager sm;

    // Start is called before the first frame update
    void Start()
    {
        //현재 스크립트의 정보를 불러옴
        Phase = PlayerPrefs.GetInt("Phase");
        Branch = PlayerPrefs.GetInt("Branch");
        Count = PlayerPrefs.GetInt("Count");

        Debug.Log("check");
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
