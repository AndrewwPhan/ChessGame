using System;
using System.Drawing;

namespace FieldAgent
{
    public class Camera : Obstacle
    {   
        // Store location, direction and field of view of camera
        public Point Location { get; set; }
        public Direction Direction { get; set; }
        public int FieldOfView { get; set; } = 90; // Default 90-degree Field of View

        // Constructor to initialise camera with location and direction
        public Camera(Point location, Direction direction)
        {
            this.Location = location;
            this.Direction = direction;
        }

        // Override from obstacle class
        public override bool CanOccupy(char cellContent)
        {
            return cellContent == '.'; // Can only occupy an empty space
        }
        public override void AddToMap(Map map)
        {
            // Occupy the camera's location first
            map.AddPoint('c', Location);

            // Set Max (X,Y) for boundaries
            int maxY = 1000;
            int maxX = 1000;

            // Calculate the boundary points of the camera's Field of View based on Bresenham's algorithm
            Point end1 = default;
            Point end2 = default;

            // Determine boundaries based on camera location           
            switch (Direction)
            {
                case Direction.North:
                    end1 = new Point(Location.X - maxY, Location.Y - maxY);
                    end2 = new Point(Location.X + maxY, Location.Y - maxY);
                    break;
                case Direction.East:
                    end1 = new Point(Location.X + maxX, Location.Y - maxX);
                    end2 = new Point(Location.X + maxX, Location.Y + maxX);
                    break;
                case Direction.South:
                    end1 = new Point(Location.X - maxY, Location.Y + maxY);
                    end2 = new Point(Location.X + maxY, Location.Y + maxY);
                    break;
                case Direction.West:
                    end1 = new Point(Location.X - maxX, Location.Y - maxX);
                    end2 = new Point(Location.X - maxX, Location.Y + maxX);
                    break;
            }

            // Draw a line for the Field of Vision
            DrawLine(map, Location, end1, 'c');
            DrawLine(map, Location, end2, 'c');
            
            // Mark all grid cells within the Field of Vision as 'c' based on the camera's direction
            var fromX = Math.Min(end1.X, end2.X);
            var toX = Math.Max(end1.X, end2.X);
            var fromY = Math.Min(end1.Y, end2.Y);
            var toY = Math.Max(end1.Y, end2.Y);
            
            if (fromX == toX)
            {
                if(Direction == Direction.East)
                {
                    fromX = Location.X;
                }
                else
                {
                    toX = Location.X;
                }
            }
            if(fromY == toY)
            {
                if (Direction == Direction.South)
                {
                    fromY = Location.Y;
                }
                else
                {
                    toY = Location.Y;
                }
            }

            for (int i = fromX; i < toX; i++)
            {
                for (int j = fromY; j < toY; j++)
                {
                    Point currentPoint = new Point(i, j);
                    if (IsPointInView(currentPoint))
                    {
                        map.AddPoint('c', new Point(i, j));
                    }
                }
            }
        }

        // Draw a line between two points on grid
        private void DrawLine(Map map, Point start, Point end, char value)
        {
            int dx = Math.Abs(end.X - start.X);
            int dy = Math.Abs(end.Y - start.Y);
            int sx = start.X < end.X ? 1 : -1;
            int sy = start.Y < end.Y ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2;
            int e2;

            int x = start.X;
            int y = start.Y;
            while (true)
            {   
                // Add symbol to the map coordinates
                map.AddPoint(value, new Point(x, y));

                if (x == end.X && y == end.Y) break;
                e2 = err;
                if (e2 > -dx)
                {
                    err -= dy;
                    x += sx;
                }
                if (e2 < dy)
                {
                    err += dx;
                    y += sy;
                }
            }
        }

        // Check if point is visible within camera direction
        public bool IsPointInView(Point point)
        {
            switch (Direction)
            {
                case Direction.North:
                    if (point.Y < Location.Y && point.X >= Location.X - (Location.Y - point.Y) && point.X <= Location.X + (Location.Y - point.Y))
                        return true;
                    break;
                case Direction.East:
                    if (point.X > Location.X && point.Y >= Location.Y - (point.X - Location.X) && point.Y <= Location.Y + (point.X - Location.X))
                        return true;
                    break;
                case Direction.South:
                    if (point.Y > Location.Y && point.X >= Location.X - (point.Y - Location.Y) && point.X <= Location.X + (point.Y - Location.Y))
                        return true;
                    break;
                case Direction.West:
                    if (point.X < Location.X && point.Y >= Location.Y - (Location.X - point.X) && point.Y <= Location.Y + (Location.X - point.X))
                        return true;
                    break;
            }
            return false;
        }
    }
}
