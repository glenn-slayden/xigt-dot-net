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
using System.Xaml;

using xigt;

namespace xigt_editor
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// 
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class ui_tier : ListBoxItem
	{
		public static readonly DependencyProperty TierProperty =
			DependencyProperty.Register("Tier", typeof(Tier), typeof(ui_tier), new FrameworkPropertyMetadata((o, e) => ((ui_tier)o).tier_change()));

		public Tier Tier
		{
			get { return (Tier)GetValue(TierProperty); }
			set { SetValue(TierProperty, value); }
		}

		public static readonly DependencyProperty TierControlsWidthProperty =
			DependencyProperty.Register("TierControlsWidth", typeof(Double), typeof(ui_tier));

		public Double TierControlsWidth
		{
			get { return (Double)GetValue(TierControlsWidthProperty); }
			set { SetValue(TierControlsWidthProperty, value); }
		}

		public ui_tier(igt_control igtc)
		{
			this.igtc = igtc;
			this.Padding = new Thickness(1);
		}

		public readonly igt_control igtc;

		void tier_change()
		{
			this.Content = new ui_tier_shell(this);
		}

		public ui_tier_shell uts { get { return (ui_tier_shell)Content; } }
	};

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// 
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class ui_tier_shell : Grid
	{
		public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty;

		static ui_tier_shell()
		{
			OrientationProperty.AddOwner(typeof(ui_tier_shell), new FrameworkPropertyMetadata(Orientation.Horizontal));
		}

		public Orientation Orientation
		{
			get { return (Orientation)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}

		public ui_tier_shell(ui_tier uit)
		{
			this.uit = uit;

			this.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
			this.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

			// do items first to establish binding in case tier_controls changes our Orientation 
			var itx = new ui_items(this);
			BindingOperations.SetBinding(itx, ui_items.OrientationProperty, new Binding
			{
				Source = this,
				Path = new PropertyPath(OrientationProperty)
			});
			Grid.SetColumn(itx, 1);
			this.Children.Add(itx);

			this.tcx = new tier_controls(this)
			{
				VerticalAlignment = VerticalAlignment.Top,
			};
			Grid.SetColumn(tcx, 0);
			BindingOperations.SetBinding(tcx, FrameworkElement.WidthProperty, new Binding
			{
				Source = uit,
				Path = new PropertyPath(ui_tier.TierControlsWidthProperty),
			});
			this.Children.Add(tcx);
		}
		readonly ui_tier uit;

		readonly public tier_controls tcx;

		public Tier Tier { get { return uit.Tier; } }
	};


	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// 
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class ui_items : Border
	{
		public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty;

		static ui_items()
		{
			OrientationProperty.AddOwner(typeof(ui_items), new FrameworkPropertyMetadata(Orientation.Horizontal, (o, e) => ((ui_items)o).orientation_change()));
		}

		public Orientation Orientation
		{
			get { return (Orientation)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}


		public ui_items(ui_tier_shell uts)
		{
			this.uts = uts;
			this.BorderBrush = Brushes.LightGray;
			this.BorderThickness = new Thickness(0, 0, 1, 1);
			this.Child = this.g = new _grid(this);

			orientation_change();
		}

		readonly ui_tier_shell uts;

		readonly _grid g;

		public Tier Tier { get { return uts.Tier; } }

		void orientation_change() { g.orientation_change(Tier.Items); }

		class _grid : Grid
		{
			public _grid(ui_items uii)
			{
				this.uii = uii;
			}

			ui_items uii;

			public void orientation_change(IList<Item> items)
			{
				Double b_thick = uii.BorderThickness.Right;

				Children.Clear();
				ColumnDefinitions.Clear();
				RowDefinitions.Clear();

				DependencyProperty pts;

				if (uii.Orientation == Orientation.Horizontal)
				{
					items.ForEach(_ => ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }));
					pts = Grid.ColumnProperty;
				}
				else
				{
					items.ForEach(_ => RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }));
					pts = Grid.RowProperty;
				}

				for (int i = 0; i < items.Count; i++)
				{
					var bb = new Border
					{
						BorderBrush = uii.BorderBrush,
						BorderThickness = new Thickness(b_thick, b_thick, 0, 0),
						Child = new TextBlock
						{
							Text = items[i].Content.Replace(' ', '_'),
						}
					};

					bb.SetValue(pts, i);
					this.Children.Add(bb);
				}
			}
		}
	};
}