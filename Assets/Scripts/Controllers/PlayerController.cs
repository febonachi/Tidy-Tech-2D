using System;
using UnityEngine;
using System.Collections;

using Utilsf;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MovableEntity))]
public class PlayerController : MonoBehaviour {
    #region editor
    [SerializeField] private GameObject hint = default;
    #endregion

    public event Action moved;
    public event Action exited;
    public event Action trapped;
    public event Action repairing;
    public event Action enemyKilled;
    public event Action obstacleEntered;

    private Animator animator;
    private bool inRepairAnimation = false;

    #region private
    private void Awake() {
        animator = GetComponent<Animator>();
        animator.logWarnings = false;

        MovableEntity entity = GetComponent<MovableEntity>();
        entity.moved += onMoved;
        entity.obstacleEntered += onObstacleEntered;

        showHint();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (Utility.isEnemy(collision.gameObject)){
            collision.gameObject.GetComponent<Enemy>().die();
            enemyKilled?.Invoke();
        }
    }

    private void onMoved() => moved?.Invoke();

    private void onObstacleEntered() => obstacleEntered?.Invoke();
    #endregion

    #region public
    public void exitAnimation() {
        animator.SetTrigger("exit");

        exited?.Invoke();
    }

    public void destroyAnimation() {
        animator.SetTrigger("destroy");

        trapped?.Invoke();
    }

    public IEnumerator repairAnimation() {
        repairing?.Invoke();
        inRepairAnimation = true;
        animator.SetBool("repair", inRepairAnimation);
        //while (inRepairAnimation) yield return null;
        yield return null;
    }

    public void stopRepairAnimation() {
        inRepairAnimation = false;
        animator.SetBool("repair", inRepairAnimation);
    }

    public void showHint() {
        hint.SetActive(true);
    }

    public void hideHint() {
        hint.SetActive(false);
    }
    #endregion
}
