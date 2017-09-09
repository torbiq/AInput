using System;
using UnityEngine;
using AJSON;

public class PlayerData {
    [JSONProperty(propertyName = "coins", serializedType = SerializedType.Integer)]
    public int _coins;
    [JSONProperty(propertyName = "position", serializedType = SerializedType.Float)]
    public Vector3 _position;

    //public int coins { 
    //    get {
    //        return _coins;
    //    }
    //    set {
    //        _coins = value;
    //        if (OnCoinsChangedEvent != null) {
    //            OnCoinsChangedEvent(_coins);
    //        }
    //    }
    //}
    //public Vector3 position {
    //    get {
    //        return _position;
    //    }
    //    set {
    //        _position = value;
    //        if (OnPositionChangedEvent != null) {
    //            OnPositionChangedEvent(_position);
    //        }
    //    }
    //}

    //public Action<int> OnCoinsChangedEvent;
    //public Action<Vector3> OnPositionChangedEvent;

    //public PlayerData() {
    //    coins = 0;
    //    _position = new Vector3();
    //}
}
