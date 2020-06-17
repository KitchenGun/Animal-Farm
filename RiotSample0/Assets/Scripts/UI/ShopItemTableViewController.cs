using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ShopItemTableViewController : TableViewController<ShopItemData>
{
    //리스트 항목의 데이터를 읽어들이는 메서드
    private void LoadData()
    {
        //일반적인 데이터는 데이터 소스로부터 가져오는데 여기서는 하드 코드를 사용해 정의 한다.
        tableData = new List<ShopItemData>()
        {
            new ShopItemData{ iconName ="drink1", name = "WATER",price=100,description="Nothing else, just water"},
            new ShopItemData{ iconName ="drink2", name = "SODA",price=150,description="Sugar free and low calroie"},
            new ShopItemData{ iconName ="drink3", name = "COFFEE",price=200,description="How would u like ur coffee"},
            new ShopItemData{ iconName ="drink4", name = "ENERGY DRINK",price=300,description="GIVING MORE ENERGY!!!"},
            new ShopItemData{ iconName ="drink5", name = "BEER",price=500,description="THIS IS !@#$%@#!%^&^&##!!"}
        };
        //스크롤 할 내용의 크기를 갱신
        UpdateContents();
    }

    protected override float CellheightAtIndex(int index)
    {
        if(index>=0&&index<=tableData.Count -1)
        {
            if (tableData[index].price >= 1000)
            {
                //가격이 1000원 이상인 아이템을 표시하는 셀의 높이를 반환한다.
                return 240.0f;
            }
            else if (tableData[index].price >= 500) 
            {
                //가격이 500원이상인 아이템을 표시하는 셀의 높이를 반환한다
                return 160.0f;
            }
        }
        return 128.0f;
    }

    protected override void Awake()
    {
        base.Awake();
        //아이콘의 스프라이트 시트에 포함된 스프라이트를 캐시
        SpriteSheetManager.Load("IconAtlas");
    }

    protected override void Start()
    {
        base.Start();
        LoadData();
    }
}
