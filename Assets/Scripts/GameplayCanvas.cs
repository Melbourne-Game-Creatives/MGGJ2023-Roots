using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameplayCanvas : MonoBehaviour
{
    [SerializeField] private Button frenzyButton;
    [SerializeField] private Button growthButton;
    [SerializeField] private Image frenzyBar;
    [SerializeField] private Image growthBar;

    public void EnableFrenzyButton()
    {
        frenzyButton.interactable = true;
    }

    public void DisableFrenzyButton()
    {
        frenzyButton.interactable = false;
    }

    public void EnableGrowthButton()
    {
        growthButton.interactable = true;
    }

    public void DisableGrowthButton()
    {
        growthButton.interactable = false;
    }

    public void UpdateUI(float currentFrenzyCooldown, float frenzyCooldown, float currentGrowthCooldown, float growthCooldown)
    {
        frenzyBar.DOFillAmount((frenzyCooldown - currentFrenzyCooldown) / frenzyCooldown, 0.4f);
        growthBar.DOFillAmount((growthCooldown - currentGrowthCooldown) / growthCooldown, 0.4f);

        if (currentFrenzyCooldown <= 0)
        {
            EnableFrenzyButton();
        }

        if (currentGrowthCooldown <= 0)
        {
            EnableGrowthButton();
        }
    }
}
