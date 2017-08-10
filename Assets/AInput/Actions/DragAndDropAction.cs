using UnityEngine;
using DG.Tweening;
using System;
using AInput.Info;
using System.Collections.Generic;

namespace AInput {
    public class DragAndDropAction : BaseAction {
        public enum DragType {
            Follow,
            MoveWith,
        }
        public enum SafeDropType {
            UnableToTakeAny,
            DraggedOffCollider,
        }
        public DragType dragType { get; private set; }
        public SafeDropType safeDropType { get; private set; }
        public Action<Collider2D> OnTaken;
        public Action<Collider2D> OnTakeFailed;
        public Action<Collider2D> OnDragged;
        public Action<Collider2D> OnDragCollided;
        public Action<Collider2D> OnDragCollideFailed;
        public Action<Collider2D> OnDropped;
        public Action<Collider2D> OnDropFailed;
        public GameObject draggableObject { get; private set; }
        public Vector3 startPosition { get; private set; }
        public Collider2D takenObjectCollider { get; private set; }
        public BaseInfo takableInfo { get; private set; }
        public BaseInfo dragCollidableInfo { get; private set; }
        public BaseInfo droppableInfo { get; private set; }
        public bool canTake { get; private set; }
        public float followSpeed { get; private set; }
        public float dropDuration { get; private set; }
        #region Constructors
        private DragAndDropAction(BaseInfo takableInfo,
            BaseInfo dragCollidableInfo,
            BaseInfo droppableInfo,
            Action<Collider2D> OnTaken,
            Action<Collider2D> OnTakeFailed,
            Action<Collider2D> OnDragged,
            Action<Collider2D> OnDragCollided,
            Action<Collider2D> OnDragCollideFailed,
            Action<Collider2D> OnDropped,
            Action<Collider2D> OnDropFailed,
            SafeDropType safeDropType,
            DragType dragType,
            float followSpeed,
            float dropDuration) {
            this.OnTaken += OnTaken;
            this.OnTakeFailed += OnTakeFailed;
            this.OnDragged += OnDragged;
            this.OnDragCollided += OnDragCollided;
            this.OnDragCollideFailed += OnDragCollideFailed;
            this.OnDropped += OnDropped;
            this.OnDropFailed += OnDropFailed;
            this.takableInfo = takableInfo;
            this.dragCollidableInfo = dragCollidableInfo;
            this.droppableInfo = droppableInfo;
            this.safeDropType = safeDropType;
            this.dragType = dragType;
            this.followSpeed = followSpeed;
            this.dropDuration = dropDuration;
            this.canTake = true;
            this.draggableObject = null;
            this.startPosition = default(Vector3);
            this.takenObjectCollider = null;
        }
        public static DragAndDropAction Colliders(List<Collider2D> correctTakeColliders = null,
            List<Collider2D> uncorrectTakeColliders = null,
            List<Collider2D> correctDragCollidableColliders = null,
            List<Collider2D> uncorrectDragCollidableColliders = null,
            List<Collider2D> correctDropColliders = null,
            List<Collider2D> uncorrectDropColliders = null,
            Action<Collider2D> OnTaken = null,
            Action<Collider2D> OnTakeFailed = null,
            Action<Collider2D> OnDragged = null,
            Action<Collider2D> OnDragCollided = null,
            Action<Collider2D> OnDragCollideFailed = null,
            Action<Collider2D> OnDropped = null,
            Action<Collider2D> OnDropFailed = null,
            SafeDropType safeDropType = SafeDropType.UnableToTakeAny,
            DragType dragType = DragType.MoveWith,
            float followSpeed = 0.5f,
            float dropDuration = 0.5f) {
            return new DragAndDropAction(OnTaken: OnTaken,
                OnTakeFailed: OnTakeFailed,
                OnDragged: OnDragged,
                OnDragCollided: OnDragCollided,
                OnDragCollideFailed: OnDragCollideFailed,
                OnDropped: OnDropped,
                OnDropFailed: OnDropFailed,
                takableInfo: new CollidersInfo(correctTakeColliders, uncorrectTakeColliders),
                dragCollidableInfo: new CollidersInfo(correctDragCollidableColliders, uncorrectDragCollidableColliders),
                droppableInfo: new CollidersInfo(correctDropColliders, uncorrectDropColliders),
                safeDropType: safeDropType,
                dragType: dragType,
                followSpeed: followSpeed,
                dropDuration: dropDuration);
        }
        public static DragAndDropAction NamePart(string correctTakeNamePart = "",
            string uncorrectTakeNamePart = "",
            string correctDragCollidableNamePart = "",
            string uncorrectDragCollidableNamePart = "",
            string correctDropNamePart = "",
            string uncorrectDropNamePart = "", 
            Action<Collider2D> OnTaken = null,
            Action<Collider2D> OnTakeFailed = null,
            Action<Collider2D> OnDragged = null,
            Action<Collider2D> OnDragCollided = null,
            Action<Collider2D> OnDragCollideFailed = null,
            Action<Collider2D> OnDropped = null,
            Action<Collider2D> OnDropFailed = null,
            SafeDropType safeDropType = SafeDropType.UnableToTakeAny,
            DragType dragType = DragType.MoveWith,
            float followSpeed = 0.5f,
            float dropDuration = 0.5f) {
            return new DragAndDropAction(OnTaken: OnTaken,
                OnTakeFailed: OnTakeFailed,
                OnDragged: OnDragged,
                OnDragCollided: OnDragCollided,
                OnDragCollideFailed: OnDragCollideFailed,
                OnDropped: OnDropped,
                OnDropFailed: OnDropFailed,
                takableInfo: new NamePartInfo(correctTakeNamePart, uncorrectTakeNamePart),
                dragCollidableInfo: new NamePartInfo(correctDragCollidableNamePart, uncorrectDragCollidableNamePart),
                droppableInfo: new NamePartInfo(correctDropNamePart, uncorrectDropNamePart),
                safeDropType: safeDropType,
                dragType: dragType,
                followSpeed: followSpeed,
                dropDuration: dropDuration);
        }
        public static DragAndDropAction FullNames(List<string> correctTakeFullNames = null,
            List<string> uncorrectTakeFullNames = null,
            List<string> correctDragCollidableFullNames = null,
            List<string> uncorrectDragCollidableFullNames = null,
            List<string> correctDropFullNames = null,
            List<string> uncorrectDropFullNames = null,
            Action<Collider2D> OnTaken = null,
            Action<Collider2D> OnTakeFailed = null,
            Action<Collider2D> OnDragged = null,
            Action<Collider2D> OnDragCollided = null,
            Action<Collider2D> OnDragCollideFailed = null,
            Action<Collider2D> OnDropped = null,
            Action<Collider2D> OnDropFailed = null,
            SafeDropType safeDropType = SafeDropType.UnableToTakeAny,
            DragType dragType = DragType.MoveWith,
            float followSpeed = 0.5f,
            float dropDuration = 0.5f) {
            return new DragAndDropAction(OnTaken: OnTaken,
                OnTakeFailed: OnTakeFailed,
                OnDragged: OnDragged,
                OnDragCollided: OnDragCollided,
                OnDragCollideFailed: OnDragCollideFailed,
                OnDropped: OnDropped,
                OnDropFailed: OnDropFailed,
                takableInfo: new FullNamesInfo(correctTakeFullNames, uncorrectTakeFullNames),
                dragCollidableInfo: new FullNamesInfo(correctDragCollidableFullNames, uncorrectDragCollidableFullNames),
                droppableInfo: new FullNamesInfo(correctDropFullNames, uncorrectDropFullNames),
                safeDropType: safeDropType,
                dragType: dragType,
                followSpeed: followSpeed,
                dropDuration: dropDuration);
        }
        public Tween DropDraggableTo(Vector3 position, float duration) {
            if (!draggableObject) {
                Log.Error("No draggable object.");
                return null;
            }
            var objectDragged = draggableObject;
            switch (safeDropType) {
                case SafeDropType.UnableToTakeAny:
                    canTake = false;
                    DOVirtual.DelayedCall(duration, () => {
                        canTake = true;
                    }, false);
                    break;
                case SafeDropType.DraggedOffCollider:
                    var draggedCollider = takenObjectCollider;
                    if (draggedCollider) {
                        draggedCollider.enabled = false;
                    }
                    DOVirtual.DelayedCall(duration, () => {
                        if (draggedCollider) {
                            draggedCollider.enabled = true;
                        }
                    }, false);
                    break;
                default:
                    throw new Exception("Safe drop type unrecognized.");
            }
            draggableObject = null;
            takenObjectCollider = null;
            startPosition = default(Vector3);
            return objectDragged.transform.DOMove(position, duration);
        }
        public override void Update() {
            if (inputController.mouseDown && canTake) {
                var touchedColliders = inputController.TouchedCollidersWorld();
                int length = touchedColliders.Length;
                for (int i = 0; i < length; i++) {
                    var collider = touchedColliders[i];
                    if (takableInfo.IsCorrect(collider)) {
                        if (OnTaken != null) {
                            OnTaken(collider);
                        }
                        draggableObject = collider.gameObject;
                        takenObjectCollider = draggableObject.GetComponent<Collider2D>();
                        startPosition = draggableObject.transform.position;
                        break;
                    }
                    if (takableInfo.IsUncorrect(collider)) {
                        if (OnTakeFailed != null) {
                            OnTakeFailed(collider);
                        }
                        break;
                    }
                }
            }
            if (inputController.mouseDrag && draggableObject) {
                switch (dragType) {
                    case DragType.Follow:
                        draggableObject.transform.position = Vector3.Lerp(draggableObject.transform.position, inputController.currMouseWorldPos.Value, followSpeed * Time.unscaledDeltaTime);
                        break;
                    case DragType.MoveWith:
                        draggableObject.transform.position += (Vector3)inputController.currentDragDeltaVectorWorld;
                        break;
                    default:
                        throw new Exception("Drag type unrecognized");
                }
                if (OnDragged != null) {
                    OnDragged(takenObjectCollider);
                }
                bool wasColliderEnabled = false;
                if (takenObjectCollider) {
                    if (takenObjectCollider.enabled) {
                        takenObjectCollider.enabled = false;
                        wasColliderEnabled = true;
                    }
                }
                var touchedColliders = inputController.TouchedCollidersWorld();
                if (wasColliderEnabled) {
                    takenObjectCollider.enabled = true;
                }
                int length = touchedColliders.Length;
                for (int i = 0; i < length; i++) {
                    var collider = touchedColliders[i];
                    if (dragCollidableInfo.IsCorrect(collider)) {
                        if (OnDragCollided != null) {
                            OnDragCollided(collider);
                        }
                        DropDraggableTo(collider.transform.position, dropDuration);
                        break;
                    }
                    if (dragCollidableInfo.IsUncorrect(collider)) {
                        if (OnDragCollideFailed != null) {
                            OnDragCollideFailed(collider);
                        }
                        DropDraggableTo(startPosition, dropDuration);
                        break;
                    }
                }
            }
            if (inputController.mouseUp && draggableObject) {
                bool wasColliderEnabled = false;
                if (takenObjectCollider) {
                    if (takenObjectCollider.enabled) {
                        takenObjectCollider.enabled = false;
                        wasColliderEnabled = true;
                    }
                }
                var touchedColliders = inputController.TouchedCollidersWorld();
                if (wasColliderEnabled) {
                    takenObjectCollider.enabled = true;
                }
                int length = touchedColliders.Length;
                for (int i = 0; i < length; i++) {
                    var collider = touchedColliders[i];
                    if (droppableInfo.IsCorrect(collider)) {
                        if (OnDropped != null) {
                            OnDropped(collider);
                        }
                        DropDraggableTo(collider.transform.position, dropDuration);
                        return;
                    }
                    if (droppableInfo.IsUncorrect(collider)) {
                        if (OnDropFailed != null) {
                            OnDropFailed(collider);
                        }
                        DropDraggableTo(startPosition, dropDuration);
                        return;
                    }
                }
                DropDraggableTo(startPosition, dropDuration);
            }
        }
        #endregion
    }
}
