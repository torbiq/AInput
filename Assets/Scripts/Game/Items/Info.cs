using System;
using UnityEngine;

[Serializable]
public abstract class Info {
    [SerializeField]
    private string _name;
    [SerializeField]
    private string _description;
    [SerializeField]
    private string _packPrefabPath;
    [SerializeField]
    private string _groundPrefabPath;
    [SerializeField]
    private float _weight;
    [SerializeField]
    private int _width;
    [SerializeField]
    private int _height;

    public string name { get { return _name; } set { _name = value; } }
    public string description { get { return _description; } set { _description = value; } }
    public string packPrefabPath { get { return _packPrefabPath; } set { _packPrefabPath = value; } }
    public string groundPrefabPath { get { return _groundPrefabPath; } set { _groundPrefabPath = value; } }
    public float weight { get { return _weight; } set { _weight = value; } }
    public int width { get { return _width; } set { _width = value; } }
    public int height { get { return _height; } set { _height = value; } }
}
