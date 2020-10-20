using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDebug : MonoBehaviour
{
    public static Text TextComponent;

    private void Start()
    {
        TextComponent = GetComponent<Text>();
        TextComponent.text = "debug text";
    }
}
