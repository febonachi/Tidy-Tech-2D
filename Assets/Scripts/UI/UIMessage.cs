using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIMessage : MonoBehaviour {
    private Animator animator;
    private TextMeshProUGUI textMesh;

    private void Awake() {
        animator = GetComponent<Animator>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void message(string str, Color color) {
        textMesh.text = str;
        textMesh.color = color;

        Destroy(gameObject, 2f);
    }
}
