using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemTableViewCell : TableViewCell<ShopItemData>
{
    [SerializeField]
    private Image iconImage;//아이콘 표시할 이미지
    [SerializeField]
    private Text nameLabel;//아이템 이름을 표시할 텍스트
    [SerializeField]
    private Text priceLable;//가격을 표시할 텍스트

    public override void UpdateContent(ShopItemData itemData)
    {
        nameLabel.text = itemData.name;
        priceLable.text = itemData.price.ToString();

        //스프라이트 시트 이름과 스프라이트 이름을 지정해 아이콘 스프라이트 변경
        iconImage.sprite =
            SpriteSheetManager.GetSpriteByName("IconAtlas", itemData.iconName);

    }
}
