using System;
using System.Runtime.InteropServices.JavaScript;
using System.Collections.Generic;

public partial class AStarExport
{
    const int x = 9;
    const int y = 9;

    static void Main(){}

    static int[,] StringToList(String value, int x, int y)
    {
        int[,] list = new int[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                bool tmp = value[i * x + j] == '1';
                list[i,j] = tmp ? 1 : 0;
            }
        }
        return list;
    }

    static string Vector2ToCSV(List<Vector2> list)
    {
        string csv = "";
        foreach (var pos in list)
        {
            csv += pos.x + "," + pos.y + "\n";
        }
        return csv;
    }

    [JSExport]
    internal static string Find(string _map)
    {
        var map = StringToList(_map, 3, 3);
        var astar = new Star.AStar(3, 3);
        astar.SetData(map);
        var paths = astar.Find(new Vector2(1.1f, 0f), new Vector2(1f, 0f));
        return Vector2ToCSV(paths);
    }
}
