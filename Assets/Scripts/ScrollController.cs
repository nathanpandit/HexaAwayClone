using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    
    void Awake()
    {
        if (scrollRect == null)
        {
            scrollRect = GetComponent<ScrollRect>();
        }
    }

    public void ScrollByAmount(float amount)
    {
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(
            scrollRect.verticalNormalizedPosition + amount
        );
    }
    
    public void ScrollByAmountAnimated(float amount, float duration = 0.5f)
    {
        float targetPos = Mathf.Clamp01(scrollRect.verticalNormalizedPosition + amount);
        scrollRect.DOVerticalNormalizedPos(targetPos, duration);
    }

    void OnEnable()
    {
        if(GameManager.scrollFlag) ScrollByAmountAnimated(0.25f);
        GameManager.scrollFlag = false;
    }
}

