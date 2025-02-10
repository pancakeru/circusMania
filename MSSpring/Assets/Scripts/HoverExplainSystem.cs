using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverExplainSystem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public event EventHandler<OnHoverEventArgs> OnHover;

	public class OnHoverEventArgs : EventArgs
	{
		public GameObject hoverExplanation;
	}

	[SerializeField] private GameObject hoverExplanationPrefab;

	private GameObject hoverExplanation;

	private void Update()
	{
		if (enabled == false) {
			if (hoverExplanation != null) {
				Destroy(hoverExplanation.gameObject);
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		hoverExplanation = Instantiate(hoverExplanationPrefab, transform);
		OnHover?.Invoke(this, new OnHoverEventArgs { hoverExplanation = hoverExplanation });
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (hoverExplanation != null) {
			Destroy(hoverExplanation.gameObject);
		}
	}
}
