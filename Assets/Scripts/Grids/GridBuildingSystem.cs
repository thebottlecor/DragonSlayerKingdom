using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using AYellowpaper.SerializedCollections;
using UnityEngine.EventSystems;

public class GridBuildingSystem : Singleton<GridBuildingSystem>
{

    public GridLayout gridLayout;
    public Tilemap mainTilemap;
    public Tilemap tempTilemap;


    [SerializedDictionary("이게뭐여?", "이것은 말이지")]
    public SerializedDictionary<TileType, TileBase> tileBases;

    public enum TileType
    {
        Empty,
        White,
        Green,
        Red,
    }

    private Building temp;
    private Vector3 prevPos;
    private BoundsInt prevArea;

    private void Start()
    {
        mainTilemap.gameObject.SetActive(false);
    }

    private TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    private void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }

    private void FillTiles(TileBase[] arr, TileType type)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }

    // 버튼으로 건설 시작
    public void InitWithBuilding(GameObject building)
    {
        if (temp != null)
        {
            ClearArea();
            Destroy(temp.gameObject);
        }
        mainTilemap.gameObject.SetActive(true);

        temp = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<Building>();
        temp.Init();
        FollowBuilding();
    }

    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y];
        FillTiles(toClear, TileType.Empty);
        tempTilemap.SetTilesBlock(prevArea, toClear);
    }

    private void FollowBuilding()
    {
        ClearArea();

        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;

        TileBase[] baseArray = GetTilesBlock(buildingArea, mainTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < size; i++)
        {
            if (baseArray[i] == tileBases[TileType.White])
            {
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }

        tempTilemap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, mainTilemap);
        foreach (var b in baseArray)
        {
            if (b != tileBases[TileType.White])
            {
                Debug.Log("Cannot place Here");
                return false;
            }
        }
        return true;
    }
    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, tempTilemap);
        SetTilesBlock(area, TileType.Green, mainTilemap);
    }

    private void Update()
    {
        if (temp == null)
        {
            return;

        }

        if (Input.GetMouseButtonDown(1))
        {
            ClearArea();
            Destroy(temp.gameObject);
            temp = null;
            mainTilemap.gameObject.SetActive(false);
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject(0))
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (temp.CanBePlaced())
            {
                temp.Place();
                temp = null;
                mainTilemap.gameObject.SetActive(false);
                return;
            }
        }

        if (!temp.Placed)
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

            if (prevPos != cellPos)
            {
                temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(0.5f, 0.5f, 0f));
                prevPos = cellPos;
                FollowBuilding();
            }
        }
    }
}
