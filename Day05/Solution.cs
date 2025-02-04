using AocHelper;

namespace Day05;

internal static partial class Program
{
  private const string Title = "\n## Day 5: Print Queue ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/5";
  private const long ExpectedPartOne = 5948;
  private const long ExpectedPartTwo = 3062;

  private static List<int[]> _failedSets = [];

  private static long PartOne(string input)
  {
    _failedSets = [];
    var (rules, pageSets) = GetData(input);

    long tally = 0;
    foreach (var pages in pageSets) {
      var pageCount = pages.Length;
      var currentPage = 0;
      var isValid = true;
      while (currentPage < pageCount - 1 && isValid) {
        for (var i = currentPage + 1; i < pageCount; i++) {
          if (SatisfiesRules(rules, pages[currentPage], pages[i]))
            continue;
          
          _failedSets.Add(pages);
          isValid = false;
          break;
        }

        if (isValid)
          currentPage++;
      }

      if (isValid)
        tally += pages[pageCount / 2];
    }

    return tally;
  }

  private static long PartTwo(string input)
  {
    var (rules, _) = GetData(input);
    long tally = 0;

    foreach (var pages in _failedSets) {
      var pageCount = pages.Length;
      var currentPage = 0;

      while (currentPage < pageCount - 1) {
        for (var i = currentPage + 1; i < pageCount; i++) {
          if (SatisfiesRules(rules, pages[currentPage], pages[i]))
            continue;

          (pages[i], pages[currentPage]) = (pages[currentPage], pages[i]);
          // reset and start pageSet again
          currentPage = 0;
        }

        currentPage++;
      }

      tally += pages[pageCount / 2];
    }


    return tally;
  }

  private static bool SatisfiesRules(Dictionary<int, List<int>> orderings, int first, int second)
  {
    return !orderings.TryGetValue(second, out var value) || !value.Contains(first);
  }

  private static (Dictionary<int, List<int>> rules, List<int[]> pagesets) GetData(string input)
  {
    var data = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

    var ruleSets = data[0].Split('\n', StringSplitOptions.RemoveEmptyEntries);
    var updates = data[1].Split('\n', StringSplitOptions.RemoveEmptyEntries);

    Dictionary<int, List<int>> rules = [];
    foreach (var ruleSet in ruleSets) {
      var key = ruleSet[..2].ToInt();
      var value = ruleSet[3..].ToInt();
      if (!rules.TryAdd(key, [value]))
        rules[key].Add(value);
    }

    List<int[]> pageSets = [];
    pageSets.AddRange(updates.Select(update => update.Split(",").ToIntArray()));

    return (rules, pageSets);
  }
}
