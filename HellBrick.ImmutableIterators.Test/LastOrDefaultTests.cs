using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace HellBrick.ImmutableIterators.Test
{
	public class LastOrDefaultTests
	{
		[Fact]
		public void Empty() => VerifyLastOrDefault( Array.Empty<int>(), () => 42 );

		[Fact]
		public void SingleItem() => VerifyLastOrDefault( new int[] { 42 }, () => throw null );

		[Fact]
		public void MultipleItems() => VerifyLastOrDefault( new int[] { 42, 64 }, () => throw null );

		private void VerifyLastOrDefault( int[] source, Func<int> defaultFactory )
		{
			int expected = source.Length > 0 ? source.Last() : defaultFactory();
			int actual = source.AsImmutableIterator().LastOrDefault( defaultFactory );
			actual.Should().Be( expected );
		}
	}
}
