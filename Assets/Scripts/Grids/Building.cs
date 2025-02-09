using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour
{
    public int idx;

    public bool Placed { get; private set; }
    public BoundsInt area;

    public SpriteRenderer sprite;
    private Color baseColor;

    public enum CannotBuild
    {
        okay,
        wrong_position,
        short_resource,
    }

    public void Set_Idx(int idx)
    {
        this.idx = idx;
    }

    public void Init()
    {
        baseColor = sprite.color;
        Color tempColor = baseColor;
        tempColor.a = 0.7f;

        sprite.color = tempColor;
    }

    public CannotBuild CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.Instance.gridLayout.WorldToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (!GM.Instance.CheckRes(idx))
        {
            return CannotBuild.short_resource;
        }

        if (!GridBuildingSystem.Instance.CanTakeArea(areaTemp))
        {
            return CannotBuild.wrong_position;
        }

        return CannotBuild.okay;
    }

    public void Place()
    {
        if (BuildingManager.Instance.AddPlayerBuilding(this))
        {
            Vector3Int positionInt = GridBuildingSystem.Instance.gridLayout.WorldToCell(transform.position);
            BoundsInt areaTemp = area;
            areaTemp.position = positionInt;

            Placed = true;
            sprite.color = baseColor;

            GridBuildingSystem.Instance.TakeArea(areaTemp);
        }
    }

    public void Remove()
    {
        Vector3Int positionInt = GridBuildingSystem.Instance.gridLayout.WorldToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        Placed = false;
        BuildingManager.Instance.RemovePlayerBuilding(this);
        GridBuildingSystem.Instance.RemoveArea(areaTemp);

        Destroy(gameObject);
    }
}
