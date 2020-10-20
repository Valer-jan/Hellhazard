using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalerUI : MonoBehaviour
{
    [Tooltip("script use rect and screen height as start velues, it change object scale")]
    public int HeightPercent = 10;

    void Start()
    {
        float rect = GetComponent<RectTransform>().sizeDelta.y;
        float height = Screen.height;
        Vector2 scale = gameObject.transform.localScale;
        float koef = height * HeightPercent / 100f / rect;

        gameObject.transform.localScale = scale * koef;
    }
}
