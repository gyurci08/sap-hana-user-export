using System;
using System.IO;
using System.Threading.Tasks;

namespace sap_hana_user_export.file
{
    internal class FileIO
    {
        // Method to read the contents of a file asynchronously
        public async Task<string> readFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
                throw;
            }
        }

        // Method to write contents to a file asynchronously
        public async Task writeFileAsync(string filePath, string content)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false)) // false to overwrite the file if it exists
                {
                    await writer.WriteAsync(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
                throw;
            }
        }
    }
}
