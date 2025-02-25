﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class IntroButtonManager : MonoBehaviour
{

    private string buttonName;

    [SerializeField]
    private GameObject ExamplePanel;

    private void Start()
    {
        ExamplePanel.SetActive(false);
        PlayerPrefs.SetInt("Screenmanager Resolution Width", 1920);
        PlayerPrefs.SetInt("Screenmanager Resolution Height", 1080);
        PlayerPrefs.Save();

    }

    public void UIButtonClick()
    {//버튼 클릭시 실행함수
        buttonName = EventSystem.current.currentSelectedGameObject.name;//현재 클릭한 오브젝트 이름
        //string 문으로 함수 실행 
        SendMessage(buttonName);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }


    private void StartGame()
    {
        PlayerPrefs.SetInt("Phase", 0);
        PlayerPrefs.SetInt("Branch", 0);
        PlayerPrefs.SetInt("Count", 1);
        SceneManager.LoadScene(1);
    }

    private void Manual()
    {
        ExamplePanel.SetActive(true);
    }

    private void ClosePanel()
    {
        ExamplePanel.SetActive(false);
    }

    private void Exit()
    {//게임 종료
        Camera.main.GetComponent<PlayerInfoSet>().ResetInfo();
        Application.Quit();
    }
}
