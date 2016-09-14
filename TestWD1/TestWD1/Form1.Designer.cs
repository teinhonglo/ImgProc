namespace TestWD1
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.UploadBt = new System.Windows.Forms.Button();
            this.originImg = new System.Windows.Forms.PictureBox();
            this.procImg = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.originImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.procImg)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.30556F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.69444F));
            this.tableLayoutPanel.Controls.Add(this.UploadBt, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.originImg, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.procImg, 1, 1);
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.6F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.4F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(576, 499);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // UploadBt
            // 
            this.UploadBt.Location = new System.Drawing.Point(3, 3);
            this.UploadBt.Name = "UploadBt";
            this.UploadBt.Size = new System.Drawing.Size(75, 23);
            this.UploadBt.TabIndex = 0;
            this.UploadBt.Text = "Upload";
            this.UploadBt.UseVisualStyleBackColor = true;
            this.UploadBt.Click += new System.EventHandler(this.UploadBt_Click);
            // 
            // originImg
            // 
            this.originImg.Location = new System.Drawing.Point(3, 65);
            this.originImg.Name = "originImg";
            this.originImg.Size = new System.Drawing.Size(278, 300);
            this.originImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.originImg.TabIndex = 1;
            this.originImg.TabStop = false;
            // 
            // procImg
            // 
            this.procImg.Location = new System.Drawing.Point(287, 65);
            this.procImg.Name = "procImg";
            this.procImg.Size = new System.Drawing.Size(286, 300);
            this.procImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.procImg.TabIndex = 2;
            this.procImg.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(577, 499);
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "Form1";
            this.Text = "AIP 60547047S";
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.originImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.procImg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button UploadBt;
        private System.Windows.Forms.PictureBox originImg;
        private System.Windows.Forms.PictureBox procImg;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

