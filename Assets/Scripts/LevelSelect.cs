using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void SelectLevelOne() {
        SceneManager.LoadScene("Level_1");
    }

    public void SelectLevelTwo() {
        // SceneManager.LoadScene("Level_2");
    }

    public void SelectLevelThree() {
        // SceneManager.LoadScene("Level_3");
    }
}
