using System;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

using Utilsf;

using static UnityEngine.Random;

[RequireComponent(typeof(Grid))]
public class Maze : MonoBehaviour {
    #region singleton
    static public Maze instance;
    #endregion

    #region editor
    public Tilemap traps = default;
    public Tilemap obstacles = default;
    public Tilemap background = default;
    [SerializeField] private Tilemap playground = default;
    #endregion

    public event Action<int> tileRepaired;
    public event Action<int> tileBrokeDown;

    public Vector2 cellSize => background.cellSize;
    public float cellSizeFloat => (cellSize.x + cellSize.y) / 2f;
    public int repairTilesTotalCount => repairTiles.Count;

    private List<TrapTile> trapTiles;
    private List<ExitTile> exitTiles;
    private List<FloorTile> floorTiles;
    private List<RepairTile> repairTiles;

    private CustomTile[] playgroundTiles;

    #region private
    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        playgroundTiles = playground.GetComponentsInChildren<CustomTile>();
        foreach (CustomTile tile in playgroundTiles) tile.setTilemap(playground);

        trapTiles = getTiles<TrapTile>();
        exitTiles = getTiles<ExitTile>();
        floorTiles = getTiles<FloorTile>();
        repairTiles = getTiles<RepairTile>();

        repairTiles.ForEach(tile => tile.tileRepaired += onTileRepaired);
        repairTiles.ForEach(tile => tile.tileBrokeDown += onTileBrokeDown);
    }

    private void onTileRepaired() {
        int repairedTiles = repairTiles.Count(tile => tile.repaired);
        if (repairedTiles == repairTiles.Count) {
            FindObjectOfType<EnemySpawner>()?.stopSpawn();
            FindObjectsOfType<Enemy>().ToList().ForEach(enemy => enemy.die());
            floorTiles.ForEach(tile => tile.interact());
            exitTiles.ForEach(tile => tile.openDoors());
        }
        tileRepaired?.Invoke(repairedTiles);
    }

    private void onTileBrokeDown() {
        exitTiles.ForEach(tile => tile.closeDoors());
        tileBrokeDown?.Invoke(repairTiles.Count(tile => tile.repaired));
    }
    #endregion

    #region public
    public void setTileColor(Tilemap tilemap, Color color) {
        BoundsInt bounds = tilemap.cellBounds;
        for (int i = bounds.xMin; i < bounds.xMax; i++) {
            for (int j = bounds.yMin; j < bounds.yMax; j++) {
                Vector3Int cell = new Vector3Int(i, j, 0);
                setTileColor(tilemap, cell, color);
            }
        }
    }

    public void setTileColor(Tilemap tilemap, Vector3Int cell, Color color) {
        Utility.setTileColor(tilemap, cell, color);
    }

    public void setTileColor(Vector3Int cell, Color color) {
        setTileColor(background, cell, color);
    }

    public void setTileColor(Tilemap tilemap, Vector2 position, Color color) {
        Utility.setTileColor(tilemap, background.WorldToCell(position), color);
    }

    public void setTileColor(Vector2 position, Color color) {
        setTileColor(background, position, color);
    }

    public Vector3Int getLastInDirection(Vector3Int cell, Vector3Int direction) {
        while (!obstacles.HasTile(cell + direction)) cell += direction;
        return cell;
    }

    public Vector2 getLastInDirection(Vector2 position, Vector3Int direction) {
        Vector3Int cell = playground.WorldToCell(position);
        Vector3Int lastCell = getLastInDirection(cell, direction);

        Vector2 steps = (Vector2Int)(lastCell - cell) * cellSize;
        return position + steps;
    }

    public CustomTile getCustomTile(Vector2 position) {
        Vector3Int cell = playground.WorldToCell(position);

        return playgroundTiles.FirstOrDefault(ft => playground.WorldToCell(ft.position) == cell);
    }

    public FloorTile getRandomFloorTile() {
        return floorTiles.ElementAt(Range(0, floorTiles.Count));
    }

    public T getTile<T>(Vector2 position) where T : CustomTile {
        return getCustomTile(position) as T;
    }

    public List<T> getTiles<T>() where T: CustomTile {
        return playgroundTiles.OfType<T>().ToList();
    }
    #endregion
}
