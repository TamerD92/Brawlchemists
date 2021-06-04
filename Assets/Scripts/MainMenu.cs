using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public string findAMatch;
    public string settings;
    public string backToMainMenu;
    public string gameplay;

    public void FindAMatch()
    {
        Application.LoadLevel(findAMatch);
    }
    public void Gameplay()
    {
        Application.LoadLevel(gameplay);
    }
    public void Settings()
    {
        Application.LoadLevel(settings);
    }
    public void BackToMainMenu()
    {
        Application.LoadLevel(backToMainMenu);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
