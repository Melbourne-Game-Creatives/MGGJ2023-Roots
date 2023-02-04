using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void OnStart()
    {
        SceneManager.LoadScene("Level");
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
