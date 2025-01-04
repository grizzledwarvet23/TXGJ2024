using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject TitleScreen;

    public GameObject InfoScreen;

    public void OnStartButton() {
        SelectLevelOne();
    }

    public void OnBackButton() 
    {
        TitleScreen.SetActive(true);
        InfoScreen.SetActive(false);
    }

    public void OnInfoButton() {
        TitleScreen.SetActive(false);
        InfoScreen.SetActive(true);
    }

    public void OnQuitButton() {
        Application.Quit();
        Debug.Log("Application Quit.");
    }

    public void SelectLevelOneTutorial()
    {
        Debug.Log("loading scene 1 tutorial");
        SceneManager.LoadScene("Level_1 Tutorial");
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