using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string findAMatch;
    public string settings;
    public string backToMainMenu;
    public string gameplay;

    public void FindAMatch()
    {
        SceneManager.LoadScene(findAMatch);
    }
    public void Gameplay()
    {
        SceneManager.LoadScene(gameplay);
    }
    public void Settings()
    {
        SceneManager.LoadScene(settings);
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(backToMainMenu);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
