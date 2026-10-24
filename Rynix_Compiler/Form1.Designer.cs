namespace Rynix_Compiler
{
    partial class Form1
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
            BtnScan_Click = new Button();
            dataGridView1 = new DataGridView();
            listBoxErrors = new ListView();
            txtInputCode = new RichTextBox();
            BtnClear_Click = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // BtnScan_Click
            // 
            BtnScan_Click.Location = new Point(53, 409);
            BtnScan_Click.Name = "BtnScan_Click";
            BtnScan_Click.Size = new Size(94, 29);
            BtnScan_Click.TabIndex = 0;
            BtnScan_Click.Text = "Scan";
            BtnScan_Click.UseVisualStyleBackColor = true;
            BtnScan_Click.Click += button1_Click_1;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(405, 12);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(383, 289);
            dataGridView1.TabIndex = 1;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // listBoxErrors
            // 
            listBoxErrors.Location = new Point(405, 307);
            listBoxErrors.Name = "listBoxErrors";
            listBoxErrors.Size = new Size(383, 131);
            listBoxErrors.TabIndex = 2;
            listBoxErrors.UseCompatibleStateImageBehavior = false;
            listBoxErrors.SelectedIndexChanged += listView1_SelectedIndexChanged;
            // 
            // txtInputCode
            // 
            txtInputCode.Location = new Point(12, 12);
            txtInputCode.Name = "txtInputCode";
            txtInputCode.Size = new Size(375, 381);
            txtInputCode.TabIndex = 3;
            txtInputCode.Text = "";
            txtInputCode.TextChanged += richTextBox1_TextChanged;
            // 
            // BtnClear_Click
            // 
            BtnClear_Click.Location = new Point(207, 409);
            BtnClear_Click.Name = "BtnClear_Click";
            BtnClear_Click.Size = new Size(94, 29);
            BtnClear_Click.TabIndex = 4;
            BtnClear_Click.Text = "Clear";
            BtnClear_Click.UseVisualStyleBackColor = true;
            BtnClear_Click.Click += BtnClear_Click_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(BtnClear_Click);
            Controls.Add(txtInputCode);
            Controls.Add(listBoxErrors);
            Controls.Add(dataGridView1);
            Controls.Add(BtnScan_Click);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button BtnScan_Click;
        private DataGridView dataGridView1;
        private ListView listBoxErrors;
        private RichTextBox txtInputCode;
        private Button BtnClear_Click;
    }
}
