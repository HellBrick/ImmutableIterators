using System.Collections.Generic;

namespace HellBrick.ImmutableIterators
{
	public static partial class ImmutableIterator
	{
		public static List<T> ToList<T, TState>( this in ImmutableIterator<T, TState> iterator )
			where TState : struct, IIteratorState<T, TState>
			=> iterator.Aggregate
			(
				new List<T>(),
				( list, item ) =>
				{
					list.Add( item );
					return list;
				}
			);
	}
}
