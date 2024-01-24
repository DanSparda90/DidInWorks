using UnityEngine;
using System;
using TMPro;

public class LoadingPanel : MyMonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textVersion;

    internal bool LOADING_SUCCESFUL = false;
    internal bool isStarted;

    public static event Action OnLoadingCompleted;

    private void Start()
    {
        Initialize();
        ContentManager.instance.OnContentLoaded += LoadingSuccesful;                
    }

    private void OnDestroy()
    {
        ContentManager.instance.OnContentLoaded -= LoadingSuccesful;
    }

    internal void Initialize()
    {
        isStarted = true;
        gameObject.SetActive(true);
        textVersion.text = gameManager.projectVersion.version;
    }

    public void LoadingSuccesful(bool refresh = false)
    {
        gameManager.SetGameState(GameState.In_Game);

        if (InitialTutorialController.isTutorialRunning)
            gameManager.SetGameState(GameState.Initial_Tutorial);

        if (uiCustomizer.inCustomZone)
            gameManager.SetGameState(GameState.In_Customizer);

        LOADING_SUCCESFUL = true;
        gameObject.SetActive(false);

        isStarted = false;

        if (refresh == false)
		{
            if (OnLoadingCompleted != null)
                OnLoadingCompleted();
		}
    }
}
