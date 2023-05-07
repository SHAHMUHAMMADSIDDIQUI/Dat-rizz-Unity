using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject gridTile,piece;
    public XAxis gridClass;
    public XAxis newPiece;
    public int xCount, yCount;
    public Vector2 changeInPosition;
    
    public float level;
    private void Start()
    {
        CreateGrid();
        newPiece = null;
        //Instantiate();
        //StartCoroutine(ChangePositonByTime());
    }
   
    [ContextMenu("Random")]
    public void Instantiate()
    {


        XAxis grid = new(2,1);
                
        


        Vector2 newGridPositon = new Vector2(Random.Range(0, xCount-grid.x.Length+1),yCount-grid.x[0].y.Length-1);

                gridClass.AddGrid(grid,newGridPositon);
        newPiece = grid;
        
    }
    [ContextMenu("Down")]
   public void SetDown()
    {

        gridClass.SetDown(newPiece);
    }
    IEnumerator ChangePositonByTime()
    {
        while (true)
        {

            yield return new WaitForSeconds(level);

            if (newPiece.x != null)
            {
                
                
                bool success = gridClass.SetDown(newPiece);
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
        float width = gridTile.transform.localScale.x,height = gridTile.transform.localScale.y;
        changeInPosition = new Vector2(width,height);
        
        gridClass = new(xCount,yCount);
        for (int i = 0; i < gridClass.x.Length; i++)
        {
            for (int j = 0; j < gridClass.x[i].y.Length; j++)
            {

                Vector2 position = new Vector2((i * width -
                    gridClass.x.Length * width / 2) + width / 2,



                    (j * height - gridClass.x[i].y.Length * height / 2) + height / 2);

                Instantiate(gridTile,position
                    
                    
                    , Quaternion.identity,transform);
                gridClass.x[i].y[j].postion = position;
                gridClass.x[i].y[j].gridPosition = new Vector2(i,j);
                
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
    public bool IsTileNull(Vector2 gridPosition)
    {


        return GetTile(gridPosition).gameObject == null;
    }
        public void AddGrid(XAxis grid,Vector2 newGridPosition)
    {
        var start = newGridPosition;
        foreach (var item in grid.x)
        {

            foreach (var item1 in item.y)
            {
                GetTile(item1.gridPosition).gameObject = item1.gameObject;
                GetTile(item1.gridPosition).gridPosition= newGridPosition;
                SetTilePosition(item1);
                newGridPosition.y++;
            }
            newGridPosition.x ++;
        }
    }
    public void SetGOAndGridPositionByIndex(Vector2 index,GameObject go,Vector2 gridPositon)
    {

        x[(int)index.x].y[(int)index.y].gameObject = go;
        x[(int)index.x].y[(int)index.y].gridPosition = gridPositon;

        
    }
    public void ChangePiecePosition(Vector2 oldPosition, Vector2 change)
    {
        Tile oldTile = GetTile(oldPosition);
         Tile newTile = GetTile(oldPosition + change);
        newTile.gameObject = oldTile.gameObject;
        newTile.gridPosition = oldTile.gridPosition + change;
        SetTilePosition(newTile);        
        oldTile.gameObject = null;
        

    }
    public void SetTilePosition(Tile tile)
    {
        var obj = GetTile(tile.gridPosition).gameObject;
        obj.transform.position = GetPosition(tile.gridPosition);
        obj.transform.Translate(Vector3.back * 1);
    }    
    
    public bool SetDown(XAxis piece)
    {
        foreach (var item in piece.x)
        {
            foreach (var item2 in item.y)
            {
                

                if (item2.gridPosition.y ==0 || !IsTileNull(item2.gridPosition + Vector2.down*1))
                {
                    return false;
                }
            }
        }
        foreach (var item in piece.x)
        {
            foreach (var item2 in item.y)
            {

                ChangePiecePosition(item2.gridPosition, Vector2.down * 1);
                item2.gridPosition += Vector2.down ;
            }
        }
        return true;
    }
   
    public YAxis[] x;
    public XAxis(int xCount,int yCount)
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
    public Tile(GameObject gameObject,Vector2 gridPosition)
    {
        this.gameObject = gameObject;
        this.gridPosition = gridPosition;
           }
    public GameObject gameObject;
    public Vector2 postion;
    public Vector2 gridPosition;
   
}
[System.Serializable]
public class Piece
{
     
    public List<Tile> tiles = new();
}
