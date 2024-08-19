using System.Drawing;
using FieldAgent;
using Point = System.Drawing.Point; // Variable for Point Struct

public class Menu 
{
    private Map map;
    private Agent agent;

    public Menu()
    {
        this.map = new Map(); // Initialise the Map Class
        this.agent = new Agent(); // Initialise the Agent Class
    }

    // Create the User Interface
    public void Display() 
    {
        while (true)
        {   
            Console.WriteLine("Select one of the following options:");
            Console.WriteLine("g) Add 'Guard' obstacle");
            Console.WriteLine("f) Add 'Fence' obstacle");
            Console.WriteLine("s) Add 'Sensor' obstacle");
            Console.WriteLine("c) Add 'Camera' obstacle");
            Console.WriteLine("b) Add 'Booby Trap' obstacle");
            Console.WriteLine("d) Show safe directions");
            Console.WriteLine("m) Display obstacle map");
            Console.WriteLine("p) Find safe path");
            Console.WriteLine("x) Exit");

            char code = Console.ReadLine().ToLower()[0];

            // Interpret user input and add to map
            switch (code)
            {   
                // Add 'Guard' Obstacle
                case 'g':
                    Console.WriteLine("Enter the guard's location (X,Y):");
                    var guardCoords = Console.ReadLine().Split(',');
                    var guardLocation = new Point(int.Parse(guardCoords[0]), int.Parse(guardCoords[1]));                    
                    var guard = new Guard(guardLocation);
                    map.AddObstacle(guard);
                    break;

                // Add 'Fence' Obstacle
                case 'f':
                    try
                    {
                        Console.WriteLine("Enter the location where the fence starts (X,Y):");
                        var startCoords = Console.ReadLine().Split(',');
                        Console.WriteLine("Enter the location where the fence ends (X,Y):");
                        var endCoords = Console.ReadLine().Split(',');
                        var start = new Point(int.Parse(startCoords[0]), int.Parse(startCoords[1]));
                        var end = new Point(int.Parse(endCoords[0]), int.Parse(endCoords[1]));
                        var fence = new Fence(start, end);
                        map.AddObstacle(fence);
                    }
                    // Ensure appropriate 'Fence' obstacle
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;
                
                // Add 'Sensor' Obstacle
                case 's':
                    try
                    {
                        Console.WriteLine("Enter the sensor's location (X,Y):");
                        var sensorCoords = Console.ReadLine().Split(',');
                        Console.WriteLine("Enter the sensor's radius:");
                        double radius = double.Parse(Console.ReadLine());
                        var sensor = new Sensor(new Point(int.Parse(sensorCoords[0]), int.Parse(sensorCoords[1])), radius);
                        map.AddObstacle(sensor);
                    }
                     // Ensure appropriate 'Sensor' obstacle
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;

                // Add 'Camera' Obstacle
                case 'c':
                    try
                    {
                        Console.WriteLine("Enter the camera's location (X,Y):");
                        var cameraCoords = Console.ReadLine().Split(',');
                        var cameraLocation = new Point(int.Parse(cameraCoords[0]), int.Parse(cameraCoords[1]));

                        // Add Camera Direction
                        Direction cameraDirection;
                        while (true)
                        {
                            Console.WriteLine("Enter the direction the camera is facing (n, s, e or w):");
                            char dir = Console.ReadLine().ToLower()[0];
                            switch (dir)
                            {
                                case 'n':
                                    cameraDirection = Direction.North;
                                    break;
                                case 's':
                                    cameraDirection = Direction.South;
                                    break;
                                case 'e':
                                    cameraDirection = Direction.East;
                                    break;
                                case 'w':
                                    cameraDirection = Direction.West;
                                    break;
                                default:
                                    Console.WriteLine("Invalid direction.");
                                    continue;
                            }
                            break;
                        }

                        var camera = new Camera(cameraLocation, cameraDirection);
                        map.AddObstacle(camera);
                    }

                    // Ensure appropriate 'Camera' obstacle
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;
                
                // Implementation of Custom Obstacle (Booby Trap)
                case 'b':
                    try
                    {
                        Console.WriteLine("Enter the position of the BoobyTrap (X,Y):");
                        var boobyTrapCoords = Console.ReadLine().Split(',');
                        var boobyTrapLocation = new Point(int.Parse(boobyTrapCoords[0]), int.Parse(boobyTrapCoords[1]));
                        var boobyTrap = new BoobyTrap(boobyTrapLocation); 
                        map.AddObstacle(boobyTrap);
                    }
                    // Ensure appropriate 'Booby Trap' obstacle
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;

                // Display Safe Directions
                case 'd':
                    Console.WriteLine("Enter your current location (X,Y):");
                    var directionalCoords = Console.ReadLine().Split(',');
                    var agentLocation = new Point(int.Parse(directionalCoords[0]), int.Parse(directionalCoords[1]));

                    if (map.Occupied(agentLocation))
                    {
                        Console.WriteLine("Agent, your location is compromised. Abort mission.");
                        continue; // Skip the rest of the loop iteration
                    }

                    agent.CurrentLocation = agentLocation;
                    var safeDirs = agent.SafeDirections(map);

                    if (safeDirs == "You cannot safely move in any direction. Abort mission.")
                    {
                        Console.WriteLine(safeDirs);
                    }
                    else
                    {
                        Console.WriteLine("You can safely take any of the following directions: " + safeDirs);
                    }
                    break;

                // Display Current Map
                case 'm':
                    try
                    {
                        Console.WriteLine("Enter the location of the top-left cell of the map (X,Y):");
                        string[] topLeftInput = Console.ReadLine().Split(',');

                        Console.WriteLine("Enter the location of the bottom-right cell of the map (X,Y):");
                        string[] bottomRightInput = Console.ReadLine().Split(',');

                        if (topLeftInput.Length != 2 || bottomRightInput.Length != 2 ||
                            !int.TryParse(topLeftInput[0], out int topLeftX) || !int.TryParse(topLeftInput[1], out int topLeftY) ||
                            !int.TryParse(bottomRightInput[0], out int bottomRightX) || !int.TryParse(bottomRightInput[1], out int bottomRightY))
                        {
                            Console.WriteLine("Invalid input.");
                        }
                        else if (topLeftX > bottomRightX || topLeftY > bottomRightY)
                        {
                            Console.WriteLine("Invalid map bounds.");
                        }
                        else
                        {
                            map.Display(topLeftX, topLeftY, bottomRightX, bottomRightY);
                        }
                    }
                    // Ensure appropriate 'Map'
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;

                // Display Path to objective
                case 'p':
                    try
                    {
                        Console.WriteLine("Enter your current location (X,Y):");
                        var coords = Console.ReadLine().Split(',');
                        agent.CurrentLocation = new Point(int.Parse(coords[0]), int.Parse(coords[1]));
                        Console.WriteLine("Enter the location of your objective (X,Y):");
                        var objCoords = Console.ReadLine().Split(',');
                        agent.ObjectiveLocation = new Point(int.Parse(objCoords[0]), int.Parse(objCoords[1]));
                        var path = agent.FindSafePathToObjective(map);

                        if (path == "Agent, you are already at the objective.")
                        {
                            Console.WriteLine(path);
                        }
                        else
                        {
                            Console.WriteLine($"The following path will take you to the objective:");
                            Console.WriteLine(path);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;

                // Exit
                case 'x':
                    return;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}

    // Struct for Point in Map Class
    public struct MapPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public MapPoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }




