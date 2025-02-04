using AocHelper;

namespace Day04;

internal static partial class Program
{
  private const string Title = "\n## Day 4: Ceres Search ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/4";
  private const long ExpectedPartOne = 2685;
  private const long ExpectedPartTwo = 2048;

  private static long PartOne(string input)
  {
    var map = input.To2DCharArray();
    var height = map.Length;
    var width = map[0].Length;
    long tally = 0;

    for (var y = 0; y < height; y++)
      for (var x = 0; x < width; x++) {
        if (map[y][x] != 'X')
          continue;

        foreach (var dy in Enumerable.Range(-1, 3))
          foreach (var dx in Enumerable.Range(-1, 3)) {
            if (dy == 0 && dx == 0)
              continue;
            if (!IsInBounds(x + 3 * dx, y + 3 * dy, height, width))
              continue;

            if (map[y + dy][x + dx] == 'M'
                && map[y + 2 * dy][x + 2 * dx] == 'A'
                && map[y + 3 * dy][x + 3 * dx] == 'S')
              tally++;
          }
      }

    return tally;
  }

  private static long PartTwo(string input)
  {
    var map = input.To2DCharArray();
    var height = map.Length;
    var width = map[0].Length;
    long tally = 0;

    foreach (var y in Enumerable.Range(1, height - 2))
      foreach (var x in Enumerable.Range(1, width - 2))
        if (map[y][x] == 'A' && Has_Xmas(x, y, map)) {
          tally++;
        }

    return tally;
  }

  private static bool Has_Xmas(int x, int y, char[][] map)
  {
    return ((AboveLeft(x, y, 'M', map) && BelowRight(x, y, 'S', map))
            || (AboveLeft(x, y, 'S', map) && BelowRight(x, y, 'M', map)))
           && ((AboveRight(x, y, 'M', map) && BelowLeft(x, y, 'S', map))
               || (AboveRight(x, y, 'S', map) && BelowLeft(x, y, 'M', map)));
  }

  private static bool AboveLeft(int x, int y, char ch, char[][] map)
  {
    return map[y - 1][x - 1] == ch;
  }

  private static bool AboveRight(int x, int y, char ch, char[][] map)
  {
    return map[y - 1][x + 1] == ch;
  }

  private static bool BelowLeft(int x, int y, char ch, char[][] map)
  {
    return map[y + 1][x - 1] == ch;
  }

  private static bool BelowRight(int x, int y, char ch, char[][] map)
  {
    return map[y + 1][x + 1] == ch;
  }

  private static bool IsInBounds(int x, int y, int height, int width)
  {
    return 0 <= x && x < width && 0 <= y && y < height;
  }
}
