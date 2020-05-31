using System.Windows.Forms;

//AutoCAD API
using Autodesk.AutoCAD.Windows;

namespace ShowLink
{
	public class PaletteWindow
	{
		public PaletteSet palette_set;
		
		public PaletteControl links_control;

		public PaletteWindow(string _palette_set_name) 
		{

			palette_set = new PaletteSet(_palette_set_name);
			links_control = new PaletteControl();
			//palette_set.Add("Ссылки", links_control);

			palette_set.Visible = true;

			palette_set.Icon = ShowLink.Properties.Resources.application_icon;

			palette_set.Style =
				//PaletteSetStyles.NameEditable
				//| 
				PaletteSetStyles.Notify
				| PaletteSetStyles.ShowAutoHideButton
				| PaletteSetStyles.ShowCloseButton
				| PaletteSetStyles.ShowPropertiesMenu
				//| PaletteSetStyles.ShowTabForSingle
				| PaletteSetStyles.SingleRowDock
				| PaletteSetStyles.Snappable
				| PaletteSetStyles.UsePaletteNameAsTitleForSingle
				;

			palette_set.Dock = DockSides.None;


			palette_set.Size = new System.Drawing.Size(750, 250);
			palette_set.MinimumSize = new System.Drawing.Size(300, 250);
			
			palette_set.DockEnabled = DockSides.Bottom | DockSides.Left | DockSides.Right | DockSides.Top;


			palette_set.Add("Ссылки", links_control);

			palette_set.Visible = false;

		}
		


	}
}
