using System;
using FieldAgent;
using System.Drawing;

namespace FieldAgent
{
    public class Guard : Obstacle
    {   
        // Store location of Guard
        public Point Pos { get; set; }

        // Constructor to initialise Guard with grid coordinates
        public Guard(int x, int y)
        {
            Pos = new Point(x, y);
        }

        // Constructor to initialise Guard with Point
        public Guard(Point location)
        {
            this.Pos = location;
        }

        // Override methods from obstacle class
        public override bool CanOccupy(char cellContent)
        {
            return cellContent == '.' || cellContent == 'c' || cellContent == 's';
        }

        public override void AddToMap(Map map)
        {
            if (Pos.X >= int.MinValue && Pos.X <= int.MaxValue && 
                Pos.Y >= int.MinValue && Pos.Y <= int.MaxValue)
            {
                map.AddPoint('g', Pos);
            }
            else
            {
                throw new Exception("Guard is not on map.");
            }
        }
    }
}
