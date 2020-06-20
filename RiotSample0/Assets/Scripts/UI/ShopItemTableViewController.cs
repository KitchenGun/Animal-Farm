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
            new ShopItemData{ iconName ="8"},
            new ShopItemData{ iconName ="2" },
            new ShopItemData{ iconName ="3" },
            new ShopItemData{ iconName ="5"},
            new ShopItemData{ iconName ="4"}
        };
        //스크롤 할 내용의 크기를 갱신
        UpdateContents();
    }

    protected override float CellWidthIndex(int index)
    {
        
        return 300.0f;
    }
    protected override void Awake()
    {
        base.Awake();
        //아이콘의 스프라이트 시트에 포함된 스프라이트를 캐시
        SpriteSheetManager.Load("AnimalIdle");
    }

    protected override void Start()
    {
        base.Start();
        LoadData();
    }
}
