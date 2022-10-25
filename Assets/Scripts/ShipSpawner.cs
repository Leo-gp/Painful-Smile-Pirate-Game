using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    [field: SerializeField] public List<Transform> SpawnPoints { get; private set; }
    [field: SerializeField] public List<Ship> ShipsPrefabs { get; private set; }
    [field: SerializeField] public int MaxShips { get; private set; }

    private float enemySpawnTime;

    private void Start()
    {
        enemySpawnTime = GameSessionConfiguration.Instance.EnemySpawnTime;
        InvokeRepeating("Spawn", enemySpawnTime, enemySpawnTime);
    }

    private void Spawn()
    {
        if (AIShipCount() >= MaxShips)
        {
            return;
        }
        int shipIndex = Random.Range(0, ShipsPrefabs.Count);
        Transform spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Count)];
        Instantiate(ShipsPrefabs[shipIndex], spawnPoint.position, spawnPoint.rotation);
    }

    private int AIShipCount()
    {
        var ships = FindObjectsOfType<Ship>();
        var aiShips = ships.Where(ship => ship is not IControllableShip).ToList();
        return aiShips.Count;
    }
}