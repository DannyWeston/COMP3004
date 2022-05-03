using System.Buffers.Binary;

namespace MazeGenerator
{
    public struct MazeCell
    {
        public bool Top = false;
        public bool Left = false;
        public bool Bottom = false;
        public bool Right = false;
        public bool Visited = false;

        public MazeCell(bool top, bool left, bool bottom, bool right, bool visited)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
            Visited = visited;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            int width, height;

            // Parse width
            if (!int.TryParse(args[0], out width)) {
                Console.WriteLine("Couldn't parse maze width");
                return;
            }
            
            // Parse height
            if (!int.TryParse(args[1], out height))
            {
                Console.WriteLine("Couldn't parse maze height");
                return;
            }

            var grid = RecursiveMaze(width, height);

            //WriteMazeToFile("C:\\Users\\danie\\Desktop\\test.maze", width, height, grid);
        }

        public static MazeCell[] RecursiveMaze(int width, int height, int start = 0)
        {
            // Generate maze
            // Assume starting point is 0,0 for all grids for now
            MazeCell[] grid = new MazeCell[width * height];

            grid[start].Top = true;
            grid[start].Bottom = true;
            grid[start].Left = true;
            grid[start].Right = true;
            grid[start].Visited = true;

            Stack<int> stack = new Stack<int>();
            stack.Push(start);

            Random random = new Random(0);
            int current, chosen;

            List<int> neighbours;

            // Loop while unvisited nodes
            while (stack.Count != 0)
            {
                current = stack.Peek();

                neighbours = new();

                // Generate the current cell's neighbours

                if (0 <= current - width && !grid[current - width].Visited) // Top
                    neighbours.Add(current - width);

                if (0 != current % width && !grid[current - 1].Visited) // Left
                    neighbours.Add(current - 1);

                if (current + width < width * height && !grid[current + width].Visited) // Bottom
                    neighbours.Add(current + width);

                if (0 != (current + 1) % width && !grid[current + 1].Visited) // Right
                    neighbours.Add(current + 1);

                // No neighbours means pop the top of the stack and try again
                if (neighbours.Count == 0)
                {
                    stack.Pop();
                    continue;
                }

                // Choose a random valid neighbour
                chosen = neighbours[random.Next(neighbours.Count)];

                grid[chosen].Top = true;
                grid[chosen].Bottom = true;
                grid[chosen].Left = true;
                grid[chosen].Right = true;
                grid[chosen].Visited = true;

                if (chosen == current - width)
                {
                    grid[current].Top = false;
                    grid[chosen].Bottom = false;
                }
                else if (chosen == current + width)
                {
                    grid[chosen].Top = false;
                    grid[current].Bottom = false;
                }
                else if (chosen == current - 1)
                {
                    grid[current].Left = false;
                    grid[chosen].Right = false;
                }
                else
                {
                    grid[chosen].Left = false;
                    grid[current].Right = false;
                }

                // Push chosen onto the stack
                stack.Push(chosen);
            }

            return grid;
        }

        private static void WriteMazeToFile(string path, int width, int height, int[] grid)
        {
            byte[] data = new byte[8 + width * height * 4];
            Span<byte> span = new(data);

            BinaryPrimitives.WriteInt32LittleEndian(span.Slice(0, 4), width);
            BinaryPrimitives.WriteInt32LittleEndian(span.Slice(4, 4), height);

            for (int i = 0; i < grid.Length; i++)
            {
                BinaryPrimitives.WriteInt32LittleEndian(span.Slice(8 + i * 4, 4), grid[i]);
            }

            // Flush to disk
            File.WriteAllBytes(path, data);
        }
    }
}