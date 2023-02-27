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
            this.DenoisedVsRaw = new System.Windows.Forms.CheckBox();
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
            this.UndoGraph = new System.Windows.Forms.Button();
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
            this.formsPlot1.Location = new System.Drawing.Point(14, 13);
            this.formsPlot1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(1319, 914);
            this.formsPlot1.TabIndex = 0;
            // 
            // denoise
            // 
            this.denoise.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.denoise.AutoSize = true;
            this.denoise.Location = new System.Drawing.Point(1339, 253);
            this.denoise.Name = "denoise";
            this.denoise.Size = new System.Drawing.Size(85, 24);
            this.denoise.TabIndex = 1;
            this.denoise.Text = "Denoise";
            this.denoise.UseVisualStyleBackColor = true;
            this.denoise.CheckedChanged += new System.EventHandler(this.Denoise_CheckedChanged);
            // 
            // showPeakProtectionIntensity
            // 
            this.showPeakProtectionIntensity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showPeakProtectionIntensity.AutoSize = true;
            this.showPeakProtectionIntensity.Location = new System.Drawing.Point(1305, 342);
            this.showPeakProtectionIntensity.Name = "showPeakProtectionIntensity";
            this.showPeakProtectionIntensity.Size = new System.Drawing.Size(229, 24);
            this.showPeakProtectionIntensity.TabIndex = 2;
            this.showPeakProtectionIntensity.Text = "Show PeakProtection Intensity";
            this.toolTip1.SetToolTip(this.showPeakProtectionIntensity, "デノイズによって鈍るピークを保護するためのパラメータです。0のときはデノイズ結果をそのまま表示し、1のときは元データをそのまま表示します。");
            this.showPeakProtectionIntensity.UseVisualStyleBackColor = true;
            this.showPeakProtectionIntensity.CheckedChanged += new System.EventHandler(this.ShowPeakProtectionIntensity_CheckedChanged);
            // 
            // plot
            // 
            this.plot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.plot.Location = new System.Drawing.Point(1298, 866);
            this.plot.Name = "plot";
            this.plot.Size = new System.Drawing.Size(229, 29);
            this.plot.TabIndex = 3;
            this.plot.Text = "Plot";
            this.plot.UseVisualStyleBackColor = true;
            this.plot.Click += new System.EventHandler(this.Plot_Click);
            // 
            // peakProtectionIntensity
            // 
            this.peakProtectionIntensity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.peakProtectionIntensity.Location = new System.Drawing.Point(1297, 371);
            this.peakProtectionIntensity.Maximum = 40;
            this.peakProtectionIntensity.Name = "peakProtectionIntensity";
            this.peakProtectionIntensity.Size = new System.Drawing.Size(229, 56);
            this.peakProtectionIntensity.TabIndex = 4;
            this.peakProtectionIntensity.Scroll += new System.EventHandler(this.Threshold_Scroll);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1340, 408);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "PeakProtectionIntensity: ";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.Location = new System.Drawing.Point(1483, 408);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "0.00";
            // 
            // selectFile
            // 
            this.selectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectFile.Location = new System.Drawing.Point(1298, 728);
            this.selectFile.Name = "selectFile";
            this.selectFile.Size = new System.Drawing.Size(229, 29);
            this.selectFile.TabIndex = 7;
            this.selectFile.Text = "Select File";
            this.selectFile.UseVisualStyleBackColor = true;
            this.selectFile.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // saveFigure
            // 
            this.saveFigure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveFigure.Location = new System.Drawing.Point(1298, 901);
            this.saveFigure.Name = "saveFigure";
            this.saveFigure.Size = new System.Drawing.Size(229, 29);
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
            this.isPlotMax.Location = new System.Drawing.Point(1341, 76);
            this.isPlotMax.Name = "isPlotMax";
            this.isPlotMax.Size = new System.Drawing.Size(167, 24);
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
            this.isAlignMax.Location = new System.Drawing.Point(1296, 224);
            this.isAlignMax.Name = "isAlignMax";
            this.isAlignMax.Size = new System.Drawing.Size(201, 24);
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
            this.SetIgnitionTimeZero.Location = new System.Drawing.Point(1298, 194);
            this.SetIgnitionTimeZero.Name = "SetIgnitionTimeZero";
            this.SetIgnitionTimeZero.Size = new System.Drawing.Size(215, 24);
            this.SetIgnitionTimeZero.TabIndex = 32;
            this.SetIgnitionTimeZero.Text = "Set the IgnitionTime to 0 [s]";
            this.toolTip1.SetToolTip(this.SetIgnitionTimeZero, "燃焼開始時刻を0秒としてグラフを描画します。Align the Maximum Valueと共存できません。");
            this.SetIgnitionTimeZero.UseVisualStyleBackColor = true;
            this.SetIgnitionTimeZero.CheckedChanged += new System.EventHandler(this.SetIgnitionTimeZero_CheckedChanged);
            // 
            // DenoisedVsRaw
            // 
            this.DenoisedVsRaw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DenoisedVsRaw.AutoSize = true;
            this.DenoisedVsRaw.Checked = true;
            this.DenoisedVsRaw.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DenoisedVsRaw.Location = new System.Drawing.Point(1338, 283);
            this.DenoisedVsRaw.Name = "DenoisedVsRaw";
            this.DenoisedVsRaw.Size = new System.Drawing.Size(143, 24);
            this.DenoisedVsRaw.TabIndex = 36;
            this.DenoisedVsRaw.Text = "Denoised vs Raw";
            this.toolTip1.SetToolTip(this.DenoisedVsRaw, "ノイズ除去済みデータと元データを同時に表示します。");
            this.DenoisedVsRaw.UseVisualStyleBackColor = true;
            this.DenoisedVsRaw.CheckedChanged += new System.EventHandler(this.DenoisedVsRaw_CheckedChanged);
            // 
            // dataColumn
            // 
            this.dataColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dataColumn.AutoSize = true;
            this.dataColumn.Location = new System.Drawing.Point(1341, 698);
            this.dataColumn.Name = "dataColumn";
            this.dataColumn.Size = new System.Drawing.Size(103, 20);
            this.dataColumn.TabIndex = 10;
            this.dataColumn.Text = "Data Column: ";
            // 
            // timeColumn
            // 
            this.timeColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.timeColumn.AutoSize = true;
            this.timeColumn.Location = new System.Drawing.Point(1340, 665);
            this.timeColumn.Name = "timeColumn";
            this.timeColumn.Size = new System.Drawing.Size(104, 20);
            this.timeColumn.TabIndex = 11;
            this.timeColumn.Text = "Time Column: ";
            // 
            // dataColumnNum
            // 
            this.dataColumnNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dataColumnNum.Location = new System.Drawing.Point(1408, 695);
            this.dataColumnNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dataColumnNum.Name = "dataColumnNum";
            this.dataColumnNum.Size = new System.Drawing.Size(119, 27);
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
            this.timeColumnNum.Location = new System.Drawing.Point(1408, 662);
            this.timeColumnNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timeColumnNum.Name = "timeColumnNum";
            this.timeColumnNum.Size = new System.Drawing.Size(119, 27);
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
            this.graphInit.Location = new System.Drawing.Point(1298, 762);
            this.graphInit.Name = "graphInit";
            this.graphInit.Size = new System.Drawing.Size(229, 29);
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
            this.isPlotImpulse.Location = new System.Drawing.Point(1344, 106);
            this.isPlotImpulse.Name = "isPlotImpulse";
            this.isPlotImpulse.Size = new System.Drawing.Size(149, 24);
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
            this.offsetRemoval.Location = new System.Drawing.Point(1340, 312);
            this.offsetRemoval.Name = "offsetRemoval";
            this.offsetRemoval.Size = new System.Drawing.Size(132, 24);
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
            this.isPlotBurnTime.Location = new System.Drawing.Point(1343, 135);
            this.isPlotBurnTime.Name = "isPlotBurnTime";
            this.isPlotBurnTime.Size = new System.Drawing.Size(125, 24);
            this.isPlotBurnTime.TabIndex = 18;
            this.isPlotBurnTime.Text = "Plot Burn time";
            this.isPlotBurnTime.UseVisualStyleBackColor = true;
            // 
            // showReadme
            // 
            this.showReadme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.showReadme.Location = new System.Drawing.Point(1297, 561);
            this.showReadme.Name = "showReadme";
            this.showReadme.Size = new System.Drawing.Size(229, 29);
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
            this.readAsVol.Location = new System.Drawing.Point(1338, 17);
            this.readAsVol.Name = "readAsVol";
            this.readAsVol.Size = new System.Drawing.Size(146, 24);
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
            this.readAsMs.Location = new System.Drawing.Point(1338, 47);
            this.readAsMs.Name = "readAsMs";
            this.readAsMs.Size = new System.Drawing.Size(163, 24);
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
            this.label3.Location = new System.Drawing.Point(1483, 470);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 20);
            this.label3.TabIndex = 24;
            this.label3.Text = "0.05";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1340, 470);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(193, 20);
            this.label4.TabIndex = 23;
            this.label4.Text = "IgnitionDetectionThreshold:";
            // 
            // igniThreshold
            // 
            this.igniThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.igniThreshold.LargeChange = 10;
            this.igniThreshold.Location = new System.Drawing.Point(1297, 433);
            this.igniThreshold.Maximum = 100;
            this.igniThreshold.Name = "igniThreshold";
            this.igniThreshold.Size = new System.Drawing.Size(229, 56);
            this.igniThreshold.TabIndex = 22;
            this.igniThreshold.Value = 5;
            this.igniThreshold.Scroll += new System.EventHandler(this.IgniThreshold_Scroll);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label5.Location = new System.Drawing.Point(1483, 531);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 20);
            this.label5.TabIndex = 27;
            this.label5.Text = "0.05";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1340, 531);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(194, 20);
            this.label6.TabIndex = 26;
            this.label6.Text = "BurnoutDetectionThreshold:";
            // 
            // burnoutThreshold
            // 
            this.burnoutThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.burnoutThreshold.LargeChange = 10;
            this.burnoutThreshold.Location = new System.Drawing.Point(1297, 495);
            this.burnoutThreshold.Maximum = 100;
            this.burnoutThreshold.Name = "burnoutThreshold";
            this.burnoutThreshold.Size = new System.Drawing.Size(229, 56);
            this.burnoutThreshold.TabIndex = 25;
            this.burnoutThreshold.Value = 5;
            this.burnoutThreshold.Scroll += new System.EventHandler(this.BurnOutThreshold_Scroll);
            // 
            // skipTime
            // 
            this.skipTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.skipTime.Location = new System.Drawing.Point(1408, 596);
            this.skipTime.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.skipTime.Name = "skipTime";
            this.skipTime.Size = new System.Drawing.Size(119, 27);
            this.skipTime.TabIndex = 31;
            this.skipTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cutoffTime
            // 
            this.cutoffTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cutoffTime.Location = new System.Drawing.Point(1408, 630);
            this.cutoffTime.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.cutoffTime.Name = "cutoffTime";
            this.cutoffTime.Size = new System.Drawing.Size(119, 27);
            this.cutoffTime.TabIndex = 30;
            this.cutoffTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1340, 598);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 20);
            this.label7.TabIndex = 29;
            this.label7.Text = "Skip[ms]: ";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1341, 632);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 20);
            this.label8.TabIndex = 28;
            this.label8.Text = "Cut off[ms]: ";
            // 
            // GraphScaleInit
            // 
            this.GraphScaleInit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.GraphScaleInit.Location = new System.Drawing.Point(1298, 797);
            this.GraphScaleInit.Name = "GraphScaleInit";
            this.GraphScaleInit.Size = new System.Drawing.Size(229, 29);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 930);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1539, 26);
            this.statusStrip1.TabIndex = 34;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.Name = "toolStripStatus";
            this.toolStripStatus.Size = new System.Drawing.Size(63, 20);
            this.toolStripStatus.Text = "Standby";
            this.toolStripStatus.Click += new System.EventHandler(this.ToolStripStatus_Click);
            // 
            // isPlotAverageThrust
            // 
            this.isPlotAverageThrust.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.isPlotAverageThrust.AutoSize = true;
            this.isPlotAverageThrust.Location = new System.Drawing.Point(1342, 165);
            this.isPlotAverageThrust.Name = "isPlotAverageThrust";
            this.isPlotAverageThrust.Size = new System.Drawing.Size(160, 24);
            this.isPlotAverageThrust.TabIndex = 35;
            this.isPlotAverageThrust.Text = "Plot Average Thrust";
            this.isPlotAverageThrust.UseVisualStyleBackColor = true;
            this.isPlotAverageThrust.CheckedChanged += new System.EventHandler(this.IsPlotAverageThrust_CheckedChanged);
            // 
            // UndoGraph
            // 
            this.UndoGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UndoGraph.Location = new System.Drawing.Point(1298, 831);
            this.UndoGraph.Name = "UndoGraph";
            this.UndoGraph.Size = new System.Drawing.Size(229, 29);
            this.UndoGraph.TabIndex = 37;
            this.UndoGraph.Text = "Undo Graph";
            this.UndoGraph.UseVisualStyleBackColor = true;
            this.UndoGraph.Click += new System.EventHandler(this.UndoGraph_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1539, 956);
            this.Controls.Add(this.UndoGraph);
            this.Controls.Add(this.DenoisedVsRaw);
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
            this.MinimumSize = new System.Drawing.Size(1538, 972);
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
        private NumericUpDown cutoffTime;
        private Label label7;
        private Label label8;
        private CheckBox SetIgnitionTimeZero;
        private Button GraphScaleInit;
        private StatusStrip statusStrip1;
        private CheckBox isPlotAverageThrust;
        internal ToolStripStatusLabel toolStripStatus;
        private NumericUpDown timeColumnNum;
        internal NumericUpDown skipTime;
        internal Button graphInit;
        private CheckBox DenoisedVsRaw;
        private Button UndoGraph;
    }
}