using System;
using UnityEngine;
using System.Collections;

using Utilsf;

public class SwipeController : MonoBehaviour {
    public event Action swipe;

    private float swipeDelay = .15f;
    private float elapsedSinceLastSwipe = 0f;

    private bool canSwipe = true;
    private MovableEntity[] entities;
    private FloatingJoystick joystick;

    #region private
    private void Start() {
        joystick = GameController.instance.ui.joystick;

        //WebGLInput.captureAllKeyboardInput = true;
    }

    private void Update() {
        //text.text = canSwipe.ToString();
//#if UNITY_EDITOR || UNITY_WEBGL

//#else
//        Vector2 direction = new Vector2(joystick.Horizontal, joystick.Vertical);
//#endif

        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (direction.x < -.5f) MoveEntities(Vector3Int.left);
        else if (direction.x > .5f) MoveEntities(Vector3Int.right);
        else if (direction.y < -.5f) MoveEntities(Vector3Int.down);
        else if (direction.y > .5f) MoveEntities(Vector3Int.up);

        elapsedSinceLastSwipe += Time.deltaTime;
        elapsedSinceLastSwipe = Mathf.Clamp(elapsedSinceLastSwipe, 0f, swipeDelay);
    }

    private void FindMovableEntities() {
        entities = FindObjectsOfType<MovableEntity>();
    }

    private void MoveEntities(Vector3Int direction) {
        if (canSwipe && elapsedSinceLastSwipe >= swipeDelay) {
            swipe?.Invoke();
            FindMovableEntities();
            StartCoroutine(MoveEntitiesCoroutine(direction));

            elapsedSinceLastSwipe = 0f;
        }
    }

    private IEnumerator MoveEntitiesCoroutine(Vector3Int direction) {
        canSwipe = false;

        foreach (MovableEntity entity in entities) {
            entity.Move(direction).parallel(this, "moving");
        }

        while (CoroutineExtension.inProcess("moving")) yield return null;

        canSwipe = true;
    }
    #endregion
}
