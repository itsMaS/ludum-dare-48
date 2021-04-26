using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways, DefaultExecutionOrder(-5)]
public abstract class ColorTarget<T> : MonoBehaviour
{
    [System.Serializable]
    public class ColorProperties
    {
        [ColorUsageAttribute(true, true)]
        public Color tint = Color.white;
        public ColorManager.BaseColors colorType;
    }

    protected T target;
    public abstract void SetColors(Color[] color);
    public List<ColorProperties> Properties = new List<ColorProperties>();

    private void Update()
    {
        List<Color> Colors = new List<Color>();
        Properties.ForEach(prop => Colors.Add(ColorManager.GetColor(prop.colorType)*prop.tint));

        SetColors(Colors.ToArray());
    }
    protected virtual int ColorCount() => 1;
    private void GetTarget()
    {
        target = GetComponent<T>();
    }
    private void OnValidate()
    {
        while (Properties.Count < ColorCount()) Properties.Add(new ColorProperties());
        while (Properties.Count > ColorCount()) Properties.RemoveAt(Properties.Count-1);
        GetTarget();
    }
    private void Awake()
    {
        GetTarget();
        Update();
    }
}
