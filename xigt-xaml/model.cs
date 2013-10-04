using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xaml;
using System.Windows.Markup;

namespace xigt.xaml
{
	public class xigtcorpus : IEnumerable<Icorpus_item>
	{
		public xigtcorpus()
		{
			this.contentref = "ref";
			this.annotationref = "ref";
			this.alignmentmethod = "auto";
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String id { get; set; }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public readonly List<igt> igts = new List<igt>();
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public readonly List<metadata> metadatas = new List<metadata>();

		[DebuggerDisplay("{contentref}", Name = "content-ref")]
		public String contentref { get; set; }
		[DebuggerDisplay("{annotationref}", Name = "annotation-ref")]
		public String annotationref { get; set; }
		[DebuggerDisplay("{alignmentmethod}", Name = "alignment-method")]
		public String alignmentmethod { get; set; }

		public void Add(Icorpus_item item)
		{
			if (item is igt)
				igts.Add((igt)item);
			else if (item is metadata)
				metadatas.Add((metadata)item);
			else
				throw new NotImplementedException();
		}
		IEnumerator<Icorpus_item> IEnumerable<Icorpus_item>.GetEnumerator()
		{
			return igts.Cast<Icorpus_item>().Concat(metadatas).GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() { return ((IEnumerable<Icorpus_item>)this).GetEnumerator(); }
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public igt[] _dbg_items { get { return igts.ToArray(); } }
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public metadata[] _dbg_metadata { get { return metadatas.ToArray(); } }
	};

	[DebuggerDisplay("type:{type,nq}")]
	public class metadata : IEnumerable<meta>, Itype, Icorpus_item
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String type { get; set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		readonly public List<meta> metas = new List<meta>();
		public void Add(meta meta) { metas.Add(meta); }
		IEnumerator<meta> IEnumerable<meta>.GetEnumerator() { return metas.GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return metas.GetEnumerator(); }
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public meta[] _dbg_items { get { return metas.ToArray(); } }
	};

	[DebuggerDisplay("type:{type.PadRight(15),nq} tiers:{tiers,nq} iso-639-3:{iso6393,nq} name{name,nq}")]
	public class meta : Itype
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String type { get; set; }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String tiers { get; set; }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String iso6393 { get; set; }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String name { get; set; }
	};

	[DebuggerDisplay("{(\"(\"+tiers.Count.ToString()+\")\").PadRight(5),nq}", Name = "{id,nq}")]
	public class igt : IEnumerable<tier>, Iid, Itype, Icorpus_item
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String id { get; set; }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String type { get; set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public readonly List<metadata> metadatas = new List<metadata>();

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public readonly List<tier> tiers = new List<tier>();
		public void Add(Object item)
		{
			if (item is tier)
				tiers.Add((tier)item);
			else if (item is metadata)
				metadatas.Add((metadata)item);
			else
				throw new NotImplementedException();
		}
		IEnumerator<tier> IEnumerable<tier>.GetEnumerator() { return tiers.GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return ((IEnumerable<tier>)this).GetEnumerator(); }
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public tier[] _dbg_items { get { return tiers.ToArray(); } }
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public metadata[] _dbg_metadata { get { return metadatas.ToArray(); } }
	};

	[DebuggerDisplay("{(\"(\"+items.Count.ToString()+\")\").PadRight(5),nq} type:{type.PadRight(15),nq} ref:{(_ref??\"\").PadRight(5),nq} txtref:{(txtref??\"\").PadRight(5),nq}", Name = "{id,nq}")]
	public class tier : IEnumerable<item>, Iid, Itype, Iref
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String id { get; set; }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String type { get; set; }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String txtref { get; set; }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String _ref { get; set; }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String alignref { get; set; }

		public IEnumerable<KeyValuePair<String, String>> attribs
		{
			get
			{
				if (_ref != null)
					yield return new KeyValuePair<String, String>("ref", _ref);
				if (txtref != null)
					yield return new KeyValuePair<String, String>("txtref", txtref);
				if (alignref != null)
					yield return new KeyValuePair<String, String>("alignref", alignref);
			}
		}


		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public readonly List<metadata> metadatas = new List<metadata>();

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public readonly List<item> items = new List<item>();
		public void Add(Object item)
		{
			if (item is item)
				items.Add((item)item);
			else if (item is metadata)
				metadatas.Add((metadata)item);
			else
				throw new NotImplementedException();
		}
		IEnumerator<item> IEnumerable<item>.GetEnumerator() { return items.GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return ((IEnumerable<item>)this).GetEnumerator(); }
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public item[] _dbg_items { get { return items.ToArray(); } }
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public metadata[] _dbg_metadata { get { return metadatas.ToArray(); } }
	};

	[ContentProperty("Content")]
	[DebuggerDisplay("ref:{(_ref??\"\").PadRight(15),nq} txtref:{(txtref??\"\").PadRight(15),nq} line:{(line??\"\").PadRight(5),nq} tag:{(tag??\"\").PadRight(5),nq}", Name = "{id,nq}")]
	public class item : Iid, Itype, Iref
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String id { get; set; }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String type { get; set; }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String _ref { get; set; }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String txtref { get; set; }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String alignref { get; set; }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String line { get; set; }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public String tag { get; set; }
		[DebuggerDisplay("{Content,nq}")]
		public String Content { get; set; }


		public IEnumerable<KeyValuePair<String, String>> attribs
		{
			get
			{
				if (_ref != null)
					yield return new KeyValuePair<String, String>("ref", _ref);
				if (txtref != null)
					yield return new KeyValuePair<String, String>("txtref", txtref);
				if (alignref != null)
					yield return new KeyValuePair<String, String>("alignref", alignref);
			}
		}
	};
}
