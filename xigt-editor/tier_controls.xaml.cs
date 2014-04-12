using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using xigt;

namespace xigt_editor
{
	public partial class tier_controls : Grid
	{
		public tier_controls()
		{
			InitializeComponent();
		}

		public tier_controls(ui_tier_shell uts)
			: this()
		{
			this.uts = uts;

			w_type.Text = Tier.Type;
			w_id.Text = Tier.Id;
		}
		readonly ui_tier_shell uts;

		private void ToggleButton_Click_1(object sender, RoutedEventArgs e)
		{
			uts.Orientation = w_orientation.IsChecked.Value ? Orientation.Vertical : Orientation.Horizontal;
		}

		public Tier Tier { get { return uts.Tier; } }
	};
}
