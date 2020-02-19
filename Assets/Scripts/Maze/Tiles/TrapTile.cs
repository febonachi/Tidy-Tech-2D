using System;
using UnityEngine;

using Utilsf;

public class TrapTile : CustomTile {
    #region private
    private void Awake() {
        info.interuptNextAction = true;
    }
    #endregion

    #region public
    public override void interact(GameObject entity) {
        if (Utility.isPlayer(entity)) {
            info.inAction = true;

            entity.GetComponent<PlayerController>().destroyAnimation();

            //inActionDelay(.5f);
        }
    }
    #endregion
}
