﻿using System;
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
	public partial class xigt_corpus_control : Grid
	{
		public static readonly DependencyProperty XigtCorpusProperty =
			DependencyProperty.Register(
			"XigtCorpus",
			typeof(XigtCorpus),
			typeof(xigt_corpus_control),
			new FrameworkPropertyMetadata((o, e) => ((xigt_corpus_control)o).xigt_corpus_change()));

		public XigtCorpus XigtCorpus
		{
			get { return (XigtCorpus)GetValue(XigtCorpusProperty); }
			set { SetValue(XigtCorpusProperty, value); }
		}

		public xigt_corpus_control()
		{
			InitializeComponent();
		}

		void xigt_corpus_change()
		{
			this.DataContext = this.XigtCorpus;

			w_items.SelectedIndex = 0;

			w_xml.Text = foo;
		}

		private void nav_Prev(object sender, RoutedEventArgs e)
		{
			int c = w_items.SelectedIndex - 1;
			if (c >= 0)
				w_items.SelectedIndex = c;
		}
		private void nav_Next(object sender, RoutedEventArgs e)
		{
			int c = w_items.SelectedIndex + 1;
			if (c < w_items.Items.Count)
				w_items.SelectedIndex = c;
		}

		const String foo = @"<igt id=""i0"">
    <metadata type=""test-igt-meta"">
        <meta type=""language"" />
    </metadata>
    <tier type=""odin-txt"" id=""o"" xml:space=""preserve"">
        <item id=""o0"">doc id=397 959 961 L G T</item>
        <item id=""o1"">language: korean (kor)</item>
        <item id=""o2"" line=""959"" tag=""L"">   1 Nay-ka ai-eykey pap-ul mek-i-ess-ta</item>
        <item id=""o3"" line=""960"" tag=""G"">     I-Nom child-Dat rice-Acc eat-Caus-Pst-Dec</item>
        <item id=""o4"" line=""961"" tag=""T"">     `I made the child eat rice.'</item>
    </tier>
    <tier type=""phrases"" id=""p"" txtref=""o"">
        <item id=""p0"" txtref=""o2[5:40]""/>
    </tier>
    <tier type=""words"" id=""w"" _ref=""p"" txtref=""o"">
        <metadata type=""test-tier-meta"">
            <meta type=""language"" />
        </metadata>
        <item id=""w0"" _ref=""p0[0:6]"" txtref=""o2[5:11]""/>
        <item id=""w1"" _ref=""p0[7:15]"" txtref=""o2[12:20]""/>
        <item id=""w2"" _ref=""p0[16:22]"" txtref=""o2[21:27]""/>
        <item id=""w3"" _ref=""p0[23:35]"" txtref=""o2[28:40]""/>
    </tier>
    <tier type=""morphemes"" id=""m"" _ref=""w"" txtref=""o"">
        <item id=""w0.m0"" _ref=""w0[0:3]"" txtref=""o2[5:8]""/>
        <item id=""w0.m1"" _ref=""w0[4:6]"" txtref=""o2[9:11]""/>
        <item id=""w1.m0"" _ref=""w1[0:2]"" txtref=""o2[12:14]""/>
        <item id=""w1.m1"" _ref=""w1[3:8]"" txtref=""o2[15:20]""/>
        <item id=""w2.m0"" _ref=""w2[0:3]"" txtref=""o2[21:24]""/>
        <item id=""w2.m1"" _ref=""w2[4:6]"" txtref=""o2[25:27]""/>
        <item id=""w3.m0"" _ref=""w3[0:3]"" txtref=""o2[28:31]""/>
        <item id=""w3.m1"" _ref=""w3[4:5]"" txtref=""o2[32:33]""/>
        <item id=""w3.m2"" _ref=""w3[6:9]"" txtref=""o2[34:37]""/>
        <item id=""w3.m3"" _ref=""w3[10:12]"" txtref=""o2[38:40]""/>
    </tier>
    <tier type=""glosses"" id=""g"" _ref=""m"" txtref=""o"">
        <item id=""w0.g0"" _ref=""w0.m0"" txtref=""o3[5:6]""/>
        <item id=""w0.g1"" _ref=""w0.m1"" txtref=""o3[7:10]""/>
        <item id=""w1.g0"" _ref=""w1.m0"" txtref=""o3[11:16]""/>
        <item id=""w1.g1"" _ref=""w1.m1"" txtref=""o3[17:20]""/>
        <item id=""w2.g0"" _ref=""w2.m0"" txtref=""o3[21:25]""/>
        <item id=""w2.g1"" _ref=""w2.m1"" txtref=""o3[26:29]""/>
        <item id=""w3.g0"" _ref=""w3.m0"" txtref=""o3[30:33]""/>
        <item id=""w3.g1"" _ref=""w3.m1"" txtref=""o3[34:38]""/>
        <item id=""w3.g2"" _ref=""w3.m2"" txtref=""o3[39:42]""/>
        <item id=""w3.g3"" _ref=""w3.m3"" txtref=""o3[43:46]""/>
    </tier>
    <tier type=""translations"" id=""t"" _ref=""p"" txtref=""o"">
        <item id=""t0"" _ref=""p0"" txtref=""o4[6:32]""/>
    </tier>
</igt>";
	};
}
