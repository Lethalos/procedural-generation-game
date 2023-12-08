using UnityEngine;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour
{
    private Vector3 initialScale;
    private RectTransform rectTransform;
    private Tween scaleTween;

    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float scaleMultiplier = 0.9f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialScale = rectTransform.localScale;
        StartScaleAnimation();
    }

  

    public void StartScaleAnimation()
    {
        if((GameManager.Instance.currentGameState == GameState.Idle)){
            rectTransform.localScale = initialScale;

            scaleTween = rectTransform.DOScale(initialScale * scaleMultiplier, animationDuration / 1f)
                .SetEase(Ease.OutSine)
                .OnComplete(ReverseScaleAnimation);
        }
        else
        {
            scaleTween.Kill();
        }
        
    }

    private void ReverseScaleAnimation()
    {
        if ((GameManager.Instance.currentGameState == GameState.Idle))
        {
            scaleTween = rectTransform.DOScale(initialScale, animationDuration / 1f)
            .SetEase(Ease.InSine)
            .OnComplete(StartScaleAnimation);
        }
        else
        {
            {
                scaleTween.Kill();
            }
        }

    }

    public void StopAnimation()
    {
        if (scaleTween != null && scaleTween.IsPlaying())
        {
            scaleTween.Kill(true);
        }
       
    }

    private void OnDestroy()
    {
        StopAnimation();
    }


}
