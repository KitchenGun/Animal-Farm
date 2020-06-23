using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class IntroButtonManager : MonoBehaviour
{

    private string buttonName;
    public void UIButtonClick()
    {//버튼 클릭시 실행함수
        buttonName = EventSystem.current.currentSelectedGameObject.name;//현재 클릭한 오브젝트 이름
        //string 문으로 함수 실행 
        SendMessage(buttonName);
    }


    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void Credit()
    {

    }

    private void Exit()
    {//게임 종료
        Application.Quit();
    }
}
