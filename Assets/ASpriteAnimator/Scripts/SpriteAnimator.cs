using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class SpriteAnimation {
    [SerializeField]
    private string _name;
    [SerializeField]
    private float _fps;
    [SerializeField]
    private List<Sprite> _spriteFrames;

    public string name { get { return _name; } private set { _name = value; } }
    public float fps { get { return _fps; } private set { _fps = value; } }
    public List<Sprite> spriteFrames { get { return _spriteFrames; } private set { _spriteFrames = value; } }
}
