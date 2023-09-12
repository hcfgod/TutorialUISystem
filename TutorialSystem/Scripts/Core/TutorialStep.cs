using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep
{
	private List<ITutorialElement> elements;
	private float duration;

	public TutorialStep(List<ITutorialElement> elements, float duration)
	{
		this.elements = elements;
		this.duration = duration;
	}

	public void Display(GameObject parent)
	{
		foreach (var element in elements)
		{
			element.Display(parent);
		}
	}

	public float GetDuration()
	{
		return duration;
	}
}
