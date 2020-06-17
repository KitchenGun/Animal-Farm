using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TableViewController<T> : ViewController
{
    protected List<T> tableData = new List<T>();//리스트 항목 데이터 저장
    [SerializeField]
    private RectOffset padding;//스크롤할 내용의 패딩
    [SerializeField]
    private float spacingHeight = 4.0f;//각셀의 간격
    //scroll rect 컴포넌 캐시
    private ScrollRect cachedScrollRect;
    public ScrollRect CachedScrollRect
    {
        get
        {
            if(cachedScrollRect == null)
            {
                cachedScrollRect = GetComponent<ScrollRect>();
            }
            return cachedScrollRect;
        }
    }

    [SerializeField]
    private GameObject cellBase;//복사 원본 셀
    private LinkedList<TableViewCell<T>> cells = new LinkedList<TableViewCell<T>>();//셀 저장

    private Rect visibleRect; //리스트 항목을 셀의 형태로 표시하는 범위를 나타내는 사각형
    [SerializeField]
    private RectOffset visibleRectPadding;//visibleRect의 패딩

    private Vector2 prevScrollPos;//바로 전의 스크롤 위치


    //인스턴트를 로드할때 호출 된다
    protected virtual void Awake()
    {

    }
    //리스트 항목에 대응하는 셀의 높이 반환하는 메서드
    protected virtual float CellheightAtIndex(int index)
    {
        //실제 값을 반환하는 처리는 상속한 클래스에서 구현
        return 0f;
    }
    //스크롤할 내용전체의 높이를 갱신하는 메서드
    protected void UpdateContentSize()
    {
        //스크롤할 내용 전체의 높이를 계산
        float contentHeight = 0f;
        for(int i=0;i<tableData.Count;i++)
        {
            contentHeight += CellheightAtIndex(i);
            if (i > 0) { contentHeight += spacingHeight; }
        }

        //스크롤할 내용의 높이를 설정
        Vector2 sizeDelta = CachedScrollRect.content.sizeDelta;
        sizeDelta.y = padding.top + contentHeight + padding.bottom;
        CachedScrollRect.content.sizeDelta = sizeDelta;
    }

    protected virtual void Start()
    {
        cellBase.SetActive(false);
        //scroll rect 컴포넌트에 속한 OnValueChanged 이벤트 리스너 설정
        CachedScrollRect.onValueChanged.AddListener(OnScrollPosChanged);
    }

    private TableViewCell<T> CreateCellForIndex(int index)
    {
        //복사 원본 셀을 이용해 새로운 셀 생성
        GameObject obj = Instantiate(cellBase) as GameObject;
        TableViewCell<T> cell = obj.GetComponent<TableViewCell<T>>();

        //보모 요소를 바꾸면 스케일이나 크기를 잃어버리므로 변수에 저장
        Vector3 scale = cell.transform.localScale;
        Vector2 sizeDelta = cell.CachedRectTransform.sizeDelta;
        Vector2 offsetMin = cell.CachedRectTransform.offsetMin;
        Vector2 offsetMax = cell.CachedRectTransform.offsetMin;

        cell.transform.SetParent(cellBase.transform.parent);

        //셀의 스케일과 크기를 설정
        cell.transform.localScale = scale;
        cell.CachedRectTransform.sizeDelta = sizeDelta;
        cell.CachedRectTransform.offsetMin = offsetMin;
        cell.CachedRectTransform.offsetMax = offsetMax;
        //지정된 인덱스가 붙은 리스트 항목에 대응하는 셀로 내용을 갱신
        UpdateCellForIndex(cell, index);
        cells.AddLast(cell);

        return cell;
    }

    private void UpdateCellForIndex(TableViewCell<T> cell,int index)
    {
        //셀에 대응하는 리스트 항목의 인덱스 설정
        cell.DataIndex = index;

        if (0 <= cell.DataIndex && cell.DataIndex <= tableData.Count - 1) 
        {
            //셀에 대응하는 리스트 항목이 있다면 셀을 활성화 해서 내용을 갱신하고 높이를 설정
            cell.gameObject.SetActive(true);
            cell.UpdateContent(tableData[cell.DataIndex]);
            cell.Height = CellheightAtIndex(cell.DataIndex);
        }
        else
        {
            //셀에 대응하는 리스트 항목이 없다면 셀을 비활성 시켜서 표시되지 않도록 한다
            cell.gameObject.SetActive(false);
        }
    }

    private void UpdateVisibleRect()
    {
        //visibleRect의 위치는 스크롤 할 내용의 기준으로 부터 상대적인 위치다
        visibleRect.x = cachedScrollRect.content.anchoredPosition.x + visibleRectPadding.left;
        visibleRect.y = cachedScrollRect.content.anchoredPosition.y + visibleRectPadding.top;
        //visibleRect의 크기는 스크롤 뷰의 크기 + 패딩
        visibleRect.width = CachedRectTransform.rect.width + visibleRectPadding.left + visibleRectPadding.right;
        visibleRect.height = CachedRectTransform.rect.height + visibleRectPadding.top + visibleRectPadding.bottom;
    }

    protected void UpdateContents()
    {
        UpdateContentSize();
        UpdateVisibleRect();
        if(cells.Count<1)
        {
            //셀이 하나도 없을 때는 visibleRect의 범위에 들어가는 첫번째 리스트항목을 찾아서
            //그에 대응하는 셀을 작성한다.
            Vector2 cellTop = new Vector2(0f, -padding.top);
            for (int i = 0; i < tableData.Count; i++)
            {
                float cellHeight = CellheightAtIndex(i);
                Vector2 cellBottom = cellTop + new Vector2(0f, -cellHeight);
                if ((cellTop.y <= visibleRect.y && cellTop.y >= visibleRect.y - visibleRect.height) ||
                    (cellBottom.y <= visibleRect.y && cellBottom.y >= visibleRect.y - visibleRect.height)) 
                {
                TableViewCell<T> cell = CreateCellForIndex(i);
                cell.Top = cellTop;
                break;
                }
                cellTop = cellBottom + new Vector2(0f, spacingHeight);
            }
            //visible의 범위에 빈곳이 있으면 셀을 작성한다
            FillVisibleRectWithCell();
        }
        else
        {
            //이미 셀이 있을 때는 첫 번째 셀부터 순서대로 대응하는 리스트 항목의
            //인덱스를 다시 설정하고 위치와 내용을 갱신한다
            LinkedListNode<TableViewCell<T>> node = cells.First;
            UpdateCellForIndex(node.Value, node.Value.DataIndex);

            node = node.Next;

            while (node != null) 
            {
                UpdateCellForIndex(node.Value, node.Previous.Value.DataIndex + 1);
                node.Value.Top =
                    node.Previous.Value.Bottom + new Vector2(0f, -spacingHeight);
                node = node.Next;
            }
            //visibleRect의 범위에 빈곳이 있으면 셀을 작성
            FillVisibleRectWithCell();
        }
    }

    private void FillVisibleRectWithCell()
    {
        //셀
        if(cells.Count<1)
        {
            return;
        }
        //표시도니 마지막 셀에 대응하는 리스트 항목의 다음 리스트 항목이 있고
        //또한 그셀이 visibleRect의 범위 에 들어온다면 대응한느 셀을 작성
        TableViewCell<T> lastCell = cells.Last.Value;
        int nextCellDataIndex = lastCell.DataIndex + 1;
        Vector2 nextCellTop = lastCell.Bottom + new Vector2(0f, -spacingHeight);

        while(nextCellDataIndex<tableData.Count&&nextCellTop.y>=visibleRect.y-visibleRect.height)
        {
            TableViewCell<T> cell = CreateCellForIndex(nextCellDataIndex);
            cell.Top = nextCellTop;

            lastCell = cell;
            nextCellDataIndex = lastCell.DataIndex + 1;
            nextCellTop = lastCell.Bottom + new Vector2(0f, -spacingHeight);
        }
    }

    public void OnScrollPosChanged(Vector2 scrollPos)
    {
        //visibleReck
        UpdateVisibleRect();
        //스크롤 방향에 따라 셀을 다시 이용해 표시를 갱신
        ReuseCells((scrollPos.y < prevScrollPos.y) ? 1 : -1);
        prevScrollPos = scrollPos;
    }

    private void ReuseCells(int scrollDirection)
    {
        if(cells.Count<1)
        {
            return;
        }
        if (scrollDirection > 0)
        {
            //위로 스크롤 하고 있을때는 visibleRect에 지정된 범위 보다 위에 있는 셀을
            //아래를 향해 순서대로 이동 시켜 내용을 갱신한다
            TableViewCell<T> firstCell = cells.First.Value;
            while (firstCell.Bottom.y > visibleRect.y)
            {
                TableViewCell<T> lastCell = cells.Last.Value;
                UpdateCellForIndex(firstCell, lastCell.DataIndex + 1);
                firstCell.Top = lastCell.Bottom + new Vector2(0f, -spacingHeight);

                cells.AddLast(firstCell);
                cells.RemoveFirst();
                firstCell = cells.First.Value;
            }
            //visibleRect에 지정된 범위안에 빈곳이 있으면 셀을 작성한다
            FillVisibleRectWithCell();
        }
        else if (scrollDirection < 0) 
        {
            //아래로 스크롤 하고 있을 때는 visibleRect에 지정된 범위 보다 아래에 있는 셀을
            //위를 향해 순서대로 이동시켜 내용을 갱신한다.
            TableViewCell<T> lastCell = cells.Last.Value;
            while(lastCell.Top.y<visibleRect.y - visibleRect.height)
            {
                TableViewCell<T> firstCell = cells.First.Value;
                UpdateCellForIndex(lastCell, firstCell.DataIndex - 1);
                lastCell.Bottom = firstCell.Top + new Vector2(0f, spacingHeight);

                cells.AddFirst(lastCell);
                cells.RemoveLast();
                lastCell = cells.Last.Value;
            }
        }
    }

}
