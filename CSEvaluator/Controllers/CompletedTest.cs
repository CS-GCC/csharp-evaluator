using Newtonsoft.Json.Serialization;

namespace CSEvaluator.Controllers
{
    public class CompletedTest : Test
    {
        public CompletedTest(Test test, bool passed, string errorMessage) : base(test.Title, test.Category,
            test.FirstInput, test.SecondInput, test.ThirdInput, test.Expect, test.ExceptionExpected, test.ExceptionText)
        {
            this.passed = passed;
            message = errorMessage;
        }

        public bool passed { get; }

        public string message { get; }
    }
}