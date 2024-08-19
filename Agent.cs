using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace FieldAgent
{
    public class Agent : Map
    {
        public Point CurrentLocation { get; set; }
        public Point ObjectiveLocation { get; set; }

        // Constructor to initialise Agent
        public Agent(Point startingLocation)
        {
            this.CurrentLocation = startingLocation;

            // Check if the agent's location has a negative coordinate
            if (startingLocation.X < 0 || startingLocation.Y < 0)
            {
                Console.WriteLine("Agent, your location is compromised. Abort mission.");
           
            }
        }

        public Agent()
        {
        }
            // Display safe directions to objective if possible
            public void PrintSafeDirections(Map map)
            {
                string safeDirections = SafeDirections(map);

                if (safeDirections != "You cannot safely move in any direction. Abort mission.")
                {
                    Console.WriteLine($"You can safely take the following directions: {safeDirections}");
                }
                else
                {
                    Console.WriteLine(safeDirections);
                }
            }

        // Set point for objective
            public void SetObjective(Point objective)
        {
            this.ObjectiveLocation = objective;
        }

        // Check if direction is clear from obstacles
        public bool IsDirectionClear(Point currentLocation, Direction direction)
        {
            Point nextLocation = new Point(currentLocation.X, currentLocation.Y);  // Create a copy of the current location

            while (true)
            {
                // Check if there's an obstacle in the given direction
                if (IsObstacleInDirection(direction, nextLocation))
                {
                    return false; // Direction is not clear
                }

                // Update location based on direction
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
                return true;
            }
        }

        // Move agent in specific direction
        public void Move(Direction direction)
        {
            // Get a copy of the current location
            Point newLocation = CurrentLocation;

            // Add to the location
            switch (direction)
            {
                case Direction.North:
                    newLocation.Y -= 1;
                    break;
                case Direction.East:
                    newLocation.X += 1;
                    break;
                case Direction.South:
                    newLocation.Y += 1;
                    break;
                case Direction.West:
                    newLocation.X -= 1;
                    break;
            }

            // Create new location
            CurrentLocation = newLocation;
        }

        // Find safe path from current location to objective location
        public string FindSafePathToObjective(Map map)
        {
            // Check if the agent is at the objective with respect to neighbour cells
            if (CurrentLocation == ObjectiveLocation)
            {
                return "Agent, you are already at the objective.";
            }

            var openSet = new HashSet<Point>();
            var closedSet = new HashSet<Point>();
            var cameFrom = new Dictionary<Point, Point>();
            var gScore = new Dictionary<Point, float> { { CurrentLocation, 0 } };
            var fScore = new Dictionary<Point, float> { { CurrentLocation, Heuristic(CurrentLocation, ObjectiveLocation) } };

            openSet.Add(CurrentLocation);

            while (openSet.Count > 0)
            {
                // Find the node with the lowest fScore value
                var current = openSet.OrderBy(point => fScore.ContainsKey(point) ? fScore[point] : float.MaxValue).First();

                if (current == ObjectiveLocation)
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (var neighbor in GetNeighbors(current))
                {
                    if (closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    // Check if the neighbor cell contains an obstacle (guards, cameras, fences, sensors)
                    if (map.Contains('b', neighbor) || map.Contains('c', neighbor) ||
                        map.Contains('f', neighbor) || map.Contains('g', neighbor) || map.Contains('s', neighbor))
                    {
                        continue; // Skip this neighbor as it's an obstacle
                    }

                    var tentative_gScore = (gScore.ContainsKey(current) ? gScore[current] : float.MaxValue) + 1; // assuming all moves have of 1

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else if (tentative_gScore >= (gScore.ContainsKey(neighbor) ? gScore[neighbor] : float.MaxValue))
                    {
                        continue;
                    }

                    // Most efficient path is created and stored
                    if (!cameFrom.ContainsKey(neighbor))
                    {
                        cameFrom[neighbor] = current;
                    }

                    if (!gScore.ContainsKey(neighbor) || gScore[neighbor] > tentative_gScore)
                    {
                        gScore[neighbor] = tentative_gScore;
                    }
                    if (!fScore.ContainsKey(neighbor) || fScore[neighbor] > tentative_gScore + Heuristic(neighbor, ObjectiveLocation))
                    {
                        fScore[neighbor] = tentative_gScore + Heuristic(neighbor, ObjectiveLocation);
                    }
                }
            }

            return "There is no safe path to the objective.";
        }

        // Get points for possible moves
        private IEnumerable<Point> GetNeighbors(Point current)
        {
            var neighbors = new List<Point>
                {
                    new Point(current.X + 1, current.Y),
                    new Point(current.X - 1, current.Y),
                    new Point(current.X, current.Y + 1),
                    new Point(current.X, current.Y - 1)
                };

            return neighbors.Where(point => IsDirectionClear(current, ToDirection(current, point)));
        }

        // Convert points to directions
        private Direction ToDirection(Point from, Point to)
        {
            Size difference = new Size(to.X - from.X, to.Y - from.Y);

            if (difference.Width == 1) return Direction.East;
            if (difference.Width == -1) return Direction.West;
            if (difference.Height == 1) return Direction.South;
            if (difference.Height == -1) return Direction.North;

            throw new ArgumentException("Invalid direction", nameof(difference));
        }

        // Reconstruct path into dictionary
        private string ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
        {
            var path = new List<Direction>();
            while (cameFrom.ContainsKey(current))
            {
                var previous = cameFrom[current];
                path.Add(ToDirection(previous, current));
                current = previous;
            }

            path.Reverse();
            return string.Concat(path.Select(dir => dir.ToString().Substring(0, 1))); // Create Directional String as per GradeScope Tests
        }

        private float Heuristic(Point start, Point goal)
        {
            // Using Manhattan distance to calculate path
            return Math.Abs(start.X - goal.X) + Math.Abs(start.Y - goal.Y);
        }

        // Return safe directions
        public string SafeDirections(Map map)
        {
            List<char> safeDirections = new List<char>();

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                Point nextLocation = CurrentLocation;

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

                if (CanMove(direction, map) && !map.IsObstacleInDirection(direction, CurrentLocation))
                {
                    switch (direction)
                    {
                        case Direction.North:
                            safeDirections.Add('N');
                            break;
                        case Direction.East:
                            safeDirections.Add('E');
                            break;
                        case Direction.South:
                            safeDirections.Add('S');
                            break;
                        case Direction.West:
                            safeDirections.Add('W');
                            break;
                    }
                }
            }

            if (safeDirections.Count == 0)
            {
                return "You cannot safely move in any direction. Abort mission.";
            }

            return string.Join("", safeDirections);
        }

        // Check if agent can move in a direction
        public bool CanMove(Direction direction, Map map)
        {
            Point nextLocation = CurrentLocation;

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

            return !map.IsObstacleInDirection(direction, CurrentLocation);
        }

    }

    public enum Direction
    {
        North,
        East,
        South,
        West
    }
    

}
