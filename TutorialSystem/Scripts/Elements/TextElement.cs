using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextElement : ITutorialElement
{
	private string text;
	private Color color;
	private int fontSize;
	private Vector2 position;

	public TextElement(string text, Color color, int fontSize, Vector2 position)
	{
		this.text = text;
		this.color = color;
		this.fontSize = fontSize;
		this.position = position;
	}

	public void Display(GameObject parent)
	{
		GameObject textObject = new GameObject("TutorialText");
		textObject.transform.SetParent(parent.transform, false);
        
		TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
		textComponent.text = text;
		textComponent.color = color;
		textComponent.fontSize = fontSize;
        
		RectTransform rectTransform = textObject.GetComponent<RectTransform>();
		rectTransform.anchoredPosition = position;
        
		textComponent.font = Resources.Load<TMP_FontAsset>("LiberationSans SDF");
	}
}
