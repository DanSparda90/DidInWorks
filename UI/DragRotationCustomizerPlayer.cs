using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragRotationCustomizerPlayer : MyMonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region Attibutes
    [SerializeField] private Camera customCam;
    [SerializeField] private float rotationSpeed = 10;
    //[SerializeField] private float accelerationPower = 2;
    [SerializeField] private Texture2D rotationMouseCursor;

    internal bool isDragging;
    private bool isInitialized;
    private Transform target;
    private float angle;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        uiCustomizer.OnExitCustomZone += ResetRotation;        
    }

    private void Update() 
    {
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            player.customizerCameraBodyPivot.parent = player.playerBodyPivot.parent;
            player.customizerCameraFacePivot.parent = player.playerFacePivot.parent;
            isDragging = false;
        }
    }

    private void OnDestroy()
    {
        uiCustomizer.OnExitCustomZone -= ResetRotation;       
    }

    #endregion

    #region Interfaces
    public void OnDrag(PointerEventData eventData) 
    {
        if(customCam == null)
            InitializeDragRotation();        
    
        isDragging = true;
        SetTarget();
        angle = eventData.delta.x * rotationSpeed * Time.deltaTime;
        customCam.transform.RotateAround(target.position, Vector3.up, angle);
        player.customizerCameraBodyPivot.parent = player.customizerCamera.transform;
        player.customizerCameraFacePivot.parent = player.customizerCamera.transform;
    }  

    public void OnPointerEnter(PointerEventData eventData) 
    {
        Cursor.SetCursor(rotationMouseCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    #endregion

    #region Methods
    /// <summary>
    /// Initialize the camera and target for the drag rotation
    /// </summary>
    public void InitializeDragRotation(bool directReset = false) 
    {
        if (!isInitialized)
        {
            customCam = player.customizerCamera;
            isInitialized = true;
        }

        if (directReset)
            ResetRotation();
    }   

    /// <summary>
    /// Set the target that the camera have to look
    /// </summary>
    internal void SetTarget() 
    {
        if (inGame.uiCustomizer.isInHead)
            target = player.playerFacePivot;
        else
            target = player.playerBodyPivot;
    }

    /// <summary>
    /// Reset the rotation of the camera and related objects to the initial position and rotation and put the camera on the body position.
    /// </summary>
    internal void ResetRotation()
    {
        uiCustomizer.SetPlayerRenderCamera(ShapeSections.Body, true);
    }

    #endregion
}