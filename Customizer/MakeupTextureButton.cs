using UnityEngine;
using UnityEngine.UI;

public class MakeupTextureButton : MyMonoBehaviour
{
	#region Attributes
	[Header("Parameters")]
	public CustomableTexturePicker texturePicker;
    public Texture2D currentTexture;
    public Image textureImage;
	public bool isTextureTheThumbnail = true;
	
    internal Toggle makeupTgl;
    internal int id;

	private MakeupTexturesController makeupCtrl;

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
		makeupCtrl = texturePicker.GetComponentInParent<MakeupTexturesController>(true);
		if(isTextureTheThumbnail)
			textureImage.sprite = Sprite.Create(currentTexture, new Rect(0.0f, 0.0f, currentTexture.width, currentTexture.height), new Vector2(0.5f, 0.5f), 100f);
        makeupTgl = GetComponent<Toggle>();
        makeupTgl.onValueChanged.AddListener((bool value) => MakeupSelected(value));
    }
    
	internal virtual void UpdateMakeupTexture()
    {
		string shaderTextureParameter = texturePicker.shaderTextureParameter;

		if(id == 0)
		{
            foreach (Material sharedMat in player.playerCustomizer.makeupSkin.sharedMaterials)
                sharedMat.SetTexture(shaderTextureParameter, texturePicker.transparentTexture);
        }
		else
		{
			foreach (Material sharedMat in player.playerCustomizer.makeupSkin.sharedMaterials)		
				sharedMat.SetTexture(shaderTextureParameter, currentTexture);
		}


		texturePicker.idTextureSelected = id;
	}	

    internal void MakeupSelected(bool selected)
    {
		if (selected)
		{
			UpdateMakeupTexture();
			makeupCtrl.idCurrentSelected = id;
			makeupCtrl.currentSelectedMakeupType = texturePicker.makeupPart;
			if (id != 0 && texturePicker.makeupPart != MakeupParts.Freckles)
			{
                uiCustomizer.ShowColorsPanel(true);
                uiCustomizer.colorPicker.color = makeupCtrl.GetCurrentTextureColor();
				
				uiCustomizer.colorPicker.colorButtons[0].Refresh(0, uiCustomizer.colorPicker.color);

			}
			else
			{
				uiCustomizer.ShowColorsPanel(false);
			}
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