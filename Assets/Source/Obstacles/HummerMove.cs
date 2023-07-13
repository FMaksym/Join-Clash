using UnityEngine;
using DG.Tweening;


public class HummerMove : MonoBehaviour
{
    [SerializeField] private Vector3 _rotatePosition;

    private void Start()
    {
        transform.DORotate(_rotatePosition, 1).SetLoops(1000, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
