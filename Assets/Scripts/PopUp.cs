using DG.Tweening;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] private float displayDuration = 0.1f;
    [SerializeField] private float targetScaleX = 1f;
    [SerializeField] private float targetScaleY = 1f;

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(targetScaleX, targetScaleY, 1f), displayDuration).SetEase(Ease.InSine);
    }
}