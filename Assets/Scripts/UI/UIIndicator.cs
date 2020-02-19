using UnityEngine;
using UnityEngine.UI;

public class UIIndicator : MonoBehaviour {
    #region editor
    [SerializeField] private Image lamp = default;
    [SerializeField] private Color onColor = Color.white;
    [SerializeField] private Color offColor = Color.white;
    [SerializeField] private Color defaultColor = Color.white;
    #endregion

    public bool higlighted => lamp.color == onColor;

    #region private
    private void Awake() {
        setDefault();
    }
    #endregion

    #region public
    public void highlight(bool state) {
        lamp.color = state ? onColor : offColor;
    }

    public void setDefault() {
        lamp.color = defaultColor;
    }
    #endregion
}
