using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [HideInInspector] public Camera cam;
    [SerializeField] CinemachineVirtualCamera GameCamera;
    [SerializeField] CinemachineVirtualCamera TransitionCamera;


    private void Awake()
    {
        Instance = this;
        cam = GetComponentInChildren<Camera>();
    }

    public static Vector2 CursorWorldPosition 
    { 
        get
        {
            return Instance.cam.ScreenToWorldPoint(new Vector2(Mathf.Clamp(Input.mousePosition.x,0 ,Screen.width),Mathf.Clamp(Input.mousePosition.y, 0, Screen.height)));
        } 
    }

    public void SetCamera(bool game)
    {
        GameCamera.gameObject.SetActive(game);
        TransitionCamera.gameObject.SetActive(!game);
    }
}
