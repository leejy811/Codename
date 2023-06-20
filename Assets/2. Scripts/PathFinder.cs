using System.Collections.Generic;

public class PathFinder 
{
    // move to 8-directions
    public  readonly static int[,] dirs = { { -1, -1 },  { -1, 0 }, { -1, 1 }, { 0, 1 },  { 1, 1 }, { 1, 0 }, { 1, -1 },  { 0, -1 } };
    
    // returns the first step of the "Best Route" 
    // using BFS, generic queue
    public static int[] GetDir(int[,] map,int[] playerPos,int[] currentPos,int sizeX,int sizeY)
    {
        Queue<(int, int, int)> queue = new Queue<(int, int, int)>();
        int[,] visited = new int[map.GetLength(0),map.GetLength(1)];
        int xGap = sizeX % 2;
        int yGap = sizeY % 2;

        // Start from the initial monster posiiton
        queue.Enqueue((currentPos[0], currentPos[1], -1));
        visited[currentPos[0], currentPos[1]] = 1;

        while (queue.Count > 0)
        {
            var temp = queue.Dequeue();

            // return the 'first' direction of the route. monster will move to the direction and calc the best route again.
            if (temp.Item1 == playerPos[0] && temp.Item2 == playerPos[1])
                return new int[] { dirs[temp.Item3, 0], dirs[temp.Item3, 1] };          

            // check for all available tiles and put into the queue
            for (int i = 0; i < dirs.GetLength(0); i++)
            {
                int dx = temp.Item1 + dirs[i, 0];
                int dy = temp.Item2 + dirs[i, 1];
                bool isBlocked = false;

                for(int x = -xGap; x < xGap + 1; x++)
                {
                    for(int y = -yGap; y < yGap + 1; y++)
                    {
                        if (dx+x < 0 || dy+y < 0 || dx+x >= map.GetLength(0) || dy+y >= map.GetLength(1))
                            isBlocked = true;
                        if (map[x, y] == 1)
                            isBlocked = true;
                    }
                }
                if (isBlocked)  
                    continue;   // pass if outside of the map
                if (visited[dx, dy] == 1)
                    continue;   // pass if already visited

                if (temp.Item3 == -1)   // set the initial direction when calculating the first tile
                    queue.Enqueue((dx, dy, i)); 
                else                             // maintain the first direction of the route until reaching the player
                    queue.Enqueue((dx, dy, temp.Item3));
                visited[dx, dy] = 1;
            }
        }

        return new int[]{ -1,-1};   // return error when the monster cannot reach the player
    }
}
