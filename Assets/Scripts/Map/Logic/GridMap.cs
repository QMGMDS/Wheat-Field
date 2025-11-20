using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode] //该特性运行代码在编辑模式时运行
public class GridMap : MonoBehaviour
{
    public MapData_SO mapData;
    public GridType gridType;
    private Tilemap currentTilemap;

    private void OnEnable()
    {
        if (!Application.IsPlaying(this)) //游戏运行前执行，即编辑过程中执行
        {
            currentTilemap = GetComponent<Tilemap>();

            if(mapData != null)
                mapData.tileProperties.Clear();
        }
    }

    private void OnDisable()
    {
        if (!Application.IsPlaying(this)) //游戏运行前执行，即编辑过程中执行
        {
            currentTilemap = GetComponent<Tilemap>();

            UpdateTileProperties();
#if UNITY_EDITOR
            if(mapData != null)
                EditorUtility.SetDirty(mapData);
#endif
        }
    }


    private void UpdateTileProperties()
    {
        currentTilemap.CompressBounds(); //压缩当前瓦片地图信息，仅保留已经绘制的信息

        if (!Application.IsPlaying(this))
        {
            if(mapData != null)
            {
                //已经绘制范围左下角坐标
                Vector3Int startPos = currentTilemap.cellBounds.min;
                //已经绘制范围右上角坐标
                Vector3Int endPos = currentTilemap.cellBounds.max;

                for (int x = startPos.x; x < endPos.x; x++)
                {
                    for (int y = startPos.y; y < endPos.y; y++)
                    {
                        TileBase tile = currentTilemap.GetTile(new Vector3Int(x,y,0)); //拿到瓦片地图中一块一块的瓦片

                        if(tile != null)
                        {
                            TileProperty newTile = new TileProperty
                            {
                                tileCoordinate = new Vector2Int(x,y),
                                gridType = this.gridType,
                                boolTypeValue = true
                            };

                            mapData.tileProperties.Add(newTile);
                        }
                    }
                }
            }
        }
    }
}
