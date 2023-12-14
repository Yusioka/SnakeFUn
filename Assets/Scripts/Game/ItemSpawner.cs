using Snakefun.Game.Control;
using UnityEngine;

namespace Snakefun.Game
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] GameObject objectToSpawn;
        [SerializeField] Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);

        private int numberOfObjects = 1;
        SnakeController controller;

        void Start()
        {
            SpawnObjects();

            controller = GameObject.FindGameObjectWithTag("Player").GetComponent<SnakeController>();
            controller.OnEat += SpawnObjects;
        }

        void SpawnObjects()
        {
            for (int i = 0; i < numberOfObjects; i++)
            {
                Vector3 randomPosition = GetRandomSpawnPosition();
                Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                Instantiate(objectToSpawn, randomPosition, randomRotation);
            }
        }

        Vector3 GetRandomSpawnPosition()
        {
            float randomX = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
            float randomZ = Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f);

            Vector3 randomPosition = new Vector3(randomX, 0f, randomZ) + transform.position;

            return randomPosition;
        }
    }
}
