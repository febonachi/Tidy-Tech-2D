using TMPro;
using UnityEngine;

public class UIEnemyPanel : MonoBehaviour {
    #region editor
    [SerializeField] private TextMeshProUGUI text = default;
    #endregion

    private int enemyKilledCount = 0;

    #region private
    private void displayText() {
        text.text = enemyKilledCount.ToString();
    }
    #endregion

    #region public
    public void reset() {
        enemyKilledCount = 0;

        displayText();
    }

    public void enemyKilled() {
        enemyKilledCount++;

        displayText();
    }
    #endregion
}
