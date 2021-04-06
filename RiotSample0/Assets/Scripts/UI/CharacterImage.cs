using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterImage : MonoBehaviour
{
    public Sprite major;
    public Sprite mouse;


    public void FaceChange(string name,int face)
    {
        switch (name)
        {
            case "Narration"://나레이션
                this.gameObject.GetComponent<Image>().color = new Vector4(0, 0, 0, 0);
                break;
            case "Major"://메이어
                this.gameObject.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
                this.gameObject.GetComponent<Image>().sprite = major;
                break;
            case "Mouse"://생쥐
                this.gameObject.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
                this.gameObject.GetComponent<Image>().sprite = mouse;
                break;
        }

    }
}
