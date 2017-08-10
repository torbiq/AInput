using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using AInput;

public class TestAInput : MonoBehaviour {
    private DragAndDropAction _dragRedToYellow;
    private DragAndDropAction _dragPurpleToViolet;
    private DragAndDropAction _dragGreenToOrange;
    private DragAndDropAction _dragCyanToEmerald;

    [SerializeField]
    private List<Transform> _blackColliders = new List<Transform>();

    private void RandomScaling(Transform transform, float minScale, float maxScale, float minDuration, float maxDuration) {
        transform.DOScale(Random.Range(minScale, maxScale), Random.Range(minDuration, maxDuration)).SetEase(Ease.Linear).SetLoops(1, LoopType.Yoyo).OnComplete(() => {
            RandomScaling(transform, minScale, maxScale, minDuration, maxDuration);
        });
    }

    private void Awake() {
        foreach (var black in _blackColliders) {
            RandomScaling(black, 0.5f, 3f, 1.5f, 3f);
        }
        _dragRedToYellow = DragAndDropAction.FullNames(correctTakeFullNames: new List<string> { "Red" },
            uncorrectTakeFullNames: new List<string> { "Yellow", "Purple", },
            OnTakeFailed: collider => {
                Log.Msg("You took " + collider.name);
            },
            uncorrectDragCollidableFullNames: new List<string> { "Black" },
            correctDropFullNames: new List<string> { "Yellow" },
            OnDropped: collider => {
                AInputMain.SetAction(_dragPurpleToViolet);
            });
        AInputMain.SetAction(_dragRedToYellow);
        _dragPurpleToViolet = DragAndDropAction.FullNames(correctTakeFullNames: new List<string> { "Purple" },
            uncorrectDragCollidableFullNames: new List<string> { "Black" },
            correctDropFullNames: new List<string> { "Violet" },
            OnDropped: collider => {
                AInputMain.SetAction(_dragGreenToOrange);
            });
        _dragGreenToOrange = DragAndDropAction.FullNames(correctTakeFullNames: new List<string> { "Green" },
            uncorrectDragCollidableFullNames: new List<string> { "Black" },
            correctDropFullNames: new List<string> { "Orange" },
            OnDropped: collider => {
                AInputMain.SetAction(_dragCyanToEmerald);
            });
        _dragCyanToEmerald = DragAndDropAction.FullNames(correctTakeFullNames: new List<string> { "Cyan" },
            uncorrectDragCollidableFullNames: new List<string> { "Black" },
            correctDropFullNames: new List<string> { "Emerald" },
            OnDropped: collider => {
                AInputMain.SetAction(null);
            });
    }
}
