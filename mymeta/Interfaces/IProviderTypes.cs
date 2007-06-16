using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace MyMeta
{
	[GuidAttribute("957c18aa-7538-4fbc-8cf3-7414068e93a0"),InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IProviderTypes
	{
		[DispId(0)]
		IProviderType this[object index] { get; }

		// ICollection
		bool IsSynchronized { get; }
		int Count { get; }
		void CopyTo(System.Array array, int index);
		object SyncRoot { get; }

		// IEnumerable
		[DispId(-4)]
		IEnumerator GetEnumerator();
	}
}


