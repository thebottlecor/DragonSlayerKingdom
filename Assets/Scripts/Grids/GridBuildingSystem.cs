using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using AYellowpaper.SerializedCollections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    private Building removeTemp;
    public Slider removeCastingBar;
    private int bulidingLayer;
    private float removeTimer;
    private const float removeTime = 0.5f;

    private void Start()
    {
        mainTilemap.gameObject.SetActive(false);
        //bulidingLayer = 1 << LayerMask.NameToLayer("Building") | 1 << LayerMask.NameToLayer("Ramp");
        bulidingLayer = 1 << LayerMask.NameToLayer("Building");
        removeCastingBar.gameObject.SetActive(false);
        removeTimer = 0f;
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
    public void InitWithBuilding(int idx)
    {
        if (temp != null)
        {
            ClearArea();
            Destroy(temp.gameObject);
        }
        mainTilemap.gameObject.SetActive(true);

        temp = Instantiate(BuildingManager.Instance.buildingInfos[idx].prefab, Vector3.zero, Quaternion.identity);
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
    public void RemoveArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.White, mainTilemap);
    }

    private void Update()
    {
        if (temp == null)
        {
            if (removeTemp == null)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    var others = Physics2D.Raycast(touchPos, Vector2.zero, 100f, bulidingLayer);

                    if (others.collider != null)
                    {
                        var b = others.collider.GetComponent<Building>();
                        if (!BuildingManager.Instance.buildingInfos[b.idx].indestructible)
                        {
                            removeTemp = b;
                            removeCastingBar.value = 0f;
                            removeTimer = 0f;
                            removeCastingBar.gameObject.SetActive(true);
                        }
                    }
                }
            }
            else
            {
                if (Input.GetMouseButton(1))
                {
                    removeTimer += Time.deltaTime;
                    removeCastingBar.value = removeTimer / removeTime;

                    if (removeTimer >= removeTime)
                    {
                        removeTemp.Remove();
                        removeCastingBar.gameObject.SetActive(false);
                        removeTemp = null;
                    }
                }
                else
                {
                    removeCastingBar.gameObject.SetActive(false);
                    removeTemp = null;
                }
            }

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
            var result = temp.CanBePlaced();
            switch (result)
            {
                case Building.CannotBuild.okay:
                    temp.Place();
                    temp = null;
                    mainTilemap.gameObject.SetActive(false);
                    return;
                case Building.CannotBuild.wrong_position:
                    break;
                case Building.CannotBuild.short_resource:
                    break;
                case Building.CannotBuild.short_housing:
                    break;
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
