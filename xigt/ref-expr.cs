using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xigt
{
	[DebuggerDisplay("[{i_from}:{i_to}]{delim,nq}  {ToString(),nq}")]
	public class SliceRef
	{
		public static readonly SliceRef[] None = new SliceRef[0];

		public SliceRef()
		{
			this.delim = String.Empty;
			this.i_from = 0;
			this.i_to = -1;
		}

		public Item target;
		public int i_from;
		public int i_to;
		public String delim;

		public override String ToString()
		{
			var s = target.Content;
			if (s == null || (uint)i_from >= s.Length)
				return String.Empty;
			var i = i_to;
			if ((uint)i > s.Length)
				i = s.Length;
			return target.Content.Substring(i_from, i - i_from) + delim;
		}
	};

	[DebuggerDisplay("{ToString(),nq}")]
	public class RefExpr : IReadOnlyList<SliceRef>
	{
		public RefExpr(Item source, Tier tgt_tier, String to_parse)
		{
			this.source = source;
			this.a = SliceRef.None;

			var rgs = to_parse.Split(new[] { ',', '+', ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var _s in rgs)
			{
				var s = _s;
				int ix = s.IndexOf('[');
				var sr = new SliceRef();
				if (ix != -1)
				{
					Debug.Assert(s[s.Length - 1] == ']');
					var r = s.Substring(ix + 1, s.Length - ix - 2).Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
					sr.i_from = int.Parse(r[0]);
					sr.i_to = int.Parse(r[1]);
					s = s.Substring(0, ix);
				}
				sr.target = tgt_tier.Items.First(z => z.Id == s);

				arr.Append(ref a, sr);
			}

			Debug.Print("{0,6}   {1,20}  {2}", source.Id, to_parse, ToString());
		}

		public Item source;

		SliceRef[] a;

		public SliceRef this[int index] { get { return a[index]; } }

		public int Count { get { return a.Length; } }

		public IEnumerator<SliceRef> GetEnumerator() { return ((IEnumerable<SliceRef>)a).GetEnumerator(); }

		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

		public override String ToString()
		{
			return String.Join<Object>(String.Empty, a);
		}
	};


	public class RefSet : Dictionary<Item, RefExpr>
	{
		public RefSet(Tier target_tier)
		{
			this.target_tier = target_tier;
		}
		readonly public Tier target_tier;

		public void resolve(Tier tier, String sref, IEnumerable<xaml.item> xitems)
		{
			Debug.Print("{0} {1}  -->  {2}", tier.Id, sref, target_tier.Id);
			int i = 0;
			foreach (var xi in xitems)
			{
				var xiref = xi.attribs.FirstOrDefault(a => a.Key == sref);
				if (xiref.Key != null)
				{
					var rex = new RefExpr(tier.Items[i], target_tier, xiref.Value);
					base.Add(rex.source, rex);
				}
				else
				{
					throw not.impl;
				}
				i++;
			}
		}
	};
}
