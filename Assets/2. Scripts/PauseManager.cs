using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject OptionPanel;
    
    public void NewGameButton_onMouseClick()
    {
        TopDownEngineEvent.Trigger(TopDownEngineEventTypes.UnPause, null);
        MMSceneLoadingManager.LoadScene("IngameScene", "IngameScene");
    }

    public void ContinueButton_onMouseClick()
    {
        //
    }

    public void OptionButton_onMouseClick()
    {
        OptionPanel.SetActive(true);
    }

    public void LobbyButton_onMouseClick()
    {
        TopDownEngineEvent.Trigger(TopDownEngineEventTypes.UnPause, null);
        MMSceneLoadingManager.LoadScene("1. Scenes/StartScreen", "StartScreen");
        //SceneManager.LoadScene("1. Scenes/StartScreen");
    }

    public void ExitButton_onMouseClick()
    {
        Application.Quit();
    }

}
