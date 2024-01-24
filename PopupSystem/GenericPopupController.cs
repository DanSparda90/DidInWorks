using UnityEngine;
using UnityEngine.Events;
using SC.CallSystem;

namespace SC
{
	public class GenericPopupController : MonoBehaviour
	{
		#region Attributes
		[Header("ApiVideoCall Popup Configurations")]
		[SerializeField] private GenericPopupConfiguration _callFromPopupConfig;
		[SerializeField] private GenericPopupConfiguration _callRejectedByPopupConfig;

		[Header("MyNetworkManager Popup Configurations")]
		[SerializeField] private GenericPopupConfiguration _serverNotRespondPopupConfig;

		[Header("SpaceManager Popup Configurations")]
		[SerializeField] private GenericPopupConfiguration _proModulePopupConfig;

		[Header("ModularTeleport Popup Configurations")]
		[SerializeField] private GenericPopupConfiguration _errorInitializatingPopupConfig;

		[Header("CreatorMoudeUI Popup Configurations")]
		[SerializeField] private GenericPopupConfiguration _worldPublishedPopupConfig;

		[Header("MainMenuController Popup Configurations")]
		[SerializeField] private GenericPopupConfiguration _createSpacePopupConfiguration;

		[Header("Quiz Popup Configurations")]
		[SerializeField] private QuizTrigger _quizTrigger;
		[SerializeField] private GenericPopupConfiguration _editQuizPopupConfig;
		[SerializeField] private GenericPopupConfiguration _exitQuizPopupConfig;

		[Header("Elements Popup Configurations")]
		[SerializeField] private GenericPopupConfiguration _videoGalleryInvalidURLPopupConfig;
        [SerializeField] private GenericPopupConfiguration _elementGenericErrorLoadingPopupConfig;

        private PopupSystem _popupSystem;

		#endregion Attributes

		#region Unity Callbacks

		private void Awake()
		{
			_popupSystem = PopupSystem.Instance();
		}

		private void Start()
		{
			#region ApiVideoCall Events
			CallRequest.Instance().OnRecieveIndividualCall += (otherUser, acceptAction, cancelAction) => ShowPopupWithAddedTexts(_callFromPopupConfig, otherUser, "", acceptAction, cancelAction);
			CallRequest.Instance().OnCallRejected += (otherUser, acceptAction, cancelAction) => ShowPopupWithAddedTexts(_callRejectedByPopupConfig, "", otherUser, acceptAction, cancelAction);
			#endregion ApiVideoCall Events

			#region Admin Events
			//InGameController.instance.player.adminPowers.OnSendAlertMessage += ShowCustomPopup;
			#endregion Admin Events

			#region EventData Events
			EventData.instance.OnSpaceEditorDisconnected += ShowCustomPopup;
			#endregion EventData Events

			#region SpaceManager Events
			InGameController.instance.spaceManager.OnErrorInitializingSpace += (acceptAction, cancelAction) => ShowAcceptCancelPopup(_errorInitializatingPopupConfig, acceptAction, cancelAction);
			#endregion SpaceManager Events

			#region CreatorMode Events
			CreatorModeUI.instance.OnRefreshWorldPopup += (acceptAction, cancelAction) => ShowAcceptCancelPopup(_worldPublishedPopupConfig, acceptAction, cancelAction);
			#endregion

			#region MainMenuController Events
			InGameController.instance.mainMenuCtrl.OnWantToEditWorld += (acceptAction, cancelAction) => ShowAcceptCancelPopup(_createSpacePopupConfiguration, acceptAction, cancelAction);
			#endregion MainMenuController Events

			#region ModularTeleport Events
			//TODO: Find ModularTeleport on scene
			//ModularTeleport.OnTryEnterProModuleWithoutLicense += (acceptAction, cancelAction) => ShowAcceptCancelPopup(_proModulePopupConfiguration, acceptAction, cancelAction);
			#endregion ModularTeleport Events

			#region NetworkManager Events
			InGameController.instance.networkController.OnClientMirrorDisconnect += (acceptAction, cancelAction) => ShowAcceptCancelPopup(_serverNotRespondPopupConfig, acceptAction, cancelAction);
			#endregion

			#region Quiz Events
			if (_quizTrigger != null)
			{
				_quizTrigger.OnEditQuiz += (acceptAction, cancelAction) => ShowAcceptCancelPopup(_editQuizPopupConfig, acceptAction, cancelAction);
				_quizTrigger.OnExitQuiz += (acceptAction, cancelAction) => ShowAcceptCancelPopup(_exitQuizPopupConfig, acceptAction, cancelAction);
			}
			#endregion

			#region Element Video Player Events
			ElementsSystem.Elements.VideoPlayer.VideoPlayerController.OnLoadURL += (acceptAction, cancelAction) => ShowAcceptCancelPopup(_videoGalleryInvalidURLPopupConfig, acceptAction, cancelAction);
            ElementsSystem.Elements.VideoPlayer.VideoPlayerController.OnVideoLoadFailed += (acceptAction, cancelAction) => ShowAcceptCancelPopup(_elementGenericErrorLoadingPopupConfig, acceptAction, cancelAction);
            #endregion Element Video Player Events


            #region Element Image Gallery Events
            ElementsSystem.Elements.Gallery.GalleryElementController.OnImageLoadFailed += (acceptAction, cancelAction) => ShowAcceptCancelPopup(_elementGenericErrorLoadingPopupConfig, acceptAction, cancelAction);
            #endregion Element Image Gallery Events

            #region Element PDF Reader Events
            ElementsSystem.Elements.PDFReader.PDFReaderController.OnPDFLoadFailed += (acceptAction, cancelAction) => ShowAcceptCancelPopup(_elementGenericErrorLoadingPopupConfig, acceptAction, cancelAction);
            #endregion Element Image Gallery Events

            #region Element Video Player Events
            ElementsSystem.Elements.VideoPlayer.VideoPlayerController.OnVideoUploadFailed += (errorMsg, acceptAction, cancelAction) => ShowPopupWithAddedTexts(_elementGenericErrorLoadingPopupConfig, "", errorMsg, acceptAction, cancelAction);
            #endregion
        }

        private void OnDestroy()
		{
			#region ApiVideoCall Events
			CallRequest.Instance().OnRecieveIndividualCall -= (otherUser, acceptAction, cancelAction) => ShowPopupWithAddedTexts(_callFromPopupConfig, otherUser, "", acceptAction, cancelAction);
			CallRequest.Instance().OnCallRejected -= (otherUser, acceptAction, cancelAction) => ShowPopupWithAddedTexts(_callRejectedByPopupConfig, "", otherUser, acceptAction, cancelAction);
			#endregion ApiVideoCall Events

			#region Admin Events
			//InGameController.instance.player.adminPowers.OnSendAlertMessage -= ShowCustomPopup;
			#endregion Admin Events

			#region EventData Events
			EventData.instance.OnSpaceEditorDisconnected -= ShowCustomPopup;
			#endregion EventData Events

			#region SpaceManager Events
			InGameController.instance.spaceManager.OnErrorInitializingSpace -= (acceptAction, cancelAction) => ShowAcceptCancelPopup(_errorInitializatingPopupConfig, acceptAction, cancelAction);
			#endregion SpaceManager Events

			#region CreatorMode Events
			CreatorModeUI.instance.OnRefreshWorldPopup -= (acceptAction, cancelAction) => ShowAcceptCancelPopup(_worldPublishedPopupConfig, acceptAction, cancelAction);
			#endregion

			#region MainMenuController Events
			InGameController.instance.mainMenuCtrl.OnWantToEditWorld -= (acceptAction, cancelAction) => ShowAcceptCancelPopup(_createSpacePopupConfiguration, acceptAction, cancelAction);
			#endregion MainMenuController Events

			#region ModularTeleport Events
			//TODO: Find ModularTeleport on scene
			//ModularTeleport.OnTryEnterProModuleWithoutLicense -= (acceptAction, cancelAction) => ShowAcceptCancelPopup(_proModulePopupConfiguration, acceptAction, cancelAction);
			#endregion ModularTeleport Events

			#region NetworkManager Events
			InGameController.instance.networkController.OnClientMirrorDisconnect -= (acceptAction, cancelAction) => ShowAcceptCancelPopup(_serverNotRespondPopupConfig, acceptAction, cancelAction);
			#endregion

			#region Quiz Events
			if(_quizTrigger != null)
			{
				_quizTrigger.OnEditQuiz -= (acceptAction, cancelAction) => ShowAcceptCancelPopup(_editQuizPopupConfig, acceptAction, cancelAction);
				_quizTrigger.OnExitQuiz -= (acceptAction, cancelAction) => ShowAcceptCancelPopup(_exitQuizPopupConfig, acceptAction, cancelAction);
			}
			#endregion

			#region Element Video Player Events
			ElementsSystem.Elements.VideoPlayer.VideoPlayerController.OnLoadURL -= (acceptAction, cancelAction) => ShowAcceptCancelPopup(_videoGalleryInvalidURLPopupConfig, acceptAction, cancelAction);
            ElementsSystem.Elements.VideoPlayer.VideoPlayerController.OnVideoLoadFailed += (acceptAction, cancelAction) => ShowAcceptCancelPopup(_elementGenericErrorLoadingPopupConfig, acceptAction, cancelAction);
            #endregion Element Video Player Events

            #region Element Image Gallery Events
            ElementsSystem.Elements.Gallery.GalleryElementController.OnImageLoadFailed -= (acceptAction, cancelAction) => ShowAcceptCancelPopup(_elementGenericErrorLoadingPopupConfig, acceptAction, cancelAction);
            #endregion Element Image Gallery Events

            #region Element PDF Reader Events
            ElementsSystem.Elements.PDFReader.PDFReaderController.OnPDFLoadFailed -= (acceptAction, cancelAction) => ShowAcceptCancelPopup(_elementGenericErrorLoadingPopupConfig, acceptAction, cancelAction);
            #endregion Element Image Gallery Events
        }

        #endregion Unity Callbacks

        #region Methods

        /// <summary>
        /// Show a popup with accept/cancel buttons. If cancel action is null the cancel button is not displayed.
        /// </summary>
        private void ShowAcceptCancelPopup(GenericPopupConfiguration popupConfig, UnityAction acceptAction, UnityAction cancelAction)
		{
			_popupSystem.ShowPopup(popupConfig, acceptAction, cancelAction);
		}

		/// <summary>
		/// Show a popup on a text can be added on the title and/or the message to put dynamic texts like the user name. 
		/// </summary>
		private void ShowPopupWithAddedTexts(GenericPopupConfiguration popupConfig, string titleAddedText, string messageAddedText, UnityAction acceptAction, UnityAction cancelAction)
		{
			_popupSystem.ShowPopup(popupConfig, titleAddedText, messageAddedText, acceptAction, cancelAction); 
		}

		/// <summary>
		/// Show a popup with specific title, message and color without using a GenericPopupConfiguration.
		/// </summary>
		private void ShowCustomPopup(string title, string message, Color? popupColor = null)
		{
			_popupSystem.ShowPopup(title, message, popupColor);
		}

		#endregion Methods
	}
}


