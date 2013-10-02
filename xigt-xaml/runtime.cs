using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xaml;
using System.Windows.Markup;


namespace xigt.xaml
{
	public static class Runtime
	{
		public static xigtcorpus Load(String filename)
		{
			var xr = new XamlXmlReader(filename);

			var xw = new XamlObjectWriter(new XamlSchemaContext());

			XamlServices.Transform(xr, xw);

			return (xigtcorpus)xw.Result;
		}
	};
}
