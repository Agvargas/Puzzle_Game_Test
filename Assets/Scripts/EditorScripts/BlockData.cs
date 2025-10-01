using UnityEngine;

/// <summary>
/// ScriptableObject that contains the shared data for a specific block type.
/// </summary>
[CreateAssetMenu(fileName = "NewBlockData", menuName = "Puzzle Game/Block Data")]
public class BlockData : ScriptableObject
{
    // We use an enum to have strict control over the block types.
    public enum BlockType
    {
        Red,
        Green,
        Blue,
        Yellow,
        Purple
    }

    [Tooltip("The type of this block (e.g. Red, Green).")]
    public BlockType type;

    [Tooltip("The sprite that will be shown for this block type.")]
    public Sprite sprite;
}