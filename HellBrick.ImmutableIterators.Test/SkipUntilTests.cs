using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace HellBrick.ImmutableIterators.Test
{
	public class SkipUntilTests
	{
		[Fact]
		public void Empty() => VerifySkipUntil( Array.Empty<int>(), x => true );

		[Fact]
		public void NoMatch() => VerifySkipUntil( new int[] { 42, 64 }, x => x > 100 );

		[Fact]
		public void StopsAtFirstMatch() => VerifySkipUntil( new int[] { 42, 42, 64, 42 }, x => x == 42 );

		private static void VerifySkipUntil( int[] source, Func<int, bool> stopPredicate )
		{
			int[] expected = source.SkipWhile( x => !stopPredicate( x ) ).ToArray();
			int[] actual = source.AsImmutableIterator().SkipUntil( stopPredicate ).ToArray();
			actual.Should().BeEquivalentTo( expected );
		}

		public static IEnumerable<object[]> EnumerateTestCases()
		{
			return
				EnumerateCases()
				.Select( testCase => new object[] { testCase.Source, testCase.StopPredicate } );

			IEnumerable<(int[] Source, Func<int, bool> StopPredicate)> EnumerateCases()
			{
				yield return (Array.Empty<int>(), x => true);
			}
		}
	}
}
