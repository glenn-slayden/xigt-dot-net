using System;
using System.Diagnostics;
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
	public partial class igt_control : ListBox
	{
		public static readonly DependencyProperty IgtProperty =
			DependencyProperty.Register("Igt", typeof(Igt), typeof(igt_control), new FrameworkPropertyMetadata((o, e) => ((igt_control)o).igt_change()));

		public Igt Igt
		{
			get { return (Igt)GetValue(IgtProperty); }
			set { SetValue(IgtProperty, value); }
		}

		public igt_control()
		{
			InitializeComponent();
		}

		void igt_change()
		{
			this.Items.Clear();

			var mb = new MultiBinding
			{
				Mode = BindingMode.OneWay,
				Converter = MaxConverter.Instance,
			};

			foreach (var tier in Igt.Tiers)
			{
				var uit = new ui_tier(this) { Tier = tier };
				mb.Bindings.Add(new Binding
				{
					Source = uit.uts.tcx,
					Path = new PropertyPath(FrameworkElement.ActualWidthProperty),
				});
				this.Items.Add(uit);
			}

			foreach (ui_tier uit in this.Items)
				BindingOperations.SetBinding(uit, ui_tier.TierControlsWidthProperty, mb);

			this.SelectedIndex = 0;
			((UIElement)this.SelectedItem).Focus();
		}
	};
}
