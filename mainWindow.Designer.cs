namespace sap_hana_user_export
{
    partial class mainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            bt_generateSql = new Button();
            bt_copyQuery = new Button();
            bt_loadData = new Button();
            tb_sourceUser = new TextBox();
            tb_targetUser = new TextBox();
            la_sourceUser = new Label();
            la_targetUser = new Label();
            SuspendLayout();
            // 
            // bt_generateSql
            // 
            bt_generateSql.Location = new Point(11, 85);
            bt_generateSql.Name = "bt_generateSql";
            bt_generateSql.Size = new Size(238, 56);
            bt_generateSql.TabIndex = 0;
            bt_generateSql.Text = "Generate";
            bt_generateSql.UseVisualStyleBackColor = true;
            // 
            // bt_copyQuery
            // 
            bt_copyQuery.Location = new Point(11, 53);
            bt_copyQuery.Name = "bt_copyQuery";
            bt_copyQuery.Size = new Size(116, 26);
            bt_copyQuery.TabIndex = 1;
            bt_copyQuery.Text = "Copy query";
            bt_copyQuery.UseVisualStyleBackColor = true;
            // 
            // bt_loadData
            // 
            bt_loadData.Location = new Point(133, 53);
            bt_loadData.Name = "bt_loadData";
            bt_loadData.Size = new Size(116, 26);
            bt_loadData.TabIndex = 2;
            bt_loadData.Text = "Load data";
            bt_loadData.UseVisualStyleBackColor = true;
            // 
            // tb_sourceUser
            // 
            tb_sourceUser.Location = new Point(11, 24);
            tb_sourceUser.Name = "tb_sourceUser";
            tb_sourceUser.Size = new Size(115, 23);
            tb_sourceUser.TabIndex = 3;
            // 
            // tb_targetUser
            // 
            tb_targetUser.Location = new Point(133, 24);
            tb_targetUser.Name = "tb_targetUser";
            tb_targetUser.Size = new Size(116, 23);
            tb_targetUser.TabIndex = 4;
            // 
            // la_sourceUser
            // 
            la_sourceUser.AutoSize = true;
            la_sourceUser.Location = new Point(12, 6);
            la_sourceUser.Name = "la_sourceUser";
            la_sourceUser.Size = new Size(68, 15);
            la_sourceUser.TabIndex = 5;
            la_sourceUser.Text = "Source user";
            // 
            // la_targetUser
            // 
            la_targetUser.AutoSize = true;
            la_targetUser.Location = new Point(133, 6);
            la_targetUser.Name = "la_targetUser";
            la_targetUser.Size = new Size(64, 15);
            la_targetUser.TabIndex = 6;
            la_targetUser.Text = "Target user";
            // 
            // mainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(261, 154);
            Controls.Add(la_targetUser);
            Controls.Add(la_sourceUser);
            Controls.Add(tb_targetUser);
            Controls.Add(tb_sourceUser);
            Controls.Add(bt_loadData);
            Controls.Add(bt_copyQuery);
            Controls.Add(bt_generateSql);
            Name = "mainWindow";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button bt_generateSql;
        private Button bt_copyQuery;
        private Button bt_loadData;
        private TextBox tb_sourceUser;
        private TextBox tb_targetUser;
        private Label la_sourceUser;
        private Label la_targetUser;
    }
}
