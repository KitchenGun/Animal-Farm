using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackGroundAudio : MonoBehaviour
{

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

    private void OnLevelWasLoaded(int level)
    {
        if(level==2)
        Destroy(this.gameObject);
    }

}
