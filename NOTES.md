## Unit Testing an ASP.NET Core 6 MVC Web Application
- by Kevin Dockx
    - Unit testing your ASP.NET Core 6 MVC web application helps with improving its reliability.
    - This course will teach you the ins and outs of unit testing with xUnit in detail.

- OVERVIEW:
    - xUnit. Arrange, act, and assert pattern. Setting up. MVC-specific concerns. Integrating.

- INTRODUCTION TO UNIT TESTING:
    - Prerequisites and frameworks: C# 10. ASP.NET Core 6 MVC. Visual Studio 2022.
        - During installation: Check "ASP.NET and web development." .NET 6.0 SDK.
    - [GitHub](https://github.com/KevinDockx/UnitTestingAspNetCore6MVC)
    - Unit test: An automated test that validates a small piece of behavior.
        - Often just a part of a method of a class. With potentially testing functionally-related behavior across classes.
            - Low complexity. Fast. Well-encapsulated.
        - Reasons: Bugs are found faster and easier. And cheaper to fix.
        - Comparing unit tests, integration tests, and functional (end-to-end) tests.
        - Integration test: An automated test that validates whether or not two or more components work together correctly.
            - Less isolated. Medium complexity. Not too encapsulated.
        - Functional tests: An automated test that validates the full request/response cycle of an application.
            - Selenium (web applications.)
            - Postman (APIs)
            - Microsoft TestHost & TestServer.
            - High complexity. Slow. Poorly encapsulated.
    - Add xUnit Test Project. Avoid common using statements with ImplicitUsings:
        ```xml
            <PropertyGroup>
                <TargetFramework>net6.0</TargetFramework>
                <Nullable>enable</Nullable>
                <ImplicitUsings>true</ImplicitUsings>
                <IsPackable>false</IsPackable>
            </PropertyGroup>
        ```
    - Good and bad candidates for a unit test:
        - Good: Algorithms. Behavior. Rules. Bad: Data access. UI. System interaction(s).
    - Naming guidelines for unit tests:
        - e.g.: A_B_C
            - A.) A name for the unit that's being tested.
            - B.) The scenario under which the unit is being tested.
            - C.) The expected behavior when the scenario is invoked.
    - Arrange, Act, Assert pattern. Three different sections.
    
- TACKLING BASIC UNIT TESTING SCENARIOS:
    - Assertions: A boolean expression that should evaluate to true.
        - A test can contain 1:M asserts. Multiple assertions are acceptable if they assert the same behavior.
    - Core unit testing senarios:
        - Async code: 
            - Asserting on Exceptions. NOTES:
                - 100 is the minimum raise.
                - A minimum raise cannot be awarded twice in a row.
                - Throws EmployeeInvalidRaiseException.
            - When your Assert is async, you need to await it. Otherwise, the resulting Task is not returned and cannot be inacted upon.
            - ThrowsAny(Async)<T> takes derived versions into consideration, while Throws(Async)<T> does not.
            - Asserting on Events:
            - Asserting on private methods: Do not test this. This detail does not exist in isolation.
                - Test the method of the behavior of the method that uses the private method.
                - Do not break encapsulation. Use [InternalsVisible] as a poor alternative.
    - SUMMARY:

- SETTING UP TESTS & CONTROLLING TEST EXECUTION:
    - Patterns within the 'arrange.'
        - Constructor & dispose: Set up test context in the constructor, potentially clean up in Dispose method.
            - Context is recreated for each test. Test class inctance is not shared. Can be slow.
        - Class fixture: Create a single instance, shared across the class and cleaned-up after the class is disposed.
            - Use when context creation and clean-up is expensive.
            - However, don't let a test depend on changes made to the context by other tests. 
            - Test must remain isolated. You don't have control over the order in which tests are run.
        - Collection fixture: Create a single test context shared among tests in several test classes.
            - Context is cleaned up after all tests across classes have completed.
            - Use when context creation and clean-up is expensive.
    - Integrating test context with ASP.NET Core's dependency injection system.
        - In ASP.NET Core, dependencies are often resolved via the built-in IoC container. 
        - Can this be integrated with a unit test?
            - Newing up dependencies is the prefered approach. Simple. Fast. Concise.
            - You *might* want to integrate with the DI system:
                - If the class has got a lot of dependencies. If the dependency tree is large.
    - Categorizing tests:
        - Out of the box, tests are grouped by class.
    - Skipping tests.

- WORKING WITH DATA-DRIVEN TESTS:
    - Theories versus facts.
        - e.g.: Additional course would mean: (1) Writting additional tests. (2) Channging the existing tests.
        - Fact: A test which is always true. Testing invariant conditions.
        - Theory: A test which is only true for a particular set of data.
    - Data-driven tests:
        - Inline data:
        - Member data: Data via a property or method. 
            - e.g.: Note the static property and the return type signature.
            ```csharp
                public static IEnumerable<object[]> ExampleTestDataFroGiveRaise_WithProperty
                {
                    get
                    {
                        return new List<object[]>
                        {
                            new object[] { 100, true },
                            new object[] { 200, false }
                        };
                    }
                }

                [Theory()]
                [MemberData(nameof(ExampleTestDataFroGiveRaise_WithProperty))]
                public async Task GiveRaise_RaiseGiven_EmployeeRaiseGivenMatchesValue_Async(
                    int raise, bool isRaise)
                { }
            ```
        - Class data: Data provided via an external class.
        - Type-safe approach: use [TheoryData()] for type-safe data.
            - Abstract base: TheoryData<T1, T2... > Providing an Add(T1, T2...) method.
    - Getting Data From An External Source:
        - Providing test data to our theories. Others can manage it. (QA.)
        - e.g.: CSV file. NOTE: Copy to Output Directory: Copy Always.
    - SUMMARY:

- ISOLATING UNIT TESTS WITH ASP.NET CORE TECHNIQUES & MOCKING:
    - Isolated from other system components: Database. File system. Network.
        - Pass/fail should only be related to the couse of the code under test.
    - Test doubles: A generic term for any case where you replace a production object for testing purpose(s).
        - Fakes: A working implementation not suitable for production use. e.g.: SQLLite In-Memory.
        - Dummies: A test double that is never ccessed or used. e.g.: Newing up an instance and not using it. (via constructor)
        - Stubs: A test double that provides fake data to the SUT. e.g.: An "employee."
        - Spies: A test double capable of capturing indirect output and providing indirect input as needed. 
            - View this as a subclass of the class being tested which adds behavior before/after.
        - Mocks: A test double that implements the expected behavior.
    - Isolation approaches: (Different approaches are sometimes combined.)
        - Manually creating test doubles.
        - Using built-in framework or library functionality to create test doubles.
        - Using a mocking framework to create test dumies.
    - Isolation with EF Core:
        - EF Core: Calling into a database. In-memory to avoid i/o.
            - In-memory provider used for simple scenarios. Does not behave like a real DB in many ways. Discouraged to use.
            - Use SQLite in-memory mode. Best compatibility.
    - Issolation with HTTP client.
        - Network calls must be isolated. A custom message handler can short-circuit the actual call.
        - e.g.: client -> request messsage -> api -> response message -> client. All via the message handler.
    - Defacto: Moq. With Mock Object. Interface. And Async.
        - M
    - Use cases.