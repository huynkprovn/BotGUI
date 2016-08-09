using PoGo.NecroBot.GUI.UserControls;

namespace PoGo.NecroBot.GUI
{
    partial class GUI
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridConsole = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grpPlayer = new System.Windows.Forms.GroupBox();
            this.checkDoPokemons = new System.Windows.Forms.CheckBox();
            this.checkDoPokestops = new System.Windows.Forms.CheckBox();
            this.cmdStart = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textPlayerPokecoins = new System.Windows.Forms.TextBox();
            this.labelPlayerPokeHr = new System.Windows.Forms.Label();
            this.labelPlayerExpHr = new System.Windows.Forms.Label();
            this.labelPlayerExpOverLevelExp = new System.Windows.Forms.Label();
            this.progressPlayerExpBar = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.textPlayerStardust = new System.Windows.Forms.TextBox();
            this.textPlayerLevel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textPlayerName = new System.Windows.Forms.TextBox();
            this.tabControlInventory = new System.Windows.Forms.TabControl();
            this.tabMyItems = new System.Windows.Forms.TabPage();
            this.dataMyItems = new System.Windows.Forms.DataGridView();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabMyCandies = new System.Windows.Forms.TabPage();
            this.dataMyCandies = new System.Windows.Forms.DataGridView();
            this.Column15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabMyPokemons = new System.Windows.Forms.TabPage();
            this.cmdPowerupSelected = new System.Windows.Forms.Button();
            this.cmdEvolveSelected = new System.Windows.Forms.Button();
            this.cmdTransferSelected = new System.Windows.Forms.Button();
            this.dataMyPokemons = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridMyPokemonColCP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridMyPokemonColMaxCP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridMyPokemonColLvl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Move1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Move2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabLiveMap = new System.Windows.Forms.TabPage();
            this.checkShowPath = new System.Windows.Forms.CheckBox();
            this.checkShowPokegyms = new System.Windows.Forms.CheckBox();
            this.checkShowPokestops = new System.Windows.Forms.CheckBox();
            this.checkShowPokemons = new System.Windows.Forms.CheckBox();
            this.textCurrentLatLng = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.gMap = new GMap.NET.WindowsForms.GMapControl();
            this.tabManualSniping = new System.Windows.Forms.TabPage();
            this.dataSnipingFeeder = new System.Windows.Forms.DataGridView();
            this.dataSnipingFeederColImg = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataSnipingFeederColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataSnipingFeederColIV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataSnipingFeederColLat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataSnipingFeederColLng = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataSnipingFeederColTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataSnipingFeederColBtnSnipe = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dataSnipingFeederColSort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataSnipingFeederColEncounterId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bntStartSnipingFeed = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cmdSnipeList = new System.Windows.Forms.Button();
            this.textPokemonSnipeList = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.cmdSaveSettings = new System.Windows.Forms.Button();
            this.tabControlSettings = new System.Windows.Forms.TabControl();
            this.tabPageSettingsGlobal = new System.Windows.Forms.TabPage();
            this.tabPageSettingsSniping = new System.Windows.Forms.TabPage();
            this.tabPageSettingsPokemons = new System.Windows.Forms.TabPage();
            this.tabPageSettingsItems = new System.Windows.Forms.TabPage();
            this.toolTransferSelected = new System.Windows.Forms.ToolTip(this.components);
            this.toolEvolveSelected = new System.Windows.Forms.ToolTip(this.components);
            this.globalSettingsControl = new PoGo.NecroBot.GUI.UserControls.GlobalSettingsControl();
            this.snipingSettingsControl = new PoGo.NecroBot.GUI.UserControls.SnipingSettingsControl();
            this.pokemonSettingsControl = new PoGo.NecroBot.GUI.UserControls.PokemonSettingsControl();
            this.itemSettingsControl = new PoGo.NecroBot.GUI.UserControls.ItemSettingsControl();
            this.checkAutoSnipeFromSettings = new System.Windows.Forms.CheckBox();
            this.numMinSnipeIV = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridConsole)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpPlayer.SuspendLayout();
            this.tabControlInventory.SuspendLayout();
            this.tabMyItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataMyItems)).BeginInit();
            this.tabMyCandies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataMyCandies)).BeginInit();
            this.tabControlMain.SuspendLayout();
            this.tabMyPokemons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataMyPokemons)).BeginInit();
            this.tabLiveMap.SuspendLayout();
            this.tabManualSniping.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSnipingFeeder)).BeginInit();
            this.tabSettings.SuspendLayout();
            this.tabControlSettings.SuspendLayout();
            this.tabPageSettingsGlobal.SuspendLayout();
            this.tabPageSettingsSniping.SuspendLayout();
            this.tabPageSettingsPokemons.SuspendLayout();
            this.tabPageSettingsItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinSnipeIV)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridConsole
            // 
            this.dataGridConsole.AllowUserToAddRows = false;
            this.dataGridConsole.AllowUserToDeleteRows = false;
            this.dataGridConsole.AllowUserToResizeColumns = false;
            this.dataGridConsole.AllowUserToResizeRows = false;
            this.dataGridConsole.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.tableLayoutPanel1.SetColumnSpan(this.dataGridConsole, 2);
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridConsole.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridConsole.GridColor = System.Drawing.Color.Black;
            this.dataGridConsole.Location = new System.Drawing.Point(3, 532);
            this.dataGridConsole.Name = "dataGridConsole";
            this.dataGridConsole.ReadOnly = true;
            this.dataGridConsole.RowHeadersVisible = false;
            this.dataGridConsole.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.Black;
            this.dataGridConsole.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridConsole.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Black;
            this.dataGridConsole.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridConsole.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridConsole.Size = new System.Drawing.Size(1002, 194);
            this.dataGridConsole.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "DateTime";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 150;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "LogLevel";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 75;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.HeaderText = "Message";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridConsole, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.grpPlayer, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControlInventory, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tabControlMain, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1008, 729);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // grpPlayer
            // 
            this.grpPlayer.Controls.Add(this.checkDoPokemons);
            this.grpPlayer.Controls.Add(this.checkDoPokestops);
            this.grpPlayer.Controls.Add(this.cmdStart);
            this.grpPlayer.Controls.Add(this.label4);
            this.grpPlayer.Controls.Add(this.textPlayerPokecoins);
            this.grpPlayer.Controls.Add(this.labelPlayerPokeHr);
            this.grpPlayer.Controls.Add(this.labelPlayerExpHr);
            this.grpPlayer.Controls.Add(this.labelPlayerExpOverLevelExp);
            this.grpPlayer.Controls.Add(this.progressPlayerExpBar);
            this.grpPlayer.Controls.Add(this.label3);
            this.grpPlayer.Controls.Add(this.textPlayerStardust);
            this.grpPlayer.Controls.Add(this.textPlayerLevel);
            this.grpPlayer.Controls.Add(this.label2);
            this.grpPlayer.Controls.Add(this.label1);
            this.grpPlayer.Controls.Add(this.textPlayerName);
            this.grpPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPlayer.Location = new System.Drawing.Point(3, 3);
            this.grpPlayer.Name = "grpPlayer";
            this.grpPlayer.Size = new System.Drawing.Size(294, 194);
            this.grpPlayer.TabIndex = 1;
            this.grpPlayer.TabStop = false;
            this.grpPlayer.Text = "Player";
            // 
            // checkDoPokemons
            // 
            this.checkDoPokemons.AutoSize = true;
            this.checkDoPokemons.Checked = true;
            this.checkDoPokemons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkDoPokemons.Location = new System.Drawing.Point(194, 82);
            this.checkDoPokemons.Name = "checkDoPokemons";
            this.checkDoPokemons.Size = new System.Drawing.Size(93, 17);
            this.checkDoPokemons.TabIndex = 28;
            this.checkDoPokemons.Text = "Do Pokemons";
            this.checkDoPokemons.UseVisualStyleBackColor = true;
            this.checkDoPokemons.CheckedChanged += new System.EventHandler(this.checkDoPokemons_CheckedChanged);
            // 
            // checkDoPokestops
            // 
            this.checkDoPokestops.AutoSize = true;
            this.checkDoPokestops.Checked = true;
            this.checkDoPokestops.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkDoPokestops.Location = new System.Drawing.Point(194, 65);
            this.checkDoPokestops.Name = "checkDoPokestops";
            this.checkDoPokestops.Size = new System.Drawing.Size(93, 17);
            this.checkDoPokestops.TabIndex = 27;
            this.checkDoPokestops.Text = "Do Pokestops";
            this.checkDoPokestops.UseVisualStyleBackColor = true;
            this.checkDoPokestops.CheckedChanged += new System.EventHandler(this.checkDoPokestops_CheckedChanged);
            // 
            // cmdStart
            // 
            this.cmdStart.Image = global::PoGo.NecroBot.GUI.PoGoImages.play;
            this.cmdStart.Location = new System.Drawing.Point(215, 13);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(72, 46);
            this.cmdStart.TabIndex = 1;
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Pokecoins";
            // 
            // textPlayerPokecoins
            // 
            this.textPlayerPokecoins.Location = new System.Drawing.Point(69, 97);
            this.textPlayerPokecoins.Name = "textPlayerPokecoins";
            this.textPlayerPokecoins.ReadOnly = true;
            this.textPlayerPokecoins.Size = new System.Drawing.Size(92, 20);
            this.textPlayerPokecoins.TabIndex = 26;
            // 
            // labelPlayerPokeHr
            // 
            this.labelPlayerPokeHr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayerPokeHr.Location = new System.Drawing.Point(160, 169);
            this.labelPlayerPokeHr.Name = "labelPlayerPokeHr";
            this.labelPlayerPokeHr.Size = new System.Drawing.Size(127, 23);
            this.labelPlayerPokeHr.TabIndex = 24;
            this.labelPlayerPokeHr.Text = "POKEMON/HR";
            this.labelPlayerPokeHr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPlayerExpHr
            // 
            this.labelPlayerExpHr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayerExpHr.Location = new System.Drawing.Point(0, 169);
            this.labelPlayerExpHr.Name = "labelPlayerExpHr";
            this.labelPlayerExpHr.Size = new System.Drawing.Size(154, 23);
            this.labelPlayerExpHr.TabIndex = 23;
            this.labelPlayerExpHr.Text = "EXP/HR";
            this.labelPlayerExpHr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPlayerExpOverLevelExp
            // 
            this.labelPlayerExpOverLevelExp.Location = new System.Drawing.Point(0, 149);
            this.labelPlayerExpOverLevelExp.Name = "labelPlayerExpOverLevelExp";
            this.labelPlayerExpOverLevelExp.Size = new System.Drawing.Size(287, 19);
            this.labelPlayerExpOverLevelExp.TabIndex = 22;
            this.labelPlayerExpOverLevelExp.Text = "EXP/LEVELEXP";
            this.labelPlayerExpOverLevelExp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressPlayerExpBar
            // 
            this.progressPlayerExpBar.Location = new System.Drawing.Point(0, 123);
            this.progressPlayerExpBar.Name = "progressPlayerExpBar";
            this.progressPlayerExpBar.Size = new System.Drawing.Size(287, 23);
            this.progressPlayerExpBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressPlayerExpBar.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Stardust";
            // 
            // textPlayerStardust
            // 
            this.textPlayerStardust.Location = new System.Drawing.Point(69, 71);
            this.textPlayerStardust.Name = "textPlayerStardust";
            this.textPlayerStardust.ReadOnly = true;
            this.textPlayerStardust.Size = new System.Drawing.Size(92, 20);
            this.textPlayerStardust.TabIndex = 5;
            // 
            // textPlayerLevel
            // 
            this.textPlayerLevel.Location = new System.Drawing.Point(69, 45);
            this.textPlayerLevel.Name = "textPlayerLevel";
            this.textPlayerLevel.ReadOnly = true;
            this.textPlayerLevel.Size = new System.Drawing.Size(39, 20);
            this.textPlayerLevel.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Level";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // textPlayerName
            // 
            this.textPlayerName.Location = new System.Drawing.Point(69, 19);
            this.textPlayerName.Name = "textPlayerName";
            this.textPlayerName.ReadOnly = true;
            this.textPlayerName.Size = new System.Drawing.Size(140, 20);
            this.textPlayerName.TabIndex = 2;
            // 
            // tabControlInventory
            // 
            this.tabControlInventory.Controls.Add(this.tabMyItems);
            this.tabControlInventory.Controls.Add(this.tabMyCandies);
            this.tabControlInventory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlInventory.Location = new System.Drawing.Point(3, 203);
            this.tabControlInventory.Name = "tabControlInventory";
            this.tabControlInventory.SelectedIndex = 0;
            this.tabControlInventory.Size = new System.Drawing.Size(294, 323);
            this.tabControlInventory.TabIndex = 2;
            // 
            // tabMyItems
            // 
            this.tabMyItems.Controls.Add(this.dataMyItems);
            this.tabMyItems.Location = new System.Drawing.Point(4, 22);
            this.tabMyItems.Name = "tabMyItems";
            this.tabMyItems.Padding = new System.Windows.Forms.Padding(3);
            this.tabMyItems.Size = new System.Drawing.Size(286, 297);
            this.tabMyItems.TabIndex = 0;
            this.tabMyItems.Text = "Items (0/350)";
            this.tabMyItems.UseVisualStyleBackColor = true;
            // 
            // dataMyItems
            // 
            this.dataMyItems.AllowUserToAddRows = false;
            this.dataMyItems.AllowUserToDeleteRows = false;
            this.dataMyItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataMyItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column8,
            this.Column4,
            this.Column5,
            this.Column6});
            this.dataMyItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataMyItems.Location = new System.Drawing.Point(3, 3);
            this.dataMyItems.Name = "dataMyItems";
            this.dataMyItems.ReadOnly = true;
            this.dataMyItems.RowHeadersWidth = 10;
            this.dataMyItems.RowTemplate.Height = 40;
            this.dataMyItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataMyItems.Size = new System.Drawing.Size(280, 291);
            this.dataMyItems.TabIndex = 10;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "ID";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Visible = false;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 30;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Item";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column5.Width = 150;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Count";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column6.Width = 75;
            // 
            // tabMyCandies
            // 
            this.tabMyCandies.Controls.Add(this.dataMyCandies);
            this.tabMyCandies.Location = new System.Drawing.Point(4, 22);
            this.tabMyCandies.Name = "tabMyCandies";
            this.tabMyCandies.Padding = new System.Windows.Forms.Padding(3);
            this.tabMyCandies.Size = new System.Drawing.Size(286, 297);
            this.tabMyCandies.TabIndex = 1;
            this.tabMyCandies.Text = "Candies";
            this.tabMyCandies.UseVisualStyleBackColor = true;
            // 
            // dataMyCandies
            // 
            this.dataMyCandies.AllowUserToAddRows = false;
            this.dataMyCandies.AllowUserToDeleteRows = false;
            this.dataMyCandies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataMyCandies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column15,
            this.Column13,
            this.Column14});
            this.dataMyCandies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataMyCandies.Location = new System.Drawing.Point(3, 3);
            this.dataMyCandies.Name = "dataMyCandies";
            this.dataMyCandies.ReadOnly = true;
            this.dataMyCandies.RowHeadersWidth = 10;
            this.dataMyCandies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataMyCandies.Size = new System.Drawing.Size(280, 291);
            this.dataMyCandies.TabIndex = 1;
            // 
            // Column15
            // 
            this.Column15.HeaderText = "ID";
            this.Column15.Name = "Column15";
            this.Column15.ReadOnly = true;
            this.Column15.Visible = false;
            // 
            // Column13
            // 
            this.Column13.HeaderText = "Family";
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            // 
            // Column14
            // 
            this.Column14.HeaderText = "Count";
            this.Column14.Name = "Column14";
            this.Column14.ReadOnly = true;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabMyPokemons);
            this.tabControlMain.Controls.Add(this.tabLiveMap);
            this.tabControlMain.Controls.Add(this.tabManualSniping);
            this.tabControlMain.Controls.Add(this.tabSettings);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(303, 3);
            this.tabControlMain.Name = "tabControlMain";
            this.tableLayoutPanel1.SetRowSpan(this.tabControlMain, 2);
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(702, 523);
            this.tabControlMain.TabIndex = 1;
            // 
            // tabMyPokemons
            // 
            this.tabMyPokemons.Controls.Add(this.cmdPowerupSelected);
            this.tabMyPokemons.Controls.Add(this.cmdEvolveSelected);
            this.tabMyPokemons.Controls.Add(this.cmdTransferSelected);
            this.tabMyPokemons.Controls.Add(this.dataMyPokemons);
            this.tabMyPokemons.Location = new System.Drawing.Point(4, 22);
            this.tabMyPokemons.Name = "tabMyPokemons";
            this.tabMyPokemons.Padding = new System.Windows.Forms.Padding(3);
            this.tabMyPokemons.Size = new System.Drawing.Size(694, 497);
            this.tabMyPokemons.TabIndex = 3;
            this.tabMyPokemons.Text = "Pokemons (0/250)";
            this.tabMyPokemons.UseVisualStyleBackColor = true;
            // 
            // cmdPowerupSelected
            // 
            this.cmdPowerupSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdPowerupSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.cmdPowerupSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdPowerupSelected.ForeColor = System.Drawing.Color.White;
            this.cmdPowerupSelected.Location = new System.Drawing.Point(385, 457);
            this.cmdPowerupSelected.Name = "cmdPowerupSelected";
            this.cmdPowerupSelected.Size = new System.Drawing.Size(97, 34);
            this.cmdPowerupSelected.TabIndex = 14;
            this.cmdPowerupSelected.Text = "POWER UP";
            this.toolTransferSelected.SetToolTip(this.cmdPowerupSelected, "Powerup selected pokemons");
            this.cmdPowerupSelected.UseVisualStyleBackColor = false;
            this.cmdPowerupSelected.Click += new System.EventHandler(this.cmdPowerupSelected_Click);
            // 
            // cmdEvolveSelected
            // 
            this.cmdEvolveSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdEvolveSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.cmdEvolveSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdEvolveSelected.ForeColor = System.Drawing.Color.White;
            this.cmdEvolveSelected.Location = new System.Drawing.Point(591, 457);
            this.cmdEvolveSelected.Name = "cmdEvolveSelected";
            this.cmdEvolveSelected.Size = new System.Drawing.Size(97, 34);
            this.cmdEvolveSelected.TabIndex = 13;
            this.cmdEvolveSelected.Text = "EVOLVE";
            this.toolEvolveSelected.SetToolTip(this.cmdEvolveSelected, "Evolve selected pokemons");
            this.cmdEvolveSelected.UseVisualStyleBackColor = false;
            this.cmdEvolveSelected.Click += new System.EventHandler(this.cmdEvolveSelected_Click);
            // 
            // cmdTransferSelected
            // 
            this.cmdTransferSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdTransferSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.cmdTransferSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdTransferSelected.ForeColor = System.Drawing.Color.White;
            this.cmdTransferSelected.Location = new System.Drawing.Point(488, 457);
            this.cmdTransferSelected.Name = "cmdTransferSelected";
            this.cmdTransferSelected.Size = new System.Drawing.Size(97, 34);
            this.cmdTransferSelected.TabIndex = 12;
            this.cmdTransferSelected.Text = "TRANSFER";
            this.toolTransferSelected.SetToolTip(this.cmdTransferSelected, "Transfer selected pokemons");
            this.cmdTransferSelected.UseVisualStyleBackColor = false;
            this.cmdTransferSelected.Click += new System.EventHandler(this.cmdTransferSelected_Click);
            // 
            // dataMyPokemons
            // 
            this.dataMyPokemons.AllowUserToAddRows = false;
            this.dataMyPokemons.AllowUserToDeleteRows = false;
            this.dataMyPokemons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataMyPokemons.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataMyPokemons.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewImageColumn1,
            this.dataGridViewTextBoxColumn1,
            this.Column10,
            this.dataGridMyPokemonColCP,
            this.dataGridMyPokemonColMaxCP,
            this.dataGridViewTextBoxColumn5,
            this.dataGridMyPokemonColLvl,
            this.Move1,
            this.Move2,
            this.Column9});
            this.dataMyPokemons.Location = new System.Drawing.Point(3, 3);
            this.dataMyPokemons.Name = "dataMyPokemons";
            this.dataMyPokemons.ReadOnly = true;
            this.dataMyPokemons.RowHeadersWidth = 10;
            this.dataMyPokemons.RowTemplate.Height = 40;
            this.dataMyPokemons.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataMyPokemons.Size = new System.Drawing.Size(688, 451);
            this.dataMyPokemons.TabIndex = 11;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "ID";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Width = 30;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Pokemon";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "ID";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            this.Column10.Width = 40;
            // 
            // dataGridMyPokemonColCP
            // 
            this.dataGridMyPokemonColCP.HeaderText = "CP";
            this.dataGridMyPokemonColCP.Name = "dataGridMyPokemonColCP";
            this.dataGridMyPokemonColCP.ReadOnly = true;
            this.dataGridMyPokemonColCP.Width = 50;
            // 
            // dataGridMyPokemonColMaxCP
            // 
            this.dataGridMyPokemonColMaxCP.HeaderText = "Max CP";
            this.dataGridMyPokemonColMaxCP.Name = "dataGridMyPokemonColMaxCP";
            this.dataGridMyPokemonColMaxCP.ReadOnly = true;
            this.dataGridMyPokemonColMaxCP.Width = 50;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "IV";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 50;
            // 
            // dataGridMyPokemonColLvl
            // 
            this.dataGridMyPokemonColLvl.HeaderText = "Level";
            this.dataGridMyPokemonColLvl.Name = "dataGridMyPokemonColLvl";
            this.dataGridMyPokemonColLvl.ReadOnly = true;
            this.dataGridMyPokemonColLvl.Width = 50;
            // 
            // Move1
            // 
            this.Move1.HeaderText = "Move1";
            this.Move1.Name = "Move1";
            this.Move1.ReadOnly = true;
            this.Move1.Width = 65;
            // 
            // Move2
            // 
            this.Move2.HeaderText = "Move2";
            this.Move2.Name = "Move2";
            this.Move2.ReadOnly = true;
            this.Move2.Width = 65;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Power";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            // 
            // tabLiveMap
            // 
            this.tabLiveMap.Controls.Add(this.checkShowPath);
            this.tabLiveMap.Controls.Add(this.checkShowPokegyms);
            this.tabLiveMap.Controls.Add(this.checkShowPokestops);
            this.tabLiveMap.Controls.Add(this.checkShowPokemons);
            this.tabLiveMap.Controls.Add(this.textCurrentLatLng);
            this.tabLiveMap.Controls.Add(this.label5);
            this.tabLiveMap.Controls.Add(this.gMap);
            this.tabLiveMap.Location = new System.Drawing.Point(4, 22);
            this.tabLiveMap.Name = "tabLiveMap";
            this.tabLiveMap.Padding = new System.Windows.Forms.Padding(3);
            this.tabLiveMap.Size = new System.Drawing.Size(694, 497);
            this.tabLiveMap.TabIndex = 0;
            this.tabLiveMap.Text = "Live Map";
            this.tabLiveMap.UseVisualStyleBackColor = true;
            // 
            // checkShowPath
            // 
            this.checkShowPath.AutoSize = true;
            this.checkShowPath.Checked = true;
            this.checkShowPath.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkShowPath.Location = new System.Drawing.Point(344, 31);
            this.checkShowPath.Name = "checkShowPath";
            this.checkShowPath.Size = new System.Drawing.Size(108, 17);
            this.checkShowPath.TabIndex = 28;
            this.checkShowPath.Text = "Show Path taken";
            this.checkShowPath.UseVisualStyleBackColor = true;
            this.checkShowPath.CheckedChanged += new System.EventHandler(this.checkShowPath_CheckedChanged);
            // 
            // checkShowPokegyms
            // 
            this.checkShowPokegyms.AutoSize = true;
            this.checkShowPokegyms.Checked = true;
            this.checkShowPokegyms.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkShowPokegyms.Location = new System.Drawing.Point(232, 31);
            this.checkShowPokegyms.Name = "checkShowPokegyms";
            this.checkShowPokegyms.Size = new System.Drawing.Size(105, 17);
            this.checkShowPokegyms.TabIndex = 27;
            this.checkShowPokegyms.Text = "Show Pokegyms";
            this.checkShowPokegyms.UseVisualStyleBackColor = true;
            this.checkShowPokegyms.CheckedChanged += new System.EventHandler(this.checkShowPokegyms_CheckedChanged);
            // 
            // checkShowPokestops
            // 
            this.checkShowPokestops.AutoSize = true;
            this.checkShowPokestops.Checked = true;
            this.checkShowPokestops.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkShowPokestops.Location = new System.Drawing.Point(120, 31);
            this.checkShowPokestops.Name = "checkShowPokestops";
            this.checkShowPokestops.Size = new System.Drawing.Size(106, 17);
            this.checkShowPokestops.TabIndex = 26;
            this.checkShowPokestops.Text = "Show Pokestops";
            this.checkShowPokestops.UseVisualStyleBackColor = true;
            this.checkShowPokestops.CheckedChanged += new System.EventHandler(this.checkShowPokestops_CheckedChanged);
            // 
            // checkShowPokemons
            // 
            this.checkShowPokemons.AutoSize = true;
            this.checkShowPokemons.Checked = true;
            this.checkShowPokemons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkShowPokemons.Location = new System.Drawing.Point(8, 31);
            this.checkShowPokemons.Name = "checkShowPokemons";
            this.checkShowPokemons.Size = new System.Drawing.Size(106, 17);
            this.checkShowPokemons.TabIndex = 25;
            this.checkShowPokemons.Text = "Show Pokémons";
            this.checkShowPokemons.UseVisualStyleBackColor = true;
            this.checkShowPokemons.CheckedChanged += new System.EventHandler(this.checkShowPokemons_CheckedChanged);
            // 
            // textCurrentLatLng
            // 
            this.textCurrentLatLng.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textCurrentLatLng.Location = new System.Drawing.Point(85, 6);
            this.textCurrentLatLng.Name = "textCurrentLatLng";
            this.textCurrentLatLng.ReadOnly = true;
            this.textCurrentLatLng.Size = new System.Drawing.Size(467, 20);
            this.textCurrentLatLng.TabIndex = 24;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Current lat/lng";
            // 
            // gMap
            // 
            this.gMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gMap.Bearing = 0F;
            this.gMap.CanDragMap = true;
            this.gMap.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMap.GrayScaleMode = false;
            this.gMap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMap.LevelsKeepInMemmory = 5;
            this.gMap.Location = new System.Drawing.Point(6, 54);
            this.gMap.MarkersEnabled = true;
            this.gMap.MaxZoom = 18;
            this.gMap.MinZoom = 2;
            this.gMap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
            this.gMap.Name = "gMap";
            this.gMap.NegativeMode = false;
            this.gMap.PolygonsEnabled = true;
            this.gMap.RetryLoadTile = 0;
            this.gMap.RoutesEnabled = true;
            this.gMap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMap.ShowTileGridLines = false;
            this.gMap.Size = new System.Drawing.Size(682, 437);
            this.gMap.TabIndex = 0;
            this.gMap.Zoom = 0D;
            // 
            // tabManualSniping
            // 
            this.tabManualSniping.Controls.Add(this.label6);
            this.tabManualSniping.Controls.Add(this.numMinSnipeIV);
            this.tabManualSniping.Controls.Add(this.checkAutoSnipeFromSettings);
            this.tabManualSniping.Controls.Add(this.dataSnipingFeeder);
            this.tabManualSniping.Controls.Add(this.bntStartSnipingFeed);
            this.tabManualSniping.Controls.Add(this.label14);
            this.tabManualSniping.Controls.Add(this.label10);
            this.tabManualSniping.Controls.Add(this.cmdSnipeList);
            this.tabManualSniping.Controls.Add(this.textPokemonSnipeList);
            this.tabManualSniping.Controls.Add(this.label8);
            this.tabManualSniping.Location = new System.Drawing.Point(4, 22);
            this.tabManualSniping.Name = "tabManualSniping";
            this.tabManualSniping.Padding = new System.Windows.Forms.Padding(3);
            this.tabManualSniping.Size = new System.Drawing.Size(694, 497);
            this.tabManualSniping.TabIndex = 4;
            this.tabManualSniping.Text = "Pokemon Sniper";
            this.tabManualSniping.UseVisualStyleBackColor = true;
            // 
            // dataSnipingFeeder
            // 
            this.dataSnipingFeeder.AllowUserToAddRows = false;
            this.dataSnipingFeeder.AllowUserToDeleteRows = false;
            this.dataSnipingFeeder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataSnipingFeeder.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataSnipingFeeder.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataSnipingFeeder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataSnipingFeeder.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataSnipingFeederColImg,
            this.dataSnipingFeederColName,
            this.dataSnipingFeederColIV,
            this.dataSnipingFeederColLat,
            this.dataSnipingFeederColLng,
            this.dataSnipingFeederColTimestamp,
            this.dataSnipingFeederColBtnSnipe,
            this.dataSnipingFeederColSort,
            this.dataSnipingFeederColEncounterId});
            this.dataSnipingFeeder.Location = new System.Drawing.Point(9, 207);
            this.dataSnipingFeeder.Name = "dataSnipingFeeder";
            this.dataSnipingFeeder.ReadOnly = true;
            this.dataSnipingFeeder.RowHeadersWidth = 10;
            this.dataSnipingFeeder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataSnipingFeeder.Size = new System.Drawing.Size(679, 284);
            this.dataSnipingFeeder.TabIndex = 17;
            this.dataSnipingFeeder.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataSnipingFeeder_CellContentClick);
            // 
            // dataSnipingFeederColImg
            // 
            this.dataSnipingFeederColImg.HeaderText = "";
            this.dataSnipingFeederColImg.Name = "dataSnipingFeederColImg";
            this.dataSnipingFeederColImg.ReadOnly = true;
            this.dataSnipingFeederColImg.Width = 5;
            // 
            // dataSnipingFeederColName
            // 
            this.dataSnipingFeederColName.HeaderText = "Pokemon";
            this.dataSnipingFeederColName.Name = "dataSnipingFeederColName";
            this.dataSnipingFeederColName.ReadOnly = true;
            this.dataSnipingFeederColName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataSnipingFeederColName.Width = 58;
            // 
            // dataSnipingFeederColIV
            // 
            this.dataSnipingFeederColIV.HeaderText = "IV";
            this.dataSnipingFeederColIV.Name = "dataSnipingFeederColIV";
            this.dataSnipingFeederColIV.ReadOnly = true;
            this.dataSnipingFeederColIV.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataSnipingFeederColIV.Width = 23;
            // 
            // dataSnipingFeederColLat
            // 
            this.dataSnipingFeederColLat.HeaderText = "Lat";
            this.dataSnipingFeederColLat.Name = "dataSnipingFeederColLat";
            this.dataSnipingFeederColLat.ReadOnly = true;
            this.dataSnipingFeederColLat.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataSnipingFeederColLat.Width = 28;
            // 
            // dataSnipingFeederColLng
            // 
            this.dataSnipingFeederColLng.HeaderText = "Lng";
            this.dataSnipingFeederColLng.Name = "dataSnipingFeederColLng";
            this.dataSnipingFeederColLng.ReadOnly = true;
            this.dataSnipingFeederColLng.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataSnipingFeederColLng.Width = 31;
            // 
            // dataSnipingFeederColTimestamp
            // 
            this.dataSnipingFeederColTimestamp.HeaderText = "TimeStamp";
            this.dataSnipingFeederColTimestamp.Name = "dataSnipingFeederColTimestamp";
            this.dataSnipingFeederColTimestamp.ReadOnly = true;
            this.dataSnipingFeederColTimestamp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataSnipingFeederColTimestamp.Width = 66;
            // 
            // dataSnipingFeederColBtnSnipe
            // 
            this.dataSnipingFeederColBtnSnipe.HeaderText = "";
            this.dataSnipingFeederColBtnSnipe.Name = "dataSnipingFeederColBtnSnipe";
            this.dataSnipingFeederColBtnSnipe.ReadOnly = true;
            this.dataSnipingFeederColBtnSnipe.Text = "Snipe!";
            this.dataSnipingFeederColBtnSnipe.Width = 5;
            // 
            // dataSnipingFeederColSort
            // 
            this.dataSnipingFeederColSort.HeaderText = "Sort";
            this.dataSnipingFeederColSort.Name = "dataSnipingFeederColSort";
            this.dataSnipingFeederColSort.ReadOnly = true;
            this.dataSnipingFeederColSort.Visible = false;
            this.dataSnipingFeederColSort.Width = 51;
            // 
            // dataSnipingFeederColEncounterId
            // 
            this.dataSnipingFeederColEncounterId.HeaderText = "EncounterId";
            this.dataSnipingFeederColEncounterId.Name = "dataSnipingFeederColEncounterId";
            this.dataSnipingFeederColEncounterId.ReadOnly = true;
            this.dataSnipingFeederColEncounterId.Visible = false;
            this.dataSnipingFeederColEncounterId.Width = 90;
            // 
            // bntStartSnipingFeed
            // 
            this.bntStartSnipingFeed.Location = new System.Drawing.Point(9, 178);
            this.bntStartSnipingFeed.Name = "bntStartSnipingFeed";
            this.bntStartSnipingFeed.Size = new System.Drawing.Size(246, 23);
            this.bntStartSnipingFeed.TabIndex = 16;
            this.bntStartSnipingFeed.Text = "Start Sniper Feed";
            this.bntStartSnipingFeed.UseVisualStyleBackColor = true;
            this.bntStartSnipingFeed.Click += new System.EventHandler(this.bntStartSnipingFeed_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 41);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(317, 13);
            this.label14.TabIndex = 5;
            this.label14.Text = "Example: 51.502251182719,-0.12680284541418 Porygon 78% IV";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(642, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Example: [248 seconds remaining] 78% IV - Porygon at 51.502251182719,-0.126802845" +
    "41418 [ Moveset: QuickAttackFast/Psybeam ]";
            // 
            // cmdSnipeList
            // 
            this.cmdSnipeList.Location = new System.Drawing.Point(9, 72);
            this.cmdSnipeList.Name = "cmdSnipeList";
            this.cmdSnipeList.Size = new System.Drawing.Size(246, 23);
            this.cmdSnipeList.TabIndex = 2;
            this.cmdSnipeList.Text = "Snipe List!";
            this.cmdSnipeList.UseVisualStyleBackColor = true;
            this.cmdSnipeList.Click += new System.EventHandler(this.cmdSnipeList_Click);
            // 
            // textPokemonSnipeList
            // 
            this.textPokemonSnipeList.Location = new System.Drawing.Point(9, 101);
            this.textPokemonSnipeList.Multiline = true;
            this.textPokemonSnipeList.Name = "textPokemonSnipeList";
            this.textPokemonSnipeList.Size = new System.Drawing.Size(679, 71);
            this.textPokemonSnipeList.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(333, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Add sniping location and pokemon in the following format (1 per line): ";
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.cmdSaveSettings);
            this.tabSettings.Controls.Add(this.tabControlSettings);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(694, 497);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Profile settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // cmdSaveSettings
            // 
            this.cmdSaveSettings.Location = new System.Drawing.Point(7, 6);
            this.cmdSaveSettings.Name = "cmdSaveSettings";
            this.cmdSaveSettings.Size = new System.Drawing.Size(100, 23);
            this.cmdSaveSettings.TabIndex = 37;
            this.cmdSaveSettings.Text = "Save Settings";
            this.cmdSaveSettings.UseVisualStyleBackColor = true;
            this.cmdSaveSettings.Click += new System.EventHandler(this.cmdSaveSettings_Click);
            // 
            // tabControlSettings
            // 
            this.tabControlSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlSettings.Controls.Add(this.tabPageSettingsGlobal);
            this.tabControlSettings.Controls.Add(this.tabPageSettingsSniping);
            this.tabControlSettings.Controls.Add(this.tabPageSettingsPokemons);
            this.tabControlSettings.Controls.Add(this.tabPageSettingsItems);
            this.tabControlSettings.Location = new System.Drawing.Point(3, 39);
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new System.Drawing.Size(685, 455);
            this.tabControlSettings.TabIndex = 13;
            // 
            // tabPageSettingsGlobal
            // 
            this.tabPageSettingsGlobal.AutoScroll = true;
            this.tabPageSettingsGlobal.Controls.Add(this.globalSettingsControl);
            this.tabPageSettingsGlobal.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettingsGlobal.Name = "tabPageSettingsGlobal";
            this.tabPageSettingsGlobal.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettingsGlobal.Size = new System.Drawing.Size(677, 429);
            this.tabPageSettingsGlobal.TabIndex = 0;
            this.tabPageSettingsGlobal.Text = "Global";
            this.tabPageSettingsGlobal.UseVisualStyleBackColor = true;
            // 
            // tabPageSettingsSniping
            // 
            this.tabPageSettingsSniping.AutoScroll = true;
            this.tabPageSettingsSniping.Controls.Add(this.snipingSettingsControl);
            this.tabPageSettingsSniping.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettingsSniping.Name = "tabPageSettingsSniping";
            this.tabPageSettingsSniping.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettingsSniping.Size = new System.Drawing.Size(677, 429);
            this.tabPageSettingsSniping.TabIndex = 3;
            this.tabPageSettingsSniping.Text = "Sniping";
            this.tabPageSettingsSniping.UseVisualStyleBackColor = true;
            // 
            // tabPageSettingsPokemons
            // 
            this.tabPageSettingsPokemons.AutoScroll = true;
            this.tabPageSettingsPokemons.Controls.Add(this.pokemonSettingsControl);
            this.tabPageSettingsPokemons.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettingsPokemons.Name = "tabPageSettingsPokemons";
            this.tabPageSettingsPokemons.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettingsPokemons.Size = new System.Drawing.Size(677, 429);
            this.tabPageSettingsPokemons.TabIndex = 1;
            this.tabPageSettingsPokemons.Text = "Pokemons";
            this.tabPageSettingsPokemons.UseVisualStyleBackColor = true;
            // 
            // tabPageSettingsItems
            // 
            this.tabPageSettingsItems.AutoScroll = true;
            this.tabPageSettingsItems.Controls.Add(this.itemSettingsControl);
            this.tabPageSettingsItems.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettingsItems.Name = "tabPageSettingsItems";
            this.tabPageSettingsItems.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettingsItems.Size = new System.Drawing.Size(677, 429);
            this.tabPageSettingsItems.TabIndex = 2;
            this.tabPageSettingsItems.Text = "Items";
            this.tabPageSettingsItems.UseVisualStyleBackColor = true;
            // 
            // globalSettingsControl
            // 
            this.globalSettingsControl.AutoScroll = true;
            this.globalSettingsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.globalSettingsControl.Location = new System.Drawing.Point(3, 3);
            this.globalSettingsControl.Name = "globalSettingsControl";
            this.globalSettingsControl.Size = new System.Drawing.Size(671, 423);
            this.globalSettingsControl.TabIndex = 36;
            // 
            // snipingSettingsControl
            // 
            this.snipingSettingsControl.AutoScroll = true;
            this.snipingSettingsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.snipingSettingsControl.Location = new System.Drawing.Point(3, 3);
            this.snipingSettingsControl.Name = "snipingSettingsControl";
            this.snipingSettingsControl.Size = new System.Drawing.Size(671, 423);
            this.snipingSettingsControl.TabIndex = 0;
            // 
            // pokemonSettingsControl
            // 
            this.pokemonSettingsControl.AutoScroll = true;
            this.pokemonSettingsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pokemonSettingsControl.Location = new System.Drawing.Point(3, 3);
            this.pokemonSettingsControl.Name = "pokemonSettingsControl";
            this.pokemonSettingsControl.Size = new System.Drawing.Size(671, 423);
            this.pokemonSettingsControl.TabIndex = 0;
            // 
            // itemSettingsControl
            // 
            this.itemSettingsControl.AutoScroll = true;
            this.itemSettingsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemSettingsControl.Location = new System.Drawing.Point(3, 3);
            this.itemSettingsControl.Name = "itemSettingsControl";
            this.itemSettingsControl.Size = new System.Drawing.Size(671, 423);
            this.itemSettingsControl.TabIndex = 0;
            // 
            // checkAutoSnipeFromSettings
            // 
            this.checkAutoSnipeFromSettings.AutoSize = true;
            this.checkAutoSnipeFromSettings.Location = new System.Drawing.Point(261, 182);
            this.checkAutoSnipeFromSettings.Name = "checkAutoSnipeFromSettings";
            this.checkAutoSnipeFromSettings.Size = new System.Drawing.Size(184, 17);
            this.checkAutoSnipeFromSettings.TabIndex = 18;
            this.checkAutoSnipeFromSettings.Text = "Auto snipe (pokemon to snipe list)";
            this.checkAutoSnipeFromSettings.UseVisualStyleBackColor = true;
            // 
            // numMinSnipeIV
            // 
            this.numMinSnipeIV.Location = new System.Drawing.Point(579, 181);
            this.numMinSnipeIV.Name = "numMinSnipeIV";
            this.numMinSnipeIV.Size = new System.Drawing.Size(60, 20);
            this.numMinSnipeIV.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(465, 183);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Min IV (unknown = 0)";
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "GUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NecroBot GUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GUI_FormClosing);
            this.Load += new System.EventHandler(this.GUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridConsole)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.grpPlayer.ResumeLayout(false);
            this.grpPlayer.PerformLayout();
            this.tabControlInventory.ResumeLayout(false);
            this.tabMyItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataMyItems)).EndInit();
            this.tabMyCandies.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataMyCandies)).EndInit();
            this.tabControlMain.ResumeLayout(false);
            this.tabMyPokemons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataMyPokemons)).EndInit();
            this.tabLiveMap.ResumeLayout(false);
            this.tabLiveMap.PerformLayout();
            this.tabManualSniping.ResumeLayout(false);
            this.tabManualSniping.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSnipingFeeder)).EndInit();
            this.tabSettings.ResumeLayout(false);
            this.tabControlSettings.ResumeLayout(false);
            this.tabPageSettingsGlobal.ResumeLayout(false);
            this.tabPageSettingsSniping.ResumeLayout(false);
            this.tabPageSettingsPokemons.ResumeLayout(false);
            this.tabPageSettingsItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numMinSnipeIV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridConsole;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox grpPlayer;
        private System.Windows.Forms.TextBox textPlayerLevel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textPlayerName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textPlayerStardust;
        private System.Windows.Forms.Label labelPlayerPokeHr;
        private System.Windows.Forms.Label labelPlayerExpHr;
        private System.Windows.Forms.Label labelPlayerExpOverLevelExp;
        private System.Windows.Forms.ProgressBar progressPlayerExpBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textPlayerPokecoins;
        private System.Windows.Forms.TabControl tabControlInventory;
        private System.Windows.Forms.TabPage tabMyItems;
        private System.Windows.Forms.TabPage tabMyCandies;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabLiveMap;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.DataGridView dataMyItems;
        private System.Windows.Forms.DataGridView dataMyCandies;
        private System.Windows.Forms.DataGridView dataMyPokemons;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column15;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column14;
        private System.Windows.Forms.CheckBox checkShowPath;
        private System.Windows.Forms.CheckBox checkShowPokegyms;
        private System.Windows.Forms.CheckBox checkShowPokestops;
        private System.Windows.Forms.CheckBox checkShowPokemons;
        private System.Windows.Forms.TextBox textCurrentLatLng;
        private System.Windows.Forms.Label label5;
        private GMap.NET.WindowsForms.GMapControl gMap;
        private System.Windows.Forms.TabControl tabControlSettings;
        private System.Windows.Forms.TabPage tabPageSettingsGlobal;
        private System.Windows.Forms.TabPage tabPageSettingsPokemons;
        private System.Windows.Forms.TabPage tabPageSettingsItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewImageColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private GlobalSettingsControl globalSettingsControl;
        private System.Windows.Forms.Button cmdSaveSettings;
        private System.Windows.Forms.Button cmdStart;
        private System.Windows.Forms.TabPage tabMyPokemons;
        private System.Windows.Forms.TabPage tabPageSettingsSniping;
        private UserControls.SnipingSettingsControl snipingSettingsControl;
        private System.Windows.Forms.TabPage tabManualSniping;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button cmdSnipeList;
        private System.Windows.Forms.TextBox textPokemonSnipeList;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button cmdEvolveSelected;
        private System.Windows.Forms.Button cmdTransferSelected;
        private System.Windows.Forms.ToolTip toolEvolveSelected;
        private System.Windows.Forms.ToolTip toolTransferSelected;
        private ItemSettingsControl itemSettingsControl;
        private PokemonSettingsControl pokemonSettingsControl;
        private System.Windows.Forms.CheckBox checkDoPokemons;
        private System.Windows.Forms.CheckBox checkDoPokestops;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Button cmdPowerupSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridMyPokemonColCP;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridMyPokemonColMaxCP;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridMyPokemonColLvl;
        private System.Windows.Forms.DataGridViewTextBoxColumn Move1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Move2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.Button bntStartSnipingFeed;
        private System.Windows.Forms.DataGridView dataSnipingFeeder;
        private System.Windows.Forms.DataGridViewImageColumn dataSnipingFeederColImg;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataSnipingFeederColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataSnipingFeederColIV;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataSnipingFeederColLat;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataSnipingFeederColLng;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataSnipingFeederColTimestamp;
        private System.Windows.Forms.DataGridViewButtonColumn dataSnipingFeederColBtnSnipe;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataSnipingFeederColSort;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataSnipingFeederColEncounterId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numMinSnipeIV;
        private System.Windows.Forms.CheckBox checkAutoSnipeFromSettings;
    }
}

