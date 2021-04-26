using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Transform player => PlayerController.Instance.transform;
    protected float difficulty => StoryManager.Instance.Difficulty;
}
