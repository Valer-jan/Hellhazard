using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField]
    public AmmoType[] AmmoTypes;
}

[Serializable]
public class AmmoType
{
    public string TypeName;
    public int MaxCount, CurrentCount;
}
