using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yarn : MonoBehaviour
{
    public AudioClip soundClip; // The sound clip to play
    public SpriteRenderer imageRenderer; // The sprite renderer component to show the image
    public Sprite imageSprite; // The sprite to show when the mouse touches the collider

    private void OnMouseDown()
    {
        // Play the sound clip
        AudioSource.PlayClipAtPoint(soundClip, transform.position);

        // Show the image sprite
        imageRenderer.sprite = imageSprite;
    }

    private void OnMouseUp()
    {
        // Hide the image sprite
        imageRenderer.sprite = null;
    }
}
