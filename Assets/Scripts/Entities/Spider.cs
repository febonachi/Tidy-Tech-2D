using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Utilsf;

[RequireComponent(typeof(Animator))]
public class Spider : Enemy {
    #region editor
    [SerializeField] private float speed = 2f;
    [SerializeField] private float delay = 1f;
    [SerializeField] private float cleverChance = .2f;
    [SerializeField] private SpriteRenderer blob = default;
    [SerializeField] private ParticleSystem dieEffect = default;
    #endregion

    private Maze maze;
    private System.Random rnd;
    private Animator animator;
    private RepairTile target;

    #region private
    private void Awake() {
        animator = GetComponent<Animator>();

        speed = Random.Range(speed / 2f, speed);

        blob.transform.Rotate(Vector3.forward * Random.Range(0, 360));
    }

    private void Start() {
        maze = Maze.instance;

        rnd = new System.Random();

        StartCoroutine(moveToTarget());
    }

    private bool checkoutRepairTile() {
        RepairTile repairTile = maze.getTile<RepairTile>(transform.position);
        if (repairTile != null) {
            if (repairTile.repaired) {
                repairTile.interact(gameObject);
                return true;
            }
        }
        return false;
    }

    private RepairTile changeTarget() {
        return maze.getTiles<RepairTile>()
            .Where(tile => Vector3.Distance(transform.position, tile.position) > maze.cellSizeFloat)
            .Shuffle(new System.Random()).First();
    }

    private IEnumerator moveTowards(Vector3 position) {
        while (transform.position != position) {
            animator.SetFloat("speed", speed);
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
            yield return null;
        }
        animator.SetFloat("speed", 0f);
    }

    private IEnumerator moveToTarget() {
        IEnumerable<Vector3> nextPositions = Utility.directions()
            .Select(direction => transform.position + direction)
            .Where(position => {
                CustomTile tile = maze.getCustomTile(position);
                return !(tile is ObstacleTile || tile is TrapTile);
            }).Shuffle(rnd);

        Vector3 nextRepairTilePosition = nextPositions.Where(position => {
            RepairTile tile = maze.getTile<RepairTile>(position);
            return (tile != null && tile.repaired);
        }).FirstOrDefault();

        nextRepairTilePosition = nextRepairTilePosition != Vector3.zero ? nextRepairTilePosition : nextPositions.First();

        yield return StartCoroutine(moveTowards(nextRepairTilePosition));

        if (checkoutRepairTile()) yield break;

        yield return new WaitForSeconds(Random.Range(0f, delay));

        StartCoroutine(moveToTarget());
    }
    #endregion

    #region public
    public override void die() {
        StopAllCoroutines();

        animator.SetTrigger("die");

        Instantiate(dieEffect, transform);

        GetComponent<CircleCollider2D>().enabled = false;

        Destroy(gameObject, 1.5f);
    }
    #endregion
}
