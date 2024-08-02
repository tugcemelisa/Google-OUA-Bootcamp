using DG.Tweening;
using UnityEngine;

public class WoolScaling : MonoBehaviour
{
    [SerializeField] float duration = 5f;

    private void OnEnable()
    {
        Shake();
    }

    void Shake()
    {
        transform.DOShakeScale(duration, 1, 10, 90, true);
    }
}
