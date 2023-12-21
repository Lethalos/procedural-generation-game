
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;
using System;

public class UIManager : SingletonBehaviour<UIManager>
{
     public RectTransform idleObjectParent;
    public float animDuration = 2f;
    

    [Header("Info Button")]
        public RectTransform infoGrayBack;
        public RectTransform infoGrayCircle;
        public TextMeshProUGUI infoText;
        public RectTransform infoPanel;
        public RectTransform infoPage;
     
        bool isInfoPageOpen = false;
        Vector3 infoPanelInitialPos;
        Vector3 infoPageInitialPos;
        Vector3 infoPanelCornerPos;
        Vector3 idleParentinitialPosition;
        Vector2 infoGrayBackInitialSize;
        Quaternion infoPanelRotation;
        Quaternion idlePanelRotation;

       RectTransform infoIdleButtonStartPos;
 

    [Space]
       [Header("Cloud Popups")]
    [Space]
       public RectTransform altocumulus_popup;
       Vector2 altocumulus_initialPosition;
    [Space]
    public RectTransform altostratus_popup;
    Vector2 altostratus_initialPos;
    [Space]
    public RectTransform cirrus_popup;
    Vector2 cirrus_popupInitialPos;
    [Space]
    public RectTransform cirrocumulus_popup;
    Vector2 cirrocumulus_InitialPos;
    [Space]
    public RectTransform cirrostratus_popup;
    Vector2 cirrostratus_initialpos;
    [Space]
    public RectTransform stratus_popup;
    Vector2 stratus_initialpos;
    [Space]
    public RectTransform stratocumulus_popup;
    Vector2 stratocumulus_initialpos;
    [Space]
    public RectTransform cumulus_popup;
    Vector2 cumulus_InitialPos;
    [Space]
    public RectTransform cumulonimbus_popup;
    Vector2 cumulonimbus_initialpos;


    // Start is called before the first frame update
    void Start()
        {
            Init();
            
        }

        void Init()
        {
            
            infoPanelInitialPos = infoPanel.anchoredPosition;
            infoPageInitialPos = infoPage.anchoredPosition;
            infoGrayBackInitialSize = new Vector2(infoGrayBack.sizeDelta.x, infoGrayBack.sizeDelta.y);
            idleParentinitialPosition = idleObjectParent.anchoredPosition;
            altocumulus_initialPosition = altocumulus_popup.anchoredPosition;
            idleObjectParent.gameObject.SetActive(true);
            infoPanel.gameObject.SetActive(true);
            infoPanelRotation = infoPanel.rotation;
            idlePanelRotation = idleObjectParent.rotation;
            HideAllPopUps();
    }
        //public void SetUIelementsToIdle()
        //{
        //    IdleUIInfoPanel();
        //    IdleUIGeneral();
        //}
        //public void SetUIelementsToPlay()
        //{
        //    PlayUIGeneral();
          
        //}
        public Tween PlayUIInfoPanel()
        {
            var infoButtonSequence = DOTween.Sequence();
            infoButtonSequence.Append(infoGrayBack.DOSizeDelta(new Vector2(0, infoGrayBack.sizeDelta.y), 0.5f));
            infoButtonSequence.Join(infoGrayCircle.DOScale(0, 0.5f));
            infoButtonSequence.Play().OnComplete(() =>
            {
                Animation anim = infoPanel.GetComponent<Animation>();
                anim.Play("infoIdle");
            });
        return infoButtonSequence;
        }
        public void IdleUIInfoPanel()
        {
            Animation anim = infoPanel.GetComponent<Animation>();
            var infoButtonSequence = DOTween.Sequence();
            anim.Play("infoIdle_reverse");
            infoButtonSequence.AppendInterval(infoPanel.GetComponent<Animation>().clip.length);
            infoButtonSequence.Append(infoGrayBack.DOSizeDelta(infoGrayBackInitialSize, 0.5f));
            infoButtonSequence.Join(infoGrayCircle.DOScale(1, 0.5f));
            infoPanel.rotation = infoPanelRotation;
            
        infoButtonSequence.Play();
        }
        public void ShowInfoPage()
        {   WeatherManagerOld.Instance.ClearWeather();
            PoolManager.Instance.DeactivateAllPoolObjects();
            infoPage.gameObject.SetActive(true);
            
            isInfoPageOpen = true;
            var infoPageSquence = DOTween.Sequence();
            //info button turn and up
            infoPageSquence.Append(infoPanel.DOAnchorPos(new Vector3(900, -1680, infoPanelInitialPos.z), 0.25f));
            infoPageSquence.Join(infoPanel.DORotate(new Vector3(0, 90, 0), 0.25f));
            //info page turn and scale
            infoPageSquence.Append(infoPage.DOScale(1, 0.25f));
            infoPageSquence.Join(infoPage.DORotate(new Vector3(0, 0, 0), 0.25f));

            infoPageSquence.Play();

        }
        public void HideInfoPage()
        {
            isInfoPageOpen = false;
            var infoPageSequence = DOTween.Sequence();
            //info page turn and scale
            infoPageSequence.Append(infoPage.DOScale(0, 0.25f));
            infoPageSequence.Join(infoPage.DORotate(new Vector3(0, -90, 0), 0.25f));
            //info button turn and scale
            infoPageSequence.Append(infoPanel.DOAnchorPos(new Vector3(920, -1780, infoPanelInitialPos.z), 0.25f));
            infoPageSequence.Join(infoPanel.DORotate(new Vector3(0, 0, 0), 0.25f));
            infoPageSequence.Play().OnComplete(() =>
            {
                //infoPage.gameObject.SetActive(false);
                PoolManager.Instance.ActivateFirstObjects();

            });
            
    }
       

        public void ApplyFadeOutEffectIdle()
        {
            Sequence sequence;
            sequence = DOTween.Sequence();
            
           
            sequence.Append(idleObjectParent.transform.DOMoveX(-1000f, 6f));
            sequence.Join(idleObjectParent.transform.DORotate(new Vector3( 0, 90, 0), 5.5f));
           
           

            sequence.Join(idleObjectParent.GetComponent<CanvasGroup>().DOFade(0, 3f));
           
            sequence.Join(PlayUIInfoPanel());

            sequence.Play().OnComplete(() =>
            {
                // Reset the transform sampleCenter of idleObjectParent
                idleObjectParent.anchoredPosition = idleParentinitialPosition;
                idleObjectParent.transform.rotation = idlePanelRotation;

            });

        }
        public void CloudPopupAnimation(GameObject poolObject)
        {   
            RectTransform cloudPopup = null;
            Vector3 initialPosition = Vector3.zero;
            PoolObjectType poolObjectType = PoolManager.Instance.GetObjectTypeFromGameObject(poolObject);
            if (poolObjectType != PoolObjectType.None)
            {   
                foreach(PoolInfo poolInfo in PoolManager.Instance.listOfPool)
                {   
                    if(poolInfo.type == poolObjectType)
                    {
                    HideAllPopUps();
                    if (poolObjectType == PoolObjectType.Altocumulus)
                    {
                        cloudPopup = altocumulus_popup;
                        initialPosition = altocumulus_initialPosition;
                    }
                    else if (poolObjectType == PoolObjectType.Altostratus)
                    {
                        cloudPopup = altostratus_popup;
                        initialPosition = altostratus_initialPos;
                    }
                    else if (poolObjectType == PoolObjectType.Cirrostratus)
                    {
                        cloudPopup = cirrostratus_popup;
                        initialPosition = cirrostratus_initialpos;
                    }
                    else if (poolObjectType == PoolObjectType.Cirrucomulus)
                    {
                        cloudPopup = cirrocumulus_popup;
                        initialPosition = cirrocumulus_InitialPos;
                    }
                    else if (poolObjectType == PoolObjectType.Cirrus)
                    {
                        cloudPopup = cirrus_popup;
                        initialPosition = cirrocumulus_InitialPos;
                    }
                    else if (poolObjectType == PoolObjectType.Cumulonimbus)
                    {
                        cloudPopup = cumulonimbus_popup;
                        initialPosition = cumulonimbus_initialpos;
                    }
                    else if (poolObjectType == PoolObjectType.Cumulus)
                    {
                        cloudPopup = cumulus_popup;
                        initialPosition = cumulus_InitialPos;
                    }
                    else if (poolObjectType == PoolObjectType.Stratcumulus)
                    {
                        cloudPopup = stratocumulus_popup;
                        initialPosition = stratocumulus_initialpos;
                    }
                    else if (poolObjectType == PoolObjectType.Strutus)
                    {
                        cloudPopup = stratus_popup;
                        initialPosition = stratus_initialpos;
                    }

                    if (cloudPopup != null)
                    {
                        cloudPopup.gameObject.SetActive(true);
                        cloudPopup.localScale = Vector3.zero;
                        Sequence sequence = DOTween.Sequence();
                        sequence.Append(cloudPopup.DOAnchorPos(initialPosition, animDuration).SetEase(Ease.InBounce));
                        sequence.Join(cloudPopup.DOScale(Vector3.one, animDuration));
                        PoolManager.Instance.DeactivateAllPoolObjects();
                        
                    }

                }
            }
            PoolManager.Instance.DeactivateAllPoolObjects();
        }
        }
       
        public void AltostratusPopupAnimation()
        {

        }
        
        public void HideAllPopUps()
        {
         GameObject activePopup = null;

        if (altocumulus_popup.gameObject.activeSelf)
            activePopup = altocumulus_popup.gameObject;
        else if (altostratus_popup.gameObject.activeSelf)
            activePopup = altostratus_popup.gameObject;
        else if (cirrus_popup.gameObject.activeSelf)
            activePopup = cirrus_popup.gameObject;
        else if (cirrocumulus_popup.gameObject.activeSelf)
            activePopup = cirrocumulus_popup.gameObject;
        else if (cirrostratus_popup.gameObject.activeSelf)
            activePopup = cirrostratus_popup.gameObject;
        else if (stratus_popup.gameObject.activeSelf)
            activePopup = stratus_popup.gameObject;
        else if (stratocumulus_popup.gameObject.activeSelf)
            activePopup = stratocumulus_popup.gameObject;
        else if (cumulus_popup.gameObject.activeSelf)
            activePopup = cumulus_popup.gameObject;
        else if (cumulonimbus_popup.gameObject.activeSelf)
            activePopup = cumulonimbus_popup.gameObject;
        if (activePopup != null)
        {
            // Apply the hide animation
            Sequence sequence = DOTween.Sequence();
            sequence.Append(activePopup.transform.DOScale(Vector3.zero, 2f));
            sequence.Join(activePopup.GetComponent<CanvasGroup>().DOFade(0f, 2f).SetEase(Ease.InBounce));
            sequence.OnComplete(() =>
            {   
                // After the animation is complete, deactivate the popup
                activePopup.SetActive(false);
                PoolManager.Instance.ActivateFirstObjects();
            });
            //sequence.PlayBackwards();
        }

    }

        void OnDestroy()
        {
           
        }

        

}

