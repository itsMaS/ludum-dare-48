using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CursorVisual : MonoBehaviour
{
    CanvasGroup cursorCG;
    private void Awake()
    {
        cursorCG = GetComponent<CanvasGroup>();
    }
    [SerializeField] Vector2 bounds;

    private void Update()
    {
        transform.position = CameraController.CursorWorldPosition;
        cursorCG.alpha = Mathf.InverseLerp(bounds.x, bounds.y, Vector2.Distance(transform.position, PlayerController.Instance.transform.position));
    }
}
