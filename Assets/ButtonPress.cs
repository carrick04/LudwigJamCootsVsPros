using System.Collections;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public Sprite butUp;
    public Sprite butDown;

    private SpriteRenderer sr;
    private bool isButUp = true;
    private bool canChangeSprite = true;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = butUp;
        StartCoroutine(DisableSpriteChangeAfterDelay(10f));
    }

    private IEnumerator DisableSpriteChangeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canChangeSprite = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && canChangeSprite)
        {
            sr.sprite = butDown;
            isButUp = false;
        }
        else if (Input.GetKeyUp(KeyCode.X) && canChangeSprite)
        {
            sr.sprite = butUp;
            isButUp = true;
        }
    }
}