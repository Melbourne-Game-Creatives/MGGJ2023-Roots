using System.Collections;
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

    public void WombaxeFrenzy()
    {
        StartCoroutine(WombaxeFrenzyCoroutine());
    }

    private IEnumerator WombaxeFrenzyCoroutine()
    {
        foreach (ISelectable unit in UnitSelections.Instance.unitList)
        {
            unit.GetGameObject().GetComponent<Wombaxe>().TriggerFrenzy();
        }

        yield return new WaitForSeconds(10f);

        foreach (ISelectable unit in UnitSelections.Instance.unitList)
        {
            unit.GetGameObject().GetComponent<Wombaxe>().RemoveFrenzy();
        }
    }
}
