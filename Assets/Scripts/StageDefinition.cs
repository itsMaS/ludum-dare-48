using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage Definition", menuName = "Data/Stage Definition")]
public class StageDefinition : ScriptableObject
{
    [System.Serializable]
    public class Monologue
    {
        public string name;
        public float storyThreshold;
        public List<MonologueLine> Lines;
    }

    [System.Serializable]
    public class MonologueLine
    {
        public PlayerController.Emotions emotion = PlayerController.Emotions.Normal;
        public enum TextEffect { Normal, Shake };

        public float duration = 2;
        public string line = "";
        public TextEffect effect = TextEffect.Normal;
    }
    public GameObject StagePrefab;

    public List<Monologue> Monologues;

    public float postGameTime = 2;
    public ColorStyle colorStyle;
    public AudioClip music;
}
