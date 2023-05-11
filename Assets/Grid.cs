using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    public GameObject gridTile, piece;
    public XAxis gridClass;
    public XAxis newPiece;
    public int xCount, yCount;
    public Vector2 changeInPosition;

    public float level;

    public InputSystem inputSystem;
    public InputSystem.IPlayerActionMapActions playerAction;
    private void Awake()
    {
        inputSystem = new();

    }
    private void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.PlayerActionMap.Left.started += OnInputLeft;
        inputSystem.PlayerActionMap.Right.started += OnInputRight;
        inputSystem.PlayerActionMap.Down.started += OnInputDown;
        inputSystem.PlayerActionMap.Up.started += OnInputUp;
    }

    private void OnInputUp(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        RotatePiece();
    }

    private void OnInputRight(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        gridClass.SetPieceDirection(newPiece, Direction.right);
    }

    private void OnInputLeft(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        gridClass.SetPieceDirection(newPiece, Direction.left);

    }

    private void OnInputDown(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        gridClass.SetPieceDirection(newPiece, Direction.down);

    }

    private void Start()
    {
        CreateGrid();
        newPiece = null;
        Instantiate();
        StartCoroutine(ChangePositonByTime());
    }

    [ContextMenu("Random")]
    public void Instantiate()
    {
        XAxis grid;
        Vector2 newGridPositon;
        GetPieceInGrid((Piece)Random.Range(0, Enum.GetNames(typeof(Piece)).Length), out grid, out newGridPositon);
        newPiece = grid;
        gridClass.AddGrid(grid);

    }

    public void RotatePiece()
    {
        if (newPiece != null)
        {
            XAxis rotatedPiece = new(newPiece.x[0].y.Length, newPiece.x.Length);
            
            for (int i = 0; i < newPiece.x.Length; i++)
            {
                for (int j = 0; j < newPiece.x[i].y.Length; j++)
                {
                    rotatedPiece.x[j].y[i] = newPiece.x[i].y[j];
                    if (i != j)
                    {
                        rotatedPiece.x[j].y[i].gridPosition.x -= j;
                        rotatedPiece.x[j].y[i].gridPosition.y += i;
                    }             
                }
            }
            foreach (var item in newPiece.x)
            {
                foreach (var item2 in item.y)
                {
                    Debug.Log(item2.gridPosition);
                    gridClass.GetTile(item2.gridPosition).gameObject = null;

                }
            }
            newPiece = rotatedPiece;
            foreach (var item in rotatedPiece.x)
            {
                foreach (var item2 in item.y)
                {
                    if (item2.gameObject != null)
                    {
                        gridClass.SetTilePosition(item2);

                    }
                }
            }
          
        }
    }
    private void GetPieceInGrid(Piece pieceType, out XAxis grid, out Vector2 newGridPositon)
    {
        grid = new(2, 1);
        newGridPositon = new Vector2(Random.Range(0, xCount - grid.x.Length + 1), yCount - grid.x[0].y.Length);
        grid.SetGOAndGridPositionByIndex(new Vector2(0, 0), Instantiate(piece), newGridPositon);
        grid.SetGOAndGridPositionByIndex(new Vector2(1, 0), Instantiate(piece), newGridPositon);
        grid.piece = Piece.zeroByThree;
        return;
        switch (pieceType)
        {

            case Piece.zeroByThree:
                grid = new(3, 1);
                newGridPositon = new Vector2(Random.Range(0, xCount - grid.x.Length + 1), yCount - grid.x[0].y.Length);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(2, 0), Instantiate(piece), newGridPositon);
                grid.piece = pieceType;
                break;
            case Piece.t:
                grid = new(3, 2);
                newGridPositon = new Vector2(Random.Range(0, xCount - grid.x.Length + 1), yCount - grid.x[0].y.Length);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(2, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 1), Instantiate(piece), newGridPositon);
                grid.piece = pieceType;

                break;
            case Piece.square:
                grid = new(2, 2);
                newGridPositon = new Vector2(Random.Range(0, xCount - grid.x.Length + 1), yCount - grid.x[0].y.Length);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 1), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 1), Instantiate(piece), newGridPositon);
                grid.piece = pieceType;

                break;
            case Piece.halfT:
                grid = new(2, 2);
                newGridPositon = new Vector2(Random.Range(0, xCount - grid.x.Length + 1), yCount - grid.x[0].y.Length);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 1), Instantiate(piece), newGridPositon);
                grid.piece = pieceType;


                break;
            default:
                grid = new(2, 2);
                newGridPositon = new Vector2(Random.Range(0, xCount - grid.x.Length + 1), yCount - grid.x[0].y.Length);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 1), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 1), Instantiate(piece), newGridPositon);
                grid.piece = pieceType;

                break;
        }

    }
   
    [ContextMenu("Down")]
    public void SetPieceDirection()
    {

        gridClass.SetPieceDirection(newPiece, Direction.down);
    }
    IEnumerator ChangePositonByTime()
    {
        while (true)
        {

            yield return new WaitForSeconds(level);

            if (newPiece.x != null)
            {


                bool success = gridClass.SetPieceDirection(newPiece, Direction.down);

                if (!success)
                {

                    newPiece = null;
                    Instantiate();
                }

            }




        }

    }

    public void CreateGrid()
    {
        float width = gridTile.transform.localScale.x, height = gridTile.transform.localScale.y;
        changeInPosition = new Vector2(width, height);

        gridClass = new(xCount, yCount);
        for (int i = 0; i < gridClass.x.Length; i++)
        {
            for (int j = 0; j < gridClass.x[i].y.Length; j++)
            {

                Vector2 position = new Vector2((i * width -
                    gridClass.x.Length * width / 2) + width / 2,



                    (j * height - gridClass.x[i].y.Length * height / 2) + height / 2);

                Instantiate(gridTile, position


                    , Quaternion.identity, transform);
                gridClass.x[i].y[j].postion = position;
                gridClass.x[i].y[j].GridPosition = new Vector2(i, j);

            }
        }

    }
}

[System.Serializable]
public class XAxis
{
    public Vector2 GetPosition(Vector2 gridPosition)
    {
        return x[(int)gridPosition.x].y[(int)gridPosition.y].postion;
    }
    public Tile GetTile(Vector2 gridPosition)

    {
        return x[(int)gridPosition.x].y[(int)gridPosition.y];
    }
    public bool IsTileNull(Vector2 gridPosition, XAxis newPiece)
    {



        return GetTile(gridPosition).gameObject == null || newPiece.x.ToList().Exists(x => x.y.ToList().Exists(y => y.gameObject == GetTile(gridPosition).gameObject));
    }
    public void AddGrid(XAxis newGrid)
    {
        foreach (var item in newGrid.x)
        {

            foreach (var item1 in item.y)
            {
                if (item1.gameObject == null)
                {
                    continue;
                }
                if (GetTile(item1.GridPosition).gameObject != null)
                {
                    Debug.Log("Game Over");
                }
                GetTile(item1.GridPosition).gameObject = item1.gameObject;
                SetTilePosition(item1);
            }
        }
    }
    public void SetGOAndGridPositionByIndex(Vector2 index, GameObject go, Vector2 gridPositon)
    {
        x[(int)index.x].y[(int)index.y].gameObject = go;
        x[(int)index.x].y[(int)index.y].GridPosition = gridPositon + index;
    }
    public void ChangePiecePosition(Vector2 oldPosition, Vector2 change)
    {

        Tile oldTile = GetTile(oldPosition);
        Tile newTile = GetTile(oldPosition + change);
        Debug.Log(oldTile.gameObject);
        Debug.Log(oldTile.GridPosition);
        Debug.Log(newTile.gameObject);
        Debug.Log(newTile.GridPosition);

        newTile.gameObject = oldTile.gameObject;
        oldTile.gameObject = null;

        SetTilePosition(newTile);
        Debug.Log(oldTile.gameObject);
        Debug.Log(oldTile.GridPosition);
        Debug.Log(newTile.gameObject);
        Debug.Log(newTile.GridPosition);
    }
    public void SetTilePosition(Tile tile)
    {
        var obj = GetTile(tile.GridPosition).gameObject;
        
        obj.transform.position = GetPosition(tile.GridPosition);
        obj.transform.Translate(Vector3.back * 1);
        tile.gameObject.name = tile.GridPosition.ToString();


    }

    public bool SetPieceDirection(XAxis piece, Direction direction)
    {

        foreach (var item in piece.x)
        {
            foreach (var item2 in item.y)
            {

                if (item2.gameObject == null)
                {
                    continue;
                }
                if (direction == Direction.down && item2.GridPosition.y == 0)
                {
                    return false;
                }
                if (direction == Direction.right && item2.GridPosition.x >= x.Length - 1)
                {
                    return false;
                }
                if (direction == Direction.left && item2.GridPosition.x == 0)
                {
                    return false;
                }
                if (!IsTileNull(item2.GridPosition + Vector2.down * 1, piece))
                {
                    return false;
                }
            }
        }

        if (direction == Direction.right)
        {
            foreach (var item in piece.x.Reverse())
            {
                foreach (var item2 in item.y)
                {
                    if (item2.gameObject == null)
                    {
                        continue;
                    }
                    ChangePiecePosition(item2.GridPosition, Vector2.right);
                    item2.GridPosition += Vector2.right;

                }

            }
        }
        else
        {
            foreach (var item in piece.x)
            {
                foreach (var item2 in item.y)
                {

                    if (item2.gameObject == null)
                    {
                        continue;
                    }
                    Vector2 change;
                    switch (direction)
                    {
                        case Direction.down:
                            change = Vector2.down;
                            break;
                        case Direction.right:
                            change = Vector2.right;
                            break;
                        case Direction.left:
                            change = Vector2.left;
                            break;
                        default:
                            change = Vector2.zero;
                            break;
                    }
                    ChangePiecePosition(item2.GridPosition, change);
                    item2.GridPosition += change;

                }
            }
        }
       
        return true;
    }

    public Piece piece;
    public YAxis[] x;
    public XAxis(int xCount, int yCount)
    {
        x = new YAxis[xCount];
        for (int i = 0; i < x.Length; i++)
        {
            x[i] = new(yCount);
        }
    }
}

[System.Serializable]
public class YAxis
{
    public Tile[] y;
    public YAxis(int yCount)
    {
        y = new Tile[yCount];
        for (int i = 0; i < y.Length; i++)
        {
            y[i] = new();
        }
    }
}

[System.Serializable]
public class Tile
{
    public Tile()
    {

    }
    public Tile(GameObject gameObject, Vector2 gridPosition)
    {
        this.gameObject = gameObject;
        this.GridPosition = gridPosition;
    }
    public GameObject gameObject;
    public Vector2 postion;
    public Vector2 gridPosition;

    public Vector2 GridPosition { get => gridPosition; set { gridPosition = value; } }
}
public enum Direction
{
    down, right, left,up
}
public enum Piece
{
    none,zeroByThree, t, square, halfT
}
