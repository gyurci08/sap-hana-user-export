using sap_hana_user_export.data;
using sap_hana_user_export.file;

namespace sap_hana_user_export
{
    public partial class mainWindow : Form
    {
        private String authorizationDataQuery = """
            SELECT 
                ROLE_NAME AS OBJECT_NAME,
                NULL AS SCHEMA_NAME,
                NULL AS PRIVILEGE,
                IS_GRANTABLE
            FROM 
                GRANTED_ROLES 
            WHERE 
                GRANTEE = '{0}'

            UNION

            SELECT 
                OBJECT_NAME,
                SCHEMA_NAME,
                PRIVILEGE,
                IS_GRANTABLE
            FROM 
                GRANTED_PRIVILEGES 
            WHERE 
                GRANTEE = '{0}'
            """;

        private String authorizationDataPath = "";

        private String generatedSqlPath = "";


        private String sourceUser;
        private String targetUser;

        private FileIO fileIO = new FileIO();





        public mainWindow()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void bt_copyQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string formattedQuery;
                if (!string.IsNullOrWhiteSpace(tb_sourceUser.Text))
                {
                    formattedQuery = string.Format(authorizationDataQuery, tb_sourceUser.Text);
                }
                else
                {
                    formattedQuery = authorizationDataQuery;
                }

                Clipboard.SetText(formattedQuery);
                MessageBox.Show("SQL query copied to clipboard successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while copying to clipboard: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private async void bt_loadData_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.Title = "Select authorization data file";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        DataParser dataParser = new DataParser();

                        string rawData = await fileIO.ReadFileAsync(filePath);
                        List<string[]> data = dataParser.ParseContent(rawData);

                        // Process the parsed data as needed
                        // For example:
                        // DisplayData(data);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void bt_generateSql_Click(object sender, EventArgs e)
        {
            string fileName = "generated_sql.txt";
            string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string appDataFolder = Path.Combine(documentsFolder, "sap-hana-user-export");
            string createSqlFolder = Path.Combine(appDataFolder, "createSQL");

            // Ensure the directories exist
            Directory.CreateDirectory(createSqlFolder);

            generatedSqlPath = Path.Combine(createSqlFolder, fileName);

            _ = fileIO.WriteFileAsync(generatedSqlPath, "Test");
        }
    }
}
