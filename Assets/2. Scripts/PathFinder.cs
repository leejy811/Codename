using System.Collections.Generic;
using UnityEngine;
public class PathFinder 
{
    // move to 8-directions
    public  readonly static int[,] dirs = { { -1, -1 },  { -1, 0 }, { -1, 1 }, { 0, 1 },  { 1, 1 }, { 1, 0 }, { 1, -1 },  { 0, -1 } };
    
    // returns the first step of the "Best Route" 
    // using BFS, generic queue
    public static int[] GetDir(int[] currentPos,int sizeX,int sizeY)
    {
        Queue<(int, int, int)> queue = new Queue<(int, int, int)>();
        int[,] map = StageManager.instance.map;
        int[,] visited = new int[map.GetLength(0),map.GetLength(1)];
        int[] playerPos = StageManager.instance.PlayerPos();
        int xGap = sizeX % 2;
        int yGap = sizeY % 2;
        
        // Start from the initial monster posiiton
        queue.Enqueue((currentPos[0], currentPos[1], -1));
        visited[currentPos[0], currentPos[1]] = 1;
        
        if (currentPos[0] == playerPos[0] && currentPos[1] == playerPos[1])
            return new int[] { 0,0 };

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

                // check if selected direction is blocked or out of the map
                for(int x = -xGap; x < xGap + 1; x++)
                    for(int y = -yGap; y < yGap + 1; y++)
                        if (dx+x < 0 || dy+y < 0 || dx+x >= map.GetLength(0) || dy+y >= map.GetLength(1) || map[dx + x, dy + y] == 1)
                        {
                            isBlocked = true;
                            break;
                        }
                
                if (isBlocked)  
                    continue;   // pass if route is blocked
                if (visited[dx, dy] == 1)
                    continue;   // pass if already visited

                if (temp.Item3 == -1)   // set the initial direction when calculating the first tile
                    queue.Enqueue((dx, dy, i)); 
                else                             // maintain the first direction of the route until reaching the player
                    queue.Enqueue((dx, dy, temp.Item3));
                visited[dx, dy] = 1;
            }
        }

        return new int[]{ 0, 0};   // return error when the monster cannot reach the player
    }

}

public class AStarAlgorithm
{
    private static int[,] map; // The map information
    private static int[] destination; // The destination coordinates

    private static int width;
    private static int height;

    private static int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
    private static int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };

    public static int[] GetDir(int[] currentPosition)
    {
        width = map.GetLength(1);
        height = map.GetLength(0);

        int[] start = new int[] { currentPosition[0], currentPosition[1] };

        // Initialize the open and closed lists
        List<int[]> openList = new List<int[]>();
        List<int[]> closedList = new List<int[]>();

        // Set the G and H cost of the start position
        int[,] gCost = new int[height, width];
        int[,] hCost = new int[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                gCost[y, x] = int.MaxValue;
                hCost[y, x] = CalculateHeuristic(new int[] { x, y }, destination);
            }
        }

        gCost[start[1], start[0]] = 0;

        // Add the start position to the open list
        openList.Add(start);

        while (openList.Count > 0)
        {
            // Find the node with the lowest F cost in the open list
            int[] current = openList[0];
            int currentIndex = 0;

            for (int i = 1; i < openList.Count; i++)
            {
                int[] node = openList[i];

                if (gCost[node[1], node[0]] + hCost[node[1], node[0]] < gCost[current[1], current[0]] + hCost[current[1], current[0]])
                {
                    current = node;
                    currentIndex = i;
                }
            }

            // Remove the current node from the open list and add it to the closed list
            openList.RemoveAt(currentIndex);
            closedList.Add(current);

            // Check if the current node is the destination
            if (current[0] == destination[0] && current[1] == destination[1])
            {
                //return ReconstructPath(start, current);
            }

            // Explore the neighbors of the current node
            for (int i = 0; i < 8; i++)
            {
                int newX = current[0] + dx[i];
                int newY = current[1] + dy[i];

                // Check if the neighbor is within the map boundaries
                if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                {
                    // Check if the neighbor is walkable and not in the closed list
                    if (map[newY, newX] == 0 && !IsInList(closedList, new int[] { newX, newY }))
                    {
                        int tentativeGCost = gCost[current[1], current[0]] + CalculateMovementCost(current, new int[] { newX, newY });

                        // Check if the neighbor is already in the open list or has a lower G cost
                        if (tentativeGCost < gCost[newY, newX])
                            // Check if the neighbor is already in the open list or has a lower G cost
                            if (tentativeGCost < gCost[newY, newX] || !IsInList(openList, new int[] { newX, newY }))
                            {
                                // Update the G cost and add the neighbor to the open list
                                gCost[newY, newX] = tentativeGCost;
                                int fCost = gCost[newY, newX] + hCost[newY, newX];
                                int[] neighbor = new int[] { newX, newY };

                                // Remove the neighbor from the open list if it was already added
                                if (IsInList(openList, neighbor))
                                {
                                    openList.Remove(neighbor);
                                }

                                // Find the position to insert the neighbor in the open list based on its F cost
                                int insertIndex = 0;
                                while (insertIndex < openList.Count && fCost >= gCost[openList[insertIndex][1], openList[insertIndex][0]] + hCost[openList[insertIndex][1], openList[insertIndex][0]])
                                {
                                    insertIndex++;
                                }

                                // Insert the neighbor in the open list at the determined position
                                openList.Insert(insertIndex, neighbor);
                            }
                    }
                }
            }
        }

        // No path found
        return null;
    }

    private static int CalculateHeuristic(int[] start, int[] destination)
    {
        int dx = Mathf.Abs(start[0] - destination[0]);
        int dy = Mathf.Abs(start[1] - destination[1]);
        return Mathf.Max(dx, dy);
    }

    private static int CalculateMovementCost(int[] current, int[] neighbor)
    {
        int dx = Mathf.Abs(current[0] - neighbor[0]);
        int dy = Mathf.Abs(current[1] - neighbor[1]);

        if (dx == 1 && dy == 1)
        {
            return 14; // Diagonal movement cost
        }
        else
        {
            return 10; // Vertical or horizontal movement cost
        }
    }

    private static bool IsInList(List<int[]> list, int[] position)
    {
        foreach (int[] item in list)
        {
            if (item[0] == position[0] && item[1] == position[1])
            {
                return true;
            }
        }
        return false;
    }

    private static void ReconstructPath(int[] start, int[] current)
    {
        List<int[]> path = new List<int[]>();
        int[] node = current;

    }
}


