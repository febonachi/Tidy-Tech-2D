using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading.Tasks;
using System.Collections;

using Utilsf;

public abstract class CustomTile : MonoBehaviour {
    public struct TileInteractionInfo {
        public bool inAction;
        public bool interuptNextAction;
    }

    [HideInInspector] public Vector3Int cell;
    [HideInInspector] public TileInteractionInfo info;
    [HideInInspector] public Vector3 position => transform.position;

    protected Maze maze;

    private Tilemap tilemap;

    #region protected
    protected void Start() {
        maze = Maze.instance;
    }

    protected IEnumerator inActionDelay(float delay, bool state = false) {
        yield return new WaitForSeconds(delay);

        info.inAction = state;
    }

    protected IEnumerator interuptDelay(float delay, bool state = false) {
        yield return new WaitForSeconds(delay);

        info.interuptNextAction = state;
    }

    protected void highlight(SpriteRenderer sr, Color color, bool coroutine = true) {
        if (coroutine) StartCoroutine(Utility.changeColorOverTime(sr, color));
        else sr.color = color;
    }
    #endregion

    #region public
    public void setTilemap(Tilemap tilemap) {
        this.tilemap = tilemap;

        cell = tilemap.WorldToCell(position);
    }
    #endregion

    #region public abstract
    public abstract void interact(GameObject entity);
    #endregion
}
