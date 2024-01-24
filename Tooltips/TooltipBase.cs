using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipBase : MonoBehaviour
{
    #region Attributes
    [SerializeField] internal float upOffset = 5;
    [SerializeField] internal float rightOffset = 0;
    [SerializeField] internal bool showTooltipTip = true;
    [SerializeField] private GameObject tooltipTip;
        
    internal TextMeshProUGUI uiText;
    private Canvas parentCanvas;
    private Image backgroundImage;
    private bool isShowTooltip;
    private GameObject currentHoverGameObject;

    #endregion

    #region Unity Callbacks
    void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        uiText = GetComponentInChildren<TextMeshProUGUI>(true);
        backgroundImage = GetComponent<Image>();
        backgroundImage.enabled = false;
        tooltipTip.SetActive(false);            
    }

    void Update()
    {
        if(isShowTooltip)
        {
            Vector2 movePos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                Input.mousePosition, parentCanvas.worldCamera,
                out movePos);

            Vector3 mousePos = parentCanvas.transform.TransformPoint(movePos);

            //Move the Object/Panel
            transform.position = mousePos + (Vector3.up * upOffset) + (Vector3.right * rightOffset);
            
        }

        if(currentHoverGameObject != null)
        {
            if(!currentHoverGameObject.activeSelf && isShowTooltip)
                HideTooltip();
        }
        
    }

    #endregion

    #region Methods
    public void ShowTooltip(string tooltipText, GameObject tooltipGameObject)
    {
        isShowTooltip = true;
        currentHoverGameObject = tooltipGameObject;

        if(backgroundImage == null)
            backgroundImage = GetComponent<Image>();

        backgroundImage.enabled = true;
        if(showTooltipTip)
            tooltipTip.SetActive(true);

        if (uiText == null)
            uiText = GetComponentInChildren<TextMeshProUGUI>(true);

        uiText.text = tooltipText;
    }

    public void HideTooltip()
    {
        if (isShowTooltip)
        {
            isShowTooltip = false;
            if (backgroundImage == null)
                backgroundImage = GetComponent<Image>();

            backgroundImage.enabled = false;

            if (showTooltipTip)
                tooltipTip.SetActive(false);

            if(uiText == null)
                uiText = GetComponentInChildren<TextMeshProUGUI>(true);

            uiText.text = " ";
        }
    }

    #endregion
}