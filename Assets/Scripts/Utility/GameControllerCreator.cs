using UnityEngine;

public class GameControllerCreator : MonoBehaviour {
    #region editor
    [SerializeField] private GameController controller = default;
    #endregion

    #region private
    private void Awake() {
        if(FindObjectOfType<GameController>() == null) {
            Instantiate(controller);
        }
    }
    #endregion
}
