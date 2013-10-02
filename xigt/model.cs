using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using xigt.xaml;

namespace xigt
{
	public interface IXigt : IReadOnlyList<String>
	{
		String Id { get; }
		String this[String key] { get; set; }
	};

	public interface IXigt<out TParent> : IXigt
		where TParent : IXigt
	{
		TParent Parent { get; }

		String Type { get; }
	};

	public interface IMetadataHost : IXigt
	{
		IList<Metadata> Metadatas { get; }
	};

	public struct attr_entry
	{
		public static readonly attr_entry[] None = new attr_entry[0];

		public attr_entry(String key, String value)
		{
			this.key = key;
			this.value = value;
		}
		public String key;
		public String value;
	};

	public abstract class xigt_element : IXigt
	{
		public xigt_element()
		{
			this.a = attr_entry.None;
		}
		public xigt_element(String Id)
		{
			this.a = attr_entry.None;
			this.Id = Id;
		}

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

		attr_entry[] a;

		public String this[String key]
		{
			get
			{
				for (int i = 0; i < a.Length; i++)
					if (a[i].key == key)
						return a[i].value;
				return null;
			}
			set
			{
				for (int i = 0; i < a.Length; i++)
					if (a[i].key == key)
					{
						a[i] = new attr_entry(key, value);
						return;
					}
				arr.Append(ref a, new attr_entry(key, value));
			}
		}

		public String this[int ix] { get { return a[ix].value; } }

		public int Count { get { return a.Length; } }

		public IEnumerator<String> GetEnumerator() { return a.Select(ent => ent.value).GetEnumerator(); }

		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
	};

	public abstract class xigt_child_element<TParent> : xigt_element, IXigt<TParent>
		where TParent : IXigt
	{
		public xigt_child_element(TParent par, String Id, String type)
			: base(Id)
		{
			this.par = par;
			this.Type = type;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		readonly TParent par;
		public TParent Parent { get { return par; } }

		public String Type
		{
			get { return this["type"]; }
			set { this["type"] = value; }
		}
	};

	[DebuggerDisplay("{ToString(),nq}", Type = "XigtCorpus")]
	[DebuggerTypeProxy(typeof(corp_dbg))]
	public class XigtCorpus : xigt_element, IMetadataHost
	{
		public static XigtCorpus Create(xigtcorpus xcorp)
		{
			XigtCorpus c = new XigtCorpus(xcorp.id, xcorp.contentref, xcorp.annotationref);
			c.load_1(xcorp);
			c.resolve(xcorp);
			return c;
		}

		public XigtCorpus(String id, String contentref, String annotref)
			: base(id)
		{
			this.ContentRefKey = contentref ?? "ref";
			this.AnnotationRefKey = annotref ?? "ref";
			this.AlignmentMethod = "auto";

			this._igts = new List<Igt>();
			this._mdx = new List<Metadata>();
		}

		void load_1(xigtcorpus xcorp)
		{
			foreach (var xigt in xcorp.igts)
			{
				var igt = new Igt(this, xigt.type, xigt.id);
				igt.load_1(xigt);
				_igts.Add(igt);
			}
			foreach (var xmd in xcorp.metadatas)
			{
				var md = new Metadata(this, xmd.type);
				md.load_1(xmd);
				_mdx.Add(md);
			}
		}

		void resolve(xigtcorpus xcorp)
		{
			int i = 0;
			foreach (var xigt in xcorp.igts)
				_igts[i++].resolve(xigt);
		}

		public String ContentRefKey
		{
			get { return this["content-ref"]; }
			set { this["content-ref"] = value; }
		}
		public String AnnotationRefKey
		{
			get { return this["annotation-ref"]; }
			set { this["annotation-ref"] = value; }
		}
		public String AlignmentMethod
		{
			get { return this["alignment-method"]; }
			set { this["alignment-method"] = value; }
		}

		readonly List<Igt> _igts;
		public IList<Igt> Igts { get { return _igts; } }

		readonly List<Metadata> _mdx;
		public IList<Metadata> Metadatas { get { return _mdx; } }

		public override string ToString()
		{
			return string.Format("content-ref: {0}  annotation-ref: {1}  alignment-method: {2}",
				ContentRefKey,
				AnnotationRefKey,
				AlignmentMethod);
		}

		[DebuggerDisplay("{corp.ToString(),nq}", Type = "XigtCorpus")]
		public sealed class corp_dbg
		{
			public static corp_dbg[] MakeArray(IReadOnlyList<XigtCorpus> items)
			{
				var ret = new corp_dbg[items.Count];
				for (int i = 0; i < ret.Length; i++)
					ret[i] = new corp_dbg(items[i]);
				return ret;
			}

			public corp_dbg(XigtCorpus corp) { this.corp = corp; }
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			readonly XigtCorpus corp;

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public Igt.igt_dbg[] _dbg_igts { get { return Igt.igt_dbg.MakeArray(corp._igts); } }
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public Metadata.metadata_dbg[] _zdbg_mdx { get { return Metadata.metadata_dbg.MakeArray(corp._mdx); } }
		};
	};

	public class Meta : xigt_child_element<Metadata>
	{
		public Meta(Metadata m, String type)
			: base(m, default(String), type)
		{
		}
	};

	[DebuggerDisplay("{ToString(),nq}", Name = "{Id,nq}")]
	[DebuggerTypeProxy(typeof(metadata_dbg))]
	public class Metadata : xigt_child_element<IMetadataHost>
	{
		public Metadata(IMetadataHost host, String type)
			: base(host, default(String), type)
		{
			this._mx = new List<Meta>();
		}

		readonly List<Meta> _mx;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]

		public IList<Meta> Metas { get { return _mx; } }

		internal void load_1(xaml.metadata xmd)
		{
			foreach (var xmeta in xmd.metas)
			{
				var meta = new Meta(this, xmeta.type);
				_mx.Add(meta);
			}
		}

		public override string ToString()
		{
			return string.Format("{0} ({1})", Type, _mx.Count);
		}

		[DebuggerDisplay("{m.ToString(),nq}", Name = "[metadata-{ix}] {m.Id,nq}", Type = "Metadata")]
		public sealed class metadata_dbg
		{
			public static metadata_dbg[] MakeArray(IReadOnlyList<Metadata> items)
			{
				var ret = new metadata_dbg[items.Count];
				for (int i = 0; i < ret.Length; i++)
					ret[i] = new metadata_dbg(items[i]);
				return ret;
			}
			public metadata_dbg(Metadata m) { this.m = m; }
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			readonly Metadata m;

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			int ix { get { return m.Parent.Metadatas.IndexOf(m); } }
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public Meta[] _dbg_metas { get { return m._mx.ToArray(); } }
		};
	};

	[DebuggerDisplay("{ToString(),nq}", Name = "{Id,nq}")]
	[DebuggerTypeProxy(typeof(igt_dbg))]
	public class Igt : xigt_child_element<XigtCorpus>, IMetadataHost
	{
		public Igt(XigtCorpus xc, String Id, String type)
			: base(xc, Id, type)
		{
			this._tiers = new List<Tier>();
			this._mdx = new List<Metadata>();
		}

		readonly List<Tier> _tiers;
		public IList<Tier> Tiers { get { return _tiers; } }

		readonly List<Metadata> _mdx;
		public IList<Metadata> Metadatas { get { return _mdx; } }

		internal void load_1(xaml.igt xigt)
		{
			foreach (var xtier in xigt.tiers)
			{
				var tier = new Tier(this, xtier.id, xtier.type);
				tier.load_1(xtier);
				_tiers.Add(tier);
			}
			foreach (var xmd in xigt.metadatas)
			{
				var md = new Metadata(this, xmd.type);
				md.load_1(xmd);
				_mdx.Add(md);
			}
		}

		internal void resolve(xaml.igt xigt)
		{
			int i = 0;
			foreach (var xtier in xigt.tiers)
				_tiers[i++].resolve(xtier);
		}

		public override string ToString()
		{
			return string.Format("{0} ({1})", Type, _tiers.Count);
		}

		[DebuggerDisplay("{igt.ToString(),nq}", Name = "[igt-{ix}] {igt.Id,nq}", Type = "Igt")]
		public sealed class igt_dbg
		{
			public static igt_dbg[] MakeArray(IReadOnlyList<Igt> items)
			{
				var ret = new igt_dbg[items.Count];
				for (int i = 0; i < ret.Length; i++)
					ret[i] = new igt_dbg(items[i]);
				return ret;
			}
			public igt_dbg(Igt igt) { this.igt = igt; }
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			readonly Igt igt;

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			int ix { get { return igt.Parent.Igts.IndexOf(igt); } }
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public Tier.tier_dbg[] _dbg_tiers { get { return Tier.tier_dbg.MakeArray(igt._tiers); } }
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public Metadata.metadata_dbg[] _zdbg_mdx { get { return Metadata.metadata_dbg.MakeArray(igt._mdx); } }
		};
	};

	[DebuggerDisplay("{ToString(),nq}", Name = "{Id,nq}")]
	[DebuggerTypeProxy(typeof(tier_dbg))]
	public class Tier : xigt_child_element<Igt>, IMetadataHost
	{
		public Tier(Igt igt, String Id, String type)
			: base(igt, Id, type)
		{
			this._items = new List<Item>();
			this._mdx = new List<Metadata>();
			this.RefSets = new Dictionary<String, RefSet>();
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		readonly List<Item> _items;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public IList<Item> Items { get { return _items; } }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		readonly List<Metadata> _mdx;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public IList<Metadata> Metadatas { get { return _mdx; } }

		readonly public Dictionary<String, RefSet> RefSets;

		internal void load_1(xaml.tier xtier)
		{
			foreach (var xitem in xtier.items)
			{
				var item = new Item(this, xitem.id, xitem.type, xitem.Content);
				_items.Add(item);
			}
			foreach (var xmd in xtier.metadatas)
			{
				var md = new Metadata(this, xmd.type);
				md.load_1(xmd);
				_mdx.Add(md);
			}
		}

		internal void resolve(xaml.tier xtier)
		{
			var c = this.XigtCorpus;
			foreach (var kvp in xtier.attribs)
			{
				if (kvp.Key == c.ContentRefKey)
				{
					var tgt_tier = Parent.Tiers.First(t => t.Id == kvp.Value);
					var rs = new RefSet(tgt_tier);
					rs.resolve(this, kvp.Key, xtier.items);
					RefSets.Add(kvp.Key, rs);
				}
				else if (kvp.Key == c.AnnotationRefKey)
				{
					var tgt_tier = Parent.Tiers.First(t => t.Id == kvp.Value);
					var rs = new RefSet(tgt_tier);
					rs.resolve(this, kvp.Key, xtier.items);
					RefSets.Add(kvp.Key, rs);
				}
				else if (kvp.Key == "alignref")
				{
					throw new Exception();
				}
			}
		}

		public override string ToString()
		{
			return String.Format("{0} ({1})", this.Type, _items.Count);
		}


		[DebuggerDisplay("{tier.ToString(),nq}", Name = "[tier-{ix}] {tier.Id,nq}", Type = "Tier")]
		public sealed class tier_dbg
		{
			public static tier_dbg[] MakeArray(IReadOnlyList<Tier> items)
			{
				var ret = new tier_dbg[items.Count];
				for (int i = 0; i < ret.Length; i++)
					ret[i] = new tier_dbg(items[i]);
				return ret;
			}

			public tier_dbg(Tier tier) { this.tier = tier; }
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			readonly Tier tier;

			public Dictionary<String, RefSet> RefSets { get { return tier.RefSets; } }

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			int ix { get { return tier.Parent.Tiers.IndexOf(tier); } }
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public Item.item_dbg[] _dbg_items { get { return Item.item_dbg.MakeArray(tier._items); } }
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public Metadata.metadata_dbg[] _zdbg_mdx { get { return Metadata.metadata_dbg.MakeArray(tier._mdx); } }
		};
	};

	[DebuggerDisplay("{ToString(),nq}", Name = "{Id,nq}")]
	[DebuggerTypeProxy(typeof(item_dbg))]
	public class Item : xigt_child_element<Tier>
	{
		public Item(Tier t, String Id, String type, String content)
			: base(t, Id, type)
		{
			this._content = content;
		}

		String _content;
		public String Content
		{
			get
			{
				if (_content != null)
					return _content;
				return Parent.RefSets[XigtCorpus.ContentRefKey][this].ToString();
			}
			set { _content = value; }
		}

		public override string ToString()
		{
			return String.Format("{0} {1}", Type, Content);
		}

		[DebuggerDisplay("{item.ToString(),nq}", Name = "[item-{ix}] {item.Id,nq}", Type = "Item")]
		public sealed class item_dbg
		{
			public static item_dbg[] MakeArray(IReadOnlyList<Item> items)
			{
				var ret = new item_dbg[items.Count];
				for (int i = 0; i < ret.Length; i++)
					ret[i] = new item_dbg(items[i]);
				return ret;
			}
			public item_dbg(Item item) { this.item = item; }
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			readonly Item item;

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			int ix { get { return item.Parent.Items.IndexOf(item); } }
		}
	};
}
