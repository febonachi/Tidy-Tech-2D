using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ObstacleTile : CustomTile {
    #region editor
    [SerializeField] private ParticleSystem obstaclePs = default;
    #endregion

    private Animator animator;

    #region private
    private void Awake() {
        animator = GetComponent<Animator>();

        obstaclePs.Stop();
    }
    #endregion

    #region public
    public override void interact(GameObject entity = null) {
        obstaclePs.Play();

        animator.SetTrigger("scale");
    }
    #endregion
}
