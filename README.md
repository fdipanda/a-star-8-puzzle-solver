# a-star-8-puzzle-solver
A C# implementation of the A* search algorithm to solve the classic 8-puzzle problem using the Manhattan distance heuristic.

## Overview
This project implements the **A\*** search algorithm to solve the classic **8-puzzle problem**.  
The solver uses the **Manhattan distance heuristic** to efficiently determine the shortest sequence of moves from an initial puzzle configuration to a predefined goal state.

The project demonstrates informed search, heuristic evaluation, and solution path reconstruction using parent tracking.

---

## Problem Description
The 8-puzzle consists of a 3×3 grid with eight numbered tiles and one blank space (represented by `0`).  
The objective is to reach the goal configuration by sliding tiles into the blank space using the fewest possible moves.

---

## Technologies Used
- **Language:** C#
- **Paradigm:** Object-Oriented Programming
- **Algorithm:** A* Search
- **Heuristic:** Manhattan Distance
- **Data Structures:** Priority Queue (`SortedSet`), HashSet

---

## Key Features
- A* search with an admissible heuristic
- Manhattan distance heuristic calculation
- Automatic generation of a random start state
- Parent-based solution path reconstruction
- Console output of intermediate puzzle states

---

## How It Works
Each puzzle state is represented as a node with:
- `g(n)`: cost from the initial state (number of moves)
- `h(n)`: Manhattan distance to the goal
- `f(n) = g(n) + h(n)`: total estimated cost

States are expanded in order of increasing `f(n)` until the goal state is reached.

---

## How to Run

### Using Visual Studio
1. Open the `.sln` file
2. Build the solution
3. Run the project

### Using .NET CLI
```bash
dotnet run
