using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class UIController : MonoBehaviour {
    public enum UIStyle { UINext, UIRestart };

    #region editor
    public UIEnemyPanel enemyPanel = default;
    public FloatingJoystick joystick = default;
    [SerializeField] private UIMessage message = default;
    [SerializeField] private Button restartButton = default;
    [SerializeField] private Button nextLevelButton = default;
    [SerializeField] private RectTransform swipeHint = default;
    [SerializeField] private RectTransform indicatorsPanel = default;
    #endregion

    [HideInInspector] public int indicatorsCount = 0;

    private Animator animator;
    private List<UIIndicator> indicators;
    private bool swipeHintShowed = false;

    #region private
    private void Awake() {
        animator = GetComponent<Animator>();       

        indicators = indicatorsPanel.GetComponentsInChildren<UIIndicator>().ToList();
    }
    #endregion

    #region public
    public void highlightIndicators(int count = 0) {
        for(int i = 0; i < indicatorsCount; i++) indicators.ElementAt(i).highlight(i < count);
        for(int i = indicatorsCount; i < indicators.Count; i++) indicators.ElementAt(i).setDefault();
    }

    public void addRestartButtonListener(UnityAction action) {
        restartButton.onClick.AddListener(action);
    }

    public void addNextLevelButtonListener(UnityAction action) {
        nextLevelButton.onClick.AddListener(action);
    }

    public void reset(int sceneIndex) {
        //if (sceneIndex == 1) showSwipeHint();
        //else hideSwipeHint();
        enemyPanel.reset();
        highlightIndicators();
    }

    public void show(UIStyle style) {
        switch (style) {
            case UIStyle.UINext: {
                nextLevelButton.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(false);
                break;
            }
            case UIStyle.UIRestart: {
                restartButton.gameObject.SetActive(true);
                nextLevelButton.gameObject.SetActive(false);
                break;
            }
            default: break;
        }

        animator.SetTrigger("show");
    }

    public void hide() {
        restartButton.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);
        animator.SetTrigger("hide");
    }

    public void showSwipeHint() {
        swipeHintShowed = true;
        animator.SetBool("swipeHint", swipeHintShowed);
    }

    public void hideSwipeHint() {
        if (swipeHint.gameObject.activeSelf) {
            swipeHintShowed = false;
            animator.SetBool("swipeHint", swipeHintShowed);
        }
    }

    public void showMessage(string str, Color color) {
        if (!swipeHintShowed) {
            UIMessage msg = Instantiate(message, transform);
            msg.transform.SetParent(transform, false);            
            msg.message(str, color);
        }
    }
    #endregion
}
