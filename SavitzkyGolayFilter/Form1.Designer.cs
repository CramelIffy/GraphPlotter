namespace MainProcess
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
            this.components = new System.ComponentModel.Container();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.denoise = new System.Windows.Forms.CheckBox();
            this.showPeakProtectionIntensity = new System.Windows.Forms.CheckBox();
            this.plot = new System.Windows.Forms.Button();
            this.peakProtectionIntensity = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.selectFile = new System.Windows.Forms.Button();
            this.saveFigure = new System.Windows.Forms.Button();
            this.isPlotMax = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.isAlignMax = new System.Windows.Forms.CheckBox();
            this.SetIgnitionTimeZero = new System.Windows.Forms.CheckBox();
            this.dataColumn = new System.Windows.Forms.Label();
            this.timeColumn = new System.Windows.Forms.Label();
            this.dataColumnNum = new System.Windows.Forms.NumericUpDown();
            this.timeColumnNum = new System.Windows.Forms.NumericUpDown();
            this.graphInit = new System.Windows.Forms.Button();
            this.isPlotImpulse = new System.Windows.Forms.CheckBox();
            this.offsetRemoval = new System.Windows.Forms.CheckBox();
            this.isPlotBurnTime = new System.Windows.Forms.CheckBox();
            this.showReadme = new System.Windows.Forms.Button();
            this.readAsVol = new System.Windows.Forms.CheckBox();
            this.readAsMs = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.igniThreshold = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.burnoutThreshold = new System.Windows.Forms.TrackBar();
            this.skipTime = new System.Windows.Forms.NumericUpDown();
            this.cutoffTime = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.GraphScaleInit = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.isPlotAverageThrust = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.peakProtectionIntensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataColumnNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeColumnNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.igniThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.burnoutThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.skipTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cutoffTime)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(18, 14);
            this.formsPlot1.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(1649, 957);
            this.formsPlot1.TabIndex = 0;
            // 
            // denoise
            // 
            this.denoise.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.denoise.AutoSize = true;
            this.denoise.Location = new System.Drawing.Point(1677, 268);
            this.denoise.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.denoise.Name = "denoise";
            this.denoise.Size = new System.Drawing.Size(103, 25);
            this.denoise.TabIndex = 1;
            this.denoise.Text = "Denoise";
            this.denoise.UseVisualStyleBackColor = true;
            this.denoise.CheckedChanged += new System.EventHandler(this.Denoise_CheckedChanged);
            // 
            // showPeakProtectionIntensity
            // 
            this.showPeakProtectionIntensity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showPeakProtectionIntensity.AutoSize = true;
            this.showPeakProtectionIntensity.Location = new System.Drawing.Point(1677, 330);
            this.showPeakProtectionIntensity.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.showPeakProtectionIntensity.Name = "showPeakProtectionIntensity";
            this.showPeakProtectionIntensity.Size = new System.Drawing.Size(294, 25);
            this.showPeakProtectionIntensity.TabIndex = 2;
            this.showPeakProtectionIntensity.Text = "Show PeakProtection Intensity";
            this.toolTip1.SetToolTip(this.showPeakProtectionIntensity, "デノイズによって鈍るピークを保護するためのパラメータです。0のときはデノイズ結果をそのまま表示し、1のときは元データをそのまま表示します。");
            this.showPeakProtectionIntensity.UseVisualStyleBackColor = true;
            this.showPeakProtectionIntensity.CheckedChanged += new System.EventHandler(this.ShowPeakProtectionIntensity_CheckedChanged);
            // 
            // plot
            // 
            this.plot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.plot.Location = new System.Drawing.Point(1676, 903);
            this.plot.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.plot.Name = "plot";
            this.plot.Size = new System.Drawing.Size(286, 30);
            this.plot.TabIndex = 3;
            this.plot.Text = "Plot";
            this.plot.UseVisualStyleBackColor = true;
            this.plot.Click += new System.EventHandler(this.Plot_Click);
            // 
            // peakProtectionIntensity
            // 
            this.peakProtectionIntensity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.peakProtectionIntensity.LargeChange = 10;
            this.peakProtectionIntensity.Location = new System.Drawing.Point(1671, 361);
            this.peakProtectionIntensity.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.peakProtectionIntensity.Maximum = 20;
            this.peakProtectionIntensity.Name = "peakProtectionIntensity";
            this.peakProtectionIntensity.Size = new System.Drawing.Size(286, 69);
            this.peakProtectionIntensity.TabIndex = 4;
            this.peakProtectionIntensity.Scroll += new System.EventHandler(this.Threshold_Scroll);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1671, 399);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 21);
            this.label1.TabIndex = 5;
            this.label1.Text = "PeakProtectionIntensity: ";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.Location = new System.Drawing.Point(1922, 399);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 21);
            this.label2.TabIndex = 6;
            this.label2.Text = "0.0";
            // 
            // selectFile
            // 
            this.selectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectFile.Location = new System.Drawing.Point(1676, 793);
            this.selectFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.selectFile.Name = "selectFile";
            this.selectFile.Size = new System.Drawing.Size(286, 30);
            this.selectFile.TabIndex = 7;
            this.selectFile.Text = "Select File";
            this.selectFile.UseVisualStyleBackColor = true;
            this.selectFile.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // saveFigure
            // 
            this.saveFigure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveFigure.Location = new System.Drawing.Point(1676, 940);
            this.saveFigure.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.saveFigure.Name = "saveFigure";
            this.saveFigure.Size = new System.Drawing.Size(286, 30);
            this.saveFigure.TabIndex = 8;
            this.saveFigure.Text = "Save Figure";
            this.saveFigure.UseVisualStyleBackColor = true;
            this.saveFigure.Click += new System.EventHandler(this.SaveFigure_Click);
            // 
            // isPlotMax
            // 
            this.isPlotMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.isPlotMax.AutoSize = true;
            this.isPlotMax.Checked = true;
            this.isPlotMax.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isPlotMax.Location = new System.Drawing.Point(1677, 80);
            this.isPlotMax.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.isPlotMax.Name = "isPlotMax";
            this.isPlotMax.Size = new System.Drawing.Size(208, 25);
            this.isPlotMax.TabIndex = 9;
            this.isPlotMax.Text = "Plot Maximum Value";
            this.isPlotMax.UseVisualStyleBackColor = true;
            this.isPlotMax.CheckedChanged += new System.EventHandler(this.IsPlotMax_CheckedChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 100;
            this.toolTip1.AutoPopDelay = 0;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.ReshowDelay = 20;
            // 
            // isAlignMax
            // 
            this.isAlignMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.isAlignMax.AutoSize = true;
            this.isAlignMax.Location = new System.Drawing.Point(1677, 235);
            this.isAlignMax.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.isAlignMax.Name = "isAlignMax";
            this.isAlignMax.Size = new System.Drawing.Size(248, 25);
            this.isAlignMax.TabIndex = 15;
            this.isAlignMax.Text = "Align the Maximum Value";
            this.toolTip1.SetToolTip(this.isAlignMax, "二つ以上のグラフが描画された時、最大値を揃えます。Set the IgnitionTime to 0 [s]と共存できません。");
            this.isAlignMax.UseVisualStyleBackColor = true;
            this.isAlignMax.CheckedChanged += new System.EventHandler(this.IsAlignMax_CheckedChanged);
            // 
            // SetIgnitionTimeZero
            // 
            this.SetIgnitionTimeZero.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SetIgnitionTimeZero.AutoSize = true;
            this.SetIgnitionTimeZero.Checked = true;
            this.SetIgnitionTimeZero.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SetIgnitionTimeZero.Location = new System.Drawing.Point(1677, 204);
            this.SetIgnitionTimeZero.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.SetIgnitionTimeZero.Name = "SetIgnitionTimeZero";
            this.SetIgnitionTimeZero.Size = new System.Drawing.Size(268, 25);
            this.SetIgnitionTimeZero.TabIndex = 32;
            this.SetIgnitionTimeZero.Text = "Set the IgnitionTime to 0 [s]";
            this.toolTip1.SetToolTip(this.SetIgnitionTimeZero, "燃焼開始時刻を0秒としてグラフを描画します。Align the Maximum Valueと共存できません。");
            this.SetIgnitionTimeZero.UseVisualStyleBackColor = true;
            this.SetIgnitionTimeZero.CheckedChanged += new System.EventHandler(this.SetIgnitionTimeZero_CheckedChanged);
            // 
            // dataColumn
            // 
            this.dataColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dataColumn.AutoSize = true;
            this.dataColumn.Location = new System.Drawing.Point(1676, 761);
            this.dataColumn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dataColumn.Name = "dataColumn";
            this.dataColumn.Size = new System.Drawing.Size(128, 21);
            this.dataColumn.TabIndex = 10;
            this.dataColumn.Text = "Data Column: ";
            // 
            // timeColumn
            // 
            this.timeColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.timeColumn.AutoSize = true;
            this.timeColumn.Location = new System.Drawing.Point(1675, 726);
            this.timeColumn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.timeColumn.Name = "timeColumn";
            this.timeColumn.Size = new System.Drawing.Size(132, 21);
            this.timeColumn.TabIndex = 11;
            this.timeColumn.Text = "Time Column: ";
            // 
            // dataColumnNum
            // 
            this.dataColumnNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dataColumnNum.Location = new System.Drawing.Point(1814, 758);
            this.dataColumnNum.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dataColumnNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dataColumnNum.Name = "dataColumnNum";
            this.dataColumnNum.Size = new System.Drawing.Size(149, 28);
            this.dataColumnNum.TabIndex = 12;
            this.dataColumnNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.dataColumnNum.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // timeColumnNum
            // 
            this.timeColumnNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.timeColumnNum.Location = new System.Drawing.Point(1814, 723);
            this.timeColumnNum.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.timeColumnNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timeColumnNum.Name = "timeColumnNum";
            this.timeColumnNum.Size = new System.Drawing.Size(149, 28);
            this.timeColumnNum.TabIndex = 13;
            this.timeColumnNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.timeColumnNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // graphInit
            // 
            this.graphInit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.graphInit.Location = new System.Drawing.Point(1676, 829);
            this.graphInit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.graphInit.Name = "graphInit";
            this.graphInit.Size = new System.Drawing.Size(286, 30);
            this.graphInit.TabIndex = 14;
            this.graphInit.Text = "Graph Initialization";
            this.graphInit.UseVisualStyleBackColor = true;
            this.graphInit.Click += new System.EventHandler(this.GraphInit_Click);
            // 
            // isPlotImpulse
            // 
            this.isPlotImpulse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.isPlotImpulse.AutoSize = true;
            this.isPlotImpulse.Checked = true;
            this.isPlotImpulse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isPlotImpulse.Location = new System.Drawing.Point(1677, 111);
            this.isPlotImpulse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.isPlotImpulse.Name = "isPlotImpulse";
            this.isPlotImpulse.Size = new System.Drawing.Size(189, 25);
            this.isPlotImpulse.TabIndex = 16;
            this.isPlotImpulse.Text = "Plot Total Impulse";
            this.isPlotImpulse.UseVisualStyleBackColor = true;
            this.isPlotImpulse.CheckedChanged += new System.EventHandler(this.IsPlotImpulse_CheckedChanged);
            // 
            // offsetRemoval
            // 
            this.offsetRemoval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.offsetRemoval.AutoSize = true;
            this.offsetRemoval.Checked = true;
            this.offsetRemoval.CheckState = System.Windows.Forms.CheckState.Checked;
            this.offsetRemoval.Location = new System.Drawing.Point(1677, 299);
            this.offsetRemoval.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.offsetRemoval.Name = "offsetRemoval";
            this.offsetRemoval.Size = new System.Drawing.Size(163, 25);
            this.offsetRemoval.TabIndex = 17;
            this.offsetRemoval.Text = "Offset Removal";
            this.offsetRemoval.UseVisualStyleBackColor = true;
            this.offsetRemoval.CheckedChanged += new System.EventHandler(this.Deoffset_CheckedChanged);
            // 
            // isPlotBurnTime
            // 
            this.isPlotBurnTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.isPlotBurnTime.AutoSize = true;
            this.isPlotBurnTime.Checked = true;
            this.isPlotBurnTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isPlotBurnTime.Location = new System.Drawing.Point(1677, 142);
            this.isPlotBurnTime.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.isPlotBurnTime.Name = "isPlotBurnTime";
            this.isPlotBurnTime.Size = new System.Drawing.Size(158, 25);
            this.isPlotBurnTime.TabIndex = 18;
            this.isPlotBurnTime.Text = "Plot Burn time";
            this.isPlotBurnTime.UseVisualStyleBackColor = true;
            // 
            // showReadme
            // 
            this.showReadme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.showReadme.Location = new System.Drawing.Point(1675, 617);
            this.showReadme.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.showReadme.Name = "showReadme";
            this.showReadme.Size = new System.Drawing.Size(286, 30);
            this.showReadme.TabIndex = 19;
            this.showReadme.Text = "Readme";
            this.showReadme.UseVisualStyleBackColor = true;
            this.showReadme.Click += new System.EventHandler(this.ShowReadme_Click);
            // 
            // readAsVol
            // 
            this.readAsVol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.readAsVol.AutoSize = true;
            this.readAsVol.Checked = true;
            this.readAsVol.CheckState = System.Windows.Forms.CheckState.Checked;
            this.readAsVol.Location = new System.Drawing.Point(1677, 18);
            this.readAsVol.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.readAsVol.Name = "readAsVol";
            this.readAsVol.Size = new System.Drawing.Size(178, 25);
            this.readAsVol.TabIndex = 20;
            this.readAsVol.Text = "Read as Volutage";
            this.readAsVol.UseVisualStyleBackColor = true;
            this.readAsVol.CheckedChanged += new System.EventHandler(this.RreadAsVol_CheckedChanged);
            // 
            // readAsMs
            // 
            this.readAsMs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.readAsMs.AutoSize = true;
            this.readAsMs.Checked = true;
            this.readAsMs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.readAsMs.Location = new System.Drawing.Point(1677, 49);
            this.readAsMs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.readAsMs.Name = "readAsMs";
            this.readAsMs.Size = new System.Drawing.Size(199, 25);
            this.readAsMs.TabIndex = 21;
            this.readAsMs.Text = "Read as Millisecond";
            this.readAsMs.UseVisualStyleBackColor = true;
            this.readAsMs.CheckedChanged += new System.EventHandler(this.ReadAsMs_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.Location = new System.Drawing.Point(1912, 464);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 21);
            this.label3.TabIndex = 24;
            this.label3.Text = "0.10";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1671, 464);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(247, 21);
            this.label4.TabIndex = 23;
            this.label4.Text = "IgnitionDetectionThreshold:";
            // 
            // igniThreshold
            // 
            this.igniThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.igniThreshold.LargeChange = 10;
            this.igniThreshold.Location = new System.Drawing.Point(1671, 426);
            this.igniThreshold.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.igniThreshold.Maximum = 100;
            this.igniThreshold.Name = "igniThreshold";
            this.igniThreshold.Size = new System.Drawing.Size(286, 69);
            this.igniThreshold.TabIndex = 22;
            this.igniThreshold.Value = 10;
            this.igniThreshold.Scroll += new System.EventHandler(this.IgniThreshold_Scroll);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label5.Location = new System.Drawing.Point(1912, 529);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 21);
            this.label5.TabIndex = 27;
            this.label5.Text = "0.03";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1671, 529);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(252, 21);
            this.label6.TabIndex = 26;
            this.label6.Text = "BurnoutDetectionThreshold:";
            // 
            // burnoutThreshold
            // 
            this.burnoutThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.burnoutThreshold.LargeChange = 10;
            this.burnoutThreshold.Location = new System.Drawing.Point(1671, 491);
            this.burnoutThreshold.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.burnoutThreshold.Maximum = 100;
            this.burnoutThreshold.Name = "burnoutThreshold";
            this.burnoutThreshold.Size = new System.Drawing.Size(286, 69);
            this.burnoutThreshold.TabIndex = 25;
            this.burnoutThreshold.Value = 3;
            this.burnoutThreshold.Scroll += new System.EventHandler(this.BurnOutThreshold_Scroll);
            // 
            // skipTime
            // 
            this.skipTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.skipTime.Location = new System.Drawing.Point(1814, 654);
            this.skipTime.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.skipTime.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.skipTime.Name = "skipTime";
            this.skipTime.Size = new System.Drawing.Size(149, 28);
            this.skipTime.TabIndex = 31;
            this.skipTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cutoffTime
            // 
            this.cutoffTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cutoffTime.Location = new System.Drawing.Point(1814, 689);
            this.cutoffTime.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cutoffTime.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.cutoffTime.Name = "cutoffTime";
            this.cutoffTime.Size = new System.Drawing.Size(149, 28);
            this.cutoffTime.TabIndex = 30;
            this.cutoffTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1675, 656);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 21);
            this.label7.TabIndex = 29;
            this.label7.Text = "Skip[ms]: ";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1676, 692);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 21);
            this.label8.TabIndex = 28;
            this.label8.Text = "Cut off[ms]: ";
            // 
            // GraphScaleInit
            // 
            this.GraphScaleInit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.GraphScaleInit.Location = new System.Drawing.Point(1676, 866);
            this.GraphScaleInit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.GraphScaleInit.Name = "GraphScaleInit";
            this.GraphScaleInit.Size = new System.Drawing.Size(286, 30);
            this.GraphScaleInit.TabIndex = 33;
            this.GraphScaleInit.Text = "Graph Scale Initialization";
            this.GraphScaleInit.UseVisualStyleBackColor = true;
            this.GraphScaleInit.Click += new System.EventHandler(this.GraphScaleInit_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 973);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 18, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1978, 28);
            this.statusStrip1.TabIndex = 34;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.Name = "toolStripStatus";
            this.toolStripStatus.Size = new System.Drawing.Size(80, 21);
            this.toolStripStatus.Text = "Standby";
            this.toolStripStatus.Click += new System.EventHandler(this.toolStripStatus_Click);
            // 
            // isPlotAverageThrust
            // 
            this.isPlotAverageThrust.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.isPlotAverageThrust.AutoSize = true;
            this.isPlotAverageThrust.Location = new System.Drawing.Point(1677, 173);
            this.isPlotAverageThrust.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.isPlotAverageThrust.Name = "isPlotAverageThrust";
            this.isPlotAverageThrust.Size = new System.Drawing.Size(200, 25);
            this.isPlotAverageThrust.TabIndex = 35;
            this.isPlotAverageThrust.Text = "Plot Average Thrust";
            this.isPlotAverageThrust.UseVisualStyleBackColor = true;
            this.isPlotAverageThrust.CheckedChanged += new System.EventHandler(this.IsPlotAverageThrust_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1978, 1001);
            this.Controls.Add(this.isPlotAverageThrust);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.GraphScaleInit);
            this.Controls.Add(this.SetIgnitionTimeZero);
            this.Controls.Add(this.skipTime);
            this.Controls.Add(this.cutoffTime);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.burnoutThreshold);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.igniThreshold);
            this.Controls.Add(this.readAsMs);
            this.Controls.Add(this.readAsVol);
            this.Controls.Add(this.showReadme);
            this.Controls.Add(this.isPlotBurnTime);
            this.Controls.Add(this.offsetRemoval);
            this.Controls.Add(this.isPlotImpulse);
            this.Controls.Add(this.isAlignMax);
            this.Controls.Add(this.graphInit);
            this.Controls.Add(this.timeColumnNum);
            this.Controls.Add(this.dataColumnNum);
            this.Controls.Add(this.timeColumn);
            this.Controls.Add(this.dataColumn);
            this.Controls.Add(this.isPlotMax);
            this.Controls.Add(this.saveFigure);
            this.Controls.Add(this.selectFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.peakProtectionIntensity);
            this.Controls.Add(this.plot);
            this.Controls.Add(this.showPeakProtectionIntensity);
            this.Controls.Add(this.denoise);
            this.Controls.Add(this.formsPlot1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(1994, 1047);
            this.Name = "Form1";
            this.Text = "GraphPlotter";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.peakProtectionIntensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataColumnNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeColumnNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.igniThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.burnoutThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.skipTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cutoffTime)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScottPlot.FormsPlot formsPlot1;
        private CheckBox denoise;
        private CheckBox showPeakProtectionIntensity;
        private Button plot;
        private TrackBar peakProtectionIntensity;
        private Label label1;
        private Label label2;
        private Button selectFile;
        private Button saveFigure;
        private CheckBox isPlotMax;
        private ToolTip toolTip1;
        private Label dataColumn;
        private Label timeColumn;
        private NumericUpDown dataColumnNum;
        private NumericUpDown timeColumnNum;
        private Button graphInit;
        private CheckBox isAlignMax;
        private CheckBox isPlotImpulse;
        private CheckBox offsetRemoval;
        private CheckBox isPlotBurnTime;
        private Button showReadme;
        private CheckBox readAsVol;
        private CheckBox readAsMs;
        private Label label3;
        private Label label4;
        private TrackBar igniThreshold;
        private Label label5;
        private Label label6;
        private TrackBar burnoutThreshold;
        private NumericUpDown skipTime;
        private NumericUpDown cutoffTime;
        private Label label7;
        private Label label8;
        private CheckBox SetIgnitionTimeZero;
        private Button GraphScaleInit;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatus;
        private CheckBox isPlotAverageThrust;
    }
}