using UnityEngine;
using UnityEngine.SceneManagement;

public class Burrow : MonoBehaviour
{
    [SerializeField] private Canvas loseScreen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Wombaxe wombaxe))
        {
            wombaxe.StartHealing();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Wombaxe wombaxe))
        {
            wombaxe.StopHealing();
        }
    }

    public void ShowEndScreen()
    {
        loseScreen.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level");
    }
}
