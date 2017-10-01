using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitcoinMiner {
    public class UINode : MonoBehaviour {
        [SerializeField]
        private UINode _parent;
        [SerializeField]
        private AUIElement _element;
        [SerializeField]
        private List<UINode> _children;

        public UINode parent { get { return _parent; } set { _parent = value; } }
        public AUIElement element { get { return _element; } set { _element = value; } }
        public List<UINode> children { get { return _children; } set { _children = value; } }
    }
}
