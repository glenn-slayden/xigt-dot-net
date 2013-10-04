using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xigt
{
	public interface IXigt
	{
	};

	public interface IXigt<out TParent> : IXigt
		where TParent : IXigt
	{
	};

	public interface IMetadataHost : IXigt
	{
	};

	//public interface IMetadataHost<TParent> : IXigt<TParent>, IMetadataHost
	//	where TParent : IMetadataHost
	//{
	//	IReadOnlyList<Metadata<TParent>> Metadatas { get; }
	//};

	public interface IMetadataHost<TParent, TMetadata> : IXigt<TParent>, IMetadataHost
		where TParent : IXigt
	{
		IReadOnlyList<TMetadata> Metadatas { get; }
	};


	public abstract class xigt_element<TParent> : IXigt<TParent>
		where TParent : IXigt
	{
		public xigt_element(TParent par)
		{
			this.par = par;
		}
		public readonly TParent par;
	};


	public class XigtCorpus : IXigt, IMetadataHost<IXigt, Metadata<XigtCorpus>>
	{
		public XigtCorpus()
		{
			this.ContentRef = "ref";
			this.AnnotationRef = "ref";
			this.AlignmentMethod = "auto";
		}

		public readonly List<Igt> Igts = new List<Igt>();

		public String ContentRef { get; set; }
		public String AnnotationRef { get; set; }
		public String AlignmentMethod { get; set; }

		public IReadOnlyList<Metadata<XigtCorpus>> Metadatas
		{
			get { throw new NotImplementedException(); }
		}
	};

	public class Metadata<TParent> : xigt_element<TParent>
		where TParent : IMetadataHost
	{
		public Metadata(TParent par)
			: base(par)
		{
		}

		public String Type { get; set; }

		public IReadOnlyList<Meta<TParent>> Metas
		{
			get { return null; }
		}
	};

	public abstract class Meta : IXigt
	{
		public String Type { get; set; }
		public String Tiers { get; set; }
		public String ISO_639_3 { get; set; }
		public String Name { get; set; }
	};

	public class Meta<TParent> : xigt_element<TParent>
		where TParent : IXigt
	{
		public Meta(TParent par)
			: base(par)
		{
		}

		public String Type { get; set; }
		public String Tiers { get; set; }
		public String ISO_639_3 { get; set; }
		public String Name { get; set; }

	};


	public class Igt : xigt_element<XigtCorpus>, IMetadataHost<XigtCorpus, Metadata<Igt>>
	{
		public Igt(XigtCorpus xc)
			: base(xc)
		{
		}

		public String Id { get; set; }


		public IReadOnlyList<Metadata<Igt>> Metadatas
		{
			get { throw new NotImplementedException(); }
		}
	};

	public class Tier : xigt_element<Igt>, IMetadataHost<Igt, Metadata<Tier>>
	{
		public Tier(Igt igt)
			: base(igt)
		{
		}

		public String Id { get; set; }
		public String Type { get; set; }
		public String TxtRef { get; set; }
		public String _ref { get; set; }

		public IReadOnlyList<Metadata<Tier>> Metadatas
		{
			get { throw new NotImplementedException(); }
		}
	};

	public class Item : xigt_element<Tier>
	{
		public Item(Tier t)
			: base(t)
		{
		}
		public String Id { get; set; }
		public String _ref { get; set; }
		public String txtref { get; set; }
		public String Line { get; set; }
		public String Tag { get; set; }
		public String Content { get; set; }
	};


}
