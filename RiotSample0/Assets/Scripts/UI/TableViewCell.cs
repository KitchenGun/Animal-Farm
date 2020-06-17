using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class TableViewCell<T> : ViewController
{
    //셀 내용 갱신
    public virtual void UpdateContent(T itemData)
    {
        //실제 처리는 상속한 클래스에서 구현
        //반드시 구현 필요없음 
    }
    //셀에 대응하는 리스트 항목의 인덱스 저장
    public int DataIndex { get; set; }

    //셀 높이
    public float Height
    {
        get { return CachedRectTransform.sizeDelta.y; }
        set
        {
            Vector2 sizeDelta = CachedRectTransform.sizeDelta;
            sizeDelta.y = value;
            CachedRectTransform.sizeDelta = sizeDelta;
        }
    }

    //셀위쪽 끝의 위치
    public Vector2 Top
    {
        get
        {
            Vector3[] coners = new Vector3[4];
            CachedRectTransform.GetLocalCorners(coners);
            return CachedRectTransform.anchoredPosition + new Vector2(0f, coners[1].y);
        }
        set
        {
            Vector3[] coners = new Vector3[4];
            CachedRectTransform.GetLocalCorners(coners);
            CachedRectTransform.anchoredPosition=value- new Vector2(0f, coners[1].y);
        }
    }

    //셀 아래쪽
    public Vector2 Bottom
    {
        get
        {
            Vector3[] coners = new Vector3[4];
            CachedRectTransform.GetLocalCorners(coners);
            return CachedRectTransform.anchoredPosition + new Vector2(0f, coners[3].y);
        }
        set
        {
            Vector3[] coners = new Vector3[4];
            CachedRectTransform.GetLocalCorners(coners);
            CachedRectTransform.anchoredPosition = value - new Vector2(0f, coners[3].y);
        }
    }


}
