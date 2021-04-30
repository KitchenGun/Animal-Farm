using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterImage : MonoBehaviour
{
    public Sprite major;
    public Sprite mouse;
    public Sprite[] jones;
    public Sprite[] napoleon;
    public Sprite[] boxer;
    public Sprite[] benjamin;
    public Sprite[] snowball;
    public Sprite squaler;
    public Sprite[] dogs;
    public Sprite animals;

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
            case "Jones"://메이어
                this.gameObject.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
                this.gameObject.GetComponent<Image>().sprite = jones[face];
                break;
            case "Rat"://생쥐
                this.gameObject.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
                this.gameObject.GetComponent<Image>().sprite = mouse;
                break;
            case "Napoleon"://나폴레옹
                this.gameObject.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
                this.gameObject.GetComponent<Image>().sprite = napoleon[face];
                break;
            case "Snowball"://스노우볼
                this.gameObject.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
                this.gameObject.GetComponent<Image>().sprite = snowball[face];
                break;
            case "Squaler"://스퀄러
                this.gameObject.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
                this.gameObject.GetComponent<Image>().sprite = squaler;
                break;
            case "Boxer"://복서
                this.gameObject.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
                this.gameObject.GetComponent<Image>().sprite = boxer[face];
                break;
            case "Dogs"://개
                this.gameObject.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
                this.gameObject.GetComponent<Image>().sprite = dogs[face];
                break;
            case "Animals"://동물들
                this.gameObject.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
                this.gameObject.GetComponent<Image>().sprite = animals;
                break; 
            case "Benjamin"://벤자민
                this.gameObject.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
                this.gameObject.GetComponent<Image>().sprite = benjamin[face];
                break;
        }

    }

    
}
