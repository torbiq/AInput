using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BaseItem {
    public string name;
    public string description;

    public string packPrefabPath;
    public string groundPrefabPath;

    public float weight;
    public int width;
    public int height;
}
