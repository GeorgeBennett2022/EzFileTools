using System.Data;
using EzFileTools;

namespace EzFileTools.IntegrationTests
{ 
    internal static class Tests
    {
        public static bool RunAllTests(bool show_messages)
        {
            bool all_tests_pass = true;
            foreach (string test_file_name in Tests.AllTests.Keys)
            {
                Func<bool> test = Tests.AllTests[test_file_name];
                bool pass = test.Invoke();
                if (show_messages && pass)
                {
                    Console.WriteLine(test_file_name + " Passed");
                }
                else if (show_messages && pass == false)
                {
                    Console.WriteLine(test_file_name + " Failed");
                }
                if (pass == false)
                {
                    all_tests_pass = false;
                }
            }
            return all_tests_pass;
        }

        internal static Dictionary<string, Func<bool>> AllTests
        {
            get =>
            new Dictionary<string, Func<bool>>
            {
                { "CsvReader_Test1.csv", CsvReader_Test1 },
                { "CsvReader_Test2.txt", CsvReader_Test2 }
            };
        }

        public static bool CsvReader_Test1()
        {
            string file_name = "CsvReader_Test1.csv";
            string path = GetPath(file_name);
            DataTable control = ControlData.CsvReader_Test1_Control();
            CsvReader reader = new CsvReader();
            DataTable result = reader.Read(path);
            return result.DisplayExpression == control.DisplayExpression;
        }

        public static bool CsvReader_Test2()
        {
            string file_name = "CsvReader_Test2.txt";
            string path = GetPath(file_name);
            DataTable control = ControlData.CsvReader_Test2_Control();
            CsvReader reader = new CsvReader(delimeter: '|', expect_header: false, qualifiers: new char[] { 'Z', ';' });
            DataTable result = reader.Read(path);
            return result.DisplayExpression == control.DisplayExpression;
        }

        private static string GetPath(string file_name) => Path.Join(@"..\..\..\TestFiles\", file_name);
    }
}
