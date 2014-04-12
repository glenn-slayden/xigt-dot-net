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

using Microsoft.Win32;
using System.Xaml;

using xigt;

namespace xigt_editor
{
	public partial class MainWindow : Window
	{
		public static readonly DependencyProperty FilenameProperty =
				DependencyProperty.Register(
					"Filename",
					typeof(String),
					typeof(MainWindow),
					new FrameworkPropertyMetadata((o, e) => ((MainWindow)o).filename_changed((String)e.NewValue)));

		public String Filename
		{
			get { return (String)GetValue(FilenameProperty); }
			set { SetValue(FilenameProperty, value); }
		}

		public MainWindow()
		{
			InitializeComponent();

			Loaded += (o, e) =>
			{
				Filename = @"sample-files\kor.xml";

				var btv = new BooleanToVisibilityConverter();

				BindingOperations.SetBinding(w_content.w_items, UIElement.VisibilityProperty, new Binding
				{
					Source = m_items,
					Path = new PropertyPath(MenuItem.IsCheckedProperty),
					Converter = btv
				});
				BindingOperations.SetBinding(w_content.render_pane, UIElement.VisibilityProperty, new Binding
				{
					Source = m_render,
					Path = new PropertyPath(MenuItem.IsCheckedProperty),
					Converter = btv
				});
				BindingOperations.SetBinding(w_content.xigt_pane, UIElement.VisibilityProperty, new Binding
				{
					Source = m_xigt,
					Path = new PropertyPath(MenuItem.IsCheckedProperty),
					Converter = btv
				});
				BindingOperations.SetBinding(w_content.nav_pane, UIElement.VisibilityProperty, new Binding
				{
					Source = m_nav,
					Path = new PropertyPath(MenuItem.IsCheckedProperty),
					Converter = btv
				});
				BindingOperations.SetBinding(w_content.xml_pane, UIElement.VisibilityProperty, new Binding
				{
					Source = m_xml,
					Path = new PropertyPath(MenuItem.IsCheckedProperty),
					Converter = btv
				});
			};
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				Close();
				e.Handled = true;
			}
			else
				base.OnKeyDown(e);
		}

		void filename_changed(String filename)
		{
			var xcorpus = xigt.xaml.Runtime.Load(filename);

			var corpus = XigtCorpus.Create(xcorpus);

			w_content.DataContext = corpus;
		}

		private void Menu_FileOpen(Object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog
			{
				Title = "Open Xigt File...",
				DefaultExt = ".xml",
				Filter = "Extensible Interlinear Glossed Text (.xigt, *.xml)|*.xigt;*.xml|All files (*.*)|*.*",
				InitialDirectory = Environment.CurrentDirectory,
				AddExtension = true,
				CheckFileExists = true,
			};

			if (dlg.ShowDialog(this).Value)
				this.Filename = dlg.FileName;
		}
	};
}
