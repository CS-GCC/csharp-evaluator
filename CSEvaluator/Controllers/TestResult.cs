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

		private static int mapDifficulty(Category difficulty) 
		{
			switch (difficulty) {
				case (Category.BASIC):
					return 0;
				case (Category.MEDIUM):
					return 1;
				case (Category.DIFFICULT):
					return 2;
				default:
					return 3;
			}
		}

        private static Dictionary<int, List<CompletedTest>> CreateDictionary(List<CompletedTest> completedTests)
        {
			
            var categories = completedTests.Select(test => test.category).Distinct();
            var dictionary = new Dictionary<int, List<CompletedTest>>();
            foreach (var category in categories)
            {
                var tests = completedTests.FindAll(test => test.category.Equals(category));
				dictionary.Add(mapDifficulty(category), tests);
            }

            return dictionary;
        }

        public Dictionary<int, List<CompletedTest>> completedTests { get; }
        public bool allTestsComplete { get; }

    }
}