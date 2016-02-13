namespace BjsScraper
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonStartScraping = new System.Windows.Forms.Button();
            this.treeViewCategories = new System.Windows.Forms.TreeView();
            this.buttonLoadCategories = new System.Windows.Forms.Button();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.buttonUnselectAll = new System.Windows.Forms.Button();
            this.buttonSaveCatFile = new System.Windows.Forms.Button();
            this.buttonOpenCategory = new System.Windows.Forms.Button();
            this.dataGridViewItems = new System.Windows.Forms.DataGridView();
            this.TitleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EstimatedDelivery = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelTotal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItems)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStartScraping
            // 
            this.buttonStartScraping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStartScraping.Enabled = false;
            this.buttonStartScraping.Location = new System.Drawing.Point(297, 320);
            this.buttonStartScraping.Name = "buttonStartScraping";
            this.buttonStartScraping.Size = new System.Drawing.Size(155, 51);
            this.buttonStartScraping.TabIndex = 1;
            this.buttonStartScraping.Text = "Start";
            this.buttonStartScraping.UseVisualStyleBackColor = true;
            this.buttonStartScraping.Click += new System.EventHandler(this.buttonStartScraping_Click);
            // 
            // treeViewCategories
            // 
            this.treeViewCategories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewCategories.CheckBoxes = true;
            this.treeViewCategories.Location = new System.Drawing.Point(11, 12);
            this.treeViewCategories.Name = "treeViewCategories";
            this.treeViewCategories.Size = new System.Drawing.Size(279, 302);
            this.treeViewCategories.TabIndex = 7;
            this.treeViewCategories.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewCategories_AfterCheck);
            // 
            // buttonLoadCategories
            // 
            this.buttonLoadCategories.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonLoadCategories.Location = new System.Drawing.Point(9, 320);
            this.buttonLoadCategories.Name = "buttonLoadCategories";
            this.buttonLoadCategories.Size = new System.Drawing.Size(141, 23);
            this.buttonLoadCategories.TabIndex = 8;
            this.buttonLoadCategories.Text = "Load categories";
            this.buttonLoadCategories.UseVisualStyleBackColor = true;
            this.buttonLoadCategories.Click += new System.EventHandler(this.buttonLoadCategories_Click);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelectAll.Location = new System.Drawing.Point(9, 349);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(141, 23);
            this.buttonSelectAll.TabIndex = 9;
            this.buttonSelectAll.Text = "Select all";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // buttonUnselectAll
            // 
            this.buttonUnselectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUnselectAll.Location = new System.Drawing.Point(9, 378);
            this.buttonUnselectAll.Name = "buttonUnselectAll";
            this.buttonUnselectAll.Size = new System.Drawing.Size(141, 23);
            this.buttonUnselectAll.TabIndex = 10;
            this.buttonUnselectAll.Text = "Unselect all";
            this.buttonUnselectAll.UseVisualStyleBackColor = true;
            this.buttonUnselectAll.Click += new System.EventHandler(this.buttonUnselectAll_Click);
            // 
            // buttonSaveCatFile
            // 
            this.buttonSaveCatFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSaveCatFile.Location = new System.Drawing.Point(156, 348);
            this.buttonSaveCatFile.Name = "buttonSaveCatFile";
            this.buttonSaveCatFile.Size = new System.Drawing.Size(134, 23);
            this.buttonSaveCatFile.TabIndex = 11;
            this.buttonSaveCatFile.Text = "Save category file";
            this.buttonSaveCatFile.UseVisualStyleBackColor = true;
            this.buttonSaveCatFile.Click += new System.EventHandler(this.buttonSaveCatFile_Click);
            // 
            // buttonOpenCategory
            // 
            this.buttonOpenCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOpenCategory.Location = new System.Drawing.Point(156, 320);
            this.buttonOpenCategory.Name = "buttonOpenCategory";
            this.buttonOpenCategory.Size = new System.Drawing.Size(134, 23);
            this.buttonOpenCategory.TabIndex = 12;
            this.buttonOpenCategory.Text = "Open category file";
            this.buttonOpenCategory.UseVisualStyleBackColor = true;
            this.buttonOpenCategory.Click += new System.EventHandler(this.buttonOpenCategory_Click);
            // 
            // dataGridViewItems
            // 
            this.dataGridViewItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TitleColumn,
            this.ItemNumber,
            this.Price,
            this.Description,
            this.EstimatedDelivery,
            this.Category});
            this.dataGridViewItems.Location = new System.Drawing.Point(297, 13);
            this.dataGridViewItems.Name = "dataGridViewItems";
            this.dataGridViewItems.Size = new System.Drawing.Size(652, 301);
            this.dataGridViewItems.TabIndex = 13;
            // 
            // TitleColumn
            // 
            this.TitleColumn.HeaderText = "Title";
            this.TitleColumn.Name = "TitleColumn";
            // 
            // ItemNumber
            // 
            this.ItemNumber.HeaderText = "ItemNumber";
            this.ItemNumber.Name = "ItemNumber";
            // 
            // Price
            // 
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            // 
            // EstimatedDelivery
            // 
            this.EstimatedDelivery.HeaderText = "Estimated Delivery";
            this.EstimatedDelivery.Name = "EstimatedDelivery";
            // 
            // Category
            // 
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSave.Location = new System.Drawing.Point(794, 320);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(155, 51);
            this.buttonSave.TabIndex = 14;
            this.buttonSave.Text = "Save results";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(294, 383);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Status:";
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(340, 383);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(24, 13);
            this.labelStatus.TabIndex = 16;
            this.labelStatus.Text = "Idle";
            // 
            // labelTotal
            // 
            this.labelTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelTotal.AutoSize = true;
            this.labelTotal.Location = new System.Drawing.Point(294, 399);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(43, 13);
            this.labelTotal.TabIndex = 17;
            this.labelTotal.Text = "Total: 0";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 415);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.dataGridViewItems);
            this.Controls.Add(this.buttonOpenCategory);
            this.Controls.Add(this.buttonSaveCatFile);
            this.Controls.Add(this.buttonUnselectAll);
            this.Controls.Add(this.buttonSelectAll);
            this.Controls.Add(this.buttonLoadCategories);
            this.Controls.Add(this.treeViewCategories);
            this.Controls.Add(this.buttonStartScraping);
            this.Name = "MainForm";
            this.Text = "Bjs scraper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStartScraping;
        private System.Windows.Forms.TreeView treeViewCategories;
        private System.Windows.Forms.Button buttonLoadCategories;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonUnselectAll;
        private System.Windows.Forms.Button buttonSaveCatFile;
        private System.Windows.Forms.Button buttonOpenCategory;
        private System.Windows.Forms.DataGridView dataGridViewItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn EstimatedDelivery;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelTotal;
    }
}

