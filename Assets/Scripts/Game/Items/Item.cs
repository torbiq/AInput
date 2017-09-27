using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Item<TInfo> : MonoBehaviour where TInfo : Info {
    [SerializeField]
    private Info _info;
    public Info info { get { return _info; } protected set { _info = value; } }
}
