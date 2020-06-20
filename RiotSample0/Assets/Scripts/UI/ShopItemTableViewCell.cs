using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemTableViewCell : TableViewCell<ShopItemData>
{
    private static int num=1;
    [SerializeField]
    private Image AnimalImage;//동물 idle image 표시할 이미지

    private void Update()
    {
        Vector3 nullZ=this.transform.localPosition;
        nullZ.z = 0f;
        this.transform.localPosition = nullZ;
    }

    public override void UpdateContent(ShopItemData itemData)
    {
       
        //스프라이트 시트 이름과 스프라이트 이름을 지정해 아이콘 스프라이트 변경
        AnimalImage.sprite =
            SpriteSheetManager.GetSpriteByName("AnimalIdle", itemData.iconName);

    }
}
