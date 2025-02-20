using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingUIs : MonoBehaviour
{

    private RectTransform rt;

    public float speed;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        rt.anchoredPosition += new Vector2(speed * Time.unscaledDeltaTime, 0f);
    }
}
