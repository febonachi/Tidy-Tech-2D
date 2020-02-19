using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

using static UnityEngine.Random;

namespace Utilsf {
    [Serializable] public struct Size {
        public float w;
        public float h;

        public Size(float w = 0, float h = 0) {
            this.w = w;
            this.h = h;
        }

        public static implicit operator Vector2(Size sz) {
            return new Vector2(sz.w, sz.h);
        }
    }

    [Serializable] public struct MinMax {
        public float min;
        public float max;
    }

    public static class Utility {
        public const int badIdx = -1;

        public static Color transparent => new Color(0, 0, 0, 0);

        public static bool maybe => Range(-1, 1) == -1 ? true : false;

        public static bool isEnemy(GameObject t) => t.tag == "Enemy";
        public static bool isPlayer(GameObject t) => t.tag == "Player";

        public static void hide(GameObject go) {
            go.SetActive(false);
        }

        public static void show(GameObject go) {
            go.SetActive(true);
        }

        public static GameObject create(string tag, GameObject parent) {
            GameObject go = null;
            if (!parent.transform.Find(tag)) {
                go = new GameObject(tag);
                go.transform.parent = parent.transform;
            }
            return go;
        }

        public static void parentTo(GameObject go, GameObject parent) {
            go.transform.parent = parent.transform;
        }

        public static Color colorByString(string str) {
            ColorUtility.TryParseHtmlString(str, out Color color);
            return color;
        }

        public static void setTileColor(Tilemap tilemap, Vector3Int cell, Color color) {
            tilemap.SetTileFlags(cell, TileFlags.None);
            tilemap.SetColor(cell, color);
        }

        public static Vector3Int getDirection(Vector3Int from, Vector3Int to) {
            Vector3 n = Vector3.Normalize(to - from);
            return new Vector3Int((int)n.x, (int)n.y, 0);
        }

        public static Vector3Int oppositeDirection(Vector3Int direction) {
            return direction * -1;
        }

        public static List<Vector2> neighbors(Vector2 position) {
            Maze maze = Maze.instance;

            Vector2Int[] directions = new Vector2Int[] {
                new Vector2Int(-1, 0),
                new Vector2Int(-1, 1),
                new Vector2Int(0, 1),
                new Vector2Int(1, 1),
                new Vector2Int(1, 0),
                new Vector2Int(1, -1),
                new Vector2Int(0, -1),
                new Vector2Int(-1, -1)
            };
            return directions.Select(d => position + d * maze.cellSize).ToList();
        }

        public static List<Vector3Int> directions() {
            return new List<Vector3Int>() {
                new Vector3Int(-1, 0, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(1, 0, 0),
                new Vector3Int(0, -1, 0),
            };
        }

        public static IEnumerator changeColorOverTime(SpriteRenderer sr, Color color, float duration = 1f) {
            float elapsed = 0f;
            float delta = Time.deltaTime / duration;
            while(elapsed < 1f) {
                sr.color = Color.Lerp(sr.color, color, delta);
                elapsed += delta;
                yield return null;
            }
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, System.Random rng) {
            T[] elements = source.ToArray();
            for (int i = elements.Length - 1; i >= 0; i--) {
                int swapIndex = rng.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }
    }

    public static class Log {        
        public static void vector2(Vector2 v, string text = "") {
            Debug.Log($"{text} x: {v.x}, y: {v.y}");
        }
    }

    public static class CoroutineExtension {
        static private readonly Dictionary<string, int> Runners = new Dictionary<string, int>();

        public static void parallel(this IEnumerator coroutine, MonoBehaviour parent, string groupName) {
            if (!Runners.ContainsKey(groupName)) Runners.Add(groupName, 0);

            Runners[groupName]++;
            parent.StartCoroutine(doParallel(coroutine, parent, groupName));
        }

        private static IEnumerator doParallel(IEnumerator coroutine, MonoBehaviour parent, string groupName) {
            yield return parent.StartCoroutine(coroutine);
            Runners[groupName]--;
        }

        public static bool inProcess(string groupName) {
            return (Runners.ContainsKey(groupName) && Runners[groupName] > 0);
        }
    }
}
