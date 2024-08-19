using System;
namespace FieldAgent
{
    public abstract class Obstacle
    {   
        // Abstract method to check if content can occupy a cell
        public abstract bool CanOccupy(char cellContent);

        // Abstract method to add to Map
        public abstract void AddToMap(Map map);
    }



}

