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
            this.originImg = new System.Windows.Forms.PictureBox();
            this.procImg = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.HistogramBT = new System.Windows.Forms.Button();
            this.UploadBt = new System.Windows.Forms.Button();
            this.Guassion_NoiseBT = new System.Windows.Forms.Button();
            this.clrSpaceBT = new System.Windows.Forms.Button();
            this.FFT_BT = new System.Windows.Forms.Button();
            this.skinDetectionBT = new System.Windows.Forms.Button();
            this.EqualizationBT = new System.Windows.Forms.Button();
            this.ImgSmoothingBT = new System.Windows.Forms.Button();
            this.EdgeDectectionBT = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.originImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.procImg)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.originImg, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.procImg, 1, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 84);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(656, 650);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // originImg
            // 
            this.originImg.Location = new System.Drawing.Point(3, 3);
            this.originImg.Name = "originImg";
            this.originImg.Size = new System.Drawing.Size(322, 319);
            this.originImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.originImg.TabIndex = 1;
            this.originImg.TabStop = false;
            // 
            // procImg
            // 
            this.procImg.Location = new System.Drawing.Point(331, 3);
            this.procImg.Name = "procImg";
            this.procImg.Size = new System.Drawing.Size(322, 319);
            this.procImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.procImg.TabIndex = 2;
            this.procImg.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.HistogramBT, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.UploadBt, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Guassion_NoiseBT, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.clrSpaceBT, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.FFT_BT, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.skinDetectionBT, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.EqualizationBT, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.ImgSmoothingBT, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.EdgeDectectionBT, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(653, 74);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // HistogramBT
            // 
            this.HistogramBT.Location = new System.Drawing.Point(84, 3);
            this.HistogramBT.Name = "HistogramBT";
            this.HistogramBT.Size = new System.Drawing.Size(75, 23);
            this.HistogramBT.TabIndex = 5;
            this.HistogramBT.Text = "Histogram";
            this.HistogramBT.UseVisualStyleBackColor = true;
            this.HistogramBT.Click += new System.EventHandler(this.HistogramBT_Click);
            // 
            // UploadBt
            // 
            this.UploadBt.Location = new System.Drawing.Point(3, 3);
            this.UploadBt.Name = "UploadBt";
            this.UploadBt.Size = new System.Drawing.Size(75, 23);
            this.UploadBt.TabIndex = 4;
            this.UploadBt.Text = "Upload";
            this.UploadBt.UseVisualStyleBackColor = true;
            this.UploadBt.Click += new System.EventHandler(this.UploadBt_Click);
            // 
            // Guassion_NoiseBT
            // 
            this.Guassion_NoiseBT.Location = new System.Drawing.Point(165, 3);
            this.Guassion_NoiseBT.Name = "Guassion_NoiseBT";
            this.Guassion_NoiseBT.Size = new System.Drawing.Size(75, 23);
            this.Guassion_NoiseBT.TabIndex = 6;
            this.Guassion_NoiseBT.Text = "Noise";
            this.Guassion_NoiseBT.UseVisualStyleBackColor = true;
            this.Guassion_NoiseBT.Click += new System.EventHandler(this.Guassion_NoiseBT_Click);
            // 
            // clrSpaceBT
            // 
            this.clrSpaceBT.Location = new System.Drawing.Point(246, 3);
            this.clrSpaceBT.Name = "clrSpaceBT";
            this.clrSpaceBT.Size = new System.Drawing.Size(75, 23);
            this.clrSpaceBT.TabIndex = 7;
            this.clrSpaceBT.Text = "Color Space";
            this.clrSpaceBT.UseVisualStyleBackColor = true;
            this.clrSpaceBT.Click += new System.EventHandler(this.clrSpaceBT_Click);
            // 
            // FFT_BT
            // 
            this.FFT_BT.Location = new System.Drawing.Point(327, 3);
            this.FFT_BT.Name = "FFT_BT";
            this.FFT_BT.Size = new System.Drawing.Size(75, 23);
            this.FFT_BT.TabIndex = 8;
            this.FFT_BT.Text = "FFT";
            this.FFT_BT.UseVisualStyleBackColor = true;
            this.FFT_BT.Click += new System.EventHandler(this.FFT_BT_Click);
            // 
            // skinDetectionBT
            // 
            this.skinDetectionBT.Location = new System.Drawing.Point(408, 3);
            this.skinDetectionBT.Name = "skinDetectionBT";
            this.skinDetectionBT.Size = new System.Drawing.Size(75, 23);
            this.skinDetectionBT.TabIndex = 9;
            this.skinDetectionBT.Text = "skin Detection";
            this.skinDetectionBT.UseVisualStyleBackColor = true;
            this.skinDetectionBT.Click += new System.EventHandler(this.skinDetectionBT_Click);
            // 
            // EqualizationBT
            // 
            this.EqualizationBT.Location = new System.Drawing.Point(489, 3);
            this.EqualizationBT.Name = "EqualizationBT";
            this.EqualizationBT.Size = new System.Drawing.Size(75, 23);
            this.EqualizationBT.TabIndex = 10;
            this.EqualizationBT.Text = "Equalization";
            this.EqualizationBT.UseVisualStyleBackColor = true;
            this.EqualizationBT.Click += new System.EventHandler(this.EqualizationBT_Click);
            // 
            // ImgSmoothingBT
            // 
            this.ImgSmoothingBT.Location = new System.Drawing.Point(570, 3);
            this.ImgSmoothingBT.Name = "ImgSmoothingBT";
            this.ImgSmoothingBT.Size = new System.Drawing.Size(75, 23);
            this.ImgSmoothingBT.TabIndex = 11;
            this.ImgSmoothingBT.Text = "Smoothing";
            this.ImgSmoothingBT.UseVisualStyleBackColor = true;
            this.ImgSmoothingBT.Click += new System.EventHandler(this.ImgSmoothingBT_Click);
            // 
            // EdgeDectectionBT
            // 
            this.EdgeDectectionBT.Location = new System.Drawing.Point(3, 32);
            this.EdgeDectectionBT.Name = "EdgeDectectionBT";
            this.EdgeDectectionBT.Size = new System.Drawing.Size(75, 23);
            this.EdgeDectectionBT.TabIndex = 12;
            this.EdgeDectectionBT.Text = "Edge";
            this.EdgeDectectionBT.UseVisualStyleBackColor = true;
            this.EdgeDectectionBT.Click += new System.EventHandler(this.EdgeDectectionBT_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(671, 420);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "Form1";
            this.Text = "AIP 60547047S";
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.originImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.procImg)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox procImg;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button HistogramBT;
        private System.Windows.Forms.Button UploadBt;
        private System.Windows.Forms.Button Guassion_NoiseBT;
        private System.Windows.Forms.Button clrSpaceBT;
        private System.Windows.Forms.Button FFT_BT;
        private System.Windows.Forms.Button skinDetectionBT;
        private System.Windows.Forms.Button EqualizationBT;
        private System.Windows.Forms.PictureBox originImg;
        private System.Windows.Forms.Button ImgSmoothingBT;
        private System.Windows.Forms.Button EdgeDectectionBT;
    }
}

