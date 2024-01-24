using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomImageColorPickerController : MyMonoBehaviour
{
    #region Attributes
    [Header("Parameters")]
    [SerializeField] private ColorPickerType colorPickerType;
    [SerializeField] private RawImage colorPicker;
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Vector2 hotSpot = Vector2.zero;
    [SerializeField] private GameObject pointerOnButtonUp;
    [SerializeField] private DragRotationCustomizerPlayer avatarDragRotationCtrl;

    internal Color currentColor = new Color32(212,151,121,255);

    private Texture2D tex;
    private Color32 color;
    private Rect rect;
    private Vector2 localPoint;
    private bool isPointerExited;
    private bool isInsideImage;
    private int pX;
    private int pY;

    #endregion

    #region Unity Callbacks
    private void Update() 
    {
        if (!avatarDragRotationCtrl.isDragging)
        {
            if (Input.GetMouseButton(0))
            {
                tex = colorPicker.texture as Texture2D;
                rect = colorPicker.rectTransform.rect;
                pointerOnButtonUp.SetActive(false);

                RectTransformUtility.ScreenPointToLocalPointInRectangle(colorPicker.rectTransform, Input.mousePosition, null, out localPoint);
                isInsideImage = localPoint.x > rect.x && localPoint.y > rect.y && localPoint.x < (rect.width + rect.x) && localPoint.y < rect.height + rect.y;

                if (isInsideImage)
                {
                    pX = Mathf.Clamp(0, (int)(((localPoint.x - rect.x) * tex.width) / rect.width), tex.width);
                    pY = Mathf.Clamp(0, (int)(((localPoint.y - rect.y) * tex.height) / rect.height), tex.height);

                    color = tex.GetPixel(pX, pY);

                    SetColorTo(color);
                    Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
                }
                else if (!isPointerExited)
                {
                    Cursor.SetCursor(null, hotSpot, CursorMode.Auto);
                    isPointerExited = true;
                }
            }

            if (Input.GetMouseButtonUp(0) && isInsideImage)
            {
                ResetCursorAndPointer();
                isPointerExited = false;
            }
            else if (Input.GetMouseButtonUp(0) && !isInsideImage)
            {
                Cursor.SetCursor(null, hotSpot, CursorMode.Auto);
                isPointerExited = false;
            }
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Set the color of the parameter shaderParameter of the player material with the color putted as a parameter 
    /// </summary>
    public void SetColorTo(Color col)
    {
        if (colorPickerType.ToString() =="Skin")
        {
            foreach (KeyValuePair<BodyParts, SkinnedMeshRenderer> bodySkinPart in player.playerCustomizer.skinBodyPartsDic)
                bodySkinPart.Value.sharedMaterial.SetColor(colorPickerType.ToString(), col);

            player.playerCustomizer.bodySkin.sharedMaterial.SetColor(colorPickerType.ToString(), col);
            player.playerCustomizer.headSkin.sharedMaterial.SetColor(colorPickerType.ToString(), col);
        }

        if(colorPickerType.ToString() == "Eyes_Color")
        {
            foreach(SkinnedMeshRenderer eye in player.playerCustomizer.eyesSkin)
                eye.sharedMaterial.SetColor(colorPickerType.ToString(), col);            
        }

        currentColor = col;
    }

    /// <summary>
    /// Show normal cursor and show the pointer on the mouse position
    /// </summary>
    private void ResetCursorAndPointer()
    {
        Cursor.SetCursor(null, hotSpot, CursorMode.Auto);
        pointerOnButtonUp.transform.position = Input.mousePosition;
        pointerOnButtonUp.SetActive(true);
    }

    #endregion
}