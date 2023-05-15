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
    private void Start()
    {
        CreateGrid();
        newPiece = null;
        Instantiate();
        StartCoroutine(ChangePositonByTime());
    }
    private void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.PlayerActionMap.Left.started += OnInputLeft;
        inputSystem.PlayerActionMap.Right.started += OnInputRight;
        inputSystem.PlayerActionMap.Down.started += OnInputDown;
        inputSystem.PlayerActionMap.Up.started += OnInputUp;
        inputSystem.PlayerActionMap.Space.started += OnInputSpace;
    }

    private void OnInputSpace(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        bool success = gridClass.SetPieceDirection(newPiece, Vector2.down,true);
        if (!success)
        {
            newPiece = null;
            Instantiate();
        }
    }

    public void CreateGrid()
    {
        float width = gridTile.transform.localScale.x, height = gridTile.transform.localScale.y;
        changeInPosition = new Vector2(width, height);

        gridClass = new(xCount, yCount);
        for (int i = 0; i < gridClass.x.Length; i++)
        {
            for (int j = gridClass.x[i].y.Length-1; j >=0; j--)
            {

                Vector2 position = new Vector2((i * width -
                    gridClass.x.Length * width / 2) + width / 2,



                    (j * height - gridClass.x[i].y.Length * height / 2) + height / 2);

                Instantiate(gridTile, position, Quaternion.identity, transform);
                gridClass.x[i].y[j].postion = position;
                gridClass.x[i].y[j].GridPosition = new Vector2(i, j);

            }
        }

    }
    private void OnInputUp(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        RotatePiece();
    }

    private void OnInputRight(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        gridClass.SetPieceDirection(newPiece, Vector2.right);
    }

    private void OnInputLeft(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        gridClass.SetPieceDirection(newPiece, Vector2.left);

    }

    private void OnInputDown(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
       bool success=  gridClass.SetPieceDirection(newPiece, Vector2.down);
        if (!success )
        {
            newPiece = null;
            Instantiate();
        }
    }

   
    public void Instantiate()
    {
        XAxis grid;
        Vector2 newGridPositon;
        GetPieceInGrid((Piece)Random.Range(0, Enum.GetNames(typeof(Piece)).Length), out grid, out newGridPositon);
        if (gridClass.IsSpaceAvailable(grid))
        {
            newPiece = grid;
            gridClass.AddGrid(grid, newGridPositon);
        }
        else
        {
            Instantiate();    
        }
        

    }

    public void RotatePiece()
    {
       
        
        
        if (newPiece != null)
        {


            XAxis rot = new XAxis(newPiece.x[0].y.Length, newPiece.x.Length);
            
            gridClass.RemovePiecePosition(newPiece);
            for (int x = 0; x < newPiece.x.Length; x++)
            {
                for (int y = 0; y < newPiece.x[x].y.Length; y++)
                {

                    rot.x[y].y[x].gameObject = newPiece.x[x].y[y].gameObject;
                }
            }
            gridClass.OnlyAssignGridPosition(rot, newPiece.x[0].y[0].gridPosition);
            if (gridClass.IsSpaceAvailable(rot))
            {
                gridClass.AddGrid(rot, newPiece.x[0].y[0].gridPosition);
                newPiece = rot;
            }

            else
            {

                gridClass.AddGrid(newPiece,newPiece.x[0].y[0].gridPosition);
            }

        }

    }
    private void GetPieceInGrid(Piece pieceType, out XAxis grid, out Vector2 newGridPositon)
    {
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
                grid = new(3, 2);
                newGridPositon = new Vector2(Random.Range(0, xCount - grid.x.Length + 1), yCount - grid.x[0].y.Length);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 1), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 1), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(2, 1), Instantiate(piece), newGridPositon);
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
   
      IEnumerator ChangePositonByTime()
    {
        while (true)
        {

            yield return new WaitForSeconds(level);

            if (newPiece.x != null)
            {


                bool success = gridClass.SetPieceDirection(newPiece, Vector2.down);
                
                if (!success)
                {

                    newPiece = null;
                    IsLayerCompleted();
                    Instantiate();
                }

            }




        }

    }
    void IsLayerCompleted()
    {
        for (int i = yCount-1; i >=0; i--)
        {
            for (int x = 0; x < gridClass.x.Length; x++)
            {
                if (gridClass.GetTile(new Vector2(x,i)).gameObject == null)
                {
                    break;
                }
                if (x==xCount-1)
                {
                    for (int xx = 0; xx < gridClass.x.Length; xx++)
                    {
                        Destroy(gridClass.GetTile(new Vector2(xx, i)).gameObject);
                        gridClass.GetTile(new Vector2(xx, i)).gameObject = null;
                        for (int yy = i+1; yy <yCount; yy++)
                        {
                            gridClass.GetTile(new Vector2(xx, yy)).gridPosition += Vector2.down;
                            gridClass.SetTilePosition(gridClass.GetTile(new Vector2(xx, yy)));
                        }
                        
                    }
                }
            }
        }
        
        
    }

   
}

[System.Serializable]
public class XAxis
{
    public bool IsSpaceAvailable(XAxis piece)
    {
        
        foreach (var item in piece.x)
        {
            foreach (var item2 in item.y)
            {
                if (item2.gridPosition.x>=xCount || item2.gridPosition.y>=yCount )
                {
                    return false;
                }
                if (GetTile(item2.gridPosition).gameObject != null)
                {
                    return false;
                }

            }
        }
        return true;
    }
    public void RemovePiecePosition(XAxis piece)
    {

        for (int i = 0; i < piece.x.Length; i++)
        {
            for (int j = 0; j < piece.x[i].y.Length; j++)
            {
                if (piece.x[i].y[j].gameObject != null)
                {
                    GetTile(piece.x[i].y[j].gridPosition).gameObject = null;
                }
            }
        }
    }
    public Vector2 GetPosition(Vector2 gridPosition)
    {
        return x[(int)gridPosition.x].y[(int)gridPosition.y].postion;
    }
    public Tile GetTile(Vector2 gridPosition)

    {
        return x[(int)gridPosition.x].y[(int)gridPosition.y];
    }
    public bool IsTileNull(Vector2 gridPosition)
    {



        return GetTile(gridPosition).gameObject == null ;
    }
    public void OnlyAssignGridPosition(XAxis newGrid, Vector2 newGridPosition)
    {
        for (int i = newGrid.x.Length - 1; i >= 0; i--)
        {
            for (int j = 0; j < newGrid.x[i].y.Length; j++)
            {

                newGrid.x[i].y[j].gridPosition = newGridPosition + Vector2.right * i - Vector2.down * j;

            }
        }

    }
    public void AddGrid(XAxis newGrid, Vector2 newGridPosition, bool isRight = false)
    {
        if (isRight)
        {
            for (int i = newGrid.x.Length-1; i >=0; i--)
            {
                for (int j = 0; j < newGrid.x[i].y.Length; j++)
                {
                    if (GetTile(newGrid.x[i].y[j].GridPosition).gameObject != null)
                    {
                        Debug.Log("Game Over");

                        return;
                    }
                    newGrid.x[i].y[j].gridPosition = newGridPosition + Vector2.right * i - Vector2.down * j;
                    GetTile(newGrid.x[i].y[j].GridPosition).gameObject = newGrid.x[i].y[j].gameObject;

                    SetTilePosition(GetTile(newGrid.x[i].y[j].GridPosition));
                }
            }

        }
        else
        {
            for (int i = 0; i < newGrid.x.Length; i++)
            {
                for (int j = 0; j < newGrid.x[i].y.Length; j++)
                {
                    if (GetTile(newGrid.x[i].y[j].GridPosition).gameObject != null)
                    {
                        Debug.Log("Game Over");

                        return;
                    }
                    newGrid.x[i].y[j].gridPosition = newGridPosition + Vector2.right * i - Vector2.down * j;
                    GetTile(newGrid.x[i].y[j].GridPosition).gameObject = newGrid.x[i].y[j].gameObject;

                    SetTilePosition(GetTile(newGrid.x[i].y[j].GridPosition));
                }
            }
        }
       
    }
    public void SetGOAndGridPositionByIndex(Vector2 index, GameObject go, Vector2 gridPositon)
    {
        x[(int)index.x].y[(int)index.y].gameObject = go;
        x[(int)index.x].y[(int)index.y].GridPosition = gridPositon + index;
    } 
    public void SetGOAndGridPosition(Tile tile)
    {
        GetTile(tile.gridPosition).gameObject = tile.gameObject;
        SetTilePosition(tile);
    }
    public void ChangePiecePosition(Vector2 oldPosition, Vector2 change)
    {


        Tile oldTile = GetTile(oldPosition);
        Tile newTile = GetTile(oldPosition + change);

        newTile.gameObject = oldTile.gameObject;
        oldTile.gameObject = null;

        SetTilePosition(newTile);
    }
    public void SetTilePosition(Tile tile)
    {
        if (tile.gameObject == null)
        {
            return;
        }
        var obj = GetTile(tile.GridPosition).gameObject;
        obj.transform.position = GetPosition(tile.GridPosition);
        obj.transform.Translate(Vector3.back * 1);
        tile.gameObject.name = tile.GridPosition.ToString();
    }

    public bool SetPieceDirection(XAxis piece, Vector2 direction, bool space = false)
    {
        RemovePiecePosition(piece);
                       
        foreach (var item in piece.x)
        {
            foreach (var item2 in item.y)
            {

                if (item2.gameObject == null)
                {
                    continue;
                }
                if (direction == Vector2.down)
                {
                    if (item2.GridPosition.y == 0 || !IsTileNull(item2.GridPosition + Vector2.down))

                    {
                        AddGrid(piece, piece.x[0].y[0].gridPosition);

                        return false;

                    }
                                   }
                if (direction == Vector2.right)
                {
                    if (item2.GridPosition.x == x.Length - 1 || !IsTileNull(item2.gridPosition + Vector2.right))
                    {
                        AddGrid(piece, piece.x[0].y[0].gridPosition, isRight: true);
                        return true;
                    }
                                   }
                if (direction == Vector2.left)
                {
                    if (item2.GridPosition.x == 0 || !IsTileNull(item2.gridPosition + Vector2.left))
                    {
                        AddGrid(piece, piece.x[0].y[0].gridPosition);

                        return true;
                    }
                                   }
                
            }
        }
        if (direction == Vector2.right)
        {

            AddGrid(piece, piece.x[0].y[0].gridPosition+direction,isRight : true);
        }
        else
        {
            AddGrid(piece, piece.x[0].y[0].gridPosition + direction);
            if (space)
            {
                SetPieceDirection(piece, direction, true);
            }
        }
        return true;
    }

    public Piece piece;
    public YAxis[] x;
    public int xCount, yCount;
    public XAxis(int xCount, int yCount)
    {
        this.xCount = xCount;
        this.yCount = yCount;
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
