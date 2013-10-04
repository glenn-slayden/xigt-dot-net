using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xigt
{
	public interface IXigt //: IReadOnlyDictionary<String, String>
	{
	};

	public interface IXigt<out TParent> : IXigt
		where TParent : IXigt
	{
		TParent Parent { get; }

		String Type { get; }
	};

	public interface IMetadataHost : IXigt
	{
		IReadOnlyList<Metadata> Metadatas { get; }
	};

#if false
	public abstract class Meta : IXigt
	{
		public String Type { get; set; }
		public String Tiers { get; set; }
		public String ISO_639_3 { get; set; }
		public String Name { get; set; }
	};

	public class Metadata : IReadOnlyList<Meta>, IXigt
	{
		public String Type { get; set; }

		public Meta this[int index]
		{
			get { throw new NotImplementedException(); }
		}

		public int Count
		{
			get { throw new NotImplementedException(); }
		}

		public IEnumerator<Meta> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	};
#else
	public class Metadata : xigt_child_element<IMetadataHost>
	{
		public Metadata(IMetadataHost host)
			: base(host)
		{
		}
	};
#endif




	public abstract class xigt_element : IXigt
	{
		public String Id { get; set; }

		public XigtCorpus XigtCorpus
		{
			get
			{
				XigtCorpus xc;
				IXigt x = this;
				while ((xc = x as XigtCorpus) == null)
					x = ((IXigt<IXigt>)x).Parent;
				return xc;
			}
		}
	};

	public abstract class xigt_child_element<TParent> : xigt_element, IXigt
		where TParent : IXigt
	{
		public xigt_child_element(TParent par)
		{
			this.par = par;
		}

		readonly TParent par;
		public IXigt Parent { get { return par; } }

		public String Type { get; set; }
	};

	public class XigtCorpus : xigt_element, IMetadataHost
	{
		public XigtCorpus()
		{
			this.ContentRef = "ref";
			this.AnnotationRef = "ref";
			this.AlignmentMethod = "auto";

			this._igts = new SortedList<string, Igt>();
		}

		public String ContentRef { get; set; }
		public String AnnotationRef { get; set; }
		public String AlignmentMethod { get; set; }

		readonly SortedList<String, Igt> _igts;

		public IReadOnlyList<Metadata> Metadatas
		{
			get { throw new NotImplementedException(); }
		}

		public IList<Igt> Igts
		{
			get { return _igts.Values; }
		}
	};

	public class Igt : xigt_child_element<XigtCorpus>, IMetadataHost
	{
		public Igt(XigtCorpus xc)
			: base(xc)
		{
			this._tiers = new SortedList<string, Tier>();
		}

		readonly SortedList<String, Tier> _tiers;

		public IReadOnlyList<Metadata> Metadatas
		{
			get { throw new NotImplementedException(); }
		}

		public IList<Tier> Tiers
		{
			get { return _tiers.Values; }
		}
	};


	public class Tier : xigt_child_element<Igt>, IMetadataHost
	{
		public Tier(Igt igt)
			: base(igt)
		{
			this._items = new SortedList<string, Item>();
			this.RefSets = new Dictionary<string, RefSet>();
		}

		public String TxtRef { get; set; }

		readonly SortedList<String, Item> _items;

		readonly public Dictionary<String, RefSet> RefSets;

		public IReadOnlyList<Metadata> Metadatas
		{
			get { throw new NotImplementedException(); }
		}

		public IList<Item> Items
		{
			get { return _items.Values; }
		}
	};

	public class Item : xigt_child_element<Tier>
	{
		public Item(Tier t)
			: base(t)
		{
		}

		public String Content { get; set; }
	};


}
