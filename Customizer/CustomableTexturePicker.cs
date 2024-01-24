using System;
using UnityEngine;
using UnityEngine.UI;

public class CustomableTexturePicker : MyMonoBehaviour
{
    #region Attributes
    [SerializeField] internal MakeupParts makeupPart;
    [SerializeField] internal Slider opacitySlider;
    [SerializeField] internal string shaderTextureParameter;
    [SerializeField] internal string shaderOpacityParameter;
    [SerializeField] internal Texture2D transparentTexture;
    
    internal int idTextureSelected;
    internal Color currentColor;

    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        opacitySlider.onValueChanged.AddListener((value) => ChangeTextureOpacity(value));
    }

    #endregion

    #region Methods
    internal void ChangeTextureOpacity(float value)
    {
        foreach (Material sharedMat in player.playerCustomizer.makeupSkin.sharedMaterials)
            sharedMat.SetFloat(shaderOpacityParameter, value);
    }

    internal virtual void ForceUpdateTextureMakeup(int id, MakeupParts makeupPartToUpdate, PlayerCustomizer playerCusto = null)
    {
        MakeupTextureButton[] textureButtons = GetComponentsInChildren<MakeupTextureButton>(true);
        PlayerCustomizer playerCust;

        if (playerCusto == null)
            playerCust = player.playerCustomizer;
        else
            playerCust = playerCusto;
       
        foreach (Material sharedMat in playerCust.makeupSkin.sharedMaterials)
        {
            if(id == 0)
                sharedMat.SetTexture(shaderTextureParameter, transparentTexture);
            else
                sharedMat.SetTexture(shaderTextureParameter, textureButtons[id].currentTexture);
        }

        if(playerCusto == null) { 
            textureButtons[id].GetComponent<Toggle>().isOn = true;
            idTextureSelected = id;
        }
    }
    #endregion
}