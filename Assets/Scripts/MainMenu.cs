using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject TitleScreen;
    public GameObject LevelSelectScreen;
    public void OnStartButton() {
        LevelSelectScreen.SetActive(true);
        TitleScreen.SetActive(false);
    }

    public void OnBackButton() 
    {
        TitleScreen.SetActive(true);
        LevelSelectScreen.SetActive(false);
    }

    public void OnQuitButton() {
        Application.Quit();
        Debug.Log("Application Quit.");
    }
}