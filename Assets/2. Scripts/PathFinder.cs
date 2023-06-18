using System.Collections.Generic;

public class PathFinder 
{
    public static int[,] dirs = { { -1, -1 },  { -1, 0 }, { -1, 1 }, { 0, 1 },  { 1, 1 }, { 1, 0 }, { 1, -1 },  { 0, -1 } };
    
    public static int[] GetDir(int[,] map,int[] playerPos,int[] currentPos,int sizeX,int sizeY)
    {
        Queue<(int, int, int)> queue = new Queue<(int, int, int)>();
        int[,] visited = new int[map.GetLength(0),map.GetLength(1)];
        queue.Enqueue((currentPos[0], currentPos[1], -1));

        while (queue.Count > 0)
        {
            var temp = queue.Dequeue();
            
        }


        return new int[]{ -1,-1};
    }
}
