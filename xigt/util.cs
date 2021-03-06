﻿using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// 
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public static class _ext
{
	public static void ForEach(this IEnumerable seq, Action<Object> a)
	{
		var e = seq.GetEnumerator();
		while (e.MoveNext())
			a(e.Current);
	}
};


public static class Nop
{
	public static void X() { }
}
public static class not
{
	public static Exception impl { get { return new NotImplementedException(); } }
}

public static class arr
{
	public static void Append<T>(ref T[] src, T item)
	{
		src = Append(src, item);
	}
	public static T[] Append<T>(this T[] src, T item)
	{
		T[] dst;
		int c = src == null ? 0 : src.Length;
		(dst = new T[c + 1])[c] = item;
		while (--c >= 0)
			dst[c] = src[c];
		return dst;
	}
}