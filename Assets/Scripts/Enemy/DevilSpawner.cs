using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _devilPrefabs;
    [SerializeField] private float _devilInterval = 1f;
    [SerializeField] private Transform _spawnPoint;

    private void Start()
    {
        StartCoroutine(SpawnLittleDevil(_devilInterval, _devilPrefabs));
    }

    private IEnumerator SpawnLittleDevil(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);

        GameObject newEnemy = Instantiate(enemy, _spawnPoint.position, Quaternion.identity);

        StartCoroutine(SpawnLittleDevil(interval, enemy));
    }
}
