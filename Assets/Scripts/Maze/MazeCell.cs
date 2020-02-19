using System;
using Utilsf;
using UnityEngine;

[ExecuteInEditMode]
public class MazeCell : MonoBehaviour {
    /*
    [Flags] public enum Direction {
        None = 0x0,
        Up = 0x01,
        Down = 0x02,
        Right = 0x04,
        Left = 0x08,
        UpDown = Up | Down,
        LeftRight = Left | Right,
        UpLeft = Up | Left,
        DownLeft = Down | Left,
        UpRight = Up | Right,
        DownRight = Down | Right,
        UpDownLeft = UpDown | Left,
        UpDownRight = UpDown | Right,
        All = UpDown | LeftRight
    }

    #region editor
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private BoxCollider2D mazeCellCollider;
    #endregion

    public Size size { get; private set; }

    private Size platformSize;
    private GameObject platformsParent;
    private Direction direction = Direction.None;
    private GameObject[] platforms = new GameObject[4];
        
    #region private
    private void Awake() {
        size = new Size(mazeCellCollider.size.x, mazeCellCollider.size.y);
        BoxCollider2D platformCollider = platformPrefab.GetComponent<BoxCollider2D>();
        platformSize = new Size(platformCollider.size.x, platformCollider.size.y);

        platformsParent = Utility.create("platforms", gameObject) ?? transform.Find("platforms").gameObject;
    }

    private int directionToIdx(Direction d) {
        int idx = Utility.badIdx;
        if (d == Direction.Up) idx = 0;
        else if (d == Direction.Down) idx = 1;
        else if (d == Direction.Left) idx = 2;
        else if (d == Direction.Right) idx = 3;
        return idx;
    }

    private void clipTo(GameObject go, Direction d) {
        float offset = ((size.h / 2f) - (platformSize.h / 2f));
        switch (d) {
            case Direction.Up: {
                go.transform.localPosition = new Vector2(0f, offset);
                break;
            }
            case Direction.Down: {
                go.transform.localPosition = new Vector2(0f, -offset);
                break;
            }
            case Direction.Left: {
                go.transform.localPosition = new Vector2(-offset, 0f);
                go.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
            }
            case Direction.Right: {
                go.transform.localPosition = new Vector2(offset, 0f);
                go.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
            }
            default: break;
        }
    }

    private void createPlatform(Direction d) {
        (bool up, bool down, bool left, bool right) dir = hasDirections(d);

        void createPlatformLocal(Direction ld) {
            int idx = directionToIdx(ld);
            if (idx != Utility.badIdx) {
                platforms[idx] = Instantiate(platformPrefab);
                Utility.parentTo(platforms[idx], platformsParent);
                clipTo(platforms[idx], ld);

                direction |= ld;
            }
        }

        if (dir.up && !hasDirection(Direction.Up)) createPlatformLocal(Direction.Up);
        if (dir.down && !hasDirection(Direction.Down)) createPlatformLocal(Direction.Down);
        if (dir.left && !hasDirection(Direction.Left)) createPlatformLocal(Direction.Left);
        if (dir.right && !hasDirection(Direction.Right)) createPlatformLocal(Direction.Right);
    }

    private void removePlatform(Direction d) {
        (bool up, bool down, bool left, bool right) dir = hasDirections(d);

        void removePlatformLocal(Direction ld) {
            int idx = directionToIdx(ld);
            if(idx != Utility.badIdx) Destroy(platforms[idx]);

            direction &= ~ld;
        }

        if (dir.up && hasDirection(Direction.Up)) removePlatformLocal(Direction.Up);
        if (dir.down && hasDirection(Direction.Down)) removePlatformLocal(Direction.Down);
        if (dir.left && hasDirection(Direction.Left)) removePlatformLocal(Direction.Left);
        if (dir.right && hasDirection(Direction.Right)) removePlatformLocal(Direction.Right);
    }

    private void removePlatforms() {
        foreach(GameObject go in platforms) Destroy(go);
    }

    private (bool, bool, bool, bool) hasDirections(Direction d) {
        bool up = (d & Direction.Up) != Direction.None;
        bool down = (d & Direction.Down) != Direction.None;
        bool left = (d & Direction.Left) != Direction.None;
        bool right = (d & Direction.Right) != Direction.None;
        return (up, down, left, right);
    }
    #endregion

    #region public
    public bool hasDirection(Direction d) => (direction & d) != Direction.None;

    public void addDirection(Direction d) => createPlatform(d);

    public void setDirection(Direction d) {
        removePlatform(direction ^ d);
        createPlatform(d);
    }

    public void unsetDirection(Direction d) => removePlatform(direction & d);
    #endregion
    */
}
