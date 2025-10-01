using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all grid logic: generation, interaction, collection, and refilling.
/// </summary>
public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [Header("Grid Settings")]
    [SerializeField] private int width = 6;
    [SerializeField] private int height = 5;
    [SerializeField] private float cellSize = 1.1f;
    [SerializeField] private Vector3 gridOriginOffset;

    [Header("Block Assets")]
    [SerializeField] private Block blockPrefab;
    [SerializeField] private List<BlockData> blockTypes;

    private Block[,] grid;
    private bool isProcessingMove = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStarted += GenerateGrid;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStarted -= GenerateGrid;
    }

    /// <summary>
    /// Generates a new grid, clearing the previous one if it exists.
    /// </summary>
    public void GenerateGrid()
    {
        isProcessingMove = false;
        grid = new Block[width, height];

        // Clears blocks from a previous game
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                CreateBlock(x, y);
            }
        }
    }

    public void HandleBlockClick(Block block)
    {
        if (isProcessingMove) return; // Ignore clicks while a move is being processed.
        StartCoroutine(ProcessMoveCoroutine(block));
    }

    private IEnumerator ProcessMoveCoroutine(Block clickedBlock)
    {
        isProcessingMove = true;

        List<Block> connectedBlocks = FindConnectedBlocks(clickedBlock);

        if (connectedBlocks.Count > 1) // Only process if there are at least 2 connected blocks.
        {
            // Notify the GameManager about the turn.
            GameManager.Instance.MakeMove(connectedBlocks.Count);

            // Remove the collected blocks.
            foreach (var block in connectedBlocks)
            {
                grid[block.X, block.Y] = null;
                Destroy(block.gameObject);
            }

            yield return new WaitForSeconds(1f);

            // Apply gravity and fill the spaces.
            CollapseColumns();
            yield return new WaitForSeconds(0.2f); 
            RefillGrid();
        }

        isProcessingMove = false;
    }

    private List<Block> FindConnectedBlocks(Block startBlock)
    {
        var connectedBlocks = new List<Block>();
        var queue = new Queue<Block>();
        var visited = new HashSet<Block>();

        queue.Enqueue(startBlock);
        visited.Add(startBlock);

        while (queue.Count > 0)
        {
            Block current = queue.Dequeue();
            connectedBlocks.Add(current);

            // Check the 4 neighbors (up, down, left, right)
            CheckNeighbor(current.X + 1, current.Y, startBlock.Type, queue, visited);
            CheckNeighbor(current.X - 1, current.Y, startBlock.Type, queue, visited);
            CheckNeighbor(current.X, current.Y + 1, startBlock.Type, queue, visited);
            CheckNeighbor(current.X, current.Y - 1, startBlock.Type, queue, visited);
        }
        return connectedBlocks;
    }

    private void CheckNeighbor(int x, int y, BlockData.BlockType type, Queue<Block> queue, HashSet<Block> visited)
    {
        // Checks if the coordinate is valid.
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            Block neighbor = grid[x, y];
            // Checks if the neighbor exists, is of the same type, and has not been visited.
            if (neighbor != null && neighbor.Type == type && !visited.Contains(neighbor))
            {
                visited.Add(neighbor);
                queue.Enqueue(neighbor);
            }
        }
    }

    private void CollapseColumns()
    {
        for (int x = 0; x < width; x++)
        {
            int emptyCellCount = 0;
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                {
                    emptyCellCount++;
                }
                else if (emptyCellCount > 0)
                {
                    Block blockToMove = grid[x, y];
                    grid[x, y - emptyCellCount] = blockToMove;
                    grid[x, y] = null;
                    blockToMove.UpdatePosition(x, y - emptyCellCount);
                }
            }
        }
    }

    private void RefillGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                {
                    CreateBlock(x, y);
                }
            }
        }
    }

    private void CreateBlock(int x, int y)
    {
        Vector3 position = new Vector3(x * cellSize, y * cellSize, 0) + gridOriginOffset;
        Block newBlock = Instantiate(blockPrefab, position, Quaternion.identity, transform);
        BlockData randomType = blockTypes[Random.Range(0, blockTypes.Count)];
        newBlock.Init(x, y, randomType);
        grid[x, y] = newBlock;
    }

    // Helper for a possible ScriptableObject or struct that defines block types.
    [System.Serializable]
    public struct BlockData
    {
        public enum BlockType { Red, Green, Blue, Yellow, Purple }
        public BlockType Type;
        public Sprite Sprite;
    }
}