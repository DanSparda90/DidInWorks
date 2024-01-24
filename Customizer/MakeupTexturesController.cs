using System;
using UnityEngine;

public class MakeupTexturesController : MyMonoBehaviour
{
    #region Attributes
    [SerializeField] private CustomableTexturePicker cheecksTexturePicker;
    [SerializeField] private CustomableTexturePicker eyelinerTexturePicker;
    [SerializeField] private CustomableTexturePicker lipsTexturePicker;
    [SerializeField] private CustomableTexturePicker frecklesTexturePicker;

    internal Color currentColor;
    internal int idCurrentSelected;
    internal MakeupParts currentSelectedMakeupType;
    internal bool isMakeupVisible = true;

    private ColorPicker colorPicker;
    private string[] colorShaderNames = new string[] { "Cheeks_Color", "Eyeliner_Color", "Lips_Color"};

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        colorPicker = uiCustomizer.colorPicker;
        colorPicker.OnColorChanged += (value) => {
            if(uiCustomizer.currentSubSection == BodyParts.Makeup)
                UpdateMakeupColor(value);
        };
        uiCustomizer.ShowColorsPanel(false);
    }

    private void OnEnable()
    {
        //uiCustomizer.ShowColorsPanel(true);
        colorPicker.transform.parent.gameObject.SetActive(false);
        colorPicker.colorButtons[1].gameObject.SetActive(false);
        colorPicker.colorButtons[2].gameObject.SetActive(false);
        colorPicker.texturesTgl.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        colorPicker.OnColorChanged -= (value) => {
            if (uiCustomizer.currentSubSection == BodyParts.Makeup)
                UpdateMakeupColor(value);
        };
    }

    private void Update()
    {
        if (cheecksTexturePicker.idTextureSelected == 0 && frecklesTexturePicker.idTextureSelected == 0 && eyelinerTexturePicker.idTextureSelected == 0 && lipsTexturePicker.idTextureSelected == 0)
        {
            if (isMakeupVisible)
                ChangeMakeupVisibilty(false);
        }
        else
        {
            if (!isMakeupVisible)
                ChangeMakeupVisibilty(true);
        }
    }

    #endregion

    #region Methods
    internal void ChangeMakeupVisibilty(bool isVisible, PlayerCustomizer playerCusto = null)
    {
        PlayerCustomizer playerC;
        if (playerCusto == null)
            playerC = player.playerCustomizer;
        else
            playerC = playerCusto;

        playerC.makeupSkin.gameObject.SetActive(isVisible);
        isMakeupVisible = isVisible;
    }

    private void UpdateMakeupColor(Color p_currentColor, PlayerCustomizer playerCusto = null)
    {
        string colorName = colorShaderNames[(int)currentSelectedMakeupType];
        PlayerCustomizer playerC;

        if (playerCusto == null)
            playerC = player.playerCustomizer;
        else
            playerC = playerCusto;

        currentColor = p_currentColor;


        foreach (Material sharedMat in playerC.makeupSkin.sharedMaterials)
            sharedMat.SetColor(colorName, currentColor);

        if (playerCusto == null)
        {
            if (currentSelectedMakeupType == MakeupParts.Eyeliner)
                eyelinerTexturePicker.currentColor = currentColor;
            if (currentSelectedMakeupType == MakeupParts.Lips)
                lipsTexturePicker.currentColor = currentColor;
        }

        if(uiCustomizer.inCustomZone)
            uiCustomizer.colorPicker.colorButtons[0].Refresh(0, currentColor);
    }

    internal Color GetCurrentTextureColor()
    {
        Color selectedTextureColor = Color.white;
        string colorName = colorShaderNames[(int)currentSelectedMakeupType];

        selectedTextureColor = player.playerCustomizer.makeupSkin.sharedMaterials[0].GetColor(colorName);

        return selectedTextureColor;
    }

    internal int GetIdSelectedTextureByType(MakeupParts makeupPart)
    {
        int id = 0;

        //if (makeupPart == MakeupParts.Cheeks)
        //    id = cheecksTexturePicker.idTextureSelected;
        if (makeupPart == MakeupParts.Eyeliner)
            id = eyelinerTexturePicker.idTextureSelected;
        if (makeupPart == MakeupParts.Lips)
            id = lipsTexturePicker.idTextureSelected;
        if (makeupPart == MakeupParts.Freckles)
            id = frecklesTexturePicker.idTextureSelected;

        return id;
    }

    internal float GetOpacityByType(MakeupParts makeupPart)
    {
        float opacity = 1;

        //if (makeupPart == MakeupParts.Cheeks)
        //    opacity = cheecksTexturePicker.opacitySlider.value;
        if (makeupPart == MakeupParts.Eyeliner)
            opacity = eyelinerTexturePicker.opacitySlider.value;
        if (makeupPart == MakeupParts.Lips)
            opacity = lipsTexturePicker.opacitySlider.value;
        if (makeupPart == MakeupParts.Freckles)
            opacity = frecklesTexturePicker.opacitySlider.value;

        return Mathf.Round(opacity * 100f) / 100f;
    }

    internal void SetOpacityByType(float opacity, MakeupParts makeupPart)
    {
        //if (makeupPart == MakeupParts.Cheeks)
        //    cheecksTexturePicker.opacitySlider.value = opacity;
        if (makeupPart == MakeupParts.Eyeliner)
            eyelinerTexturePicker.opacitySlider.value = opacity;
        if (makeupPart == MakeupParts.Lips)
            lipsTexturePicker.opacitySlider.value = opacity;
        if (makeupPart == MakeupParts.Freckles)
            frecklesTexturePicker.opacitySlider.value = opacity;
    }

    internal void SelectTextureById(int id, MakeupParts makeupPart, PlayerCustomizer playerCusto = null)
    {
        //if(makeupPart == MakeupParts.Cheeks)
        //    cheecksTexturePicker.ForceUpdateTextureMakeup(id, makeupPart, playerCusto);
        if (makeupPart == MakeupParts.Eyeliner)
            eyelinerTexturePicker.ForceUpdateTextureMakeup(id, makeupPart, playerCusto);
        if (makeupPart == MakeupParts.Lips)
            lipsTexturePicker.ForceUpdateTextureMakeup(id, makeupPart, playerCusto);
        if (makeupPart == MakeupParts.Freckles)
            frecklesTexturePicker.ForceUpdateTextureMakeup(id, makeupPart, playerCusto);
    }

    internal void ForceSetTextureOpacity(float opacity, MakeupParts makeupPart, PlayerCustomizer playerCusto = null)
    {
        PlayerCustomizer playerC;
        string shaderOpacityParameter = "";

        if (playerCusto == null)
            playerC = player.playerCustomizer;
        else
            playerC = playerCusto;

        //if (makeupPart == MakeupParts.Cheeks)
        //    shaderOpacityParameter = cheecksTexturePicker.shaderOpacityParameter;
        if (makeupPart == MakeupParts.Eyeliner)
            shaderOpacityParameter = eyelinerTexturePicker.shaderOpacityParameter;
        if (makeupPart == MakeupParts.Lips)
            shaderOpacityParameter = lipsTexturePicker.shaderOpacityParameter;
        if (makeupPart == MakeupParts.Freckles)
            shaderOpacityParameter = frecklesTexturePicker.shaderOpacityParameter;

        foreach (Material sharedMat in playerC.makeupSkin.sharedMaterials)
            sharedMat.SetFloat(shaderOpacityParameter, opacity);
    }

    internal Color GetCurrentColorByType(MakeupParts makeupPart)
    {
        Color color = Color.white;

        //if (makeupPart == MakeupParts.Cheeks)
        //    color = cheecksTexturePicker.currentColor;
        if (makeupPart == MakeupParts.Eyeliner)
            color = eyelinerTexturePicker.currentColor;
        if (makeupPart == MakeupParts.Lips)
            color = lipsTexturePicker.currentColor;

        return color;
    }

    internal void SetCurrentColorByType(Color color, MakeupParts makeupPart, PlayerCustomizer playerCusto = null)
    {
        currentSelectedMakeupType = makeupPart;
        UpdateMakeupColor(color, playerCusto);
    }
    #endregion
}