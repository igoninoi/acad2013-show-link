using System;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Windows.Forms;


//Geospatial Platform API
using Autodesk.Gis.Map.Platform;
using Autodesk.Gis.Map.Platform.Interop;

//
using OSGeo.MapGuide;

//AutoCAD API
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;


// использование арибутов ExtensionApplication и CommandClass для одного и того же класса приводит к повторному созданию
// экземпляра класса при вызове команды Autocad. Необходимо разнести класс команд и класс 
[assembly: ExtensionApplication(typeof(ShowLink.Core))]


namespace ShowLink
{
	/// <summary>
	/// Перечисление типов команд выбора объектов
	/// </summary>
	enum SelectCommandType : byte
	{
		Single=0,	// 
		CrossingPoint=1 //
	}
	
	/// <summary>
	/// Класс для информации о ссылках для конкретного FeatureClass
	/// </summary>
	internal class FeatureClassLinkInfo
	{
		public string FeatureClassName;
		public string LinkCommandPath;
		public string LinkTitle;
		public string LinkURL;

		public FeatureClassLinkInfo(
				string _FeatureClassName,
				string _LinkCommandPath,
				string _LinkTitle,
				string _LinkURL)
		{
			FeatureClassName = _FeatureClassName;
			LinkCommandPath = _LinkCommandPath;
			LinkTitle = _LinkTitle;
			LinkURL = _LinkURL;

		}
	}

	/// <summary>
	/// Класс для информации о ссылках (выражение для ссылки, подписи и т.д.)
	/// </summary>
	internal class Configuration
	{
		public FeatureClassLinkInfo[] link_info_array = new FeatureClassLinkInfo[0];

		public int IndexOf(string _FeatureClassName)
		{
			FeatureClassLinkInfo record;
			if (link_info_array == null)
			{
				return -1;
			}
			for (int i = 0; i < link_info_array.Length; i++)
			{
				record = link_info_array[i];
				if (record.FeatureClassName == _FeatureClassName)
				{
					return i;
				}
			}
			return -1;
		}

		public void ClearConfig()
		{
			Array.Clear(link_info_array, 0, link_info_array.Length);
			link_info_array = new FeatureClassLinkInfo[0]; 
			//link_info_array = null;
		}

		public void AddFeatureClass(FeatureClassLinkInfo _add_value)
		{
			FeatureClassLinkInfo[] tmp = new FeatureClassLinkInfo[link_info_array.Length + 1];
			link_info_array.CopyTo(tmp, 0);
			tmp[link_info_array.Length] = _add_value;
			link_info_array = tmp;
		}

		public void ReadConfigFromFile(string _file_path)
		{
			try
			{
				FileInfo info = new FileInfo(_file_path);
				if (!info.Exists)
				{
					Core.WriteMessage("Файл конфигурации " + _file_path + " не найден.\n");
					return;
				}

				XmlDocument config_doc = new XmlDocument();
				config_doc.Load(_file_path);

				XmlNodeList param_nodes = config_doc.DocumentElement.SelectNodes("Parameter"); // выборка всех узлов "Parameter"

				foreach (XmlNode node in param_nodes) // цикл по всем узлам "Parameter"
				{
					XmlNode FeatureClassName_node =	node.SelectSingleNode("FeatureClassName");
					XmlNode LinkCommandPath_node =	node.SelectSingleNode("LinkCommandPath");
					XmlNode LinkTitle_node =		node.SelectSingleNode("LinkTitle");
					XmlNode LinkURL_node =			node.SelectSingleNode("LinkURL");

					if ((FeatureClassName_node != null) & (LinkTitle_node != null) & (LinkURL_node != null))
					{
						string LinkCommandPath_val = (LinkCommandPath_node == null) ? "" : LinkCommandPath_node.InnerText;

						this.AddFeatureClass(
							new FeatureClassLinkInfo(
								FeatureClassName_node.InnerText,
								LinkCommandPath_val,
								LinkTitle_node.InnerText,
								LinkURL_node.InnerText
							)
						);
					}
				}

			}
			catch (System.Exception)
			{
				Core.WriteMessage("Файл конфигурации " + _file_path + " вызвал ошибку обработки.\n");
				return;
			}

			Core.WriteMessage("Получено описаний: " + this.link_info_array.Length.ToString() + "\n");

		}//method

		public Configuration()
		{
		}

		public Configuration(string _file_path)
		{
			this.ReadConfigFromFile(_file_path);
		}

	} // class

	internal class SelectedFeatureInfo
	{
		public MgLayerBase layer;
		public MgPropertyCollection properties;
		
		public SelectedFeatureInfo(MgLayerBase _layer, MgPropertyCollection _properties)
		{
			layer = _layer;
			properties = _properties;
		}
	}

	public class Core : IExtensionApplication
    {
		internal static string assembly_name = null;
		internal static string assembly_path = null;
		
		internal static string command_splitter = "-->";

		internal static bool no_handle_selection = false;

		internal static Configuration config; //= new ShowLinkConfig();

		public static PaletteWindow palette_window;

		#region --- Auxilliary members ---

		/// <summary>Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager</summary>
		internal static DocumentCollection dm = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager;


		/// <summary>Возвращает имя сборки</summary>
		internal static string current_assembly_name
		{
			get
			{
				if (string.IsNullOrEmpty(assembly_name))
				{
					Assembly asm = Assembly.GetExecutingAssembly();
					assembly_name = asm.GetName().Name;
				}
				return assembly_name;
			}
		}
	
		/// <summary>Возвращает путь до файла текущей сборки</summary>
		internal static string current_assembly_path
		{
			get
			{
				if (string.IsNullOrEmpty(assembly_path))
				{
					Assembly asm = Assembly.GetExecutingAssembly();
					assembly_path = asm.Location;
				}
				return assembly_path;
			}
		}

		/// <summary>Возвращает Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument</summary>
		internal static Document current_document
		{
			get
			{
				return dm.MdiActiveDocument;
			}
		}
		
		/// <summary>Возвращает Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor или null</summary>
		internal static Editor current_editor
		{
			get
			{	
			 	return (dm.MdiActiveDocument == null) ? null : dm.MdiActiveDocument.Editor;
			}
		}

		/// <summary>Вывод сообщения в командной строке</summary>
		internal static void WriteMessage(string _message)
		{
			Document curr_doc = dm.MdiActiveDocument;
			if (curr_doc == null)
			{
				MessageBox.Show(_message);
			}
			else
			{
				curr_doc.Editor.WriteMessage(_message);
			}
		}

		/// <summary>Вывод сообщения в командной строке (переменное число параметров)</summary>
		internal static void WriteMessage(string _message, params object[] _parameter)
		{
			Document curr_doc = dm.MdiActiveDocument;
			if (curr_doc == null)
			{
				MessageBox.Show(_message);
			}
			else
			{
				curr_doc.Editor.WriteMessage(_message,_parameter);
			}
		}

		/// <summary>Выполнение команты через командную строку</summary>
		internal static void SendStringToExecute(string _command, bool _activate, bool _wrapUpInactiveDoc, bool _echoCommand) 
		{ 
			Document curr_doc = dm.MdiActiveDocument;
			if (curr_doc == null)
			{
				MessageBox.Show("Нет открытых документов. Невозможно выполнить команду.");
			}
			else
			{
				curr_doc.SendStringToExecute(_command, _activate, _wrapUpInactiveDoc, _echoCommand);
			}
		}


		#endregion



		#region --- Selection Processing ---

		/// <summary>Очистка выборки</summary>
		internal static void ClearImpliedSelection()
		{
			MgSelectionBase sel_base = new MgSelectionBase(AcMapMap.GetCurrentMap());
			SelectionSet new_sel_set = AcMapFeatureEntityService.AddFeaturesToSelectionSet(null, sel_base);
			Core.current_editor.SetImpliedSelection(new_sel_set);
			AcMapMap.ForceScreenRefresh();
		}


		/// <summary>Обработка выборки в документе _document</summary>
		/// <param name="_document"></param>
		internal static void HandleSelectedFDO(Document _document) 
		{
			PromptSelectionResult sel_result;
			
			// для обработки случая, когда Editor.SelectImplied() вызывает сбой во время операции _PASTECLIP
			// AutoCAD меняет акт.документ на тот откуда берется копия.
			try
			{
				sel_result = _document.Editor.SelectImplied();
			}
			catch (System.Exception)
			{
				return;
			}


			if (sel_result.Status != PromptStatus.OK)
			{
				Core.ClearDataGridView();
				return;
			}

			MgSelectionBase selection_base = AcMapFeatureEntityService.GetSelection(sel_result.Value);
			if (selection_base.GetLayers() == null)
			{
				Core.ClearDataGridView();
			}
			else
			{
				Core.SelectionToDataGridView(selection_base);
			}
		}

		/// <summary>Обработка выборки в текущем документе</summary>
		internal static void HandleSelectedFDO()
		{
			HandleSelectedFDO(current_document);
		}

		#endregion


		#region --- Event Handlers ---

		/// <summary>
		/// обработчик изменения выборки
		/// </summary>
		private static void OnImpliedSelectionChanged(object sender, EventArgs e)
		{
			if ((palette_window.links_control.lock_data_grid_trigger) 
			|| (!palette_window.palette_set.Visible))
			{
				return;
			}

			if (no_handle_selection) 
			{
				no_handle_selection = false;
				return;
			}

			Document curr_doc =  sender as Document;

			HandleSelectedFDO(curr_doc);
		}

		/// <summary>обработчик создания документа</summary>
		private static void OnDocumentCreated(object sender, DocumentCollectionEventArgs e)
		{
			e.Document.ImpliedSelectionChanged += new EventHandler(OnImpliedSelectionChanged);
		}

		
		internal static Document current_doc_var = null;
		
		/// <summary>обработчик смены активного документа</summary>
		private static void OnDocumentBecameCurrent(object sender, DocumentCollectionEventArgs e)
		{
			if (current_doc_var == e.Document)  /// обрабатывается событие только для вновь текущего документа
											/// нужно попробовать заменить на проверку 	
											/// dm.MdiActiveDocument == e.Document

			{
				return;
			}
			else
			{
				current_doc_var = e.Document;
			}


			if ((palette_window.links_control.lock_data_grid_trigger) 
			|| (!palette_window.palette_set.Visible))
			{
			    return;
			}

			if (no_handle_selection)
			{
				no_handle_selection = false;
				return;
			}

			HandleSelectedFDO(current_doc_var);
		}

		/// <summary>Добавление обработчиков ImpliedSelectionChanged для всех документов</summary>
		internal static void RegisterHandlers()
		{
			dm.DocumentCreated += new DocumentCollectionEventHandler(OnDocumentCreated);
			dm.DocumentBecameCurrent += new DocumentCollectionEventHandler(OnDocumentBecameCurrent);
			foreach (Document curr_doc in dm)
			{
				curr_doc.ImpliedSelectionChanged += new EventHandler(OnImpliedSelectionChanged);
			}
		}

		/// <summary>Удаление обработчиков ImpliedSelectionChanged для всех документов</summary>
		internal static void UnregisterHandlers()
		{
			dm.DocumentCreated -= new DocumentCollectionEventHandler(OnDocumentCreated);
			dm.DocumentBecameCurrent -= new DocumentCollectionEventHandler(OnDocumentBecameCurrent);

			foreach (Document curr_doc in dm)
			{
				curr_doc.ImpliedSelectionChanged -= new EventHandler(OnImpliedSelectionChanged);
			}
		}

		#endregion



		/// <summary>Подстановка значений полей из текущей записи _reader в строке _in_str по шаблону "[имя поля]"</summary>
		/// <param name="_reader"></param>
		/// <param name="_in_str"></param>
		/// <returns></returns>
		internal static string ResolveLinkStringByValueFromReader(MgFeatureReader _reader, string _in_str)
		{
			if (String.IsNullOrEmpty(_in_str) )
			{
				return null;
			}

			string replace_str;
			MgPropertyDefinitionCollection prop_defs = _reader.GetClassDefinition().GetProperties();
			foreach (MgPropertyDefinition curr_prop in prop_defs) 
			{
				replace_str = "[" + curr_prop.Name + "]";
				if (_in_str.Contains(replace_str))
				{
					_in_str = _in_str.Replace(replace_str, PropertyValueToString(_reader, curr_prop.Name));
				}
			}

			return _in_str;
		}

		/// <summary>Получает набор свойств MgPropertyCollection из текущей записи _reader в соответствии со списком _listed_prop_defs</summary>
		/// <param name="_reader"></param>
		/// <param name="_listed_prop_defs"></param>
		/// <returns></returns>
		internal static MgPropertyCollection GetPropertiesFromReader(MgFeatureReader _reader, MgPropertyDefinitionCollection _listed_prop_defs)
		{
			MgPropertyCollection result_props = new MgPropertyCollection();

			MgPropertyDefinitionCollection reader_prop_defs = _reader.GetClassDefinition().GetProperties();
			
			foreach (MgPropertyDefinition curr_prop_def in _listed_prop_defs)
			{
				if (!reader_prop_defs.Contains(curr_prop_def.Name) || _reader.IsNull(curr_prop_def.Name) )
				{
					continue;	
				}

				int property_type = _reader.GetPropertyType(curr_prop_def.Name); 
				if (property_type == MgPropertyType.Blob)
				{
					result_props.Add(
						new MgBlobProperty(curr_prop_def.Name, _reader.GetBLOB(curr_prop_def.Name))
					);
				}
				else if (property_type == MgPropertyType.Boolean)
				{
					result_props.Add(
						new MgBooleanProperty(curr_prop_def.Name,_reader.GetBoolean(curr_prop_def.Name))
					);
				}
				else if (property_type == MgPropertyType.Byte)
				{
					result_props.Add(
						new MgByteProperty(curr_prop_def.Name, _reader.GetByte(curr_prop_def.Name))
					);
				}
				else if (property_type == MgPropertyType.Clob)
				{
					result_props.Add(
						new MgClobProperty(curr_prop_def.Name, _reader.GetCLOB(curr_prop_def.Name))
					);
				}
				else if (property_type == MgPropertyType.DateTime)
				{
					result_props.Add(
						new MgDateTimeProperty(curr_prop_def.Name, _reader.GetDateTime(curr_prop_def.Name))
					);
				}
				else if (property_type == MgPropertyType.Double)
				{
					result_props.Add(
						new MgDoubleProperty(curr_prop_def.Name, _reader.GetDouble(curr_prop_def.Name))
					);
				}
				else if (property_type == MgPropertyType.Feature)
				{
					result_props.Add(
						new MgFeatureProperty(curr_prop_def.Name, _reader.GetFeatureObject(curr_prop_def.Name))
					);
				}
				else if (property_type == MgPropertyType.Geometry)
				{
					result_props.Add(
						new MgGeometryProperty(curr_prop_def.Name, _reader.GetGeometry(curr_prop_def.Name))
					);
				}
				else if (property_type == MgPropertyType.Int16)
				{
					result_props.Add(
						new MgInt16Property(curr_prop_def.Name, _reader.GetInt16(curr_prop_def.Name))
					);
				}
				else if (property_type == MgPropertyType.Int32)
				{
					result_props.Add(
						new MgInt32Property(curr_prop_def.Name, _reader.GetInt32(curr_prop_def.Name))
					);
				}
				else if (property_type == MgPropertyType.Int64)
				{
					result_props.Add(
						new MgInt64Property(curr_prop_def.Name, _reader.GetInt64(curr_prop_def.Name))
					);
				}
				else if (property_type == MgPropertyType.Raster)
				{
					result_props.Add(
						new MgRasterProperty(curr_prop_def.Name, _reader.GetRaster(curr_prop_def.Name))
					);
				}
				else if (property_type == MgPropertyType.Single)
				{
					result_props.Add(
						new MgSingleProperty(curr_prop_def.Name, _reader.GetSingle(curr_prop_def.Name))
					);
				}
				else if (property_type == MgPropertyType.String)
				{
					result_props.Add(
						new MgStringProperty(curr_prop_def.Name, _reader.GetString(curr_prop_def.Name))
					);
				}

			}  //foreach

			return result_props;
		}
		
		/// <summary>Получает строковое представление поля _property_name из текущей записи _reader</summary>
		/// <param name="_reader"></param>
		/// <param name="_property_name"></param>
		/// <returns></returns>
		internal static string PropertyValueToString(MgFeatureReader _reader, string _property_name)
		{
			MgPropertyDefinitionCollection prop_defs = _reader.GetClassDefinition().GetProperties();
			if (String.IsNullOrEmpty(_property_name) || !prop_defs.Contains(_property_name))
			{
				return null;
			}
			
			int property_type = _reader.GetPropertyType(_property_name);
			if (_reader.IsNull(_property_name))
			{
				return null;
			}
			else if (property_type == MgPropertyType.Boolean)
			{
				return _reader.GetBoolean(_property_name).ToString();
			}
			else if (property_type == MgPropertyType.Byte)
			{
				return _reader.GetByte(_property_name).ToString();
			}
			else if (property_type == MgPropertyType.DateTime)
			{
				return _reader.GetDateTime(_property_name).ToString();
			}
			else if (property_type == MgPropertyType.Double)
			{
				return _reader.GetDouble(_property_name).ToString();
			}
			else if (property_type == MgPropertyType.Int16)
			{
				return _reader.GetInt16(_property_name).ToString();
			}
			else if (property_type == MgPropertyType.Int32)
			{
				return _reader.GetInt32(_property_name).ToString();
			}
			else if (property_type == MgPropertyType.Int64)
			{
				return _reader.GetInt64(_property_name).ToString();
			}
			else if (property_type == MgPropertyType.Single)
			{
				return _reader.GetSingle(_property_name).ToString();
			}
			else if (property_type == MgPropertyType.String)
			{
				return _reader.GetString(_property_name);
			}
			else
			{
				return null;
			}
		}//method





		internal static void ExecuteSelectCommand(SelectCommandType _cmd_type)
		{
			switch (_cmd_type)
			{
				case SelectCommandType.Single:
					Core.SendStringToExecute("ShowLinkSingleSelect\n", true, false, true);
					break;
				case SelectCommandType.CrossingPoint:
					Core.SendStringToExecute("ShowLinkCrossingPointSelect\n", true, false, true);
					break;
			}

		}//method

	
		internal static void SelectionToDataGridView(MgSelectionBase _selection_base)
		{

			palette_window.links_control.is_suspended = true; // информируем о заморозке
			palette_window.links_control.SuspendLayout(); // замораживаем логику
			palette_window.links_control.data_grid.Enabled = false;
			palette_window.links_control.data_grid.SuspendLayout(); // замораживаем логику DataGridView
			palette_window.links_control.layers_list.Enabled =false;

			palette_window.links_control.progressbar_in_status.Visible = true; // включение ProgressBar
			palette_window.links_control.status_strip.Refresh();




			MgReadOnlyLayerCollection layers = _selection_base.GetLayers();

			int curr_features_count = 0, features_count = 0; 
			
			// подсчет количества выбранных объектов
			foreach (MgLayerBase layer in layers)
			{
				features_count += _selection_base.GetSelectedFeaturesCount(layer, layer.FeatureClassName);
			}
			
			if (features_count == 0) 
			{
				return;
			}

			// заполнение списка слоев
			palette_window.links_control.layers_list.Items.Clear(); // очищаем список
			
			if (layers.Count > 1)
			{
				palette_window.links_control.layers_list.Items.Add("(Все выбранные)");
			}
			
			foreach (MgLayerBase layer in layers)
			{
				palette_window.links_control.layers_list.Items.Add(layer.Name);
			}
			palette_window.links_control.layers_list.SelectedIndex = 0;
			
			// заполнение DataGridView
			palette_window.links_control.data_grid.Rows.Clear();  // очищаем строки

			foreach (MgLayerBase layer in layers)
			{

				int curr_ftr_class_idx = config.IndexOf(layer.FeatureClassName);
				if (curr_ftr_class_idx == -1)
				{
					curr_features_count += _selection_base.GetSelectedFeaturesCount(layer, layer.FeatureClassName); ;
					palette_window.links_control.UpdateStatusRowCounterLabelAndProgressBar(curr_features_count, features_count);
					continue;
				}
				FeatureClassLinkInfo curr_link_info = config.link_info_array[curr_ftr_class_idx];

				MgFeatureReader ftr_rdr = _selection_base.GetSelectedFeatures(layer, layer.FeatureClassName, false);

				MgPropertyDefinitionCollection identity_prop_defs = ftr_rdr.GetClassDefinition().GetIdentityProperties(); // получаем набор идентификационных полей

				while (ftr_rdr.ReadNext())
				{
					curr_features_count++;

					DataGridViewRow link_row = new DataGridViewRow(); // новая строка

					DataGridViewLinkCell link_cell; // ячейка

					// формирование URL
					link_cell = new DataGridViewLinkCell();

					string title_str = ResolveLinkStringByValueFromReader(ftr_rdr, curr_link_info.LinkTitle);
					link_cell.Value = String.IsNullOrEmpty(title_str)? "<Null>": title_str;
					link_cell.ToolTipText =	
						curr_link_info.LinkCommandPath
						+ command_splitter
						+ ResolveLinkStringByValueFromReader(ftr_rdr, curr_link_info.LinkURL);
					link_row.Cells.Add(link_cell); // вставка ячейки
					
					// формирование ID
					MgPropertyCollection curr_identity_props = GetPropertiesFromReader(ftr_rdr, identity_prop_defs);

					SelectedFeatureInfo sel_info = new SelectedFeatureInfo(layer, curr_identity_props);
					link_cell = new DataGridViewLinkCell();
					link_cell.Value = sel_info;
					link_row.Cells.Add(link_cell); // вставка ячейки

					// добавлени строки в DataGridView
					palette_window.links_control.data_grid.Rows.Add(link_row);

					// обновление ProgressBar'а и счетчика строк
					palette_window.links_control.UpdateStatusRowCounterLabelAndProgressBar(curr_features_count, features_count);


				} /// while (ftr_rdr.ReadNext())
			} /// foreach (MgLayerBase layer in layers)

			palette_window.links_control.URL.SortMode = DataGridViewColumnSortMode.NotSortable; // сброс сортировки
			palette_window.links_control.URL.SortMode = DataGridViewColumnSortMode.Automatic;   // на исходную автоматическую

			palette_window.links_control.data_grid.SelectAll(); // выбираем все строки

			palette_window.links_control.progressbar_in_status.Visible = false;

			palette_window.links_control.layers_list.Enabled =true;
			palette_window.links_control.data_grid.Enabled = true;
			palette_window.links_control.data_grid.ResumeLayout(); // размораживаем логику DataGridView
			palette_window.links_control.ResumeLayout(); // размораживаем логику
			palette_window.links_control.is_suspended = false;

			palette_window.links_control.UpdateStatusRowCounterLabel(); // обновляем строку статуса

		}


		internal static void ClearDataGridView()
		{
			palette_window.links_control.is_suspended = true; // информируем о заморозке
			palette_window.links_control.SuspendLayout(); // замораживаем логику
			palette_window.links_control.data_grid.Enabled = false;
			palette_window.links_control.data_grid.SuspendLayout(); // замораживаем логику DataGridView
			palette_window.links_control.layers_list.Enabled =false;
			{
				// сброс списка слоев
				
				palette_window.links_control.layers_list.Items.Clear();
				palette_window.links_control.layers_list.Items.Add("(Выборка пуста)");
				palette_window.links_control.layers_list.SelectedIndex = 0;
				

				// сброс DataGridView 
				palette_window.links_control.data_grid.Rows.Clear();  // очищаем строки
				palette_window.links_control.URL.SortMode = DataGridViewColumnSortMode.NotSortable; // сброс сортировки
				palette_window.links_control.URL.SortMode = DataGridViewColumnSortMode.Automatic;   // на исходную автоматическую
			}
			//palette_window.links_control.layers_list.Enabled =true;
			//palette_window.links_control.data_grid.Enabled = true;
			palette_window.links_control.data_grid.ResumeLayout(); // размораживаем логику DataGridView
			palette_window.links_control.ResumeLayout(); // размораживаем логику
			palette_window.links_control.is_suspended = false;

			palette_window.links_control.UpdateStatusRowCounterLabel(); // обновляем строку статуса

		}

		
		//public ShowLinkClass() // конструктор по умолчанию
		//{
		//}


		public void Initialize()
		{
			Core.WriteMessage("\nЗапуск приложения " + current_assembly_name + " ...\n");

			FileInfo file_info = new FileInfo(Core.current_assembly_path);
			config = new Configuration(file_info.DirectoryName + "\\" + file_info.Name + ".xml");

			palette_window = new PaletteWindow("ShowLink");

			Core.RegisterHandlers();

			Core.WriteMessage("...приложение " + current_assembly_name + " запущено\n");
		}

		public void Terminate()
		{
			//Core.WriteMessage("--- Terminate() ---");
		
		}


    }//class
}
