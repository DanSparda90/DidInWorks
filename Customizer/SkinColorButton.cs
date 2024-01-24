using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinColorButton : MyMonoBehaviour
{
    private Color currentColor;
    private Image colorImage;

    event Action<Color> OnSkinColorChange;

    internal void Initialize(SkinColorPicker p_colorPicker)
    {
        colorImage = GetComponent<Image>();

        OnSkinColorChange += RefreshColorImage;
    }

    internal  void UpdateColor(Color p_color)
    {

        OnSkinColorChange(p_color);

        foreach (KeyValuePair<BodyParts, SkinnedMeshRenderer> bodySkinPart in player.playerCustomizer.skinBodyPartsDic)
        {
            bodySkinPart.Value.sharedMaterials[0].color = currentColor;
        }
    }

    void RefreshColorImage(Color p_color)
    {
        currentColor = p_color;
        colorImage.color = currentColor;
    }

}
