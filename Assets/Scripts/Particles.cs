using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    private void Update()
    {
        transform.position = PlayerController.Instance.transform.position;
    }
}
