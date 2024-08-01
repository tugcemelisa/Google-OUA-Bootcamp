using System.Collections;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    [SerializeField] float minSpawRate = 10;

    [SerializeField] float maxSpawRate = 50;

    [SerializeField] GameObject eggPrefab;

    private void Start()
    {
        StartCoroutine(SpawnEgg());
    }

    IEnumerator SpawnEgg()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawRate, maxSpawRate));
            Instantiate(eggPrefab, transform.position, Quaternion.identity);
        }
    }
}
