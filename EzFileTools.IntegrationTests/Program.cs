using System.Security.Cryptography.X509Certificates;

namespace EzFileTools.IntegrationTests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("======= START ========");
            bool all_tests_passed = Tests.RunAllTests(show_messages: true);
            if(all_tests_passed)
            {
                Console.WriteLine("== FINISHED : PASS! ==");
            }
            else
            {
                Console.WriteLine("== FINISHED : FAIL! ==");
            }
        }
    }
}