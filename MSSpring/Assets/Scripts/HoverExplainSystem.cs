using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverExplainSystem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private bool isHovering = false;

	[SerializeField] private GameObject hoverExplanation;//IMPORTANT: Keep the HoverExplanation a child gameobject under the gameobject this script is attached to!

	private void Awake()
	{
		UpdateHoverExplanationDisplayState();
	}

	private void Update()
	{
		if (enabled == false) {
			isHovering = false;
			UpdateHoverExplanationDisplayState();
		}
	}

	private void UpdateHoverExplanationDisplayState()
	{
		if (isHovering) {
			hoverExplanation.SetActive(true);
		} else {
			hoverExplanation.SetActive(false);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		isHovering = true;
		UpdateHoverExplanationDisplayState();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isHovering = false;
		UpdateHoverExplanationDisplayState();
	}
}
