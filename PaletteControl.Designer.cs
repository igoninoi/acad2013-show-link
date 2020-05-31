namespace ShowLink
{
	partial class PaletteControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaletteControl));
			this.data_grid = new System.Windows.Forms.DataGridView();
			this.URL = new System.Windows.Forms.DataGridViewLinkColumn();
			this.ID = new System.Windows.Forms.DataGridViewButtonColumn();
			this.status_strip = new System.Windows.Forms.StatusStrip();
			this.tools_dropdown_button = new System.Windows.Forms.ToolStripSplitButton();
			this.tools_select_single_button = new System.Windows.Forms.ToolStripMenuItem();
			this.tools_select_сrossingpoint_button = new System.Windows.Forms.ToolStripMenuItem();
			this.progressbar_in_status = new System.Windows.Forms.ToolStripProgressBar();
			this.row_counter_label = new System.Windows.Forms.ToolStripStatusLabel();
			this.lock_data_grid_button = new System.Windows.Forms.Button();
			this.autozoom_button = new System.Windows.Forms.Button();
			this.layers_list = new System.Windows.Forms.ComboBox();
			this.layers_list_label = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize) (this.data_grid)).BeginInit();
			this.status_strip.SuspendLayout();
			this.SuspendLayout();
			// 
			// data_grid
			// 
			this.data_grid.AllowUserToAddRows = false;
			this.data_grid.AllowUserToDeleteRows = false;
			dataGridViewCellStyle13.BackColor = System.Drawing.Color.WhiteSmoke;
			dataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.LemonChiffon;
			dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.data_grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
			this.data_grid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.data_grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.data_grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.data_grid.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
			this.data_grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.ControlDark;
			dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.data_grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
			this.data_grid.ColumnHeadersHeight = 30;
			this.data_grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.URL,
            this.ID});
			this.data_grid.Cursor = System.Windows.Forms.Cursors.Default;
			dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.LightYellow;
			dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.data_grid.DefaultCellStyle = dataGridViewCellStyle15;
			this.data_grid.Location = new System.Drawing.Point(0, 28);
			this.data_grid.Margin = new System.Windows.Forms.Padding(0);
			this.data_grid.Name = "data_grid";
			this.data_grid.ReadOnly = true;
			dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.ControlDark;
			dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.ControlDarkDark;
			dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.data_grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle16;
			this.data_grid.RowHeadersWidth = 30;
			this.data_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.data_grid.Size = new System.Drawing.Size(750, 300);
			this.data_grid.TabIndex = 1;
			this.data_grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.data_grid_CellContentClick);
			this.data_grid.CurrentCellChanged += new System.EventHandler(this.data_grid_CurrentCellChanged);
			this.data_grid.SelectionChanged += new System.EventHandler(this.data_grid_SelectionChanged);
			this.data_grid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.data_grid_CellContentClick);
			// 
			// URL
			// 
			this.URL.HeaderText = "Ссылки";
			this.URL.LinkColor = System.Drawing.Color.RoyalBlue;
			this.URL.MinimumWidth = 50;
			this.URL.Name = "URL";
			this.URL.ReadOnly = true;
			this.URL.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.URL.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.URL.Text = "";
			this.URL.VisitedLinkColor = System.Drawing.Color.Orchid;
			// 
			// ID
			// 
			this.ID.HeaderText = "ID";
			this.ID.Name = "ID";
			this.ID.ReadOnly = true;
			this.ID.Visible = false;
			// 
			// status_strip
			// 
			this.status_strip.AllowItemReorder = true;
			this.status_strip.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.status_strip.AutoSize = false;
			this.status_strip.BackColor = System.Drawing.Color.Transparent;
			this.status_strip.Dock = System.Windows.Forms.DockStyle.None;
			this.status_strip.GripMargin = new System.Windows.Forms.Padding(0);
			this.status_strip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
			this.status_strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tools_dropdown_button,
            this.progressbar_in_status,
            this.row_counter_label});
			this.status_strip.Location = new System.Drawing.Point(0, 328);
			this.status_strip.Name = "status_strip";
			this.status_strip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
			this.status_strip.Size = new System.Drawing.Size(750, 27);
			this.status_strip.TabIndex = 5;
			// 
			// tools_dropdown_button
			// 
			this.tools_dropdown_button.DropDownButtonWidth = 15;
			this.tools_dropdown_button.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tools_select_single_button,
            this.tools_select_сrossingpoint_button});
			this.tools_dropdown_button.Image = ((System.Drawing.Image) (resources.GetObject("tools_dropdown_button.Image")));
			this.tools_dropdown_button.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tools_dropdown_button.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tools_dropdown_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 0);
			this.tools_dropdown_button.Name = "tools_dropdown_button";
			this.tools_dropdown_button.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
			this.tools_dropdown_button.Size = new System.Drawing.Size(134, 25);
			this.tools_dropdown_button.Text = "Выбор объектов";
			this.tools_dropdown_button.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
			this.tools_dropdown_button.ButtonClick += new System.EventHandler(this.tools_dropdown_ButtonClick);
			this.tools_dropdown_button.MouseHover += new System.EventHandler(this.tools_dropdown_MouseHover);
			// 
			// tools_select_single_button
			// 
			this.tools_select_single_button.BackColor = System.Drawing.Color.Transparent;
			this.tools_select_single_button.Image = ((System.Drawing.Image) (resources.GetObject("tools_select_single_button.Image")));
			this.tools_select_single_button.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tools_select_single_button.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tools_select_single_button.Name = "tools_select_single_button";
			this.tools_select_single_button.Size = new System.Drawing.Size(165, 22);
			this.tools_select_single_button.Text = "Выбор объектов";
			this.tools_select_single_button.Click += new System.EventHandler(this.tools_select_single_Click);
			// 
			// tools_select_сrossingpoint_button
			// 
			this.tools_select_сrossingpoint_button.BackColor = System.Drawing.Color.Transparent;
			this.tools_select_сrossingpoint_button.Image = global::ShowLink.Properties.Resources.select_point_icon;
			this.tools_select_сrossingpoint_button.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tools_select_сrossingpoint_button.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tools_select_сrossingpoint_button.Name = "tools_select_сrossingpoint_button";
			this.tools_select_сrossingpoint_button.Size = new System.Drawing.Size(165, 22);
			this.tools_select_сrossingpoint_button.Text = "Выбор в точке";
			this.tools_select_сrossingpoint_button.Click += new System.EventHandler(this.tools_select_сrossingpoint_Click);
			// 
			// progressbar_in_status
			// 
			this.progressbar_in_status.AutoSize = false;
			this.progressbar_in_status.AutoToolTip = true;
			this.progressbar_in_status.Name = "progressbar_in_status";
			this.progressbar_in_status.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.progressbar_in_status.Size = new System.Drawing.Size(50, 21);
			this.progressbar_in_status.Step = 5;
			this.progressbar_in_status.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressbar_in_status.Tag = "";
			this.progressbar_in_status.Visible = false;
			// 
			// row_counter_label
			// 
			this.row_counter_label.AutoToolTip = true;
			this.row_counter_label.Name = "row_counter_label";
			this.row_counter_label.Size = new System.Drawing.Size(232, 22);
			this.row_counter_label.Text = "текущая: (нет), выделено: 0, в выборке: 0";
			this.row_counter_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lock_data_grid_button
			// 
			this.lock_data_grid_button.BackColor = System.Drawing.Color.Transparent;
			this.lock_data_grid_button.FlatAppearance.BorderColor = System.Drawing.SystemColors.MenuHighlight;
			this.lock_data_grid_button.FlatAppearance.BorderSize = 0;
			this.lock_data_grid_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int) (((byte) (194)))), ((int) (((byte) (224)))), ((int) (((byte) (255)))));
			this.lock_data_grid_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.lock_data_grid_button.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.lock_data_grid_button.Image = global::ShowLink.Properties.Resources.lock_datagrid;
			this.lock_data_grid_button.Location = new System.Drawing.Point(297, 1);
			this.lock_data_grid_button.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
			this.lock_data_grid_button.Name = "lock_data_grid_button";
			this.lock_data_grid_button.Size = new System.Drawing.Size(145, 25);
			this.lock_data_grid_button.TabIndex = 3;
			this.lock_data_grid_button.Text = "&Заморозить список";
			this.lock_data_grid_button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.lock_data_grid_button.UseVisualStyleBackColor = true;
			this.lock_data_grid_button.MouseLeave += new System.EventHandler(this.TriggerButton_MouseLeave);
			this.lock_data_grid_button.Click += new System.EventHandler(this.TriggerButton_Click);
			this.lock_data_grid_button.MouseEnter += new System.EventHandler(this.TriggerButton_MouseEnter);
			// 
			// autozoom_button
			// 
			this.autozoom_button.BackColor = System.Drawing.Color.Transparent;
			this.autozoom_button.FlatAppearance.BorderColor = System.Drawing.SystemColors.MenuHighlight;
			this.autozoom_button.FlatAppearance.BorderSize = 0;
			this.autozoom_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int) (((byte) (194)))), ((int) (((byte) (224)))), ((int) (((byte) (255)))));
			this.autozoom_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.autozoom_button.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.autozoom_button.Image = global::ShowLink.Properties.Resources.autozoom;
			this.autozoom_button.Location = new System.Drawing.Point(446, 1);
			this.autozoom_button.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
			this.autozoom_button.Name = "autozoom_button";
			this.autozoom_button.Size = new System.Drawing.Size(135, 25);
			this.autozoom_button.TabIndex = 4;
			this.autozoom_button.Text = "&Автозумирование";
			this.autozoom_button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.autozoom_button.UseVisualStyleBackColor = true;
			this.autozoom_button.MouseLeave += new System.EventHandler(this.TriggerButton_MouseLeave);
			this.autozoom_button.Click += new System.EventHandler(this.TriggerButton_Click);
			this.autozoom_button.MouseEnter += new System.EventHandler(this.TriggerButton_MouseEnter);
			// 
			// layers_list
			// 
			this.layers_list.BackColor = System.Drawing.SystemColors.Window;
			this.layers_list.DropDownHeight = 150;
			this.layers_list.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.layers_list.DropDownWidth = 300;
			this.layers_list.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.layers_list.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.layers_list.IntegralHeight = false;
			this.layers_list.ItemHeight = 15;
			this.layers_list.Location = new System.Drawing.Point(42, 2);
			this.layers_list.MaxDropDownItems = 10;
			this.layers_list.Name = "layers_list";
			this.layers_list.Size = new System.Drawing.Size(250, 23);
			this.layers_list.TabIndex = 2;
			this.layers_list.MouseHover += new System.EventHandler(this.layers_list_MouseHover);
			this.layers_list.SelectedIndexChanged += new System.EventHandler(this.LayersList_SelectedIndexChanged);
			// 
			// layers_list_label
			// 
			this.layers_list_label.AutoSize = true;
			this.layers_list_label.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.layers_list_label.Location = new System.Drawing.Point(3, 3);
			this.layers_list_label.Name = "layers_list_label";
			this.layers_list_label.Size = new System.Drawing.Size(33, 21);
			this.layers_list_label.TabIndex = 100;
			this.layers_list_label.Text = "Слой";
			this.layers_list_label.UseCompatibleTextRendering = true;
			// 
			// PaletteControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.layers_list_label);
			this.Controls.Add(this.layers_list);
			this.Controls.Add(this.lock_data_grid_button);
			this.Controls.Add(this.autozoom_button);
			this.Controls.Add(this.status_strip);
			this.Controls.Add(this.data_grid);
			this.MinimumSize = new System.Drawing.Size(300, 0);
			this.Name = "PaletteControl";
			this.Size = new System.Drawing.Size(750, 355);
			((System.ComponentModel.ISupportInitialize) (this.data_grid)).EndInit();
			this.status_strip.ResumeLayout(false);
			this.status_strip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		internal System.Windows.Forms.DataGridView data_grid;
		private System.Windows.Forms.ToolStripSplitButton tools_dropdown_button;
		private System.Windows.Forms.ToolStripMenuItem tools_select_single_button;
		private System.Windows.Forms.ToolStripMenuItem tools_select_сrossingpoint_button;
		internal System.Windows.Forms.Button lock_data_grid_button;
		internal System.Windows.Forms.Button autozoom_button;
		internal System.Windows.Forms.ToolStripProgressBar progressbar_in_status;
		internal System.Windows.Forms.ToolStripStatusLabel row_counter_label;
		internal System.Windows.Forms.StatusStrip status_strip;
		internal System.Windows.Forms.ComboBox layers_list;
		internal System.Windows.Forms.DataGridViewLinkColumn URL;
		internal System.Windows.Forms.DataGridViewButtonColumn ID;
		private System.Windows.Forms.Label layers_list_label;
	}
}
