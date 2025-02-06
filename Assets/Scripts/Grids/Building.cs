using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour
{
    public bool Placed { get; private set; }
    public BoundsInt area;

    public SpriteRenderer sprite;
    private Color baseColor;

    [Header("Á¤º¸")]
    public GameObject spawnUnit;
    public int spawnCount = 1;

    public void Init()
    {
        baseColor = sprite.color;
        Color tempColor = baseColor;
        tempColor.a = 0.7f;

        sprite.color = tempColor;
    }

    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.Instance.gridLayout.WorldToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (GridBuildingSystem.Instance.CanTakeArea(areaTemp))
        {
            return true;
        }
        return false;
    }

    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.Instance.gridLayout.WorldToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        Placed = true;
        sprite.color = baseColor;
        BuildingManager.Instance.playerBuildings.Add(this);
        GridBuildingSystem.Instance.TakeArea(areaTemp);
    }
}
