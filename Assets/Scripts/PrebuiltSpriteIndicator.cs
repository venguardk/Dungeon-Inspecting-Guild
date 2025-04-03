using UnityEngine;

public class PrebuiltSpriteIndicator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // This script is attached to the prebuilt asset prefabs to display the prebuilt indicator

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        // Display the prebuilt indicator if the game is in level editor mode; Otherwise, hide the prebuilt indicator
        if (GameManager.instance.IsLevelEditorMode())
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}
