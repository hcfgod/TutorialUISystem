using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	private static TutorialManager _instance;
	public static TutorialManager Instance => _instance;

	[SerializeField] private Canvas tutorialUICanvas;
	
	[SerializeField] private float fadeInDuration;
	[SerializeField] private float fadeOutDuration;
	
	private Queue<TutorialStep> tutorialSteps = new Queue<TutorialStep>();

	private CanvasGroup canvasGroup;

	public Sprite someSprite;
	
	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
		}
		
		if(!SearchForCanvasGroup())
		{
			Debug.LogWarning("No Canvas Group Found.");
		}
	}
	
	private bool SearchForCanvasGroup()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		
		if(canvasGroup == null)
		{
			canvasGroup = GetComponentInChildren<CanvasGroup>();
			
			if(canvasGroup == null)
			{
				canvasGroup = GetComponentInParent<CanvasGroup>();
				
				if(canvasGroup == null)
					return false;
			}
		}
		
		return true;
	}
	
	private void Start()
	{
		// Create a text element
		ITutorialElement textElement = new TextElement("This is a text tutorial.", Color.white, 16, new Vector2(20, 0));
		// Create an image element
		ITutorialElement imageElement = new ImageElement(someSprite, new Vector2(50, 50), new Vector2(0, 80));
		// Create a tutorial step using composition
		TutorialStep step1 = new TutorialStep(new List<ITutorialElement> { textElement, imageElement }, 10);
		// Enqueue the tutorial step
		TutorialManager.Instance.EnqueueStep(step1);
		// Display the next tutorial step
		TutorialManager.Instance.DisplayNextStep(2);
	}

	public void EnqueueStep(TutorialStep step)
	{
		tutorialSteps.Enqueue(step);
	}

	public void DisplayNextStep(float delay = 0)
	{
		if (tutorialSteps.Count > 0)
		{
			TutorialStep step = tutorialSteps.Dequeue();
			step.Display(canvasGroup.gameObject);

			// Start coroutines to handle the fade-in and fade-out
			StartCoroutine(DelayBeforeFadeIn(delay));
			StartCoroutine(RemoveTutorialAfterDuration(step.GetDuration(), canvasGroup.gameObject));
		}
	}

	private IEnumerator DelayBeforeFadeIn(float delay)
	{
		yield return new WaitForSeconds(delay);
		StartCoroutine(FadeIn());
	}
	
	private IEnumerator FadeIn()
	{
		float currentTime = 0.0f;

		while (currentTime < fadeInDuration)
		{
			float alpha = Mathf.Lerp(0, 1, currentTime / fadeInDuration);
			canvasGroup.alpha = alpha;
			currentTime += Time.deltaTime;
			yield return null;
		}

		canvasGroup.alpha = 1;
	}

	private IEnumerator FadeOut()
	{
		float currentTime = 0.0f;

		while (currentTime < fadeOutDuration)
		{
			float alpha = Mathf.Lerp(1, 0, currentTime / fadeOutDuration);
			canvasGroup.alpha = alpha;
			currentTime += Time.deltaTime;
			yield return null;
		}

		canvasGroup.alpha = 0;
	}

	private IEnumerator RemoveTutorialAfterDuration(float duration, GameObject parent)
	{
		yield return new WaitForSeconds(duration);

		// Start the fade-out coroutine
		StartCoroutine(FadeOut());

		// Wait for fade-out to complete before removing elements
		yield return new WaitForSeconds(1.0f);

		// Remove all child elements from the parent GameObject
		foreach (Transform child in parent.transform)
		{
			Destroy(child.gameObject);
		}
	}
}
