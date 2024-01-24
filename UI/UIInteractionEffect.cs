using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIInteractionEffect : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    #region Attributes
    [Header("Base Parameters")]
    [SerializeField] internal bool isEnabled = true;
    [SerializeField] private UIEffect effect;
    [SerializeField] private float duration = 0.5f;
    
    [Header("Increase Size Effect")]
    [SerializeField] private float targetSize = 0.9f;

    [Header("Bounce Effect")]
    [SerializeField] private float strength = 0.2f;
    [SerializeField] private float randomness = 69;
    [SerializeField] private int vibrato = 8;

    [Header("Image on select")]
    [SerializeField] private bool showImageOnSelectInput = false;
    [SerializeField] private Image imageToShow;
    [SerializeField] private Color32 colorImageToShow = new Color32(123, 123, 123, 0);

    private Transform element;
    private Vector3 defaultScale;

    #endregion

    #region Unity Callbacks
    private void Awake() 
    {
        element = transform;
        defaultScale = element.localScale;
    }  

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isEnabled)
        {
            switch (effect)
            {
                case UIEffect.IncreaseSize:
                    IncreaseSizeEffect(true);
                    break;
                case UIEffect.Bounce:
                    BounceEffect(true);
                    break;
            }

            if (showImageOnSelectInput)
                ShowImageOnSelect(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData) 
    {
        if (isEnabled)
        {
            switch (effect)
            {
                case UIEffect.IncreaseSize:
                    IncreaseSizeEffect(false);
                    break;
                case UIEffect.Bounce:
                    BounceEffect(false);
                    break;
            }

            if (showImageOnSelectInput)
                ShowImageOnSelect(false);
        }
    }   

    #endregion

    #region Methods
    private void IncreaseSizeEffect(bool isIncrease)
    {
        element.DOKill();
        if(isIncrease)        
            element.DOScale(targetSize, duration);
        else
            element.DOScale(defaultScale.x, duration);        
    }

    private void BounceEffect(bool isBounce)
    {
        element.DOKill();
        if(isBounce)
            element.DOShakeScale(duration, strength, vibrato, randomness, true);
        else
            element.DOScale(defaultScale, duration);
    }

    private void ShowImageOnSelect(bool state)
    {
        imageToShow.color = colorImageToShow;
        imageToShow.DOKill();

        if(state)
        {
            imageToShow.gameObject.SetActive(true);
            imageToShow.DOFade(255, 1);
        }
        else
        {
            imageToShow.DOFade(0, 1).OnComplete( () => imageToShow.gameObject.SetActive(false));
        }
    }

    #endregion
}