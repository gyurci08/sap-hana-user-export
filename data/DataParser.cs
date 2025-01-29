namespace sap_hana_user_export.data
{
    internal class DataParser
    {
        public List<string[]> parseContent(string content)
        {
            List<string[]> data = new List<string[]>();
            string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            // Header should be skipped
            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(';')
                                          .Select(field => field.Trim())
                                          .ToArray();

                // First column should be skipped
                if (fields.Length > 1)
                {
                    string[] cleanedFields = fields.Skip(1)
                                                   .Where(field => !string.IsNullOrWhiteSpace(field))
                                                   .ToArray();

                    if (cleanedFields.Length > 0)
                    {
                        data.Add(cleanedFields);
                    }
                }
            }

            return data;
        }
    }
}
