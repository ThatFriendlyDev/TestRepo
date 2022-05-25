using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelGenerationData : SerializedMonoBehaviour
{
    public int _xSize, _ySize;
    public float _scale = 1f;
    public float _destructionRadius;
    public int[,,] _data;

    [ShowInInspector]
    [TableMatrix(HorizontalTitle = "MeshData in 2D", DrawElementMethod = "DrawColoredEnumElement", ResizableColumns = false, RowHeight = 30)]
    public int[,] _data2D;

    private static int DrawColoredEnumElement(Rect rect, int value)
    {
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            value = value == 1 ? 0 : 1;
            GUI.changed = true;
            Event.current.Use();
        }

      //  UnityEditor.EditorGUI.DrawRect(rect.Padding(5, 2), value == 1 ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));

        return value;
    }

    public int _zSize;

    [OnInspectorInit]
    private void CreateData()
    {
        if (this._data2D != null)
        {
            if (this._data2D.GetLength(0) != this._xSize || this._data2D.GetLength(1) != this._ySize)
                this._data2D = new int[this._xSize, this._ySize];
            else
                return;
        }

        this._data2D = new int[this._xSize, this._ySize];

        for (int x = 0; x < this._data2D.GetLength(0); x++)
        {
            for (int y = 0; y < this._data2D.GetLength(1); y++)
            {
                this._data2D[x, y] = 1;
            }
        }
    }

    private void Awake()
    {
        this._data = new int[this._xSize, this._ySize, this._zSize];
        for (int x = 0; x < this._data.GetLength(0); x++)
        {
            for (int y = 0; y < this._data.GetLength(1); y++)
            {
                for (int z = 0; z < this._data.GetLength(2); z++)
                {
                    this._data[x, y, z] = this._data2D[x, y];
                }
            }
        }
    }
 

}
 