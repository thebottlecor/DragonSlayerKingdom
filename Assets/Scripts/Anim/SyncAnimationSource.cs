using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SyncAnimationSource : MonoBehaviour
{

    public int buildingIdx;
    public int frameCount;

    //public int count;
    public SpriteRenderer sprite;
    //public Sprite[] images;

    public static EventHandler<int> SpriteChangedEvent;


    private void Update()
    {
        //sprite.sprite = images[count];
        if (SpriteChangedEvent != null)
            SpriteChangedEvent(this, buildingIdx);

        //count++;
        //if (count >= images.Length)
        //    count = 0;
    }
}
