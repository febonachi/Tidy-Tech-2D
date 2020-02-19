using UnityEngine;

using static UnityEngine.Random;

public class FloorTile : CustomTile {
    #region editor
    [Header("Highlight")]
    public Color defaultColor = Color.white;
    public Color highlightColor = Color.white;
    public SpriteRenderer spriteToHighlight = default;
    #endregion

    public bool Highlighted => spriteToHighlight.color == highlightColor;

    #region private
    new private void Start() {
        base.Start();

        transform.Rotate(Vector3.forward, Range(0, 3) * 90f);

        highlight(spriteToHighlight, defaultColor, coroutine: false);
    }
    #endregion

    #region protected

    #endregion

    #region public
    public override void interact(GameObject entity = null) {
        highlight(spriteToHighlight, highlightColor);
    }
    #endregion
}
