using UnityEngine;
using UnityEngine.UI;

public class GameplayCanvas : MonoBehaviour
{
    [SerializeField] private Button frenzyButton;
    [SerializeField] private Button growthButton;

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
}
