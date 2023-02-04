using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;

    private Transform camTr;


    private void Awake()
    {
        camTr = Camera.main.transform;
    }


    private void Update()
    {
        transform.forward = new Vector3(camTr.forward.x, camTr.forward.y, camTr.forward.z);
    }


    public void UpdateBar(float ratio)
    {
        healthBarImage.DOFillAmount(ratio, 0.3f);
    }
}
