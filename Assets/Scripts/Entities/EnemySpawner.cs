using UnityEngine;

using Utilsf;

public class EnemySpawner : MonoBehaviour {
    #region editor
    public bool spawnMode = true;
    [SerializeField] private int maxSpawnCount = 10;
    [SerializeField] private float minSpawnDelay = 5f;
    [SerializeField] private GameObject prefab = default;
    #endregion

    private Maze maze;
    private float elapsed = 0f;
    private float nextSpawn = 0f;

    #region private
    private void Start() {
        maze = Maze.instance;
    }

    private void Update() {
        if (!spawnMode) return;

        elapsed += Time.deltaTime;

        if(elapsed > nextSpawn) {
            elapsed = 0f;

            if (transform.childCount < maxSpawnCount) {
                GameObject go = Instantiate(prefab, maze.getRandomFloorTile().position, Quaternion.identity);
                Utility.parentTo(go, gameObject);
            }

            nextSpawn = Random.Range(minSpawnDelay, minSpawnDelay * 2);
        }
    }
    #endregion

    #region public
    public void stopSpawn() {
        spawnMode = false;
    }
    #endregion
}
