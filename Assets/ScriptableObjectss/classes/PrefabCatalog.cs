using UnityEngine;

[CreateAssetMenu(fileName = "PrefabCatalog", menuName = "Roots/New prefab catalog")]
public class PrefabCatalog : ScriptableObject
{
    [NonReorderable] // fixes visualization issue of the first element
    [SerializeField] public GameObject[] prefabs;

    public GameObject getRandom()
    {
        return prefabs[Random.Range(0, prefabs.Length - 1)];
    }
}
