using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rynix_Compiler
{
    public partial class Form1 : Form
    {
        private DataTable _tokenTable;

        public Form1()
        {
            InitializeComponent();
            SetupTable();

            BtnScan_Click.Click += BtnScan_Click_Action;
            BtnClear_Click.Click += BtnClear_Click_Action;
        }

        private void SetupTable()
        {
            _tokenTable = new DataTable();
            _tokenTable.Columns.Add("Line", typeof(int));
            _tokenTable.Columns.Add("Col", typeof(int));
            _tokenTable.Columns.Add("Lexeme", typeof(string));
            _tokenTable.Columns.Add("Token Class", typeof(string));

            dataGridView1.DataSource = _tokenTable;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void BtnScan_Click_Action(object sender, EventArgs e)
        {
            listBoxErrors.Items.Clear();
            _tokenTable.Clear();

            string Code = txtInputCode.Text;

            if (string.IsNullOrWhiteSpace(Code))
            {
                MessageBox.Show("Please enter some Rynix code to scan!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            rynix_compiler.Start_Compiling(Code);

            PrintTokens();
            PrintErrors();
        }

        void PrintTokens()
        {
            for (int i = 0; i < rynix_compiler.Rynix_Scanner.Tokens.Count; i++)
            {
                var token = rynix_compiler.Rynix_Scanner.Tokens.ElementAt(i);
                _tokenTable.Rows.Add(token.line, token.col, token.lex, token.token_type.ToString());
            }
        }

        void PrintErrors()
        {
            for (int i = 0; i < Errors.Error_List.Count; i++)
            {
                listBoxErrors.Items.Add(Errors.Error_List[i]);
            }
        }

        private void BtnClear_Click_Action(object sender, EventArgs e)
        {
            _tokenTable.Clear();
            rynix_compiler.TokenStream.Clear();
            Errors.Error_List.Clear();
            listBoxErrors.Items.Clear();
            txtInputCode.Clear();
        }

        private void dataGridViewTokens_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
        }

        private void BtnClear_Click_Click(object sender, EventArgs e)
        {
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}