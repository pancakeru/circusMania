using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorParticlesScript : MonoBehaviour
{
    public GameObject particlePrefab;        
    public Canvas uiCanvas;                 

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        if (particlePrefab == null)
        {
            Debug.LogError("Particle prefab not assigned!");
        }

        if (uiCanvas == null)
        {
            Debug.LogError("UI Canvas not assigned!");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateParticleAtCursor();
        }
    }

    private void CreateParticleAtCursor()
    {
        Vector3 mousePosition = Input.mousePosition;

        Vector2 uiPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvas.transform as RectTransform, mousePosition, uiCanvas.worldCamera, out uiPosition);

        GameObject particle = Instantiate(particlePrefab, uiCanvas.transform);
        RectTransform particleRect = particle.GetComponent<RectTransform>();

        if (particleRect != null)
        {
            particleRect.anchoredPosition = uiPosition;
            particleRect.localPosition = new Vector3(particleRect.localPosition.x, particleRect.localPosition.y, 0);

        }
        else
        {
            Debug.LogError("Particle prefab is missing a RectTransform component!");
        }
    }
}
