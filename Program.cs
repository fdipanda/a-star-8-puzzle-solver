using System;
using System.Collections.Generic;
using System.Linq;

public class Puzzle
{
    public int[,] Matrix { get; private set; }
    private int blankRow, blankCol;
    public Puzzle Parent { get; private set; }
    public string Move { get; private set; }
    public int Cost { get; private set; }
    public int Heuristic { get; set; }

    public Puzzle(int[,] matrix, Puzzle parent = null, string move = "", int cost = 0, int heuristic = 0)
    {
        Matrix = (int[,])matrix.Clone();
        FindBlank();
        Parent = parent;
        Move = move;
        Cost = cost;
        Heuristic = heuristic;
    }

    private void FindBlank()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (Matrix[i, j] == 0)
                {
                    blankRow = i;
                    blankCol = j;
                    return;
                }
            }
        }
    }

    public int ComputeManhattanDistance(int[,] goal)
    {
        int distance = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (Matrix[i, j] != 0)
                {
                    for (int gi = 0; gi < 3; gi++)
                    {
                        for (int gj = 0; gj < 3; gj++)
                        {
                            if (goal[gi, gj] == Matrix[i, j])
                            {
                                distance += Math.Abs(i - gi) + Math.Abs(j - gj);
                                break;
                            }
                        }
                    }

                }
            }
        }
        return distance;
    }

    public List<Puzzle> GetPossibleMoves(int[,] goal)
    {
        var moves = new List<Puzzle>();
        var directions = new[] { (1, 0, "Down"), (-1, 0, "Up"), (0, 1, "Right"), (0, -1, "Left") };

        foreach (var (dr, dc, moveName) in directions)
        {
            int newRow = blankRow + dr;
            int newCol = blankCol + dc;

            if (newRow >= 0 && newRow < 3 && newCol < 3 && newCol >= 0)
            {
                var newMatrix = (int[,])Matrix.Clone();
                (newMatrix[blankRow, blankCol], newMatrix[newRow, newCol]) = (newMatrix[newRow, newCol], newMatrix[blankRow, blankCol]);
                int newCost = Cost + 1;
                int newHeuristic = ComputeManhattanDistance(goal);
                moves.Add(new Puzzle(newMatrix, this, moveName, newCost, newHeuristic));
            }
        }
        return moves;
    }
    public List<string> GetSolutionPath()
    {
        var path = new List<string>();
        var current = this;
        while (current.Parent != null)
        {
            path.Insert(0, current.Move); 
            current = current.Parent;
        }
        return path;
    }


    public static void PrintMatrix(int[,] matrix)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.Write(matrix[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}

public class SolvePuzzle
{
    public (bool success, List<string> moves, List<int[,]> states) UseAStar(int[,] initialMatrix, int[,] goalMatrix)
    {
        var initial = new Puzzle(initialMatrix, null, "", 0, 0);
        initial.Heuristic = initial.ComputeManhattanDistance(goalMatrix);
        var pq = new SortedSet<Puzzle>(Comparer<Puzzle>.Create((a, b) =>
        {
            int costA = a.Cost + a.Heuristic;
            int costB = b.Cost + b.Heuristic;
            return costA == costB ? a.GetHashCode().CompareTo(b.GetHashCode()) : costA.CompareTo(costB);
        }));

        var visited = new HashSet<string>();
        var stateHistory = new List<int[,]>();
        pq.Add(initial);

        while (pq.Count > 0)
        {
            var current = pq.Min;
            pq.Remove(current);
            stateHistory.Add(current.Matrix);

            if (current.ComputeManhattanDistance(goalMatrix) == 0)
            {
                return (true, current.GetSolutionPath(), stateHistory);
            }

            foreach (var move in current.GetPossibleMoves(goalMatrix))
            {
                string moveKey = string.Join(",", move.Matrix.Cast<int>());
                if (!visited.Contains(moveKey))
                {
                    visited.Add(moveKey);
                    pq.Add(move);
                }
            }
        }
        return (false, null, stateHistory);
    }
}

class Program
{
    static void ShowSolution(List<int[,]> states, List<string> moves)
    {
        Console.WriteLine("Solution sequence:");
        Console.WriteLine();

        for (int i = 0; i < states.Count; i++)
        {
            if (i == 0)
            {
                Console.WriteLine("Initial State:");
            }
            else if (i - 1 < moves.Count) 
            {
                Console.WriteLine($"After {moves[i - 1]}:");
            }

            Puzzle.PrintMatrix(states[i]);
        }
    }


    static int[,] GenerateRandomPuzzle()
    {
        var rand = new Random();
        var numbers = Enumerable.Range(0, 9).OrderBy(x => rand.Next()).ToArray();
        int[,] matrix = new int[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                matrix[i, j] = numbers[i * 3 + j];
            }
        }
        return matrix;
    }

    static void Main()
    {
        int[,] goalMatrix = {
            { 1, 2, 3 },
            { 8, 0, 4 },
            { 7, 6, 5 }
        };

        Console.WriteLine("Goal State:");
        Puzzle.PrintMatrix(goalMatrix);

        int[,] randomStart = GenerateRandomPuzzle();
        Console.WriteLine("Random Starting State:");
        Puzzle.PrintMatrix(randomStart);

        SolvePuzzle solver = new SolvePuzzle();
        var (success, moves, states) = solver.UseAStar(randomStart, goalMatrix);

        if (success)
        {
            Console.WriteLine("Solved!");
            ShowSolution(states, moves);
        }
        else
        {
            Console.WriteLine("A* failed to find a solution.");
        }
    }
}
