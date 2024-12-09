using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace sap_hana_user_export.data
{
    internal class DataParser
    {
        public List<string[]> ParseContent(string content)
        {
            List<string[]> data = new List<string[]>();
            string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string[] fields = line.Split(';');
                data.Add(fields);
            }

            return data;
        }

        public DataTable ConvertToDataTable(List<string[]> data)
        {
            DataTable dt = new DataTable();

            if (data.Count > 0)
            {
                // Add columns based on the first row
                foreach (string field in data[0])
                {
                    dt.Columns.Add(field);
                }

                // Add rows
                for (int i = 1; i < data.Count; i++)
                {
                    dt.Rows.Add(data[i]);
                }
            }

            return dt;
        }

        public async Task<List<string[]>> ParseFileAsync(string filePath)
        {
            string content = await System.IO.File.ReadAllTextAsync(filePath);
            return ParseContent(content);
        }
    }
}
