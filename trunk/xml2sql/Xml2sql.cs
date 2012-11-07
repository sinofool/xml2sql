using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace xml2sql
{
    public partial class Xml2sql : Form
    {
        public Xml2sql()
        {
            InitializeComponent();
            dataOutput.Rows.Add(new string[] { "Name", "td/div[@class=\"name\"]" });
            dataOutput.Rows.Add(new string[] { "Age", "td/div[@class=\"age\"" });
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            DialogResult result = open.ShowDialog();
            if (result != DialogResult.OK)
            {
                System.Console.WriteLine("Open file canceled.");
                return;
            }
            string[] files = open.FileNames;
            Dictionary<string, string> columns = new Dictionary<string, string>();
            foreach (DataGridViewRow row in dataOutput.Rows)
            {
                if (row.IsNewRow) continue;
                columns.Add(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString());
            }

            StringBuilder resultText = new StringBuilder();

            foreach (string file in files)
            {
                XMLParser parser = new XMLParser(file);
                List<List<string> > rows = parser.output(txtFor.Text, columns);
                foreach (List<string> row in rows)
                {
                    resultText.Append(file);
                    foreach (string field in row)
                    {
                        resultText.Append("\t").Append(field);
                    }
                    resultText.AppendLine();
                }
            }
            
            Clipboard.SetText(resultText.ToString());

            TextResult formResult = new TextResult();
            formResult.txtResult.Text = resultText.ToString();
            formResult.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            DialogResult ret = dialog.ShowDialog();
            if (ret != DialogResult.OK)
            {
                return;
            }
            StreamWriter s = new StreamWriter(dialog.OpenFile());
            s.WriteLine(txtFor.Text);
            foreach (DataGridViewRow row in dataOutput.Rows)
            {
                if (row.IsNewRow) continue;
                s.WriteLine("{0}\t{1}", row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString());
            }
            s.Flush();
            s.Close();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult ret = dialog.ShowDialog();
            if (ret != DialogResult.OK)
            {
                return;
            }
            StreamReader r = new StreamReader(dialog.OpenFile());
            txtFor.Text = r.ReadLine();
            string line = r.ReadLine();
            dataOutput.Rows.Clear();
            while (line != null)
            {
                string[] fields = line.Split('\t');
                dataOutput.Rows.Add(fields);
                line = r.ReadLine();
            }
        }
    }
}
