using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using BjcScraper;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace BjsScraper
{
    public partial class MainForm : Form
    {
        private Scraper _scraper;
        private List<Category> _categories;
        private bool _started = false;
        private long _totalItems = 0;

        public MainForm()
        {
            ThreadPool.SetMaxThreads(10000, 10000);
            InitializeComponent();
            CreateScraper();
        }

        private void CreateScraper()
        {
            _scraper = new Scraper(new SimpleHttpProvider());
            _scraper.NewItem += _scraper_NewItem;
            _scraper.StatusChange += _scraper_StatusChange;
            _scraper.ProcessEnd += _scraper_ProcessEnd;

        }

        private void DestroyScraper()
        {
            _scraper.NewItem -= _scraper_NewItem;
            _scraper.StatusChange -= _scraper_StatusChange;
            _scraper.ProcessEnd -= _scraper_ProcessEnd;
            _scraper.StopProcess();
        }

        void _scraper_ProcessEnd(object sender)
        {
            buttonStartScraping.BeginInvoke((Action)(() =>
            {
                buttonStartScraping.Text = "Start";
            }));
            _scraper.StopProcess();
            _started = false;
            _scraper_StatusChange(null, "Cancelled");
            DestroyScraper();
            CreateScraper();
        }

        void _scraper_StatusChange(object sender, string e)
        {
            if (!_started)
            {
                //labelStatus.BeginInvoke((Action)(() =>
                //{
                //    labelStatus.Text = "Cancelled";
                //}));
                return;
            };
            try
            {
                labelStatus.BeginInvoke((Action) (() =>
                {
                    labelStatus.Text = e;
                }));
            }
            catch
            {
                
            }
        }

        void _scraper_NewItem(object sender, NewItemEventArgs e)
        {
            if (!_started)
            {
                //labelStatus.BeginInvoke((Action)(() =>
                //{
                //    labelStatus.Text = "Cancelled";
                //}));
                return;
            };
            try
            {
                _totalItems++;
                dataGridViewItems.BeginInvoke((Action) (() =>
                {
                    AddGridItem(e.Item);
                }));
                labelTotal.BeginInvoke((Action)(() =>
                {
                    labelTotal.Text = string.Format("Total: {0}", _totalItems);
                }));
            }
            catch
            {
                
            }
        }

        private void AddGridItem(BjsItem item)
        {
            var row = new DataGridViewRow();
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = item.Title });
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = item.ItemNumber });
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = string.Format("${0}",item.Price) });
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = item.Description });
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = item.EstimatedDelivery });
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = item.Category });
            dataGridViewItems.Rows.Add(row);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonStartScraping_Click(object sender, EventArgs e)
        {
            if (!_started)
            {
                try
                {
                    var selectedCategories =
                        _categories.SelectMany(x => x.Childs).SelectMany(x => x.Childs).Where(x => x.Selected).ToList();
                    dataGridViewItems.Rows.Clear();
                    _scraper.StartProcess(selectedCategories);
                    _totalItems=0;
                    labelTotal.BeginInvoke((Action)(() =>
                    {
                        labelTotal.Text = string.Format("Total: {0}", _totalItems);
                    }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error selecting categories: "+ex.Message);
                }
                _started = true;
                buttonStartScraping.Text = "Cancel";
            }
            else
            {
                buttonStartScraping.Text = "Start";
                _scraper.StopProcess();
                _started = false;
                _scraper_StatusChange(null,"Cancelled");
                DestroyScraper();
                CreateScraper();
            }
        }

        private void buttonLoadCategories_Click(object sender, EventArgs e)
        {
            try
            {
                _categories = _scraper.LoadCategories();
                FillTreeView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void FillTreeView()
        {
            treeViewCategories.Nodes.Clear();
            foreach (var topCategory in _categories)
            {
                var topNode = new TreeNode(topCategory.Title) { Tag = topCategory, Checked = topCategory.Selected };
                foreach (var subCategory in topCategory.Childs)
                {
                    var subNode = new TreeNode(subCategory.Title) { Tag = subCategory, Checked = subCategory.Selected };
                    topNode.Nodes.Add(subNode);
                    foreach (var subSubCategory in subCategory.Childs)
                    {
                        var subSubNode = new TreeNode(subSubCategory.Title) { Tag = subSubCategory, Checked = subSubCategory.Selected };
                        subNode.Nodes.Add(subSubNode);
                    }
                }
                treeViewCategories.Nodes.Add(topNode);
            }
            buttonStartScraping.Enabled = true;
        }

        private void treeViewCategories_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // The code only executes if the user caused the checked state to change. 
            if (e.Action != TreeViewAction.Unknown)
            {
                ((Category)e.Node.Tag).Selected = e.Node.Checked;
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current 
                    Checked value of the TreeNode whose checked state changed. */
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }
            }
        }

        // Updates all child tree nodes recursively. 
        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                ((Category)node.Tag).Selected = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively. 
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in treeViewCategories.Nodes)
            {
                node.Checked = true;
                ((Category)node.Tag).Selected = true;
                CheckAllChildNodes(node, true);
            }
        }

        private void buttonUnselectAll_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in treeViewCategories.Nodes)
            {
                node.Checked = false;
                ((Category)node.Tag).Selected = false;
                CheckAllChildNodes(node, false);
            }
        }

        private void buttonSaveCatFile_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML file (.xml)|*.xml";
                saveFileDialog.ShowDialog(this);
                if (string.IsNullOrEmpty(saveFileDialog.FileName)) return;
                XmlSerializer xsSubmit = new XmlSerializer(typeof (List<Category>));
                StringWriter sww = new StringWriter();
                XmlWriter writer = XmlWriter.Create(sww);
                xsSubmit.Serialize(writer, _categories);
                var xml = sww.ToString();
                File.WriteAllText(saveFileDialog.FileName, xml);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving file: " + ex.Message);
            }
        }

        private void buttonOpenCategory_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML file (.xml)|*.xml";
            openFileDialog.ShowDialog(this);
            if (!File.Exists(openFileDialog.FileName)) return;
            var xml = File.ReadAllText(openFileDialog.FileName);
            XmlSerializer xsSubmit = new XmlSerializer(typeof(List<Category>));
            StringReader rdr = new StringReader(xml);
            _categories = (List<Category>)xsSubmit.Deserialize(rdr);
            FillTreeView();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel file (.xls)|*.xls";
                saveFileDialog.ShowDialog(this);
                if (string.IsNullOrEmpty(saveFileDialog.FileName)) return;
                using (
                    FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    var wb = new HSSFWorkbook();
                    ISheet sheet = wb.CreateSheet("Sheet1");
                    //Create heading
                    var style1 = wb.CreateCellStyle();
                    style1.FillPattern = FillPattern.SolidForeground;
                    // cell background
                    style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Blue.Index;
                    var font1 = wb.CreateFont();
                    font1.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
                    style1.SetFont(font1);
                    var headingRow = sheet.CreateRow(0);
                    var cell0 = headingRow.CreateCell(0);
                    cell0.SetCellValue("Item name");
                    cell0.CellStyle = style1;
                    var cell1 = headingRow.CreateCell(1);
                    cell1.SetCellValue("Item number");
                    cell1.CellStyle = style1;
                    var cell2 = headingRow.CreateCell(2);
                    cell2.SetCellValue("Price");
                    cell2.CellStyle = style1;
                    var cell3 = headingRow.CreateCell(3);
                    cell3.SetCellValue("Description");
                    cell3.CellStyle = style1;
                    var cell4 = headingRow.CreateCell(4);
                    cell4.SetCellValue("Estimated delivery");
                    cell4.CellStyle = style1;
                    var cell5 = headingRow.CreateCell(5);
                    cell5.SetCellValue("Category");
                    cell5.CellStyle = style1;
                    for (int i = 0; i < dataGridViewItems.Rows.Count; i++)
                    {
                        var dtRow = dataGridViewItems.Rows[i];
                        IRow row = sheet.CreateRow(i + 1);
                        for (var j = 0; j < dtRow.Cells.Count; j++)
                        {
                            var cell = row.CreateCell(j);
                            if (dtRow.Cells[j].Value != null)
                                cell.SetCellValue(dtRow.Cells[j].Value.ToString());
                        }
                    }
                    wb.Write(stream);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving results: " + ex.Message);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _scraper.StopProcess();
        }
    }
}
