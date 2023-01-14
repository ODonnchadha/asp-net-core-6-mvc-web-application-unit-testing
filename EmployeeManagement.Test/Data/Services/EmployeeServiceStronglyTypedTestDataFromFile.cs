using Xunit;

namespace EmployeeManagement.Test.Data.Services
{
    public class EmployeeServiceStronglyTypedTestDataFromFile : TheoryData<int, bool>
    {
        public EmployeeServiceStronglyTypedTestDataFromFile()
        {
            var lines = File.ReadAllLines("Data/External/EmployeeServiceTestData.csv");

            foreach(var l in lines)
            {
                var split = l.Split(',');

                if (int.TryParse(split[0], out int raise) && bool.TryParse(split[1], out bool isRaise))
                {
                    Add(raise, isRaise);
                }
            }
        }
    }
}
