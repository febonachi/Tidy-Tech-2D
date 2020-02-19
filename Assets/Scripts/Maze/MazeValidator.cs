using System.Linq;
using UnityEngine;
using System.Collections.Generic;

using Utilsf;

[ExecuteInEditMode]
[RequireComponent(typeof(Maze))]
public class MazeValidator : MonoBehaviour {
    #region editor
    [SerializeField] private bool validate = true;
    #endregion

    private Maze maze;

    private readonly Vector3Int badIdx = new Vector3Int(0, 0, -1);

    #region private
    private void Awake() {
        maze = GetComponent<Maze>();

        if (Application.isPlaying) {
            maze.setTileColor(maze.background, Color.white);
            validate = false;
        }
    }

    private void Update() {
        if (!validate) return;

        maze.background.CompressBounds();

        maze.setTileColor(maze.background, Color.white);

        validatePath();
    }

    private void validatePath() {
        List<(Vector3Int, Vector3Int)> badPath = new List<(Vector3Int, Vector3Int)>();

        HashSet<Vector3Int> goals = getGoals();
        foreach (Vector3Int g1 in goals) {
            foreach (Vector3Int g2 in goals) {
                bool pathFinded = findPath(g1, g2);
                if (g1 != g2 && !pathFinded) badPath.Add((g1, g2));
            }
        }

        if (badPath.Count > 0) {
            (Vector3Int, Vector3Int) goal = badPath.First();
            maze.setTileColor(goal.Item1, Color.yellow);
            maze.setTileColor(goal.Item2, Color.red);
        }
    }

    private void restorePath(Dictionary<Vector3Int, Vector3Int> path, Vector3Int node, Color color) {
        while (path[node] != badIdx) {
            highlight(node, path[node], color);
            node = path[node];
        }
    }

    private bool findPath(Vector3Int from, Vector3Int to) {
        List<Vector3Int> visited = new List<Vector3Int>();
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        queue.Enqueue(from);

        Dictionary<Vector3Int, Vector3Int> highlight = new Dictionary<Vector3Int, Vector3Int>();
        highlight[from] = badIdx;

        while (queue.Count != 0) {
            Vector3Int current = queue.Dequeue();
            visited.Add(current);
            
            if (current == to) {
                restorePath(highlight, current, Color.green);
                return true;
            }

            foreach(Vector3Int direction in getValidDirections(current)) {
                Vector3Int lastInDirection = maze.getLastInDirection(current, direction);
                if (!trapsInDirection(current, lastInDirection) && !visited.Contains(lastInDirection)) {
                    highlight[lastInDirection] = current;
                    queue.Enqueue(lastInDirection);
                }
            }
        }

        return false;
    }

    private bool trapsInDirection(Vector3Int from, Vector3Int to) {
        Vector3Int direction = Utility.getDirection(from, to);
        while (from != to) {
            if (maze.traps.HasTile(from)) return true;
            from += direction;
        }
        return false;
    }

    private HashSet<Vector3Int> getGoals() {
        HashSet<Vector3Int> goals = new HashSet<Vector3Int>();
        BoundsInt bounds = maze.background.cellBounds;
        for (int row = bounds.xMin; row < bounds.xMax; row++) {
            for (int col = bounds.yMin; col < bounds.yMax; col++) {
                Vector3Int cell = new Vector3Int(row, col, 0);
                if (!maze.obstacles.HasTile(cell)) {
                    // left-bottom corner
                    if (maze.obstacles.HasTile(new Vector3Int(cell.x - 1, cell.y, 0)) &&
                        maze.obstacles.HasTile(new Vector3Int(cell.x - 1, cell.y - 1, 0)) &&
                        maze.obstacles.HasTile(new Vector3Int(cell.x, cell.y - 1, 0))) {
                        goals.Add(cell);
                    }
                    // left-top corner
                    if (maze.obstacles.HasTile(new Vector3Int(cell.x, cell.y + 1, 0)) &&
                        maze.obstacles.HasTile(new Vector3Int(cell.x - 1, cell.y + 1, 0)) &&
                        maze.obstacles.HasTile(new Vector3Int(cell.x - 1, cell.y, 0))) {
                        goals.Add(cell);
                    }
                    // right-top corner
                    if (maze.obstacles.HasTile(new Vector3Int(cell.x + 1, cell.y, 0)) &&
                        maze.obstacles.HasTile(new Vector3Int(cell.x + 1, cell.y + 1, 0)) &&
                        maze.obstacles.HasTile(new Vector3Int(cell.x, cell.y + 1, 0))) {
                        goals.Add(cell);
                    }
                    // right-bottom corner
                    if (maze.obstacles.HasTile(new Vector3Int(cell.x + 1, cell.y, 0)) &&
                        maze.obstacles.HasTile(new Vector3Int(cell.x + 1, cell.y - 1, 0)) &&
                        maze.obstacles.HasTile(new Vector3Int(cell.x, cell.y - 1, 0))) {
                        goals.Add(cell);
                    }
                }
            }
        }
        return goals;
    }

    private void highlight(Vector3Int from, Vector3Int to, Color color) {
        Vector3Int direction = Utility.getDirection(from, to);
        while(from != to) {
            maze.setTileColor(from, color);
            from += direction;
        }
        maze.setTileColor(from, color);
    }

    private List<Vector3Int> getValidDirections(Vector3Int cell) {
        return Utility.directions().Where(e => !maze.obstacles.HasTile(cell + e)).ToList();
    }
    #endregion
}
