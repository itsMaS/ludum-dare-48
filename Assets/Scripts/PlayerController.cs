using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Emotions { Normal = 0, Happy = 1, Sad = 2, Angry = 3 }

    public Vector2 MovementVector => CameraController.CursorWorldPosition - (Vector2)transform.position;

    [Header("Dependancies")]
    [SerializeField] private Transform EyesTr;
    [SerializeField] private Animator EyesAn;

    [Header("Visual")]
    [SerializeField] private float EyesMaxDeviation = 0.1f;
    [SerializeField] private Vector2 EyesDistribution = new Vector2(0,2);


    [SerializeField] private float baseReaction = 10;
    public float speed = 20;

    public float DownwardsPull = 0;

    private Vector2 velocity;

    public static PlayerController Instance;
    private void Awake()
    {
        Instance = this;
    }
    protected virtual void Update()
    {
        Vector2 target = CameraController.CursorWorldPosition;
        if (StoryManager.Instance.transition) target.y = transform.position.y;

        transform.position = Vector2.SmoothDamp(transform.position,
            target, ref velocity,1/baseReaction, speed) + Vector2.down * DownwardsPull * Time.deltaTime;

        EyesControl();
    }
    private void EyesControl()
    {
        EyesTr.localPosition = MovementVector.normalized * EyesMaxDeviation * 
            Mathf.InverseLerp(EyesDistribution.x, EyesDistribution.y, MovementVector.magnitude);
    }

    protected virtual void OnDrawGizmos()
    {
        if(Application.isPlaying)
            Gizmos.DrawWireSphere(CameraController.CursorWorldPosition, 0.5f);
    }

    public void SetEmotion(Emotions emotion)
    {
        EyesAn.SetInteger("Emotion", (int)emotion);
    }

    public virtual bool CanSpawn()
    {
        return true;
    }
    public void CameraShake()
    {
        GetComponent<Cinemachine.CinemachineImpulseSource>().GenerateImpulse();
    }
}
