using System;
using System.Drawing;

namespace FieldAgent
{
    public class BoobyTrap : Obstacle
    {
        // Store location of Booby Trap
        public Point Location { get; set; }

        // Constructor to initialise Booby Trap with Point
        public BoobyTrap(Point location)
        {
            this.Location = location;
        }

        // Override methods from base class
        public override void AddToMap(Map map)
        {
            if (Location.X >= int.MinValue && Location.X <= int.MaxValue &&
                Location.Y >= int.MinValue && Location.Y <= int.MaxValue)
            {
                AddTrapToMap(map);
            }
            else
            {
                throw new Exception("Booby Trap location is out of map bounds.");
            }
        }

        public override bool CanOccupy(char symbol)
        {
            return symbol == 'b';
        }

        // Method to add the BoobyTrap to the map
        private void AddTrapToMap(Map map)
        {
            map.AddPoint('b', Location);
        }
    }
}
