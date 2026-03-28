using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Razor.Services;
using Razor.Services.Mapping;
using Razor.Services.Models;

namespace Razor.Tests
{
    [TestClass]
    public class CondoServiceTests
    {
        private class FakeMapper : IMapper
        {
            public TDestination Map<TSource, TDestination>(TSource source)
                where TSource : class
                where TDestination : class, new()
            {
                return new TDestination();
            }
        }

        [TestMethod]
        public void IsCondo_ReturnsFalse_ForZeroOrNegative()
        {
            var svc = new CondoService(new FakeMapper());
            Assert.IsFalse(svc.IsCondo(0));
            Assert.IsFalse(svc.IsCondo(-5));
        }

        [TestMethod]
        public void IsCondo_ReturnsTrue_ForPositive()
        {
            var svc = new CondoService(new FakeMapper());
            Assert.IsTrue(svc.IsCondo(1));
            Assert.IsTrue(svc.IsCondo(42));
        }
    }

    [TestClass]
    public class IndexPageTests
    {
        [TestMethod]
        public void OnGet_RedirectsTo_CondoList()
        {
            var model = new Razor.Web.Pages.IndexModel();
            var result = model.OnGet();
            Assert.IsNotNull(result);
            Assert.AreEqual("/condo/list", result.PageName);
        }
    }

    [TestClass]
    public class SimpleMapperTests
    {
        private class Source
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public DateTime Created { get; set; }
            public List<ChildSource> Children { get; set; } = new();
        }

        private class ChildSource
        {
            public string? Text { get; set; }
        }

        private class Dest
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public DateTime Created { get; set; }
            public ChildDest[]? Children { get; set; }
        }

        private class ChildDest
        {
            public string? Text { get; set; }
        }

        private class TestConverter : ITypeConverter
        {
            public bool CanConvert(Type sourceType, Type destinationType)
            {
                return sourceType == typeof(Source) && destinationType == typeof(Dest);
            }

            public object? Convert(object source, Type destinationType)
            {
                return new Dest { Id = 12345, Name = "converted" };
            }
        }

        [TestMethod]
        public void Map_CopiesSimpleProperties()
        {
            var mapper = new SimpleMapper();
            var src = new Source { Id = 7, Name = "abc", Created = new DateTime(2020, 1, 2) };
            var dest = mapper.Map<Source, Dest>(src);
            Assert.AreEqual(7, dest.Id);
            Assert.AreEqual("abc", dest.Name);
            Assert.AreEqual(new DateTime(2020, 1, 2), dest.Created);
        }

        [TestMethod]
        public void Map_MapsCollections_ToArray()
        {
            var mapper = new SimpleMapper();
            var src = new Source { Id = 1, Name = "x" };
            src.Children.Add(new ChildSource { Text = "c1" });
            src.Children.Add(new ChildSource { Text = "c2" });

            var dest = mapper.Map<Source, Dest>(src);
            Assert.IsNotNull(dest.Children);
            Assert.AreEqual(2, dest.Children!.Length);
            Assert.AreEqual("c1", dest.Children[0].Text);
            Assert.AreEqual("c2", dest.Children[1].Text);
        }

        [TestMethod]
        public void Map_Throws_OnNullSource()
        {
            var mapper = new SimpleMapper();
            try
            {
                mapper.Map<Source, Dest>(null!);
                Assert.Fail("Expected ArgumentNullException");
            }
            catch (ArgumentNullException)
            {
                // expected
            }
        }

        [TestMethod]
        public void WithConverters_UsesConverter_WhenAvailable()
        {
            var mapper = new SimpleMapper().WithConverters(new TestConverter());
            var src = new Source { Id = 9, Name = "orig" };
            var dest = mapper.Map<Source, Dest>(src);
            Assert.AreEqual(12345, dest.Id);
            Assert.AreEqual("converted", dest.Name);
        }
    }
}
