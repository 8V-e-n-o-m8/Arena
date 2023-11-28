using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleDevilSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _littleDevilPrefabs;
    [SerializeField] private float _littleDevilInterval = 1f;
    [SerializeField] private Transform _spawnPoint;

    private void Start()
    {
        StartCoroutine(SpawnLittleDevil(_littleDevilInterval, _littleDevilPrefabs));
    }

    private IEnumerator SpawnLittleDevil(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);

        GameObject newEnemy = Instantiate(enemy, _spawnPoint.position, Quaternion.identity);

        StartCoroutine(SpawnLittleDevil(interval, enemy));
    }
}
