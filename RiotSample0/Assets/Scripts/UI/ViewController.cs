using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ViewController : MonoBehaviour
{
    //rect transform 컴포넌트를 캐시

    private RectTransform cachedRectTransform;//크기 및 위치 저장정보

    public RectTransform CachedRectTransform
    {
        get
        {
            if (cachedRectTransform == null)
            {//메모리에 할당되면 참조
                cachedRectTransform = GetComponent<RectTransform>();
            }
            return cachedRectTransform;
        }
    }
    //뷰의 타이틀을 가져와서 설정하는 프로퍼티
    public virtual string Title
    {
        get { return ""; }
        set { }
    }
}
