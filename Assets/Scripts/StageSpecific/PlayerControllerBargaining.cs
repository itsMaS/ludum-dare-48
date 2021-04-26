using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBargaining : PlayerController
{
    [SerializeField] float orbRange;
    [SerializeField] float rangeIncreasePerOrb = 0.5f;

    List<Transform> Orbs = new List<Transform>();
    public void AddOrb(Transform tr)
    {
        AudioManager.PlaySound("Join", 0.5f, 0.2f);
        Orbs.Add(tr);
        UpdateOrbs();
    }

    void UpdateOrbs()
    {
        for (int i = 0; i < Orbs.Count; i++)
        {
            Vector2 position = (orbRange + rangeIncreasePerOrb*Orbs.Count) * new Vector2
                (Mathf.Cos(Mathf.PI * 2 / Orbs.Count * i), Mathf.Sin(Mathf.PI * 2 / Orbs.Count * i));
            Orbs[i].DOLocalMove(position, 0.5f);
        }
    }

    private void OnEnable()
    {
        StoryManager.Instance.onUpdateProgression.AddListener(UpdateProgression);
    }
    private void OnDisable()
    {
        StoryManager.Instance.onUpdateProgression.RemoveListener(UpdateProgression);
    }

    bool dispersed = false;
    void UpdateProgression(float progress)
    {
        if(progress > 2f && !dispersed) 
        {
            StartCoroutine(DisperseOrbs(new Vector2(2,5)));
        }
    }
    IEnumerator DisperseOrbs(Vector2 range)
    {
        dispersed = true;
        for (int i = 0; i < Orbs.Count; i++)
        {
            Orbs[i].DOLocalMoveX(Orbs[i].localPosition.x > 0 ? Random.Range(range.x, range.y) : Random.Range(-range.y, -range.x), 2);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(5);
        StartCoroutine(DisperseOrbs(new Vector2(30,50)));
    }
}
