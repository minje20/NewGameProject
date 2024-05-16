using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;


public struct LiquidCell
{
    public Vector2Int GridPosition { get; private set; }
    public Vector3 WorldPosition { get; private set; }
    public Vector2 CellSize { get; private set; }
    public Bounds Bounds { get; private set; }

    private int _presure;
    private int _sideFactor;

    public int Presure
    {
        get => _presure;
        set
        {
            if (value < 0) value = 0;
            //if (value > MAX_PRESURE) value = MAX_PRESURE;
            _presure = value;
        }
    }

    public bool IsBlock { get; set; }

    public const int MAX_PRESURE = 4;

    public LiquidCell(Vector2Int gridPosition, Vector3 worldPosition, Vector2 cellSize)
    {
        GridPosition = gridPosition;
        CellSize = cellSize;
        worldPosition.z = 0f;
        WorldPosition = worldPosition;

        _presure = 0;
        _sideFactor = 1;

        IsBlock = false;

        Bounds = new Bounds(worldPosition, cellSize);
    }

    public bool Intersects(Bounds bounds)
    {
        var center = Bounds.center;
        center.z = 0f;
        bounds.center = center;

        return Bounds.Intersects(bounds);
    }

    public bool Contains(Vector3 pos)
    {
        pos.z = 0f;
        return Bounds.Contains(pos);
    }

    public static bool IsValidPosition(LiquidCell[,] grid, Vector2Int pos)
    {
        Debug.Assert(grid != null);

        int w = grid.GetLength(0);
        int h = grid.GetLength(1);

        return
            pos.x < w &&
            pos.x > -1 &&
            pos.y < h &&
            pos.y > -1
            ;
    }

    public static bool IsBlockPosition(LiquidCell[,] grid, Vector2Int pos)
    {
        if (IsValidPosition(grid, pos)) return false;

        return grid[pos.x, pos.y].IsBlock;
    }

    public static void Merge(LiquidCell[,] targetGrid, Vector2Int pos, LiquidCell my, LiquidCell other)
    {
        my.Presure += other.Presure;

        targetGrid[pos.x, pos.y] = my;
    }

    public static LiquidCell GetCell(LiquidCell[,] targetGrid, Vector2Int pos)
    {
        return targetGrid[pos.x, pos.y];
    }

    public void SimulateGravity(LiquidCell[,] grid)
    {
        if (IsBlock) return;
        if (Presure == 0) return;

        var appliedGravityPos = GridPosition + Vector2Int.down;

        if (IsValidPosition(grid, appliedGravityPos))
        {
            // 중력 적용
            var appliedGravityCell = grid[appliedGravityPos.x, appliedGravityPos.y];
            if (appliedGravityCell.Presure < MAX_PRESURE && appliedGravityCell.IsBlock == false)
            {
                var cell = grid[appliedGravityPos.x, appliedGravityPos.y];
                cell.Presure++;
                Presure--;

                grid[appliedGravityPos.x, appliedGravityPos.y] = cell;
            }
        }
    }

    public void SimulateDivideSide(LiquidCell[,] grid)
    {
        if (IsBlock) return;
        if (Presure != 1) return;


        var appliedGravityPos = GridPosition + Vector2Int.down;
        if (IsValidPosition(grid, appliedGravityPos) == false) return;
        if (GetCell(grid, appliedGravityPos).IsBlock) return;

        // 사이드로 번지기 적용
        var appliedDividedPosLeft = GridPosition + Vector2Int.left;
        var appliedDividedPosRight = GridPosition + Vector2Int.right;

        int count = 0;
        count += ApplySideCell(grid, appliedDividedPosLeft) ? 1 : 0;
        count += ApplySideCell(grid, appliedDividedPosRight) ? 1 : 0;
    }

    private bool ApplySideCell(LiquidCell[,] grid, Vector2Int sidePos)
    {
        if (IsValidPosition(grid, sidePos) == false) return false;
        if (IsBlockPosition(grid, sidePos)) return false;

        var sideCell = grid[sidePos.x, sidePos.y];

        if (sideCell._sideFactor >= 5) return false;

        if (sideCell.Presure == 0)
        {
            sideCell.Presure = 1;
            
            sideCell._sideFactor = Mathf.Max(sideCell._sideFactor + 1, 5);
        }
        else
        {
            sideCell._sideFactor = 1;
        }
        grid[sidePos.x, sidePos.y] = sideCell;
        return true;
    }

    public void SimulatePresure(LiquidCell[,] grid)
    {
        if (IsBlock) return;
        if (Presure <= 1) return;

        var leftPos = GridPosition + Vector2Int.left;
        var rightPos = GridPosition + Vector2Int.right;
        var upPos = GridPosition + Vector2Int.up;


        if (IsValidPosition(grid, upPos) == false ||
            (IsValidPosition(grid, upPos) && GetCell(grid, upPos).IsBlock)
           )
        {
            return;
        }

        DividePresure(grid, GridPosition + Vector2Int.up);
    }

    private bool DividePresure(LiquidCell[,] grid, Vector2Int pos)
    {
        if (Presure <= 1) return false;

        if (IsValidPosition(grid, pos) == false) return false;
        var cell = grid[pos.x, pos.y];
        if (cell.IsBlock) return false;

        cell.Presure++;
        Presure--;
        grid[pos.x, pos.y] = cell;
        return true;
    }
}

public class CellularAutomationLiquidController : MonoBehaviour
{
    public float _simulationSpeed;
    public Vector2Int _gridSize;
    public Vector2 _cellSize;
    public Mesh _cellMesh;
    public Material[] _cellMaterials;
    public Material _blockMaterial;

    public bool EnableGridDebug;
    public bool LinearClickLiquidCreation;

    private LiquidCell[,] _grid;

    private List<Matrix4x4> blockCells;
    private List<List<Matrix4x4>> liquidCells;

    public void MakeArea()
    {
        _grid = new LiquidCell[_gridSize.x, _gridSize.y];

        for (int i = 0; i < _grid.GetLength(1); i++)
        {
            for (int j = 0; j < _grid.GetLength(0); j++)
            {
                var gridPos = new Vector2Int(j, i);
                _grid[j, i] = new LiquidCell(
                    gridPos,
                    GridToWorldPos(gridPos),
                    _cellSize
                );
            }
        }

        blockCells = new(_gridSize.x * _gridSize.y);
        liquidCells = new(_cellMaterials.Length);
        for (int i = 0; i < _cellMaterials.Length; i++)
        {
            liquidCells.Add(new List<Matrix4x4>(_gridSize.x * _gridSize.y));
        }
    }

    public Vector2Int WorldToGridPos(Vector3 worldPos, bool isInnerSystem = false)
    {
        if (isInnerSystem)
        {
            var pos = worldPos - transform.position;

            return new Vector2Int(
                (int)(pos.x / _cellSize.x),
                (int)(pos.y / _cellSize.y)
            );
        }

        for (int i = 0; i < _grid.GetLength(1); i++)
        {
            for (int j = 0; j < _grid.GetLength(0); j++)
            {
                if (_grid[j, i].Contains(worldPos)) return _grid[j, i].GridPosition;
            }
        }

        return Vector2Int.zero;
    }

    public Vector3 GridToWorldPos(Vector2Int gridPos)
    {
        var pos = new Vector3(
            gridPos.x * _cellSize.x,
            gridPos.y * _cellSize.y,
            0f
        );

        pos += transform.position;

        return pos;
    }

    public LiquidCell GetCell(Vector2Int gridPos)
        => _grid[gridPos.x, gridPos.y];

    public void SetCell(Vector2Int gridPos, LiquidCell cell)
        => _grid[gridPos.x, gridPos.y] = cell;

    private void OnTriggerCreation()
    {
        Func<int, bool> isTriggerLiquid =
            LinearClickLiquidCreation ? Input.GetMouseButton : Input.GetMouseButtonDown;

        if (isTriggerLiquid(1) == false && Input.GetMouseButton(0) == false) return;

        var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var gridPos = WorldToGridPos(worldPos);

        if (LiquidCell.IsValidPosition(_grid, gridPos) == false) return;

        var cell = GetCell(gridPos);

        if (Input.GetMouseButton(0))
        {
            cell.IsBlock = true;
            SetCell(gridPos, cell);
        }
        else if (isTriggerLiquid(1))
        {
            cell.Presure += 2;
            SetCell(gridPos, cell);
        }
    }


    private void RenderCell()
    {
        blockCells.Clear();

        foreach (var cells in liquidCells)
        {
            cells.Clear();
        }

        for (int i = 0; i < _grid.GetLength(1); i++)
        {
            for (int j = 0; j < _grid.GetLength(0); j++)
            {
                var cell = _grid[j, i];

                var m = Matrix4x4.TRS(
                    GridToWorldPos(new Vector2Int(j, i)),
                    Quaternion.identity,
                    _cellSize
                );

                if (cell.IsBlock)
                {
                    blockCells.Add(m);
                }
                else if (cell.Presure > 0)
                {
                    int mIndex = Mathf.Min(cell.Presure, liquidCells.Count);
                    mIndex = Mathf.Max(mIndex - 1, 0);
                    liquidCells[mIndex].Add(m);
                }
            }
        }


        Graphics.DrawMeshInstanced(_cellMesh, 0, _blockMaterial, blockCells.ToArray(), blockCells.Count);

        for (int i = 0; i < Mathf.Min(_cellMaterials.Length, liquidCells.Count); i++)
        {
            Graphics.DrawMeshInstanced(_cellMesh, 0, _cellMaterials[i], liquidCells[i].ToArray(), liquidCells[i].Count);
        }
    }

    private void Awake()
    {
        MakeArea();

        StartCoroutine(Simulate());
    }

    private IEnumerator Simulate()
    {
        while (true)
        {
            yield return new WaitForSeconds(_simulationSpeed);

            SimulateStep(0);
            SimulateStep(1);
            SimulateStep(2);
        }
    }

    private void SimulateStep(int callbackID)
    {
        for (int i = 0; i < _grid.GetLength(1); i++)
        {
            for (int j = 0; j < _grid.GetLength(0); j++)
            {
                var cell = _grid[j, i];

                switch (callbackID)
                {
                    case 0:
                        cell.SimulateGravity(_grid);
                        break;
                    case 1:
                        cell.SimulateDivideSide(_grid);
                        break;
                    case 2:
                        cell.SimulatePresure(_grid);
                        break;
                }

                _grid[j, i] = cell;
            }
        }
    }

    private void Update()
    {
        OnTriggerCreation();

        RenderCell();
    }

    private void OnDrawGizmos()
    {
        if (EnableGridDebug == false) return;

        for (int i = 0; i < _gridSize.y; i++)
        {
            for (int j = 0; j < _gridSize.x; j++)
            {
                Gizmos.DrawWireCube(GridToWorldPos(new Vector2Int(j, i)), _cellSize);
            }
        }
    }
}