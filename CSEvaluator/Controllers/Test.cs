namespace CSEvaluator.Controllers
{
    public class Test
    {
        public string title { get; }
        public Category category { get; }
        public string firstInput { get; }
        public string secondInput { get; }
        public string thirdInput { get; }
        public string expect { get; }
        public bool exceptionExpected { get; }
        public string exceptionText { get; }

        public Test(string title, Category category, string firstInput, string secondInput, string thirdInput, string expect,
            bool exceptionExpected, string exceptionText)
        {
            this.title = title;
            this.category = category;
            this.firstInput = firstInput;
            this.secondInput = secondInput;
            this.thirdInput = thirdInput;
            this.expect = expect;
            this.exceptionExpected = exceptionExpected;
            this.exceptionText = exceptionText;
        }

        public object[] GetInput()
        {
            return new object[] { firstInput, secondInput, thirdInput };
        }
    }
}