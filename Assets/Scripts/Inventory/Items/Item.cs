using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public new string name;
    [TextArea]
    public string description;
    public Sprite display;
}
