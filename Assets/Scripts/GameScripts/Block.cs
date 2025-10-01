using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Represents a single block in the grid.
/// Contains its data (position, type) and detects player interaction.
/// </summary>
public class Block : MonoBehaviour, IPointerDownHandler
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public GridManager.BlockData.BlockType Type { get; private set; }

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(int x, int y, GridManager.BlockData data)
    {
        X = x;
        Y = y;
        Type = data.Type;
        spriteRenderer.sprite = data.Sprite;
        gameObject.name = $"Block ({x},{y}) - {Type}";
    }

    public void UpdatePosition(int newX, int newY)
    {
        X = newX;
        Y = newY;
        float cellSize = 1.1f;
        transform.position = new Vector3(newX * cellSize, newY * cellSize, 0); // Assumes the same position calculation.
    }

    /// <summary>
    /// Executes when the player clicks on this block.
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        GridManager.Instance.HandleBlockClick(this);
    }
}