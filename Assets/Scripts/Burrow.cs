using UnityEngine;

public class Burrow : MonoBehaviour
{
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
}
