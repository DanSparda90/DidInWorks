using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Show a tooltip with the tooltipText setted and make an enlarge effect on hover and exit on the object transform.
/// </summary>
public class GeneralTooltip : MyMonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    #region Attributes
    [Header("Texts")]
    [SerializeField] private bool useDynamicText;
    private Dictionary<bool, string> statesStringDic;
    [Tooltip("Main tooltip text")][TextArea]
    [SerializeField] internal string onTooltipText;
    [Tooltip("Off text, only when check use Dynamic Text")][TextArea]
    [SerializeField] internal string offTooltipText; //Only used with dynamicText

    [Header("Tooltip Parameters")]
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private bool showTip;
    [SerializeField] private float tooltipSize = 0.5f;
    [SerializeField] private float tooltipUpOffset = 5f;
    [SerializeField] private float tooltipRightOffset = 0f;
    [SerializeField] private float tooltipTextSize = 15f;
    [SerializeField] private TextAlignmentOptions tooltipTextAlignment = TextAlignmentOptions.Center;
    [SerializeField] private bool withHoverEffect;
    [SerializeField] private float hoverSize = 1.1f;
    [SerializeField] private float normalSize = 1;

    #endregion

    #region Unity Callbacks
    private void Start() 
    {
        if(useDynamicText)
        {
            statesStringDic = new Dictionary<bool, string>()
            {
                {true, onTooltipText },
                {false, offTooltipText }
            };
        }
    }

    #endregion

    #region Interfaces Implementations
    public void OnPointerEnter(PointerEventData eventData) 
    {
        if(isEnabled)
        {
            if(withHoverEffect)
                transform.DOScale(hoverSize, 0.3f).SetEase(Ease.OutSine);

            uiGame.tooltipBasicPanel.transform.localScale = new Vector3(tooltipSize, tooltipSize, tooltipSize);
            uiGame.tooltipBasicPanel.uiText.fontSize = tooltipTextSize;
            uiGame.tooltipBasicPanel.uiText.alignment = tooltipTextAlignment;
            uiGame.tooltipBasicPanel.showTooltipTip = showTip;
            uiGame.tooltipBasicPanel.upOffset = tooltipUpOffset;
            uiGame.tooltipBasicPanel.rightOffset = tooltipRightOffset;

            if(useDynamicText)
            {    
                SwitchManager switchM = GetComponent<SwitchManager>();
                if(switchM)
                    uiGame.tooltipBasicPanel.ShowTooltip(statesStringDic[switchM.isOn], gameObject);
                else
                    uiGame.tooltipBasicPanel.ShowTooltip(onTooltipText, gameObject);
            }
            else
            {
                uiGame.tooltipBasicPanel.ShowTooltip(onTooltipText, gameObject);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isEnabled)
        {
            if(withHoverEffect)
                DoDeselectEffect();

            uiGame.tooltipBasicPanel.HideTooltip();
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if(useDynamicText)
        {
            SwitchManager switchM = GetComponent<SwitchManager>();
            if (switchM)
                uiGame.tooltipBasicPanel.uiText.text = statesStringDic[switchM.isOn];
        }
    }

    #endregion

    #region Methods
    /// <summary>
    /// Pause the current tween animation and do a deselect effect
    /// </summary>
    private void DoDeselectEffect()
    {
        transform.DOKill();
        transform.DOScale(normalSize, 0.15f).SetEase(Ease.InSine);
    }

    #endregion
}