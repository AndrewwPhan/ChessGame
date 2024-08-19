using System.Drawing;

namespace FieldAgent
{
    public class Fence : Obstacle
    {   
        // Store start and end of fence
        public Point Start { get; set; }
        public Point End { get; set; }

        // Constructor to initialise start and end point
        public Fence(Point start, Point end)
        {
            this.Start = start;
            this.End = end;
        }

        // Override from obstacle class
        public override void AddToMap(Map map)
        {
            AddFenceToMap(map);
        }

        public override bool CanOccupy(char symbol)
        {
            return symbol == 'f';
        }

        // Method to add the fence to the map
        private void AddFenceToMap(Map map)
        {
            // Using Bresenham's line algorithm to draw a straight line (chat GPT)
            int x0 = Start.X;
            int y0 = Start.Y;
            int x1 = End.X;
            int y1 = End.Y;

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = (x0 < x1) ? 1 : -1;
            int sy = (y0 < y1) ? 1 : -1;
            int err = dx - dy;

            // Add symbol to map coordinates
            while (true)
            {
                map.AddPoint('f', new Point(x0, y0)); 
                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 > -dy) { err -= dy; x0 += sx; }
                if (e2 < dx) { err += dx; y0 += sy; }
            }
        }
    }
}
