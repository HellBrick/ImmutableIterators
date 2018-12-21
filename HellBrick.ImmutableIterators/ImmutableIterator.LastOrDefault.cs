using System;

namespace HellBrick.ImmutableIterators
{
	public static partial class ImmutableIterator
	{
		public static T LastOrDefault<T, TState>( this ImmutableIterator<T, TState> iterator )
			where TState : struct, IIteratorState<T, TState>
			=> iterator.LastOrDefault( default( T ) );

		public static T LastOrDefault<T, TState>( this ImmutableIterator<T, TState> iterator, T defaultValue )
			where TState : struct, IIteratorState<T, TState>
			=> iterator.LastOrDefault( defaultValue, d => d );

		[NoCapture]
		public static T LastOrDefault<T, TState>( this ImmutableIterator<T, TState> iterator, Func<T> defaultFactory )
			where TState : struct, IIteratorState<T, TState>
			=> iterator.LastOrDefault( defaultFactory, capturedDefaultFactory => capturedDefaultFactory() );

		[NoCapture]
		public static T LastOrDefault<T, TState, TClosure>
		(
			this ImmutableIterator<T, TState> iterator,
			TClosure closure,
			Func<TClosure, T> defaultFactory
		)
			where TState : struct, IIteratorState<T, TState>
		{
			T lastObservedItem = default;
			bool itemWasObserved = false;

			for ( TState state = iterator.State; state.HasValue; state = state.GetNext() )
			{
				itemWasObserved = true;
				lastObservedItem = state.CurrentValue;
			}

			return itemWasObserved ? lastObservedItem : defaultFactory( closure );
		}
	}
}
