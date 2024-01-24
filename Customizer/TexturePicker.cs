using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TexturePicker : MyMonoBehaviour
{
    #region Attributes
    [SerializeField] internal GameObject texturesPanel;
    [SerializeField] internal Slider topTextureTillingSlider;
    [SerializeField] internal Slider bottomTextureTillingSlider;
    [SerializeField] internal Toggle textureTgl;

    internal BodyParts[] partsWithTextures;
    public int topTextureId;
    public int bottomTextureId;

    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        partsWithTextures = new BodyParts[] { BodyParts.Shirt, BodyParts.Pants };

        textureTgl.onValueChanged.AddListener((value) => ShowTexturesPanel(value));
        topTextureTillingSlider.onValueChanged.AddListener((value) => ChangeTextureTilling(value));
        bottomTextureTillingSlider.onValueChanged.AddListener((value) => ChangeTextureTilling(value));
    }

    private void OnDestroy()
    {
        topTextureTillingSlider.onValueChanged.RemoveAllListeners();
        bottomTextureTillingSlider.onValueChanged.RemoveAllListeners();
    }

    #endregion

    #region Methods
    internal void ChangeTextureTilling(float value, PlayerCustomizer playerCusto = null, string type = null)
    {
        string tillingParameter = "Tiling_Top";

        if (uiCustomizer.currentSubSection == BodyParts.Pants)
            tillingParameter = "Tiling_Bottom";
        
        if(playerCusto != null)
        {
            //Outside customizator
            if (type == "top")
            {
                foreach (Material sharedMat in playerCusto.skinBodyPartsDic[BodyParts.Shirt].sharedMaterials)
                    sharedMat.SetFloat("Tiling_Top", value);
            }

            if(type == "bottom")
            {
                foreach (Material sharedMat in playerCusto.skinBodyPartsDic[BodyParts.Pants].sharedMaterials)
                    sharedMat.SetFloat("Tiling_Bottom", value);
            }
        }
        else
        {
            //On the customizator
            foreach (Material sharedMat in player.playerCustomizer.skinBodyPartsDic[uiCustomizer.currentSubSection].sharedMaterials)
                sharedMat.SetFloat(tillingParameter, value);
        }
    }

    /// <summary>
	/// Show/hide the textures for the current cloth according to the bool putted as a parameter.
	/// </summary>
	internal void ShowTexturesPanel(bool isVisible)
    {
        ColorPicker colorPicker = uiCustomizer.colorPicker;
        CanvasGroup colorPickerCanvasGrp = colorPicker.GetComponent<CanvasGroup>();
        CanvasGroup texturePanelCanvasGrp = texturesPanel.GetComponent<CanvasGroup>();
        CustomTextureButton[] textureButtons = GetComponentsInChildren<CustomTextureButton>(true);

        colorPickerCanvasGrp.DOKill(); 
        texturePanelCanvasGrp.DOKill();
        texturePanelCanvasGrp.DOKill();

        if (isVisible)
        {
            colorPickerCanvasGrp.DOFade(0, 0.5f).OnComplete(() => colorPicker.transform.parent.gameObject.SetActive(false));
            texturePanelCanvasGrp.alpha = 0;
            texturesPanel.SetActive(true);
            textureTgl.gameObject.SetActive(true);
            texturePanelCanvasGrp.DOFade(1, 0.5f);
            if (uiCustomizer.currentSubSection == BodyParts.Shirt)
                textureButtons[topTextureId].GetComponent<Toggle>().isOn = true;
            else
                textureButtons[bottomTextureId].GetComponent<Toggle>().isOn = true;

            textureTgl.isOn = true;
            ShowTillingSlider();
        }
        else
        {
            colorPickerCanvasGrp.DOFade(0, 0.5f).OnComplete(() => colorPicker.transform.parent.gameObject.SetActive(false));
            texturePanelCanvasGrp.alpha = 0;
            texturePanelCanvasGrp.DOFade(0, 0.5f).OnComplete(() => texturesPanel.SetActive(false));
            if (!partsWithTextures.Contains(uiCustomizer.currentSubSection))
                colorPicker.texturesTgl.gameObject.SetActive(false);
        }

        colorPicker.transform.parent.gameObject.SetActive(!isVisible);
    }

    internal virtual void ForceUpdateTextureCloth(int id, BodyParts bodyPartToUpdate, PlayerCustomizer customTargetPlayer = null)
    {
        CustomTextureButton[] textureButtons = GetComponentsInChildren<CustomTextureButton>(true);
        PlayerCustomizer playerCust = player.playerCustomizer;

        string shaderTextureParameter = "Branding_Top";
        
        if (bodyPartToUpdate == BodyParts.Pants)
            shaderTextureParameter = "Branding_Bottom";

        if (customTargetPlayer != null)
            playerCust = customTargetPlayer;
       
         foreach (Material sharedMat in playerCust.skinBodyPartsDic[bodyPartToUpdate].sharedMaterials)
            sharedMat.SetTexture(shaderTextureParameter, textureButtons[id].currentTexture);

         if(customTargetPlayer == null)
        {
            if (bodyPartToUpdate == BodyParts.Pants)
                bottomTextureId = id;
            else
                topTextureId = id;
        }
    }

    private void ShowTillingSlider()
    {
        if(uiCustomizer.currentSubSection == BodyParts.Pants)
        {
            bottomTextureTillingSlider.gameObject.SetActive(true);
            topTextureTillingSlider.gameObject.SetActive(false);
        }
        if (uiCustomizer.currentSubSection == BodyParts.Shirt)
        {
            topTextureTillingSlider.gameObject.SetActive(true);
            bottomTextureTillingSlider.gameObject.SetActive(false);
        }
        
    }

    #endregion
}