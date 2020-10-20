using System.Collections;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public void Web(string url)
    {
        Application.OpenURL(url);
    }
}
