using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameplayCanvas : MonoBehaviour
{
    [SerializeField] private Button frenzyButton;
    [SerializeField] private Button growthButton;
    [SerializeField] private Image frenzyBar;
    [SerializeField] private Image growthBar;
    
    private bool helpOn = true;
    public GameObject KeyHelp;

    public void Update()
    {
        if (!helpOn || !DirectionPressed()) return;
        if (KeyHelp)
        {
            Destroy(KeyHelp);
        }
        helpOn = false;
    }

    public void EnableFrenzyButton()
    {
        if (frenzyButton.interactable) return; // already enabled

        frenzyButton.interactable = true;
        frenzyButton.transform.DOShakeScale(1f, 0.1f).SetDelay(0.5f).SetLoops(-1);
    }

    public void DisableFrenzyButton()
    {
        if (!frenzyButton.interactable) return; // already disabled

        frenzyButton.interactable = false;
        DOTween.Kill(frenzyButton.transform);
    }

    public void EnableGrowthButton()
    {
        if (growthButton.interactable) return; // already enabled

        growthButton.interactable = true;
        growthButton.transform.DOShakeScale(1, 0.1f).SetDelay(0.5f).SetLoops(-1);
    }

    public void DisableGrowthButton()
    {
        if (!growthButton.interactable) return; // already disabled

        growthButton.interactable = false;
        DOTween.Kill(growthButton.transform);
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
    
    private bool DirectionPressed()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
    }
}
