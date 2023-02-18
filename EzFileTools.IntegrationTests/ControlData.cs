using System.Data;

namespace EzFileTools.IntegrationTests
{
    internal static class ControlData
    {
        internal static DataTable CsvReader_Test1_Control()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Letters");
            table.Columns.Add("Numbers");
            table.Columns.Add("Fruit");
            table.Rows.Add(new string[] { "ABC", "123", "Lemon" });
            table.Rows.Add(new string[] { "bvs", "456", "Strawberry" });
            table.Rows.Add(new string[] { "ab\",asd\"de", "\"1,244\"", "Orange" });
            table.Rows.Add(new string[] { "", "321", "\"Cake,Icecream\"" });
            return table;
        }

        internal static DataTable CsvReader_Test2_Control()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Column0");
            table.Columns.Add("Column1");
            table.Columns.Add("Column2");
            table.Rows.Add(new string[] { "421", "LemonZ|ZCake", " 427" });
            table.Rows.Add(new string[] { "ab", " Zebra", " Turt;le" });
            table.Rows.Add(new string[] { "", "", "" });
            table.Rows.Add(new string[] { "Lemon;|;Pudding", " Icecream" });
            return table;
        }
    }
}
