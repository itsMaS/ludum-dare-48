using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    [Header("Enemies")]
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private AnimationCurve SpawnIntervalPerDifficulty;
    [SerializeField] private float spawnRange;

    [SerializeField] Transform canvas;

    IEnumerator Start()
    {
        while (!StoryManager.Instance.transition)
        {
            if(PlayerController.Instance.CanSpawn())
            {
                yield return new WaitForSeconds(SpawnIntervalPerDifficulty.Evaluate(StoryManager.Instance.Difficulty));

                Instantiate(EnemyPrefab, (Vector2)PlayerController.Instance.transform.position +    Random.insideUnitCircle.normalized * spawnRange, Quaternion.identity, canvas);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}
