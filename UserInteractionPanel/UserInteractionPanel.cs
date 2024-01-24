using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections;
using System.Threading.Tasks;
using System;
using SC.CallSystem;

public class UserInteractionPanel : MyMonoBehaviour
{
    #region Attributes
    [Header("Parameters")]
    [SerializeField] private Transform root;
    [SerializeField] private float size = 0.65f;
    [SerializeField] private float offsetY = 1.15f;
    [SerializeField] private float distanceToClose = 10f;

    [Header("Tutorial Parameters")]
    [SerializeField] private bool isTutorial;
    [SerializeField] private GameObject tutorialChat;

    [Header("Buttons")]
    [SerializeField] private Button chatBtn;
    [SerializeField] private Button videoCallBtn;
    [SerializeField] private Button viewInfoBtn;
    [SerializeField] private Button blockMuteBtn;
    [SerializeField] private Button blockBtn;
    [SerializeField] private Button muteBtn;
    [SerializeField] private Button[] returnBtns;

    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject userInfoPanel;
    [SerializeField] private GameObject blockMutePanel;

    [Header("Main Panel")]
    [SerializeField] private TextMeshProUGUI mainUsernameText;
    [SerializeField] private TextMeshProUGUI mainUserSurnameText;
    [SerializeField] private TextMeshProUGUI mainRoleText;
    [SerializeField] private TextMeshProUGUI mainCompanyText;

    [Header("User Info Panel")]
    [SerializeField] private TextMeshProUGUI userNickText;
    [SerializeField] private TextMeshProUGUI rolNameText;
    [SerializeField] private TextMeshProUGUI userNameText;
    [SerializeField] private TextMeshProUGUI surnameText;
    [SerializeField] private TextMeshProUGUI companyNameText;
    [SerializeField] private TextMeshProUGUI roleText;
    [SerializeField] private TextMeshProUGUI emailText;
    [SerializeField] private TextMeshProUGUI esloganText;

    internal bool isOpened;
    internal Transform originalParent;

    private CanvasGroup canvasGrp;
    private GameObject[] panels;
    private Vector3 localPanelPosition;
    private Vector3 characterPos;
    private bool isInfoLoaded;

    private string userRol;
    private string userNick;

    public static event Action<Transform> OnTryStartCall;

    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        chatBtn.onClick.AddListener(ShowChat);
        videoCallBtn.onClick.AddListener(StartOneToOneCall);
        viewInfoBtn.onClick.AddListener(ShowUserInfo);
        blockMuteBtn.onClick.AddListener(ShowBlockMute);
        blockBtn.onClick.AddListener(BlockUser);
        muteBtn.onClick.AddListener(MuteUser);
        foreach (Button returnBtn in returnBtns)
            returnBtn.onClick.AddListener(ReturnMainPanel);

        panels = new GameObject[] { mainPanel, userInfoPanel, blockMutePanel };

        canvasGrp = GetComponent<CanvasGroup>();
        canvasGrp.alpha = 0;
        root.localScale = Vector3.zero;
        originalParent = transform.parent;
        characterPos = originalParent.position;
        localPanelPosition = transform.localPosition;

        callSystem.OnCallConnected += CheckIfUserIsCallable;
        //callSystem.OnCallEnds += CheckIfUserIsCallable;
    }

    void Update()
    {
        //Billboard
        if (!player || !player.gameCamera) return;
        transform.forward = player.gameCamera.transform.forward;

        // Distance to close
        if (isOpened)
        {
            if (Vector3.Distance(transform.position, inGame.player.transform.position) > distanceToClose)
            {
                ShowInteractionPanel(false);
                inGame.uiGameController.atInteractPlayerSetup.playerCustomizer.ShowOutline(false);
            }
        }
    }

    private void OnDestroy()
    {
        chatBtn.onClick.RemoveListener(ShowChat);
        videoCallBtn.onClick.RemoveListener(StartOneToOneCall);
        viewInfoBtn.onClick.RemoveListener(ShowUserInfo);
        blockMuteBtn.onClick.RemoveListener(ShowBlockMute);
        blockBtn.onClick.RemoveListener(BlockUser);
        muteBtn.onClick.RemoveListener(MuteUser);
        foreach (Button returnBtn in returnBtns)
            returnBtn.onClick.RemoveListener(ReturnMainPanel);
        callSystem.OnCallConnected -= CheckIfUserIsCallable;
        callSystem.OnCallEnds -= CheckIfUserIsCallable;
    }

    #endregion

    #region Methods
    /// <summary>
    /// Show/Hide the interaction panel according to the isShow bool putted as a parameter
    /// </summary>
    public void ShowInteractionPanel(bool isShow)
    {
        if (isShow)
        {
            characterPos = transform.parent.position;
            transform.parent = null;
            if (!isInfoLoaded && !isTutorial)
                LoadDataUserProfile().WrapErrors(); //Carga de datos en los textos del perfil

            Vector3 newPos = new Vector3(characterPos.x, characterPos.y + offsetY, characterPos.z);
            transform.position = newPos;

            canvasGrp.interactable = true;
            canvasGrp.blocksRaycasts = true;
            root.DOScale(size, 1).SetEase(Ease.OutBounce);
            canvasGrp.DOFade(1, 0.5f);
            isOpened = true;

            CheckIfUserIsCallable(null);
        }
        else
        {
            root.DOScale(0, 1).SetEase(Ease.OutExpo);
            canvasGrp.DOFade(0, 0.5f).OnComplete(() =>
            {
                transform.parent = originalParent;
                transform.localPosition = localPanelPosition;
                canvasGrp.interactable = false;
                canvasGrp.blocksRaycasts = false;

                ReturnMainPanel();
                isOpened = false;
            });
        }
    }

    private void CheckIfUserIsCallable(ICallable call)
    {
        //TODO:avoid calling users when they are in the call already??? No problem, more cases?
        //ShowVideoCallButton(true);
       // Debug.Log("Show VideoCall button");
    }

    /// <summary>
    /// Show the chat and starts a private chat with the currect selected player
    /// </summary>
    private void ShowChat()
    {
        if (isTutorial)
        {
            tutorialChat.SetActive(true);
            ShowInteractionPanel(false);
        }
        else
        {
            uiGame.SwitchChatToState(true);
            ApiChat.instance.CreatePrivateChat(inGame.uiGameController.atInteractPlayerSetup.myName, true, userInteractionPanel: true);
            ShowInteractionPanel(false);
            inGame.uiGameController.atInteractPlayerSetup.playerCustomizer.ShowOutline(false);
        }
    }

    private void StartOneToOneCall()
    {
        OnTryStartCall?.Invoke(originalParent);

        //player.oneToOneManager.StartCall(originalParent);

        ShowInteractionPanel(false);
        inGame.uiGameController.atInteractPlayerSetup.playerCustomizer.ShowOutline(false);
    }

    /// <summary>
    /// Show the panel with de loaded user info
    /// </summary>
    private void ShowUserInfo()
    {
        foreach (GameObject panel in panels)
            panel.SetActive(false);

        userInfoPanel.SetActive(true);
    }

    /// <summary>
    /// Show the block and mute panel
    /// </summary>
    private void ShowBlockMute()
    {
        foreach (GameObject panel in panels)
            panel.SetActive(false);

        blockMutePanel.SetActive(true);
    }

    /// <summary>
    /// Show the main panel
    /// </summary>
    private void ReturnMainPanel()
    {
        foreach (GameObject panel in panels)
            panel.SetActive(false);

        mainPanel.SetActive(true);
    }

    private void BlockUser()
    {
        //TODO: Block the selected user
        ShowInteractionPanel(false);
        inGame.uiGameController.atInteractPlayerSetup.playerCustomizer.ShowOutline(false);
    }

    private void MuteUser()
    {
        //TODO: Mute the selected user
        ShowInteractionPanel(false);
        inGame.uiGameController.atInteractPlayerSetup.playerCustomizer.ShowOutline(false);
    }

    /// <summary>
    /// Carga el resultado de la funcion GetUserProfile en los textos
    /// </summary>
    async Task LoadDataUserProfile()
    {
        userNick = inGame.uiGameController.atInteractPlayerSetup.myName;

        //Get User Profile
        PlayerProfile profile = await ManageProfile.instance.GetUserProfile(userNick); //Peticion de datos de perfil via websocket 

        //Get User Rol
        GetRolSpecificUserRoot rolData = await RolManager.instance.GetRolUser(userNick, EventPersistence.Instance.eventName.ToLower());
        string userRol = rolData.rol;

        #region Main Panel
        mainUsernameText.text = profile._name;
        mainUserSurnameText.text = profile._surnames;
        mainRoleText.text = profile._jobposition;
        mainCompanyText.text = profile._company;
        #endregion

        #region User Info Panel
        userNickText.text = profile._username;
        rolNameText.text = userRol;
        userNameText.text = profile._name;
        surnameText.text = profile._surnames;
        companyNameText.text = profile._company;
        roleText.text = profile._jobposition;
        emailText.text = profile._email;
        esloganText.text = profile._tagline;
        #endregion
    }

    public void ShowVideoCallButton(bool state)
    {
        if (!isTutorial)
            videoCallBtn?.gameObject.SetActive(state);
    }

    #endregion
}