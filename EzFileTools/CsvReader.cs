using System.Data;
using System.Text;

namespace EzFileTools
{
    /// <summary>
    /// reads .csv files or other delimeted text files
    /// </summary>
    public class CsvReader
    {

        /// <summary>
        /// reads .csv files or other delimeted text files
        /// </summary>
        /// <param name="delimeter">character to seperate rows. default (',')</param>
        /// <param name="expect_header">boolean to deturmine whether first row should be treated as a header</param>
        /// <param name="qualifiers">a collection of qualifiers incase the delimeter needs to be escaped.
        /// These will surround the escaped delimeter character</param>
        public CsvReader(char? delimeter = null,
                           bool? expect_header = null,
                           IEnumerable<char>? qualifiers = null)
        {
            this.delimeter = delimeter ?? ',';
            this.expect_header = expect_header ?? true;
            this.qualifiers = qualifiers ?? new char[] { '"' };           
        }

        // These are used for escaping the delimeter
        private readonly string replacement_sequence_prefix = @"1avrtasf9sdasd0v8c8ty5lp2bhREPLACEfasxGF34DY4SY3";
        private readonly string replacement_sequece_suffix = @"jgikapofitzzzzz1243513348753zzzzzSRSEVasdasdsg2GH";

        /// <summary>
        /// character to seperate rows. default (',')
        /// </summary>
        public char delimeter { get; set; }

        /// <summary>
        /// boolean to deturmine whether first row should be treated as a header
        /// </summary>
        public bool expect_header { get; set; }

        /// <summary>
        /// a collection of qualifiers incase the delimeter needs to be escaped
        /// </summary>
        public IEnumerable<char> qualifiers { get; set; }

        /// <summary>
        /// Reads a .csv file or any other delimeted text file and returns a DataTable
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>DataTable made from data from the file</returns>
        public DataTable Read(string path)
        {
            DataTable table = new DataTable();
            string[] lines = File.ReadAllLines(path);

            // Set columns / read header
            string[] split_header_line = ReadLine(lines[0]);
            int column_count = 0;
            foreach(string header in split_header_line)
            {
                if (expect_header)
                {
                    table.Columns.Add(header);
                }
                else
                {
                    table.Columns.Add($"Column{column_count}");
                }
                column_count++;
            }
            

            // Add data to table
            for (int i = expect_header ? 1 : 0 ; i < lines.Length; i++)
            {
                string[] split_line = ReadLine(lines[i]);
                // Case: irregular column sizes
                if (split_line.Length > column_count) throw IrregularColumnsException; 
                if (split_line.Length < column_count)
                {
                    string[] fitted_split_line = new string[column_count];
                    int j = 0;
                    foreach(string split_line_section in split_line) 
                    {
                        fitted_split_line[j] = split_line_section ?? "";
                        j++;
                    }
                    while(j < column_count)
                    {
                        fitted_split_line[j] = "";
                        j++;
                    }
                    split_line = fitted_split_line;
                }
                table.Rows.Add(split_line);
            }
            return table;
        }

        // Reads a line and provides delimeter escape functionality
        private string[] ReadLine(string line)
        {
            bool contains_qualifieres = qualifiers.Any(x => line.Contains(x));
            if (contains_qualifieres == false)
            {
                return line.Split(delimeter);
            }
            else
            {
                Dictionary<string, string> items_to_replace = new Dictionary<string, string>();

                int match_id = 1;
                bool match_found = true;
                // Keep looping because there could be many matches
                while (match_found)
                {
                    match_found = false;
                    foreach(char qualifier in qualifiers)
                    {
                        string? escaped_text = FindEscapingText(qualifier, line);
                        if (escaped_text != null)
                        {
                            match_found = true;
                            string indexed_replacement_sequence = GenerateReplacementSequence(match_id);
                            match_id++;
                            line = line.Replace(escaped_text, indexed_replacement_sequence);
                            items_to_replace.Add(indexed_replacement_sequence, escaped_text);
                        }
                    }
                }
                string[] split_line = line.Split(delimeter);
                for(int i = 0; i < split_line.Length; i++)
                {
                    foreach (string replacement_sequence in items_to_replace.Keys)
                    {
                        split_line[i] = split_line[i].Replace(replacement_sequence, items_to_replace[replacement_sequence]);
                    }
                }
                return split_line;
            }
        }

        // Helper method for ReadLine
        private string GenerateReplacementSequence(int match_id)
        {
            return replacement_sequence_prefix + match_id.ToString() + replacement_sequece_suffix;
        }

        // Helper method for ReadLine
        private string? FindEscapingText(char qualifier, string string_to_search)
        {
            bool recording = false;
            StringBuilder escaped_string = new StringBuilder();
            foreach(char c in string_to_search)
            {
                if(recording && c == qualifier)
                {
                    escaped_string.Append(qualifier);
                    return escaped_string.ToString();
                }
                else if(c == qualifier)
                {
                    recording = true;
                }
                if (recording)
                {
                    escaped_string.Append(c);
                }
            }
            return null;
        }
        

        // Irregular columns exception
        private Exception IrregularColumnsException =>
            new ArgumentException("File has irregular amounts of columns. Try adding some commas to the first row of the file.");

    }
}