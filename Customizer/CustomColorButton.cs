using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CustomColorButton : MyMonoBehaviour
{
    #region Attributes
    public Color currentColor;
	
	//Toggle
	internal Toggle buttonColor; 

	//BodyParts
	internal BodyParts customBodyPart;

	//Color
	protected Image colorImage;
	private string[] bodyPartsColors;
	private int buttonIndex;
	private ColorPicker colorPicker;

	private int colorCount = 2;
	private bool colorButtonEventAdded;

	private Action<Color> updateButtonColor;

    #endregion

    #region Unity Callbacks
    private void Start()
    {
        updateButtonColor = (col) =>
			{
				if (uiCustomizer.currentSubSection != BodyParts.Makeup)
					UpdateColorCloth(col);
			};
    }

    #endregion

    #region Methods
    internal virtual void Initialize(ColorPicker p_colorPicker, int colorIndex)
	{
		this.buttonIndex = colorIndex;
		
		colorImage = GetComponent<Image>();
		buttonColor = GetComponent<Toggle>();
		colorPicker = p_colorPicker;
		buttonColor.onValueChanged.AddListener((bool value) => ColorSelected(value));
		ClothControllerToggle.OnPartChange += RefreshColorCloth;
	}
    
	internal virtual void UpdateColorCloth(Color p_currentColor )
    {
		currentColor = p_currentColor;		
		colorImage.color = currentColor;

		if (buttonIndex >= colorCount || buttonIndex >= bodyPartsColors.Length || uiCustomizer.currentSubSection == BodyParts.Makeup)
			return;

        string colorName = bodyPartsColors[buttonIndex];
        foreach (Material sharedMat in player.playerCustomizer.skinBodyPartsDic[customBodyPart].sharedMaterials)
            sharedMat.SetColor(colorName, currentColor);

    }

	private void RefreshColorCloth(BodyParts bodyPart, Mesh meshCloth) 
	{
		customBodyPart = bodyPart;
		bodyPartsColors = (string[]) Enum.GetNames(typeof(UniversalMaterialNames)).Where(x => x.Contains(customBodyPart.ToString())).ToArray();

		colorCount = uiCustomizer.GetColorCountCloth(meshCloth.name);
		//print("Change! Color count:" + colorCount);
			//Desactivate button if no color for it
		if (buttonIndex >= colorCount || buttonIndex >= bodyPartsColors.Length) 
		{
			gameObject.SetActive(false);
			return;
		}

		string colorName = bodyPartsColors[buttonIndex];
		if(Shader.Find(player.playerCustomizer.skinBodyPartsDic[customBodyPart].sharedMaterial.shader.name).FindPropertyIndex(colorName) != -1)
			currentColor = player.playerCustomizer.skinBodyPartsDic[ customBodyPart ].sharedMaterial.GetColor(colorName);
		
		Refresh(buttonIndex);		
	}

	internal void Refresh (int index, Color? color = null)
    {		
		//Activate Color Button
		gameObject.SetActive(true);

		//Selected Elemnnt first always
		if (index == 0)
			buttonColor.isOn = true;
		currentColor = color ?? currentColor;
		//colorPicker.color = currentColor; //TODO:Da error raro, pero hay que hacerlo
		UpdateColorCloth(currentColor);
	}

	//TODO: Refactor y revisar!!!
    internal void ColorSelected(bool selected)
    {
        if (selected)
        {
			colorPicker.transform.parent.gameObject.SetActive(true);
			colorPicker.color = currentColor;
			if(colorPicker.texturesTgl.gameObject.activeSelf)
				inGame.customizer.texturePicker.ShowTexturesPanel(false);

			if (!colorButtonEventAdded)
			{
				colorPicker.OnColorChanged += updateButtonColor;
                colorButtonEventAdded = true;
			}
		}
        else
        {
			colorPicker.transform.parent.gameObject.SetActive(false);

			if (colorButtonEventAdded)
			{
				colorPicker.OnColorChanged -= updateButtonColor;
				colorButtonEventAdded = false;
            }			
		}
    }

	#endregion
}