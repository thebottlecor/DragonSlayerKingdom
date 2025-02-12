using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour
{
    public int idx;

    public bool Placed { get; private set; }
    public BoundsInt area;

    public Transform center;
    public SpriteRenderer[] sprites;
    private Color[] baseColors;

    public enum CannotBuild
    {
        okay,
        wrong_position,
        short_resource,
        short_housing,
    }

    public void Set_Idx(int idx)
    {
        this.idx = idx;
    }

    public void Init()
    {
        baseColors = new Color[sprites.Length];
        for (int i = 0; i < baseColors.Length; i++)
        {
            baseColors[i] = sprites[i].color;
            Color tempColor = baseColors[i];
            tempColor.a = 0.7f;
            sprites[i].color = tempColor;
        }
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
        if (!GM.Instance.CheckHousing(idx))
        {
            return CannotBuild.short_housing;
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
            for (int i = 0; i < baseColors.Length; i++)
            {
                sprites[i].color = baseColors[i];
            }

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
