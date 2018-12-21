using System;
using FluentAssertions;
using Xunit;

namespace HellBrick.ImmutableIterators.Test
{
	public class ArrayIteratorTests
	{
		[Fact]
		public void DoesNotMovePastEnd()
		{
			ArrayImmutableIterator.State<int> firstItemState = new int[] { 42 }.AsImmutableIterator().State;

			ArrayImmutableIterator.State<int> noMoreItemsState = firstItemState.GetNext();
			noMoreItemsState.HasValue.Should().BeFalse();

			ArrayImmutableIterator.State<int> secondNoMoreItemsState = noMoreItemsState.GetNext();
			secondNoMoreItemsState.Should().BeEquivalentTo( noMoreItemsState );
		}

		[Fact]
		public void EmptyArrayRoundTrips() => AssertArrayRoundTrips( Array.Empty<int>() );

		[Fact]
		public void NonEmptyArrayRoundTrips() => AssertArrayRoundTrips( new int[] { 42, 64, 128 } );

		private static void AssertArrayRoundTrips( int[] source )
		{
			int[] roundTripped = source.AsImmutableIterator().ToArray();
			roundTripped.Should().BeEquivalentTo( source );
		}
	}
}
