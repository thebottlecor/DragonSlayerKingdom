using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SyncAnimation : EventListener
{

    private Building building;

    protected override void OnEnable()
    {
        base.OnEnable();

        building = GetComponentInParent<Building>();
    }

    protected override void AddListeners()
    {
        SyncAnimationSource.SpriteChangedEvent += OnSpriteChanged;
    }

    protected override void RemoveListeners()
    {
        SyncAnimationSource.SpriteChangedEvent -= OnSpriteChanged;
    }

    private void OnSpriteChanged(object sender, int e)
    {
        var source = sender as SyncAnimationSource;

        if (building.idx == e)
        {
            building.sprites[0].sprite = source.sprite.sprite;
        }
    }
}
