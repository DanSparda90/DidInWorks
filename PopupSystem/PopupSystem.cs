using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;
using I2.Loc;

namespace SC
{
    /// <summary>
    /// GenericPopup is a class that defines a generic popup setting the parameters and showing or hidding the popup.
    /// </summary>
    public class PopupSystem : MonoBehaviour
    {
        #region Fields
        [Header("Base elements")]
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _message;
        [SerializeField] private TextMeshProUGUI _accept, _cancel;
        [SerializeField] private Button _acceptBtn, _cancelBtn;

        [Header("Parameters")]
        [SerializeField] private float _fadeSpeed = 0.3f;

        [Header("Areas")]
        [SerializeField] private Image _titleArea;
        [SerializeField] private Image _messageArea;
        [SerializeField] private Image _buttonsArea;

        private CanvasGroup _canvasGrp;
        private bool _isAcceptSetted, _isCancelSetted;
        private bool _isPopupOn;

        private const string DEFAULT_TITLE = "INFO";

        #endregion Fields

        #region Properties
        public bool IsPopupOn => _isPopupOn;
		#endregion Properties

		#region Instance
		private static PopupSystem _instance;
        public static PopupSystem Instance()
        {
            if (!_instance)
                _instance = FindObjectOfType(typeof(PopupSystem)) as PopupSystem;

            return _instance;
        }
        #endregion Instance

        #region Unity Callbacks
        private void Awake()
        {
            _canvasGrp = GetComponent<CanvasGroup>();
        }
        #endregion Unity Callbacks

        #region Methods
        /// <summary>
        /// Shows the popup with the configuration putted as a parameter and sets the acceptAction and cancelAction. If the action is putted as null the button is hidded.
        /// </summary>
        public void ShowPopup(GenericPopupConfiguration configuration, UnityAction acceptAction, UnityAction cancelAction = null, bool haveToSetTexts = true)
        {
            string newTitle = configuration.Title;
            string newMessage = configuration.Message;
            string acceptText = configuration.AcceptText;
            string cancelText = configuration.CancelText;

            CheckTranslations(ref newTitle, ref newMessage, ref acceptText, ref cancelText, configuration);

            if (haveToSetTexts)
            {
                SetTitle(newTitle, configuration.UseTranslations);
                SetMessage(newMessage, configuration.UseTranslations);
            }

            if (acceptText != "")
                SetOnAccept(acceptText, acceptAction, configuration.UseTranslations);

            if (cancelText != "")
                SetOnCancel(cancelText, cancelAction, configuration.UseTranslations);

            _fadeSpeed = configuration.FadeSpeed;

            SetPopupColor(configuration.PopupColor);
            SwitchVisibility(true);
        }
        /// <summary>
        /// To show a popup with some text added on the title or the message
        /// </summary>
        public void ShowPopup(GenericPopupConfiguration configuration, string titleAddedText, string messageAddedText, UnityAction acceptAction, UnityAction cancelAction = null)
        {
            string newTitle = configuration.Title;
            string newMessage = configuration.Message;

            if (configuration.UseTranslations)
            {
                newTitle = configuration.TitleTranslation;
                newMessage = configuration.MessageTranslation;
            }

            SetTitle(newTitle, configuration.UseTranslations);
            _title.text += " " + titleAddedText;

            SetMessage(newMessage, configuration.UseTranslations);
            _message.text += " " + messageAddedText;

            ShowPopup(configuration, acceptAction, cancelAction, false);
            _isPopupOn = true;
        }

        /// <summary>
        /// To show simple information popups with variable text putted from code, not using the scriptable object.
        /// </summary>
        public void ShowPopup(string title, string message, Color? popupColor = null)
        {
            SetTitle(title, false);
            SetMessage(message, false);
            SetOnAccept("Accept", null, false);
            SetPopupColor(popupColor.GetValueOrDefault(Color.red));
            SwitchVisibility(true);
            _isPopupOn = true;
        }

        /// <summary>
        /// Change the visibility and interactibity of the popup according to the isVisible parameter.
        /// </summary>
        /// <param name="isVisible"></param>
        public void SwitchVisibility(bool isVisible)
        {
            _canvasGrp.blocksRaycasts = isVisible;
            _canvasGrp.interactable = isVisible;
            _canvasGrp.DOKill();
            _canvasGrp.DOFade(isVisible ? 1 : 0, _fadeSpeed);
            if (!isVisible)
			{
                Reset();
                SetPopupGameState(false);
                _isPopupOn = false;
            }
			else
			{
                SetButtonsVisibility();
                SetPopupGameState(true);
                _isPopupOn = true;
            }
        }

        private void SetTitle(string title, bool useTranslations)
        {
            string newTitle = title;

            if (useTranslations)
                newTitle = GetTranslatedValue(title);

            if (newTitle == "")
                newTitle = DEFAULT_TITLE;

            _title.text = newTitle;
        }

        private void SetMessage(string message, bool useTranslations)
        {
            string newMessage = message;

            if (useTranslations)
                newMessage = GetTranslatedValue(message);

            _message.text = newMessage;
        }

        private void SetOnAccept(string text, UnityAction action, bool useTranslations)
        {
            string acceptText = text;

            if (useTranslations)
                acceptText = GetTranslatedValue(text);

            _accept.text = acceptText;
            _acceptBtn.onClick.RemoveAllListeners();
            if (action != null)
                _acceptBtn.onClick.AddListener(action);

            _acceptBtn.onClick.AddListener(() => SwitchVisibility(false));
            _isAcceptSetted = true;
        }

        private void SetOnCancel(string text, UnityAction action, bool useTranslations)
        {
            string cancelText = text;

            if (useTranslations)
                cancelText = GetTranslatedValue(text);

            _cancel.text = cancelText;
            _cancelBtn.onClick.RemoveAllListeners();
            if (action != null)
                _cancelBtn.onClick.AddListener(action);

            _cancelBtn.onClick.AddListener(() => SwitchVisibility(false));
            _isCancelSetted = true;
        }

        private void SetPopupColor(Color newColor)
        {
            SetAreaColor(_titleArea, newColor);
            SetAreaColor(_messageArea, newColor);
            SetAreaColor(_buttonsArea, newColor);
        }

        private void SetAreaColor(Image area, Color newColor)
        {
            newColor.a = area.color.a;
            area.color = newColor;
        }

        private void Reset()
        {
            _isAcceptSetted = false;
            _isCancelSetted = false;
            _title.text = "";
        }

        private void SetButtonsVisibility()
        {
            _acceptBtn.gameObject.SetActive(_isAcceptSetted);
            _cancelBtn.gameObject.SetActive(_isCancelSetted);
        }

        /// <summary>
        /// If isBlocking is true, blocks the player movement and activates the background canvas to block any interaction with other things apart of the popup.
        /// </summary>
        /// <param name="isBlocking"></param>
        private void SetPopupGameState(bool isOn)
        {
            if (isOn)
                GameManager.instance.SetGameState(GameState.Popup_On);
			else
                GameManager.instance.SetGameState(GameState.In_Game);
        }

        private void CheckTranslations(ref string newTitle, ref string newMessage, ref string acceptText, ref string cancelText, GenericPopupConfiguration configuration)
        {
            if (configuration.UseTranslations)
            {
                newTitle = configuration.TitleTranslation;
                newMessage = configuration.MessageTranslation;
                acceptText = configuration.AcceptTextTranslation;
                cancelText = configuration.CancelTextTranslation;
            }
        }

        /// <summary>
        /// Returns the translated text according to the value putted as a parameter
        /// </summary>
        private string GetTranslatedValue(string value)
        {
            string newText = value;
            newText = LocalizationManager.GetTranslation("Popup/" + value);

            return newText;
        }
        #endregion Methods
    }
}