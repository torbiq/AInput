using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using AInput;

public class TestAInput : MonoBehaviour {
    private DragAndDropAction _dragRedToYellow;

    [SerializeField]
    private List<Collider2D> _grayColliders = new List<Collider2D>();
    [SerializeField]
    private List<Collider2D> _blackColliders = new List<Collider2D>();

    private void RandomScaling(Transform transform, float minScale, float maxScale, float minDuration, float maxDuration) {
        transform.DOScale(Random.Range(minScale, maxScale), Random.Range(minDuration, maxDuration)).SetEase(Ease.Linear).SetLoops(1, LoopType.Yoyo).OnComplete(() => {
            RandomScaling(transform, minScale, maxScale, minDuration, maxDuration);
        });
    }

    private void Awake() {
        foreach (var blackCollider in _blackColliders) {
            RandomScaling(blackCollider.transform, 0.5f, 3f, 1.5f, 3f);
        }
        _dragRedToYellow = new DragAndDropAction(
            takableInfo: new FullNamesInfo(new List<string> { "Red", "Purple", }, new List<string> { "Yellow", "Purple", }),
            dragCollidableInfo: new CollidersInfo(_grayColliders, _blackColliders),
            droppableInfo: new NamePartInfo("Yellow", "UncorrectDroppableColor"),
            OnTaken: collider => {
                Log.Msg("You took correct collider");
            },
            OnTakeFailed: collider => {
                Log.Msg("Uncorrect collider to take");
            },
            OnDragCollided: collider => {
                Log.Msg("While drag you collided with correct collider");
            },
            OnDragCollideFailed: collider => {
                Log.Msg("While drag you collided with uncorrect collider");
            },
            OnDropped: collider => {
                Log.Msg("Dropped to correct collider");
            },
            OnDropFailed: collider => {
                Log.Msg("Dropped to uncorrect collider");
            });
        AInputMain.SetAction(_dragRedToYellow);
    }
}
