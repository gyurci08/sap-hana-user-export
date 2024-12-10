using sap_hana_user_export.data;
using sap_hana_user_export.file;
using sap_hana_user_export.entity;
using System.Net;
using System.Data;


namespace sap_hana_user_export
{
    public partial class mainWindow : Form
    {
        private String authorizationDataQuery =
            """
            SELECT 
                ROLE_NAME AS OBJECT_NAME,
                NULL AS SCHEMA_NAME,
                NULL AS PRIVILEGE,
                IS_GRANTABLE
            FROM 
                GRANTED_ROLES 
            WHERE 
                GRANTEE LIKE '{0}'

            UNION

            SELECT 
                OBJECT_NAME,
                SCHEMA_NAME,
                PRIVILEGE,
                IS_GRANTABLE
            FROM 
                GRANTED_PRIVILEGES 
            WHERE 
                GRANTEE LIKE '{0}'
            """;


        private FileIO fileIO = new FileIO();
        List<string[]> data;


       

   
        private static string createGrantSQL(DbAuthorization dbAuthorization, string username)
        {
            switch (dbAuthorization)
            {
                case { objectName: "PUBLIC", schemaName: "?", privilege: "?" }:
                    return string.Empty;

                case { objectName: "?", privilege: "CREATE ANY", isGrantable: true }:
                    return string.Empty;

                case { schemaName: "?", privilege: "?" }:
                    return $"CALL GRANT_ACTIVATED_ROLE('{dbAuthorization.objectName}', '{username}');";

                case { schemaName: "?", objectName: "?" }:
                    return $"GRANT {dbAuthorization.privilege} TO {username}" +
                           (dbAuthorization.isGrantable ? " WITH GRANT OPTION" : "") + ";";

                case { schemaName: not "?", objectName: not "?" }:
                    return $"GRANT {dbAuthorization.privilege} ON {dbAuthorization.schemaName}.{dbAuthorization.objectName} TO {username}" +
                           (dbAuthorization.isGrantable ? " WITH GRANT OPTION" : "") + ";";

                default:
                    return $"UNKOWN CASE! Object_name: {dbAuthorization.objectName}     Schema_name: {dbAuthorization.schemaName}";
            }
        }



        private void bt_copyQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string formattedQuery;
                if (!string.IsNullOrWhiteSpace(tb_sourceUser.Text))
                {
                    formattedQuery = string.Format(authorizationDataQuery, tb_sourceUser.Text.ToUpper());
                }
                else
                {
                    formattedQuery = string.Format(authorizationDataQuery, "CHANGE_ME");
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
                        data = dataParser.ParseContent(rawData);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private async void bt_generateSql_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tb_sourceUser.Text))
            {
                MessageBox.Show("Please enter a source user name.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string targetUser = string.Empty;
                if (!string.IsNullOrWhiteSpace(tb_targetUser.Text))
                {
                    targetUser = tb_targetUser.Text.ToUpper();
                }
                else
                {   targetUser = tb_sourceUser.Text.ToUpper(); }

                string fileName = $"{targetUser}_create.txt";
                string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string appDataFolder = Path.Combine(documentsFolder, "sap-hana-user-export");
                string createSqlFolder = Path.Combine(appDataFolder, "createSQL");

                string content = string.Empty;

                List<DbAuthorization> authorizations = new List<DbAuthorization>();

                List<string> generatedSQL = new List<string>();

                // Creating authorization entities
                foreach (string[] row in data)
                {
                    authorizations.Add(new DbAuthorization(row[0], row[1], row[2], bool.Parse(row[3])));
                }

                foreach (DbAuthorization dbAuthorization in authorizations)
                {
                    string line = createGrantSQL(dbAuthorization, targetUser);
                    if (!string.IsNullOrEmpty(line))
                    {
                        content += line + "\n";
                    }
                }

                // Ensure the directories exist
                Directory.CreateDirectory(createSqlFolder);

                string generatedSqlPath = Path.Combine(createSqlFolder, fileName);

                // Save the file
                await fileIO.WriteFileAsync(generatedSqlPath, content);

                // Copy content to clipboard
                Clipboard.SetText(content);

                MessageBox.Show($"File saved successfully at:\n{generatedSqlPath}\n"+"\nContent copied to clipboard",
                                "File Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred.\nError: {ex.Message}",
                                "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }






        public mainWindow()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}
