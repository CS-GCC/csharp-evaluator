namespace CSEvaluator.Controllers
{
    public class Test
    {
        public string Title { get; }
        public Category Category { get; }
        public string FirstInput { get; }
        public string SecondInput { get; }
        public string ThirdInput { get; }
        public string Expect { get; }
        public bool ExceptionExpected { get; }
        public string ExceptionText { get; }

        public Test(string title, Category category, string firstInput, string secondInput, string thirdInput, string expect,
            bool exceptionExpected, string exceptionText)
        {
            Title = title;
            Category = category;
            FirstInput = firstInput;
            SecondInput = secondInput;
            ThirdInput = thirdInput;
            Expect = expect;
            ExceptionExpected = exceptionExpected;
            ExceptionText = exceptionText;
        }

        public object[] GetInput()
        {
            return new object[] { FirstInput, SecondInput, ThirdInput };
        }
    }
}