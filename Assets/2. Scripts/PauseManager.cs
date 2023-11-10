using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject OptionPanel;
    
    public void NewGameButton_onMouseClick()
    {
        SceneManager.LoadScene("IngameScene");
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
        SceneManager.LoadScene("StartScreen");
    }

    public void ExitButton_onMouseClick()
    {
        Application.Quit();
    }

}
