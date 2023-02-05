using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Burrow : MonoBehaviour
{
    [SerializeField] private Canvas loseScreen;
    [SerializeField] private GameplayCanvas gameplayCanvas;
    [SerializeField] private WombatSpawner wombatSpawner;

    [SerializeField] private float frenzyCooldown;
    [SerializeField] private float growthCooldown;

    private float currentFrenzyCooldown;
    private float currentGrowthCooldown;

    private void Awake()
    {
        currentFrenzyCooldown = frenzyCooldown;
        currentGrowthCooldown = growthCooldown;
    }

    private void Update()
    {
        currentFrenzyCooldown -= Time.deltaTime;
        currentGrowthCooldown -= Time.deltaTime;

        if (currentFrenzyCooldown < 0) currentFrenzyCooldown = 0;
        if (currentGrowthCooldown < 0) currentGrowthCooldown = 0;

        gameplayCanvas.UpdateUI(currentFrenzyCooldown, frenzyCooldown, currentGrowthCooldown, growthCooldown);
    }

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
        foreach (ISelectable unit in UnitSelections.Instance.unitList)
        {
            unit.GetGameObject().GetComponent<Wombaxe>().Die();
        }

        wombatSpawner.gameObject.SetActive(false);

        gameplayCanvas.gameObject.SetActive(false);
        loseScreen.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void WombaxeFrenzy()
    {
        gameplayCanvas.DisableFrenzyButton();
        currentFrenzyCooldown = frenzyCooldown;

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

    public void WombaxeGrow()
    {
        gameplayCanvas.DisableGrowthButton();
        currentGrowthCooldown = growthCooldown;

        foreach (ISelectable unit in UnitSelections.Instance.unitList)
        {
            unit.GetGameObject().GetComponent<Wombaxe>().Grow();
        }
    }
}
