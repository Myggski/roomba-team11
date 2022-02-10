using System.Collections.Generic;
using UnityEngine;

public class DishesSpawner : MonoBehaviour
{
    [SerializeField] 
    List<GameObject> dishesVariations = new List<GameObject>();

    private void Start()
    {
        int randomNumber = Random.Range(0, dishesVariations.Count);
        Instantiate(dishesVariations[randomNumber], transform.position, Quaternion.identity, transform);
    }
}
