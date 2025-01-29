using sap_hana_user_export.data;
using sap_hana_user_export.file;
using sap_hana_user_export.entity;
using System.Net;
using System.Data;
using sap_hana_user_export.utils;


namespace sap_hana_user_export
{
    public partial class mainWindow : Form
    {
        private string authorizationDataQuery =
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

        private string createUserQuery = """
            CREATE USER "{0}" PASSWORD "{1}";
            ALTER USER "{0}" FORCE PASSWORD CHANGE;
            """;


        private FileIO fileIO = new FileIO();
        List<string[]> data;


        private string version="v2.0.3"; 


       private string createCreateUserSQL(string username)
        {
            PasswordGenerator passwordGenerator = new PasswordGenerator();
            return string.Format(createUserQuery, username, passwordGenerator.generate(15,2));
        }

   
        private string createGrantSQL(DbAuthorization dbAuthorization, string sourceUser, string targetUser)
        {
            switch (dbAuthorization)
            {
                case { objectName: "PUBLIC", schemaName: "?", privilege: "?" }:
                    return string.Empty;

                case { objectName: "?", schemaName: var schema, privilege: "CREATE ANY", isGrantable: true } when schema == sourceUser:
                    return string.Empty;

                case { schemaName: "?", privilege: "?" }:
                    if (dbAuthorization.objectName.Contains("::"))
                    {
                        return $"call grant_activated_role('{dbAuthorization.objectName}','{targetUser}');";
                    }
                    else
                    {
                        return $"GRANT \"{dbAuthorization.objectName}\" TO \"{targetUser}\"" +
                               (dbAuthorization.isGrantable ? " WITH GRANT OPTION" : "") + ";";
                    }

                case { objectName: "?", schemaName: "?" }:
                    if (dbAuthorization.privilege.Contains("::"))
                    {
                        return $"call grant_activated_role('{dbAuthorization.privilege}','{targetUser}');";
                    }
                    else
                    {
                        return $"GRANT \"{dbAuthorization.privilege}\" TO \"{targetUser}\"" +
                              (dbAuthorization.isGrantable ? " WITH GRANT OPTION" : "") + ";";
                    }

                case { objectName: "?", schemaName: not "?" }:
                    return $"GRANT \"{dbAuthorization.privilege}\" ON \"{dbAuthorization.schemaName}\" TO \"{targetUser}\"" +
                           (dbAuthorization.isGrantable ? " WITH GRANT OPTION" : "") + ";";

                case { objectName: not "?", schemaName: not "?" }:
                    return $"GRANT \"{dbAuthorization.privilege}\" ON \"{dbAuthorization.schemaName}\".\"{dbAuthorization.objectName}\" TO \"{targetUser}\"" +
                           (dbAuthorization.isGrantable ? " WITH GRANT OPTION" : "") + ";";

                // case { objectName: not "?", schemaName: "?" }:
                // TODO: Look up and test this

                default:
                    return $"-- UNKOWN CASE! Object_name: \"{dbAuthorization.objectName}\"     Schema_name: \"{dbAuthorization.schemaName}\"     Privilege: \"{dbAuthorization.privilege}\"";
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

                        string rawData = await fileIO.readFileAsync(filePath);
                        data = dataParser.parseContent(rawData);
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

            if (data == null || !data.Any())
            {
                MessageBox.Show("No data available to generate SQL. Please load data first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string sourceUser = tb_sourceUser.Text.ToUpper();
                string targetUser = string.IsNullOrWhiteSpace(tb_targetUser.Text)
                    ? sourceUser
                    : tb_targetUser.Text.ToUpper();

                string fileName = $"{targetUser}_create.txt";
                string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string appDataFolder = Path.Combine(documentsFolder, "sap-hana-user-export");
                string createSqlFolder = Path.Combine(appDataFolder, "createSQL");

                string content = createCreateUserSQL(targetUser) + "\n";

                List<DbAuthorization> authorizations = data.Select(row => new DbAuthorization(row[0], row[1], row[2], bool.Parse(row[3]))).ToList();

                foreach (DbAuthorization dbAuthorization in authorizations)
                {
                    string line = createGrantSQL(dbAuthorization, sourceUser, targetUser);
                    if (!string.IsNullOrEmpty(line))
                    {
                        content += line + "\n";
                    }
                }

                Directory.CreateDirectory(createSqlFolder);

                string generatedSqlPath = Path.Combine(createSqlFolder, fileName);

                await fileIO.writeFileAsync(generatedSqlPath, content);

                Clipboard.SetText(content);

                MessageBox.Show($"File saved successfully at:\n{generatedSqlPath}\n\nContent copied to clipboard",
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
            la_version.Text = version;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}
