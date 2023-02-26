using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechScript : MonoBehaviour
{
	public TMPro.TMP_Text banter;
	private string textToWrite;
	private float timePerCharacter;
	private float timer;
	private int characterIndex;
	private bool invisibleCharacters;
	void Update()
    {
		if (banter != null)
		{
			timer -= Time.deltaTime;
			while (timer <= 0f)
			{
				//Display next character

				timer += timePerCharacter;
				characterIndex++;
				banter.text = textToWrite.Substring(0, characterIndex);

				if (characterIndex >= textToWrite.Length)
                {
					banter = null;
					return;
                }
			}
		}
	}

	public void AddWriter(TMPro.TMP_Text uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
	{
		this.banter = uiText;
		this.textToWrite = textToWrite;
		this.timePerCharacter = timePerCharacter;
		characterIndex = 0;
	}
}
