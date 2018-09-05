using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.IO;
using System.Web.Http.Cors;

namespace CSEvaluator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class EvaluateController : ControllerBase
    {

        private readonly List<Test> _tests;

        //public EvaluateController(List<Test> tests)
        //{
        //    _tests = tests;
        //}

        public EvaluateController()
        {
            _tests = GenerateTests();
        }

        private static List<Test> GenerateTests()
        {
            try
            {
                TestService testEngine = new TestService();
                int randomStringLength = testEngine.utils.RandomInt(6, 20);
                String optionalArgs = (testEngine.utils.RandomInt(0, randomStringLength / 2)).ToString();
                String inputString = testEngine.utils.RandomString(randomStringLength);
                char randomChar = testEngine.utils.RandomChar();
                String sorterString = testEngine.utils.getSorterString();

                return new List<Test>
                {
                    new Test("shuffle should throw error if action is not an integer", Category.BASIC, "",
                        "number", "", "invalid action type", true,
                        "invalid action type"),
                    new Test("shuffle should throw error is action is outside 1 and 4", Category.BASIC, "",
                        "5", "", "action is out of range", true,
                        "action is out of range"),
                    new Test("when action is 1 move number of chars specified in optionalArgs" +
                        " from the end of inputString to beginning", Category.MEDIUM, inputString, "1", "3",
                        testEngine.MoveToBeginning(inputString, optionalArgs), false, ""),
                    new Test("when action is 2 reverse a string", Category.MEDIUM, inputString, "2", "",
                        testEngine.ReverseString(inputString), false, ""),
                    new Test("when action is 3 return char with max occurences", Category.MEDIUM, inputString,
                        "3", "", testEngine.GetMaxOccurrence(testEngine.groupBy(inputString.ToList())).ToString(), false, ""),
                    new Test("when action is 4 sort the string as per sorting order in the third parameter",
                             Category.DIFFICULT, inputString.ToLower(),"4", sorterString,
                        testEngine.SortString(sorterString, inputString), false, "")
                };
            }
            catch (Exception)
            {
                return GenerateTests();
            }
        }

        public TestResult TestResult(string input)
        {
            var completedTests = new List<CompletedTest>();

            var successfullyCompiled = CompileInput(input, out var emitResult, out var ms);

            if (successfullyCompiled)
            {
                var type = PrepareForExecution(ms, out var obj);
                foreach (var test in _tests)
                {
                    var passed = RunTest(out var completedTest, test, type, obj);
                    completedTests.Add(completedTest);
                    if (!passed)
                    {
                        break;
                    }
                }
            }
            else
            {
                completedTests.Add(new CompletedTest(_tests[0], false, GetErrorMessage(emitResult)));
            }

            var testResult = new TestResult(completedTests, _tests.Count);
            return testResult;
        }

        private static string GetErrorMessage(EmitResult emitResult)
        {
            var failures = emitResult.Diagnostics.Where(diagnostic =>
                diagnostic.Severity == DiagnosticSeverity.Error);
            var failure = failures.First();
            return failure.ToString();
        }

        private static Type PrepareForExecution(MemoryStream ms, out object obj)
        {
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());

            var type = assembly.GetType("Answer");
            obj = Activator.CreateInstance(type);

            return type;
        }

        private static bool RunTest(out CompletedTest completedTest, Test test, Type type, object obj)
        {
            try
            {
                var result = (string)type.InvokeMember("Shuffle",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null,
                    obj,
                    test.GetInput());
                completedTest = result.SequenceEqual(test.Expect)
                    ? new CompletedTest(test, true, "Test passed")
                    : new CompletedTest(test, false,
                        "Expected " + test.Expect + ", Got " +
                          result);
            }
            catch (TargetInvocationException e)
            {
                if (test.ExceptionExpected)
                {
                    completedTest = test.ExceptionText.Equals(e.InnerException.Message)
                        ? new CompletedTest(test, true, "Test Passed")
                        : new CompletedTest(test, false,
                            "Expected exception with message: " + test.ExceptionText + ", but got message: " +
                            e.InnerException.Message);
                }
                else
                {
                    completedTest = new CompletedTest(test, false,
                        "Got an exception. Message: " + e.InnerException.Message);
                }
            }
            catch (MissingMethodException e)
            {
                completedTest =
                    new CompletedTest(test, false, "Got an exception. Message: " + e.Message + " Shuffle expects parameters string, string, string and has return type string.");
            }
            catch (Exception e)
            {
                completedTest =
                    new CompletedTest(test, false, "Got an exception. Message: " + e.Message);
            }

            return completedTest.passed;
        }

        private static bool CompileInput(string input, out EmitResult emitResult, out MemoryStream ms)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(input);

            var assemblyName = Path.GetRandomFileName();
            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            var compilation = CSharpCompilation.Create(
                assemblyName,
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            ms = new MemoryStream();
            emitResult = compilation.Emit(ms);

            return emitResult.Success;
        }

        // POST api/evaluate
        [HttpPost]
        public string Post()
        {

            var input = new StreamReader(Request.Body).ReadToEnd();

            var testResult = TestResult(input);

            return Newtonsoft.Json.JsonConvert.SerializeObject(testResult);
        }

    }
}
