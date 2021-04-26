using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBargaining : Enemy
{
    public float tameAdvance = 0.125f;

    [SerializeField] float speed = 20;
    [SerializeField] float scrollSpeed = 1;
    [SerializeField] float tameDecrease = 0.5f;

    [SerializeField] float tameTimeRequired = 5;

    [SerializeField] float despawnRange = 10;

    float tameTime = 0;

    [SerializeField] Image TameProgressImage;

    private static int enemyCount = 0;

    private float randomSeed;
    private void Start()
    {
        randomSeed = Random.value;
        enemyCount++;

        StartCoroutine(CheckDespawn());
    }

    private void Update()
    {
        float x = Mathf.PerlinNoise(Time.time * scrollSpeed, Time.time * scrollSpeed + randomSeed * 100);
        float y = Mathf.PerlinNoise(Time.time * scrollSpeed + randomSeed * 100, Time.time * scrollSpeed);

        Vector2 direction = new Vector2(Mathf.Lerp(-1, 1, x), Mathf.Lerp(-1, 1, y));
        transform.Translate(direction.normalized * speed * Time.deltaTime);

        tameTime = Mathf.Max(0, tameTime - Time.deltaTime * tameDecrease);

    }

    IEnumerator CheckDespawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            if(Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > despawnRange)
            {
                enemyCount--;
                Destroy(gameObject);
                yield break;
            }
        }
    }
    private void LateUpdate()
    {
        UpdateProgress();
    }

    float velocity;
    private void UpdateProgress()
    {
        float lerp = Mathf.InverseLerp(0, tameTimeRequired, tameTime);
        TameProgressImage.fillAmount = Mathf.SmoothDamp(TameProgressImage.fillAmount, lerp,ref velocity, 0.01f);
        if(lerp < 0.2f)
        {
            Color col = TameProgressImage.color;
            col.a = Mathf.InverseLerp(0, 0.2f, lerp);
            TameProgressImage.color = col;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TameProgressImage.GetComponent<ColorTargetImage>().enabled = false;

        Debug.Log($"INSIDE TRIGGER");
        tameTime += Time.deltaTime * (tameDecrease + 1);



        if( tameTime >= tameTimeRequired)
        {
            Tamed();
            StoryManager.Instance.AdvanceStage(tameAdvance);
        }
    }

    void Tamed()
    {
        enemyCount--;
        GetComponent<Collider2D>().enabled = false;
        TameProgressImage.DOFade(0, 0.2f);
        transform.parent = PlayerController.Instance.transform;
        enabled = false;
        ((PlayerControllerBargaining)PlayerController.Instance).AddOrb(transform);
    }
}
