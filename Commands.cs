using System;

//Geospatial Platform API
using Autodesk.Gis.Map.Platform;
using Autodesk.Gis.Map.Platform.Interop;

//
using OSGeo.MapGuide;

//AutoCAD API
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;

[assembly: CommandClass(typeof(ShowLink.CommandClass))]


namespace ShowLink
{


	public class CommandClass
	{
		public static DocumentCollection dm = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager; // ??? = ... .ApplicationServices.Core.Application.DocumentManager;


		#region Read Configuration

		/// <summary>
		/// Чтение настроек из файла XML, команда без открытия диалога
		/// </summary>
		[CommandMethod("-ShowLinkReadConfig", CommandFlags.Redraw |CommandFlags.NoBlockEditor | CommandFlags.NoUndoMarker)]
		public void SilentReadConfigCommand()
		{
			//object saved_FILEDIA_var = Autodesk.AutoCAD.ApplicationServices.Application.GetSystemVariable("FILEDIA");
			//Autodesk.AutoCAD.ApplicationServices.Application.SetSystemVariable("FILEDIA", 0);

			//PromptOpenFileOptions file_opt = new PromptOpenFileOptions("Укажите путь к файлу настроек ссылок");
			//file_opt.Filter = "Файлы настроек ссылок (*.xml)|*.xml|Все файлы (*.*)|*.*";
			//file_opt.FilterIndex = 1;

			//PromptFileNameResult file_res =  Core.current_editor.GetFileNameForOpen(file_opt);

			PromptStringOptions file_opt = new PromptStringOptions("Укажите путь к файлу настроек ссылок");
			file_opt.AllowSpaces = true;

			PromptResult file_res =  Core.current_editor.GetString(file_opt);


			//Autodesk.AutoCAD.ApplicationServices.Application.SetSystemVariable("FILEDIA", saved_FILEDIA_var);

			if (file_res.Status  == PromptStatus.OK)
			{
				if (String.IsNullOrEmpty(file_res.StringResult.Trim()))
				{
					Core.WriteMessage("Имя файла не может быть пустым\n");
				}
				else
				{
					Core.config = new Configuration(file_res.StringResult);
				}
			}
			else
			{
				Core.WriteMessage("Пользователь отказался\n");
				return;
			}
			Core.WriteMessage("\n");

		}

		/// <summary>
		/// Чтение настроек из файла XML
		/// </summary>
		[CommandMethod("ShowLinkReadConfig", CommandFlags.Redraw |CommandFlags.NoBlockEditor | CommandFlags.NoUndoMarker)]
		public void ReadConfigCommand()
		{

			PromptOpenFileOptions file_opt = new PromptOpenFileOptions("Укажите путь к файлу настроек ссылок");
			file_opt.Filter = "Файлы настроек ссылок (*.xml)|*.xml|Все файлы (*.*)|*.*";
			//file_opt.FilterIndex = 1;
			PromptFileNameResult file_res =  Core.current_editor.GetFileNameForOpen(file_opt);
			if (file_res.Status  == PromptStatus.OK)
			{
				Core.config = new Configuration(file_res.StringResult);

			}
			else
			{
				Core.WriteMessage("Пользователь отказался\n");
				return;
			}
			Core.WriteMessage("\n");

		}

		#endregion



		#region Through Ass zoom to selection

		/// <summary>
		/// спец. переменная для установки активной выборки после зумирования
		/// </summary>
		internal static SelectionSet internal_saved_selection;

		/// <summary>
		/// Установка активной выборки без зумирования. 
		/// </summary>
		/// <param name="sel_base"></param>
		internal static void SetImpliedSelectionBySelectionBase(MgSelectionBase sel_base)
		{
			Core.no_handle_selection = true; // чтобы изменение выборки не обрабатывалось

			internal_saved_selection = AcMapFeatureEntityService.AddFeaturesToSelectionSet(null, sel_base);	// преобразование в SelectionSet

			Core.current_editor.SetImpliedSelection(internal_saved_selection);
			AcMapMap.ForceScreenRefresh();
		}


		/// <summary>
		/// Установка активной выборки. Вспомогательная функция, идет в связке с void ThroughAssZoomAndSelection()
		/// </summary>
		[CommandMethod("ThroughAssZoomAndSelectionHelperFunction",
			CommandFlags.Redraw | CommandFlags.NoHistory | CommandFlags.NoBlockEditor | 
			CommandFlags.NoUndoMarker | CommandFlags.NoPaperSpace)]
		public static void ThroughAssZoomAndSelectionHelperFunction()
		{
			// 

			Core.no_handle_selection = true; // чтобы изменение выборки не обрабатывалось

			Core.current_editor.SetImpliedSelection(internal_saved_selection);
			AcMapMap.ForceScreenRefresh();
		}

		/// <summary>
		/// Установка зумирования и активной выборки (запуск через командную строку - отсюда название)
		/// </summary>
		/// <param name="sel_base">выборка, которую следует отобразить</param>
		internal static void ThroughAssZoomAndSelection(MgSelectionBase sel_base)
		{

			internal_saved_selection = AcMapFeatureEntityService.AddFeaturesToSelectionSet(null, sel_base);	// преобразование в SelectionSet и обратно, 
			
			sel_base = AcMapFeatureEntityService.GetSelection(internal_saved_selection);					// иначе дает сбой метод GetSelectedFeatures()

			MgEnvelope extents = new MgEnvelope();

			MgReadOnlyLayerCollection layers = sel_base.GetLayers();

			foreach (MgLayerBase layer in layers)
			{
				MgFeatureReader ftr_reader = sel_base.GetSelectedFeatures(layer, layer.FeatureClassName, false);
				string geom_prop_name = ftr_reader.GetClassDefinition().DefaultGeometryPropertyName;

				while (ftr_reader.ReadNext())
				{
					MgByteReader byte_reader = ftr_reader.GetGeometry(geom_prop_name);
					MgAgfReaderWriter agf_reader_writer = new MgAgfReaderWriter();
					MgGeometry curr_geom = agf_reader_writer.Read(byte_reader);
					extents.ExpandToInclude(curr_geom.Envelope());
				}
			}



			// зумирование и вызов функции выбора через командную строку

			Core.no_handle_selection = true; // чтобы изменение выборки не обрабатывалось

			string zoom_str;
			if ((extents.Width == 0) && (extents.Width == extents.Height))
			{
				zoom_str = "'_zoom _c "
					+ extents.LowerLeftCoordinate.X.ToString() + "," + extents.LowerLeftCoordinate.Y.ToString() + " ";
			}
			else
			{
				zoom_str = "'_zoom _w "
					+ (extents.LowerLeftCoordinate.X - extents.Width/2).ToString() 
					+ "," 
					+ (extents.LowerLeftCoordinate.Y - extents.Height/2).ToString() 
					+ " "
					+ (extents.UpperRightCoordinate.X + extents.Width/2).ToString() 
					+ "," 
					+ (extents.UpperRightCoordinate.Y + extents.Height/2).ToString();
			}


			Core.SendStringToExecute(zoom_str + "\n"
				+ "ThroughAssZoomAndSelectionHelperFunction\n"
				, true, false, false);

		}

		#endregion


		#region Selection Commands

		/// <summary>
		/// Выбор только объектов FDO, выбор один раз рамкой
		/// </summary>
		[CommandMethod("ShowLinkSingleSelect", CommandFlags.Redraw |CommandFlags.NoBlockEditor | CommandFlags.NoPaperSpace)]
		public void FDOSelectSingle()
		{
			Core.ClearImpliedSelection();

			PromptSelectionOptions std_opts = new PromptSelectionOptions();
			std_opts.MessageForAdding = "Выберите объекты карты (FDO) (Esc для выхода):";
			std_opts.MessageForRemoval = std_opts.MessageForAdding;

			std_opts.SingleOnly = true;
			//std_opts.SetKeywords("[вЫход]", "eXit");
			//std_opts.Keywords.Default = "eXit";

			PromptSelectionResult sel_result = Core.current_editor.GetSelection(std_opts); // выбор объектов
			if (sel_result.Status == PromptStatus.OK)
			{
				// преобразование SelectionSet'а AutoCAD'а в выборку MgSelectionBase (только объекты FDO)
				MgSelectionBase selection_base = AcMapFeatureEntityService.GetSelection(sel_result.Value);

				// установка активной выборки
				SelectionSet new_sel_set = AcMapFeatureEntityService.AddFeaturesToSelectionSet(null, selection_base);
				Core.current_editor.SetImpliedSelection(new_sel_set);
			}
			Core.WriteMessage("\n");
		}



		/// <summary>
		/// Выбор только объектов FDO, пересекающих указанную пользователем точку
		/// </summary>
		[CommandMethod("ShowLinkCrossingPointSelect", CommandFlags.Redraw |CommandFlags.NoBlockEditor | CommandFlags.NoPaperSpace)]
		public void FDOSelectCrossingPoint()
		{
			Core.ClearImpliedSelection();

			PromptPointOptions pnt_opts = new PromptPointOptions("Укажите точку для выбора объктов карты (FDO) (Esc или Enter чтобы отказаться):");
			pnt_opts.AllowNone = true;

			PromptPointResult point_result = Core.current_editor.GetPoint(pnt_opts); // выбор точки
			if (point_result.Status == PromptStatus.OK)
			{
				PromptSelectionResult sel_result = Core.current_editor.SelectCrossingWindow(point_result.Value, point_result.Value); // выбор объектов
				if (sel_result.Status == PromptStatus.OK)
				{
					// преобразование SelectionSet'а AutoCAD'а в выборку MgSelectionBase (только объекты FDO)
					MgSelectionBase selection_base = AcMapFeatureEntityService.GetSelection(sel_result.Value);

					// установка активной выборки
					SelectionSet new_sel_set = AcMapFeatureEntityService.AddFeaturesToSelectionSet(null, selection_base);
					Core.current_editor.SetImpliedSelection(new_sel_set);
				}
			}
			Core.WriteMessage("\n");
		}

		#endregion



		#region Show Or Hide Palette

		/// <summary>
		/// Включить или выключить палитру
		/// </summary>
		[CommandMethod("ShowLinkShowOrHide", CommandFlags.Redraw |CommandFlags.NoBlockEditor | CommandFlags.NoUndoMarker)]
		public void ShowLinkShowPaletteCommand()
		{
			if (Core.palette_window.palette_set.Visible)
			{
				Core.palette_window.palette_set.Visible = false;
				Core.WriteMessage("Панель ссылок скрыта\n");
			}
			else
			{
				Core.palette_window.palette_set.Visible = true;
				if (!Core.palette_window.links_control.lock_data_grid_trigger)
				{
					Core.HandleSelectedFDO();
				}
				Core.WriteMessage("Панель ссылок показана\n");
			}
		}


		#endregion



	}
}
