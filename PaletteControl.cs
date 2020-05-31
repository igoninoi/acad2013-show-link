using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace ShowLink
{
	public partial class PaletteControl : UserControl
	{
		internal ShowLink.SelectCommandType last_command = SelectCommandType.Single;
		internal bool is_suspended = false;

		internal bool lock_data_grid_trigger = false;
		internal bool autozoom_trigger = false;

		internal void UpdateStatusRowCounterLabel() 
		{
			if (this.data_grid.CurrentCell == null)
			{
				this.row_counter_label.Text = "текущая: (нет), выделено: 0, в выборке: 0";
			}
			else
			{
				this.row_counter_label.Text = 
					"текущая: " + (this.data_grid.CurrentCellAddress.Y+1).ToString()
					+", выделено: " + this.data_grid.SelectedRows.Count
					+ ", в выборке: " + this.data_grid.Rows.Count.ToString();
			}
		}

		public PaletteControl()
		{
			InitializeComponent();
		}

		private void data_grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				return;
			}
			string link = this.data_grid.Rows[e.RowIndex].Cells[0].ToolTipText;
			int split_pos = link.IndexOf(ShowLink.Core.command_splitter);
			string command = link.Substring(0, split_pos);
			string arguments = link.Substring(split_pos + ShowLink.Core.command_splitter.Length);

			string saved_dir_path = System.Environment.CurrentDirectory;

			System.Environment.CurrentDirectory = (new FileInfo(Core.current_assembly_path)).DirectoryName;

			try
			{
				if (String.IsNullOrEmpty(command))
				{
					Process.Start(arguments);
				}
				else
				{
					Process.Start(command, arguments);
				}
			}
			catch (System.Exception err)
			{
				MessageBox.Show(err.Message);
			}
			finally 
			{
				System.Environment.CurrentDirectory = saved_dir_path;
			}

		}

		private void data_grid_CurrentCellChanged(object sender, EventArgs e)
		{
			if (this.is_suspended)
			{
				return;
			}
			
			this.UpdateStatusRowCounterLabel();
		}
		
		/// <summary>Обработка текущего выделения в DataGridView</summary>
		private void DataGridViewToSelection()
		{
			if (Core.current_document == null)
			{
				return;
			}

			if (this.is_suspended)
			{
				return;
			}

			this.is_suspended = true; // указываем, что идет обработка
			{

				if (this.data_grid.SelectedRows.Count == 0) // если нет выделенных строк
				{
					Core.no_handle_selection = true; // чтобы не очищалось DataGridView
					Core.ClearImpliedSelection();
				}
				else // SelectedRows.Count > 0
				{
					Autodesk.Gis.Map.Platform.AcMapMap curr_map = Autodesk.Gis.Map.Platform.AcMapMap.GetCurrentMap();
					OSGeo.MapGuide.MgLayerCollection layers = curr_map.GetLayers();
					OSGeo.MapGuide.MgSelectionBase sel_base = new OSGeo.MapGuide.MgSelectionBase(curr_map); // новая пустая выборка FDO

					// создание выборки (MgSelectionBase)
					foreach (DataGridViewRow curr_row in this.data_grid.SelectedRows)
					{
						SelectedFeatureInfo sel_info = (curr_row.Cells[1].Value) as SelectedFeatureInfo;

						if ((sel_info != null) && layers.Contains(sel_info.layer))
						{
							sel_base.AddFeatureIds(sel_info.layer, sel_info.layer.FeatureClassName, sel_info.properties);
						}
					}

					// отражение выборки на карте
					if (!autozoom_trigger)
					{
						CommandClass.SetImpliedSelectionBySelectionBase(sel_base);
					}
					else
					{
						CommandClass.ThroughAssZoomAndSelection(sel_base);
					}
				}
			}
			this.is_suspended = false;
		}


		private void data_grid_SelectionChanged(object sender, EventArgs e)
		{
			if (this.is_suspended)
			{
				return;
			}

			this.UpdateStatusRowCounterLabel(); 
			
			DataGridViewToSelection();
		}


		private void tools_select_single_Click(object sender, EventArgs e)
		{
			this.tools_dropdown_button.Text = this.tools_select_single_button.Text;
			this.tools_dropdown_button.Image = ShowLink.Properties.Resources.select_simple_icon;

			this.last_command = SelectCommandType.Single;

			Core.ExecuteSelectCommand(this.last_command);
		}

		private void tools_select_сrossingpoint_Click(object sender, EventArgs e)
		{
			this.tools_dropdown_button.Text = this.tools_select_сrossingpoint_button.Text;
			this.tools_dropdown_button.Image = ShowLink.Properties.Resources.select_point_icon;

			this.last_command = SelectCommandType.CrossingPoint;

			Core.ExecuteSelectCommand(this.last_command);
		}

		private void tools_dropdown_ButtonClick(object sender, EventArgs e)
		{
			Core.ExecuteSelectCommand(this.last_command);
		}

		private void tools_dropdown_MouseHover(object sender, EventArgs e)
		{
			this.Focus();
		}

		private void TriggerButton_Click(object sender, EventArgs e)
		{
			Button btn = sender as Button;
			bool trigger_value = false;


			if (btn.Equals(lock_data_grid_button))
			{
				lock_data_grid_trigger = trigger_value = !lock_data_grid_trigger;
			}
			else if (btn.Equals(autozoom_button))
			{
				autozoom_trigger = trigger_value = !autozoom_trigger;
			}


			if (trigger_value)
			{
				btn.FlatAppearance.BorderSize = 1;
				btn.BackColor = Color.FromArgb(((int) (((byte) (194)))), ((int) (((byte) (224)))), ((int) (((byte) (255)))));
			}
			else
			{
				btn.FlatAppearance.BorderSize = 0;
				btn.BackColor = Color.Transparent;
			}

			if (btn.Equals(lock_data_grid_button) && (!lock_data_grid_trigger)) // обработка отключения заморозки списка
			{
				Core.HandleSelectedFDO();
			}

			if (btn.Equals(autozoom_button) && (autozoom_trigger)) // обработка включения автозумирования
			{
				DataGridViewToSelection();
			}
		}


		private void TriggerButton_MouseEnter(object sender, EventArgs e)
		{
			Button btn = sender as Button;
			btn.FlatAppearance.BorderSize = 1;

		}
		private void TriggerButton_MouseLeave(object sender, EventArgs e)
		{
			Button btn = sender as Button;
			if ((btn.Equals(autozoom_button) && autozoom_trigger)
			|| (btn.Equals(lock_data_grid_button) && lock_data_grid_trigger)) 
			{
				btn.FlatAppearance.BorderSize = 1;
			}
			else
			{
				btn.FlatAppearance.BorderSize = 0;
			}
		}


		/// <summary>
		/// обновление ProgressBar'а и счетчика строк
		/// </summary>
		/// <param name="curr_features_count">сколько объектов</param>
		/// <param name="features_count">из скольки</param>
		internal void UpdateStatusRowCounterLabelAndProgressBar(int curr_features_count, int features_count)
		{
			this.row_counter_label.Text = "обработано: " +((int) (100*curr_features_count / features_count)).ToString() + "%";
			this.progressbar_in_status.Value = ((int) (100*curr_features_count / features_count));
			this.status_strip.Refresh();
		}

		private void LayersList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.is_suspended)
			{
				return;
			}
	
			ComboBox layers_list = sender as ComboBox;

			this.is_suspended = true;
			this.data_grid.Enabled = false;
			this.data_grid.SuspendLayout(); // замораживаем логику DataGridView
			{
				if (layers_list.SelectedIndex == 0)
				{
					foreach (DataGridViewRow curr_row in this.data_grid.Rows)
					{
						curr_row.Visible = true;
					}
				}
				else
				{
					string layer_name = layers_list.SelectedItem as string;
					foreach (DataGridViewRow curr_row in this.data_grid.Rows)
					{
						SelectedFeatureInfo sel_info = (curr_row.Cells[1].Value) as SelectedFeatureInfo;
						if (sel_info == null)
						{
							continue;
						}
						curr_row.Visible = (sel_info.layer.Name == layer_name);
					}
				}
			}
			this.data_grid.Enabled = true;
			this.data_grid.ResumeLayout(); // размораживаем логику DataGridView
			this.is_suspended = false;

		}

		private void layers_list_MouseHover(object sender, EventArgs e)
		{
			this.layers_list.Focus();
		}





		
		
	}
}
