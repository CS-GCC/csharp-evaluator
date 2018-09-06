using Newtonsoft.Json.Serialization;

namespace CSEvaluator.Controllers
{
    public class CompletedTest : Test
    {
        public CompletedTest(Test test, bool passed, string errorMessage) : base(test.title, test.category,
            test.firstInput, test.secondInput, test.thirdInput, test.expect, test.exceptionExpected, test.exceptionText)
        {
            this.passed = passed;
            message = errorMessage;
        }

        public bool passed { get; }

        public string message { get; }
    }
}