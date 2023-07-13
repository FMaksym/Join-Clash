using UnityEngine;
using DG.Tweening;

public class SpikeMove : MonoBehaviour
{
    private void Start()
    {
        transform.DOMoveY(-0.2f,(Random.Range(1f, 1.3f))).SetLoops(10000, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
