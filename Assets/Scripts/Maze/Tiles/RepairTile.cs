using System;
using UnityEngine;
using System.Collections;

using Utilsf;

[RequireComponent(typeof(Animator))]
public class RepairTile : CustomTile {
    #region editor
    [SerializeField] private int maxRepairZone = 2;
    #endregion

    public event Action tileRepaired;
    public event Action tileBrokeDown;

    [HideInInspector] public bool repaired = false;

    private Animator animator;

    #region private
    private void Awake() {
        info.interuptNextAction = true;

        animator = GetComponent<Animator>();
    }

    private void highlightNeighbors(Vector2 currentPosition, bool state, int count = 0) {
        if (count >= maxRepairZone) return;
        foreach (Vector2 position in Utility.neighbors(currentPosition)) {
            FloorTile floorTile = maze.getTile<FloorTile>(position);
            if (floorTile != null) {
                Color color = state ? floorTile.highlightColor : floorTile.defaultColor;
                highlight(floorTile.spriteToHighlight, color);

                highlightNeighbors(position, state, count + 1);
            }
        }
    }

    private IEnumerator repairTile(PlayerController player) {
        info.inAction = true;

        animator.SetTrigger("repair");

        yield return player.repairAnimation();

        highlightNeighbors(position, true);

        repaired = true;
        tileRepaired?.Invoke();

        //inActionDelay(.5f);
        //interuptDelay(1f);
    }

    private void breakTile(Enemy enemy) {
        animator.SetTrigger("break");

        highlightNeighbors(position, false);

        enemy.die();

        repaired = false;
        tileBrokeDown?.Invoke();
        info.interuptNextAction = true;
    }
    #endregion

    #region protected

    #endregion

    #region public
    public override void interact(GameObject entity) {
        if (!repaired && Utility.isPlayer(entity)) {
            StartCoroutine(repairTile(entity.GetComponent<PlayerController>()));
        }else if (repaired && Utility.isEnemy(entity)) {
            breakTile(entity.GetComponent<Enemy>());
        }
    }
    #endregion
}
