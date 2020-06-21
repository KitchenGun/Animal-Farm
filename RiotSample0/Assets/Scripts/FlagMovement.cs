using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagMovement : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.position.x >= -20)
        {//일정 지점까지 깃발이 움직이게 만듬
            this.gameObject.transform.position += (Vector3.left*1.5f) * Time.deltaTime;
        }
        else
        {//현재는 임시로 씬이동 
            SceneManager.LoadScene("Farm");
        }
    }
}
