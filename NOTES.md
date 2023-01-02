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
    - Constructor & dispose.
    - Class fixture.
    - Collection fixture.
    - Integrating test context with ASP.NET Core's dependency injection system.
    - Categorizing tests. Skipping tests.