using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace HellBrick.ImmutableIterators.Test
{
	public class SelectIteratorTests
	{
		[Fact]
		public void TransformIsAppliedToAllItems()
		{
			int[] source = new int[] { 42, 64, 128 };
			Func<int, int> transform = x => x * 10;

			int[] actual = source.AsImmutableIterator().Select( transform ).ToArray();
			int[] expected = source.Select( transform ).ToArray();

			actual.Should().BeEquivalentTo( expected );
		}
	}
}
