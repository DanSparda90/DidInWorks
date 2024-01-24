using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This controller controls the inputs that can do the player for the different actions, including all the camera and movement inputs.
/// </summary>
public class ProPlayerInputController : MyMonoBehaviour 
{ 
    #region Attributes
    [Header("Objects")]
    [SerializeField] private ProCharacterController characterCtrl;
    [SerializeField] private Transform camFollowPoint;
    public bool testingMode;
    private ProCharacterCameraController _cam;
    internal ProCharacterCameraController cameraController
    {
        get
        {
            if (_cam == null)
                _cam = FindObjectOfType<ProCharacterCameraController>();
            return _cam;
        }
    }

    [Header("Canvas")]
    public GameObject spectatorModeCanvas;

    [Header("Parameters")]
    internal bool canMove = true;
    internal bool canMoveCam = true;
    internal bool canRotateCam;
   

    private const string mouseXInput = "Mouse X";
    private const string mouseYInput = "Mouse Y";
    private const string mouseScrollInput = "Mouse ScrollWheel";
    private const string horizontalInput = "Horizontal";
    private const string verticalInput = "Vertical";
    #endregion

    #region Unity Callbacks
    void Start(){
        if (testingMode || GetComponent<ProPlayerSetup>().isLocalPlayer)
        {
            cameraController.SetFollowTransform(camFollowPoint);
            SetIgnoredCollisionsForCam();

            ProPlayerSetup.OnLocalPlayerProfileLoaded += SetSpectatorModeForGhostRol;
        }
    }

    void Update() {
        if (canMove)
            HandleCharacterInput();

        #region Camera rotation on right mouse click
        if (canMoveCam)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                canRotateCam = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                canRotateCam = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            canRotateCam = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        #endregion
    }

    private void LateUpdate() {
        if(canMoveCam)
            HandleCameraInput();
    }

    #endregion

    #region Methods
    private void SetSpectatorModeForGhostRol(LocalPlayerProfile playerProfile)
    {
        //if (playerProfile.rol == UserRol.Ghost)
        //    characterCtrl.SetTransitionToState(CharacterState.Spectator);
        //else
        characterCtrl.SetTransitionToState(CharacterState.Default);
    }

    /// <summary>
    /// Process all the camera input and apply them to the camera
    /// </summary>
    private void HandleCameraInput() {
        // Create the look input vector for the camera
        float mouseLookAxisUp = Input.GetAxisRaw(mouseYInput);
        float mouseLookAxisRight = Input.GetAxisRaw(mouseXInput);
        Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

        if(!canRotateCam)
            lookInputVector = Vector3.zero;

        float scrollInput = -Input.GetAxis(mouseScrollInput);
                
        // Apply inputs to the camera
        cameraController.UpdateWithInput(scrollInput, lookInputVector, false);

        //Disable camera obstruction detection if character is in spectator mode
        if(characterCtrl.currentCharacterState == CharacterState.Spectator){
            cameraController.enabledObstructionDetection = false;
        }else{
            if(!cameraController.enabledObstructionDetection)
                cameraController.enabledObstructionDetection = true;
        }

        // Foto Mode?
        //if (Input.GetKeyDown(KeyCode.F)) {
        //    cam.TargetDistance = ( cam.TargetDistance == 0f ) ? cam.distanceFromPlayer : 0f;
            
        //}
    }

    /// <summary>
    /// Process all the character inputs and set them
    /// </summary>
    private void HandleCharacterInput() {
        ProPlayerCharacterInputs characterInputs = new ProPlayerCharacterInputs();

        #region Player Input Actions
        characterInputs.moveAxisForward = Input.GetAxisRaw(verticalInput);
        characterInputs.moveAxisRight = Input.GetAxisRaw(horizontalInput);
        characterInputs.run = Input.GetKey(KeyCode.LeftShift);
        characterInputs.jump = Input.GetKeyDown(KeyCode.Space);   
        characterInputs.jumpHeld = Input.GetKey(KeyCode.Space);
        characterInputs.crouch = Input.GetKey(KeyCode.C);
        characterInputs.cameraRotation = cameraController.Transform.rotation;        
        characterInputs.randomDance = Input.GetKeyDown(KeyCode.V);
        if(!testingMode && characterCtrl.canUseSpectatorMode && adminController.CheckIsRole(player.profile.rol.ToString(), characterCtrl.spectatorModeValidRoles))
            characterInputs.spectatorMode = Input.GetKeyDown(KeyCode.Alpha0); //Spectator mode

        #endregion

        characterCtrl.SetInputs(ref characterInputs);
    }
    private void SetIgnoredCollisionsForCam() {
        List<Collider> collidersToIgnore = new List<Collider>();
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
            collidersToIgnore.Add(col);

        cameraController.ignoredColliders = collidersToIgnore;
    }


    #endregion
}