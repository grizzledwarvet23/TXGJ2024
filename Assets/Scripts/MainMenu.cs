using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnStartButton() {
        SceneManager.LoadScene("Level Select");
    }

    public void OnQuitButton() {
        Application.Quit();
        Debug.Log("Application Quit.");
    }
}