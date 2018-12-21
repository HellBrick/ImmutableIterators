namespace HellBrick.ImmutableIterators
{
	public static partial class ImmutableIterator
	{
		public static T[] ToArray<T, TState>( this in ImmutableIterator<T, TState> iterator )
			where TState : struct, IIteratorState<T, TState>
			=> iterator
			.ToList()
			.ToArray();
	}
}
