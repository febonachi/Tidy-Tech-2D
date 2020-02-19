using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class MovableEntity : MonoBehaviour {
    public event Action moved;
    public event Action obstacleEntered;

    private Maze maze;
    private Collider2D c2d;
    private Rigidbody2D rb2d;

    #region private
    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        maze = Maze.instance;
    }
    #endregion

    #region public
    public IEnumerator Move(Vector3Int direction) {
        Vector2 step = new Vector2(direction.x, direction.y) * maze.cellSize;
        Vector2 position = maze.getLastInDirection(rb2d.position, direction);

        bool alreadyInPosition = (rb2d.position == position);

        if (!alreadyInPosition) moved?.Invoke();

        while (Vector3.Distance(rb2d.position, position) > .05f) {
            Vector2 nextPosition = rb2d.position + step;
            rb2d.MovePosition(nextPosition);

            /* get tile info and interact*/
            CustomTile tile = maze.getCustomTile(nextPosition);
            tile.interact(gameObject);

            bool interupt = tile.info.interuptNextAction;

            //while (tile.info.inAction) yield return null;

            if (interupt) yield break;

            yield return null;
        }

        if (!alreadyInPosition) {
            maze.getTile<ObstacleTile>(position + step)?.interact();
            obstacleEntered?.Invoke();
        }
    }
    #endregion
}
