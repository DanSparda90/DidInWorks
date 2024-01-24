using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PopupConfiguration", menuName = "Futura/Popups/Popup Configuration")]
[System.Serializable]
public class GenericPopupConfiguration : ScriptableObject
{
	#region Fields
	[SerializeField] private bool _useTranslations;
	[SerializeField, HideIf("_useTranslations")] private string _title;
	[SerializeField, ShowIf("_useTranslations")] private string _titleTranslationName;

	[SerializeField, TextArea, HideIf("_useTranslations")] private string _message;
	[SerializeField, ShowIf("_useTranslations")] private string _messageTranslationName;

	[SerializeField, HideIf("_useTranslations")] private string _acceptText, _cancelText;
	[SerializeField, ShowIf("_useTranslations")] private string _acceptTextTranslationName, _cancelTextTranslationName;

	[SerializeField] private float _fadeSpeed = 0.3f;
	[SerializeField] private Color _popupColor = Color.white;
	#endregion

	#region Properties
	public bool UseTranslations => _useTranslations;
	public string Title => _title;
	public string TitleTranslation => _titleTranslationName;
	public string Message => _message;
	public string MessageTranslation => _messageTranslationName;
	public string AcceptText => _acceptText;
	public string AcceptTextTranslation => _acceptTextTranslationName;
	public string CancelText => _cancelText;
	public string CancelTextTranslation => _cancelTextTranslationName;
	public float FadeSpeed => _fadeSpeed;
	public Color PopupColor => _popupColor;
	#endregion
}