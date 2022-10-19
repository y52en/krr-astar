using System;
using System.Runtime.InteropServices.JavaScript;
using System.Collections.Generic;

public partial class AStarExport
{
    const int x = 9;
    const int y = 9;

    static void Main() { }

    static int[,] StringToList(String value, int x, int y)
    {
        int[,] list = new int[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                bool tmp = value[i * x + j] == '1';
                list[i, j] = tmp ? 1 : 0;
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
    internal static string Find(string _map, int start_x, int start_y, int end_x, int end_y)
    {
        var map = StringToList(_map, x, y);
        var astar = new Star.AStar(x, y);
        astar.SetData(map);
        var paths = astar.Find(new Vector2(start_x, start_y), CalcFreeMovePoint(map, x, y, end_x, end_y));
        return Vector2ToCSV(paths);
    }



    static Vector2 CalcFreeMovePoint(int[,] m_GridStatus, int m_SizeX, int m_SizeY, int start_x, int start_y)
    {
        Vector2 zero = Vector2.zero;
        int num2;
        int num = num2 = start_x;
        int num4;
        int num3 = num4 = start_y;
        int num5 = 0;
        bool flag = true;
        bool flag2 = false;
        while (flag)
        {
            if (m_GridStatus[num, num3] == 1)
            {
                zero.x = (float)num;
                zero.y = (float)num3;
                break;
            }
            switch (num5)
            {
                case 0:
                    num++;
                    if (num >= m_SizeX)
                    {
                        num = num2;
                        num5++;
                    }
                    break;
                case 1:
                    num--;
                    if (num < 0)
                    {
                        num = num2;
                        if (flag2)
                        {
                            num5 = 3;
                        }
                        else
                        {
                            num5 = 2;
                        }
                    }
                    break;
                case 2:
                    num3++;
                    if (num3 >= m_SizeY)
                    {
                        num3 = num4;
                        flag2 = true;
                        num5++;
                    }
                    else
                    {
                        num5 = 0;
                    }
                    break;
                case 3:
                    num3--;
                    if (num3 < 0)
                    {
                        num3 = num4;
                        num5++;
                    }
                    else
                    {
                        num5 = 0;
                    }
                    break;
                case 4:
                    throw new Exception("Unreachable code");
            }
        }
        return zero;
    }

}
