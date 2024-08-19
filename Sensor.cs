using System;
using System.Text;

using Point = System.Drawing.Point;

namespace FieldAgent
{   
    // Store location and radius of sensor
    public class Sensor : Obstacle
    {
        public Point Location { get; set; }
        public double Radius { get; set; }

        // Constructor to initialise sensor with location and radius
        public Sensor(Point location, double radius)
        {
            this.Location = location;
            this.Radius = radius;
        }

        // Override from obstacle class
        public override bool CanOccupy(char cellContent)
        {
            // Can only occupy empty cells or 'c' cells
            return cellContent == '.' || cellContent == 'c';
        }

        public override void AddToMap(Map map)
        {
            // Calculate largest sensor extent based on radius
            var largestExtent = (int)Math.Floor(Radius);
            var top = Location.Y - largestExtent;
            var left = Location.X - largestExtent;
            var bottom = Location.Y + largestExtent;
            var right = Location.X + largestExtent;

            // Calculate distance between cell and sensor centre
            for (int y = top; y <= bottom; y++)
            {
                for (int x = left; x <= right; x++)
                {
                    var xDist = x - Location.X;
                    var yDist = y - Location.Y;
                    var c = MathF.Sqrt(MathF.Pow(xDist, 2) + MathF.Pow(yDist, 2));
                    
                    // Check if cell is within sensor extent/radius
                    if (c <= Radius)
                    {
                        map.AddPoint('s', new Point(x, y));
                    }
                }
            }
        }
    }
}


