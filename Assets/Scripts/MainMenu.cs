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

    public void SelectLevelOne() {
        Debug.Log("loading scene 1");
        SceneManager.LoadScene("Level_1");
    }

    public void SelectLevelTwo() {
        Debug.Log("loading scene 2");
        SceneManager.LoadScene("Level_2");
    }

    public void SelectLevelThree() {
        Debug.Log("loading scene 3");
        SceneManager.LoadScene("Level_3");
    }
}