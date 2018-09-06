using System.Collections.Generic;
using System.Linq;

namespace CSEvaluator.Controllers
{
    public class TestResult
    {
        public TestResult(List<CompletedTest> completedTests, int testsCount)
        {
            this.completedTests = CreateDictionary(completedTests);
            allTestsComplete = IsAllTestsComplete(completedTests, testsCount);
        }

        private static bool IsAllTestsComplete(List<CompletedTest> completedTests, int testsCount)
        {
            return testsCount == completedTests.Count && completedTests.All(completedTest => completedTest.passed);
        }

        private static Dictionary<string, List<CompletedTest>> CreateDictionary(List<CompletedTest> completedTests)
        {
            var categories = completedTests.Select(test => test.category).Distinct();
            var dictionary = new Dictionary<string, List<CompletedTest>>();
            foreach (var category in categories)
            {
                var tests = completedTests.FindAll(test => test.category.Equals(category));
                dictionary.Add(category.ToString(), tests);
            }

            return dictionary;
        }

        public Dictionary<string, List<CompletedTest>> completedTests { get; }
        public bool allTestsComplete { get; }

    }
}