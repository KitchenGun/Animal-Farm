using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackGroundAudio : MonoBehaviour
{
    public AudioClip[] BGM;
    public static BackGroundAudio instance;

    // Start is called before the first frame update
    private void Awake()
    {
        
        if(instance!=null)
        {
            this.gameObject.GetComponent<AudioSource>().Stop();
            
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);        
    }


    public void RevolutionTime()
    {
        this.gameObject.GetComponent<AudioSource>().clip = BGM[0];
        this.gameObject.GetComponent<AudioSource>().Play();
    }

   
    void OnEnable()
    {
        // 델리게이트 체인 추가
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex==1)
        {
            this.gameObject.GetComponent<AudioSource>().volume = 0.1f;
        }
        this.gameObject.GetComponent<AudioSource>().clip = BGM[scene.buildIndex];
        this.gameObject.GetComponent<AudioSource>().Play();
    }

    void OnDisable()
    {
        // 델리게이트 체인 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
