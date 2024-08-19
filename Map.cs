using System;
using System.Collections.Generic;
using System.Drawing;

namespace FieldAgent
{
    public class Map
    {
        private const int DefaultSize = 31; // Default size based on GradeScope Tests

        // Hashset to store content and points of grid cells
        private HashSet<(char, Point)> grid = new HashSet<(char, Point)>();

        // Method to check if map contains character at a given point
        public bool Contains(char c, Point point)
        {
            return grid.Contains((c, point));
        }

        // Constructor to create Map with custom size as per GradeScope Tests
        public Map(int size = DefaultSize)
        {
            // Able to create Map but not initialise the grid
        }

        // Check if guard is at a certain point in the grid
        public bool IsGuardAt(Point location)
        {
            return grid.Contains(('g', location));
        }

        // Method to add Camera to Map
        public void AddCamera(Camera camera)
        {
            camera.AddToMap(this);
        }

        // List to store Cameras within the map
        public List<Camera> Cameras { get; set; } = new List<Camera>();

        // Check if Obstacle is in a certain direction/point within the grid
        public bool IsObstacleInDirection(Direction direction, Point currentLocation)
        {
            Point nextLocation = currentLocation;

            switch (direction)
            {
                case Direction.North:
                    nextLocation.Y -= 1;
                    break;
                case Direction.East:
                    nextLocation.X += 1;
                    break;
                case Direction.South:
                    nextLocation.Y += 1;
                    break;
                case Direction.West:
                    nextLocation.X -= 1;
                    break;
            }

            // Check if next location is occupied by an obstacle
            return Occupied(nextLocation);
        }

        // Method to add obstacle to map
        public void AddObstacle(Obstacle obstacle)
        {
            obstacle.AddToMap(this);
        }

        // Method to display section of map with specific bounds
        public void Display(int topLeftX, int topLeftY, int bottomRightX, int bottomRightY)
        {
            for (int y = topLeftY; y <= bottomRightY; y++)
            {
                for (int x = topLeftX; x <= bottomRightX; x++)
                {   
                    // Check if grid cell contains symbol/obstacle
                    if (grid.Contains(('g', new Point(x, y))))
                        Console.Write('g');
                    else if (grid.Contains(('c', new Point(x, y))))
                        Console.Write('c');
                    else if (grid.Contains(('s', new Point(x, y))))
                        Console.Write('s');
                    else if (grid.Contains(('f', new Point(x, y))))
                        Console.Write('f');
                    else
                        Console.Write('.');
                }
                Console.WriteLine();
            }
        }

        // Internal method to add point with content and location to grid
        internal void AddPoint(char xp, Point location)
        {
            grid.Add((xp, location));
        }

        // Internal method to check if point is occupied by an obstacle
        internal bool Occupied(Point point)
        {
            return grid.Contains(('g', point)) || grid.Contains(('f', point)) || grid.Contains(('s', point)) || grid.Contains(('c', point)) || grid.Contains(('b', point));
        }
    }
}
