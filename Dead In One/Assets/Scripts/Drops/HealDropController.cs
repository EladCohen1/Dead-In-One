using DG.Tweening;
using UnityEngine;

public class HealDropController : MonoBehaviour
{
    public int HpHeld;

    void Start()
    {
        transform.DOMoveY(1f, 0.7f) // move to y = 1 over 1 second
                 .From(0.3f)      // start from y = 0.5
                 .SetLoops(-1, LoopType.Yoyo) // -1 = infinite, Yoyo = back and forth
                 .SetEase(Ease.InOutSine);   // smooth easing
    }
}
