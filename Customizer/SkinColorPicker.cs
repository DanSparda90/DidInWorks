using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SkinColorPicker : MyMonoBehaviour {
	
	public bool isDraggable = false;
	public Color pickedColor = Color.white;
	public GameObject pickerObj;
	[Range(0, 255)]
	public int alphaTolerancy = 0;

	public Image previewImage;

	Color32 colorInitSkin = new Color32(248, 182, 123, 255);

	void Start()
	{
		pickedColor = colorInitSkin;

		Color actColor = GetComponent<Image> ().sprite.texture.GetPixel (Mathf.RoundToInt ((pickerObj.transform.position.x - transform.position.x)*(1/GetComponent<RectTransform>().localScale.x)*(1/GetComponentInParent<Canvas>().scaleFactor)), Mathf.RoundToInt ((pickerObj.transform.position.y - transform.position.y)*(1/GetComponent<RectTransform>().localScale.y)*(1/GetComponentInParent<Canvas>().scaleFactor)) + GetComponent<Image> ().sprite.texture.height);
		if (actColor.a >= ((255 - alphaTolerancy) / 255f)) {
			pickedColor = actColor;
		}
	}

    private void Update()
    {
		previewImage.color = pickedColor;
	}

	//Called by Color Picker Event
	public void OnEnter()
	{
		isDraggable = true;
		pickerObj.gameObject.SetActive(true);
	}

	//Called by Color Picker Event
	public void OnExit()
	{
			pickerObj.gameObject.SetActive(false);
			isDraggable = false;
	}

	//Called by Color Picker Event
	public void OnDrag()
	{ 
		if (isDraggable) {
			Color actColor = GetComponent<Image> ().sprite.texture.GetPixel (Mathf.RoundToInt ((Input.mousePosition.x - transform.position.x) * (1 / GetComponent<RectTransform> ().localScale.x) * (1 / GetComponentInParent<Canvas> ().scaleFactor)), Mathf.RoundToInt ((Input.mousePosition.y - transform.position.y) * (1 / GetComponent<RectTransform> ().localScale.y) * (1 / GetComponentInParent<Canvas> ().scaleFactor)) + GetComponent<Image> ().sprite.texture.height);
			if (actColor.a >= ((255 - alphaTolerancy) / 255f)) {
				pickedColor = actColor;
				pickerObj.transform.position = Input.mousePosition;
				CursorPickSkin();
			}
		}
	}

	public void SetColor(Color p_color)
    {
		pickedColor = p_color;
		CursorPickSkin();
	}

	//Called by Color Picker Event
	public void CursorPickSkin()
	{
		foreach (KeyValuePair<BodyParts, SkinnedMeshRenderer> bodySkinPart in player.playerCustomizer.skinBodyPartsDic)
		{
			bodySkinPart.Value.material.SetColor("Skin", pickedColor);
		}
		player.playerCustomizer.bodySkin.material.SetColor("Skin", pickedColor);
		player.playerCustomizer.headSkin.material.SetColor("Skin", pickedColor);
	}

}
