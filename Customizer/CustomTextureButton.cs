using UnityEngine;
using UnityEngine.UI;

public class CustomTextureButton : MyMonoBehaviour
{
    #region Attributes
    public Texture2D currentTexture;
    public Image textureImage;
	
	//Toggle
	internal Toggle textureTgl;

    //BodyParts
    internal BodyParts customBodyPart;

	internal int id;
	private TexturePicker texturePicker;

    #endregion

    #region Unity Callbacks
    private void Awake()
    {
		Initialize();
    }

    #endregion

    #region Methods
    internal virtual void Initialize()
	{
		SetId();
		texturePicker = uiCustomizer.texturePicker;
		textureImage.sprite = Sprite.Create(currentTexture, new Rect(0.0f, 0.0f, currentTexture.width, currentTexture.height), new Vector2(0.5f, 0.5f), 100f);
		texturePicker.textureTgl.image.sprite = textureImage.sprite;
		textureTgl = GetComponent<Toggle>();
        textureTgl.onValueChanged.AddListener((bool value) => TextureSelected(value));
	}
    
	internal virtual void UpdateTextureCloth()
    {
		string shaderTextureParameter = "Branding_Top";
		customBodyPart = uiCustomizer.currentSubSection;
		if (customBodyPart == BodyParts.Pants)
			shaderTextureParameter = "Branding_Bottom";

		foreach (Material sharedMat in player.playerCustomizer.skinBodyPartsDic[customBodyPart].sharedMaterials)		
			sharedMat.SetTexture(shaderTextureParameter, currentTexture);		
		
		if(customBodyPart == BodyParts.Pants)
			texturePicker.bottomTextureId = id;
		else
			texturePicker.topTextureId = id;
	}	

    internal void TextureSelected(bool selected)
    {
        if (selected)
		{
			UpdateTextureCloth();
            texturePicker.textureTgl.image.sprite = textureImage.sprite;
        }
    }

	private void SetId()
	{
		int index = 0;

		foreach(Transform textureObj in transform.parent)
		{
			if(textureObj == transform)			
				id = index;
			
			index++;
		}
	}

	#endregion
}