using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xigt.xaml
{
	public interface Icorpus_item
	{
	};

	public interface Iid
	{
		String id { get; set; }
	};
	public interface Itype
	{
		String type { get; set; }
	};
	public interface Iref
	{
		String _ref { get; set; }
	};
}
