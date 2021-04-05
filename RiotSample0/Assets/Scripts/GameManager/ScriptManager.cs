using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    private static ScriptManager instance = null;
    private List<Script> GameScript = new List<Script>();


    void Awake()
    {
        if (null == instance)
        {
            //이 클래스 인스턴스가 탄생했을 때 전역변수 instance에 게임매니저 인스턴스가 담겨있지 않다면, 자신을 넣어준다.
            instance = this;
            //씬 전환이 되더라도 파괴되지 않게 한다.
            //gameObject만으로도 이 스크립트가 컴포넌트로서 붙어있는 Hierarchy상의 게임오브젝트라는 뜻이지만, 
            DontDestroyOnLoad(this.gameObject);
            PlayerPrefs.SetInt("Phase", 0);
            PlayerPrefs.SetInt("Branch", 0);
            PlayerPrefs.SetInt("Count", 0);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 맘껏 호출할 수 있다.
    public static ScriptManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void SetGameScript(Script gameScript)
    {
        GameScript.Add(gameScript);
    }

    public void Print()
    {
        foreach(Script script in GameScript)
        {
            Debug.Log(script.CharID);
            Debug.Log(script.Phase);
            Debug.Log(script.Branch);
            Debug.Log(script.Count);
            Debug.Log(script.Face);
            Debug.Log(script.Content);
        }
    }
}