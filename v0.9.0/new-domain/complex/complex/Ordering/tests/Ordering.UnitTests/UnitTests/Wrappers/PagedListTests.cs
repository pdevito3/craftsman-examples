namespace Ordering.UnitTests.UnitTests.Wrappers
{
    using Ordering.Core.Wrappers;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    public class PagedListTests
    {
        [Test]
        public void PagedList_Returns_Accurate_Data_For_Standard_Pagination()
        {
            var pageNumber = 2;
            var pageSize = 2;
            var source = new List<int>() { 1, 2, 3, 4, 5 }.AsQueryable();

            var list = PagedList<int>.Create(source, pageNumber, pageSize);
            list.TotalCount.Should().Be(5);
            list.PageSize.Should().Be(2);
            list.PageNumber.Should().Be(2);
            list.CurrentPageSize.Should().Be(2);
            list.CurrentStartIndex.Should().Be(3);
            list.CurrentEndIndex.Should().Be(4);
            list.TotalPages.Should().Be(3);
        }

        [Test]
        public void PagedList_Returns_Accurate_Data_With_Last_Record()
        {
            var pageNumber = 3;
            var pageSize = 2;
            var source = new List<int>() { 1, 2, 3, 4, 5 }.AsQueryable();

            var list = PagedList<int>.Create(source, pageNumber, pageSize);
            list.TotalCount.Should().Be(5);
            list.PageSize.Should().Be(2);
            list.PageNumber.Should().Be(3);
            list.CurrentPageSize.Should().Be(1);
            list.CurrentStartIndex.Should().Be(5);
            list.CurrentEndIndex.Should().Be(5);
            list.TotalPages.Should().Be(3);
        }
    }
}