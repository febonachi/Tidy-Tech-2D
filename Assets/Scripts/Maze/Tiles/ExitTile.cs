using System;
using UnityEngine;

using Utilsf;

[RequireComponent(typeof(Animator))]
public class ExitTile : CustomTile {
    #region editor
    [Header("Indicator")]
    [SerializeField] private Color stateON = Color.white;
    [SerializeField] private Color stateOFF = Color.white;
    [SerializeField] private SpriteRenderer indicator = default;
    #endregion

    private Animator animator;
    private bool closed => indicator.color == stateOFF;

    #region private
    private void Awake() {
        animator = GetComponent<Animator>();

        indicator.color = stateOFF;
    }
    #endregion

    #region public
    public void openDoors() {
        if (!closed) return;

        animator.SetTrigger("open");

        indicator.color = stateON;

        info.interuptNextAction = true;
    }

    public void closeDoors() {
        if (closed) return;

        animator.SetTrigger("close");

        indicator.color = stateOFF;

        info.interuptNextAction = false;
    }

    public override void interact(GameObject entity) {
        if (Utility.isPlayer(entity) && !closed) {
            info.inAction = true;

            entity.GetComponent<PlayerController>().exitAnimation();

            //yield return inActionDelay(.5f);
            //yield return interuptDelay(1f);
        }
    }
    #endregion
}
