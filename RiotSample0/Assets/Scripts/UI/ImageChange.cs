using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ImageChange : MonoBehaviour
{
    
    public Sprite[] Images;
    public GameObject Button;
    private int ImageNum;

    private void Start()
    {
        ImageNum = 0;
        this.gameObject.GetComponent<Image>().sprite = Images[ImageNum];
    }


    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            ImageNum++;
            if(ImageNum>=Images.Length)
            {
                ImageNum = Images.Length-1;
                Button.SetActive(false);
            }
            this.gameObject.GetComponent<Image>().sprite = Images[ImageNum];
        }
    }

    public void NextImg()
    {
        ImageNum++;
        if (ImageNum >= Images.Length)
        {
            ImageNum = Images.Length-1;
            Button.SetActive(false);
        }
        this.gameObject.GetComponent<Image>().sprite = Images[ImageNum];
    }
}
