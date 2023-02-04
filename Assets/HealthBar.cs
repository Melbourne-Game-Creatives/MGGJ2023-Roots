using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;


    private void Update()
    {
        transform.forward = new Vector3(Camera.main.transform.forward.x, transform.forward.y, Camera.main.transform.forward.z);
    }


    public void UpdateBar(float ratio)
    {
        healthBarImage.DOFillAmount(ratio, 0.3f);
    }
}
