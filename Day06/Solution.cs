using AocHelper;

namespace Day06;

internal static partial class Program
{
  private const string Title = "\n## Day 6: Guard Gallivant ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/6";
  private const long ExpectedPartOne = 4454;
  private const long ExpectedPartTwo = 1503;

  private static Dictionary<(int, int), (int, int)> Part1Visited = [];
  private static HashSet<(int, int, int, int)> Part2Visited = [];
  private static HashSet<(int, int, int, int)> NewVisited = [];


  private static long PartOne(string input)
  {
    var (map, (x, y)) = ProcessData(input);

    Part1Visited = [];
    Part2Visited = [];
    NewVisited = [];
    int dy = -1;
    int dx = 0;

    while (true) {
      Part1Visited.TryAdd((x, y), (dx, dy));
      var (nx, ny) = (x + dx, y + dy);
      if (!IsInBounds(nx, ny, map))
        break;
      if (map[ny][nx] == '#')
        (dx, dy) = (-dy, dx);
      else
        (x, y) = (nx, ny);
    }
    return Part1Visited.Count;
  }

  private static long PartTwo(string input)
  {
    var (map, start) = ProcessData(input);
    var (x, y) = start;
    int dx = 0;
    int dy = -1;

    long tally = 0;

    Part2Visited.Add((x, y, dx, dy));

    foreach (var ((nx, ny), (ndx, ndy)) in Part1Visited) {
      if ((x, y) == (nx, ny))
        continue;

      map[ny][nx] = '#';
      if (HasInfiniteLoop(x, y, dx, dy, map)) {
        tally++;
      }

      map[ny][nx] = '.';
      (x, y, dx, dy) = (nx, ny, ndx, ndy);
      Part2Visited.Add((x, y, dx, dy));
    }

    return tally;
  }

  private static bool HasInfiniteLoop(int x, int y, int dx, int dy, char[][] map)
  {
    NewVisited.Clear();

    while (true) {
      NewVisited.Add((x, y, dx, dy));
      var (nx, ny) = (x + dx, y + dy);
      if (!IsInBounds(nx, ny, map))
        return false;
      if (map[ny][nx] == '#')
        (dx, dy) = (-dy, dx);
      else
        (x, y) = (nx, ny);

      if (Part2Visited.Contains((x, y, dx, dy)) || NewVisited.Contains((x, y, dx, dy)))
        return true;
    }
  }

  private static bool IsInBounds(int x, int y, char[][] map)
  {
    return 0 <= x && x < map.Length && 0 <= y && y < map[0].Length;
  }

  private static (char[][] map, (int x, int y) start) ProcessData(string input)
  {
    var map = input.To2DCharArray();

    var height = map.Length;
    var width = map[0].Length;
    for (var y = 0; y < height; y++)
      for (var x = 0; x < width; x++)
        if (map[y][x] == '^') {
          return (map, (x, y));
        }

    throw new ApplicationException("Did not find starting location '^'");
  }
}
