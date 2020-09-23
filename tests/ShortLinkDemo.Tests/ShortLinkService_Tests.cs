using ShortLinkDemo.Models;
using ShortLinkDemo.Models.View;
using ShortLinkDemo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ShortLinkDemo.Tests
{
    /// <summary>
    /// В данном наборе тестов используется тестовая реализация ShortLinkDummyRepository на основе списка List
    /// </summary>
    public class ShortLinkService_Tests
    {
        private readonly IShortLinkService _shortLinkService;

        public ShortLinkService_Tests()
        {
            ShortLinkDummyRepository repository = new ShortLinkDummyRepository();
            _shortLinkService = new ShortLinkService(repository);
        }

        /// <summary>
        /// Тест проверяет добавление новых объектов ShortLink и получение их с помощью GetAllShortLinks
        /// </summary>
        [Fact]
        public void Save_And_GetAllShortLinks_Test()
        {
            _shortLinkService.SaveShortLink(new ShortLink(), null);
            _shortLinkService.SaveShortLink(new ShortLink(), null);
            _shortLinkService.SaveShortLink(new ShortLink(), null);

            IEnumerable<ShortLinkModel> all = _shortLinkService.GetAllShortLinks(null);

            Assert.Equal(3, all.Count());
        }

        /// <summary>
        /// Тест проверяет добавление новых объектов ShortLink и получение одного из них с помощью GetShortLinkById
        /// </summary>
        [Fact]
        public void Save_And_GetShortLinkById_Test()
        {
            _shortLinkService.SaveShortLink(new ShortLink { Id = 1 }, null);
            _shortLinkService.SaveShortLink(new ShortLink { Id = 2 }, null);

            ShortLinkModel shortLinkModel = _shortLinkService.GetShortLinkById(1, null);
            Assert.NotNull(shortLinkModel);

            shortLinkModel = _shortLinkService.GetShortLinkById(3, null);
            Assert.Null(shortLinkModel);
        }

        /// <summary>
        /// Тест проверяет добавление новых объектов ShortLink и получение одного из них с помощью GetShortLinkByUrlKey
        /// </summary>
        [Fact]
        public void Save_And_GetShortLinkByUrlKey_Test()
        {
            string urlKey = Guid.NewGuid().ToString();
            string wrongKey = Guid.NewGuid().ToString();
            _shortLinkService.SaveShortLink(new ShortLink { UrlKey = urlKey }, null);

            ShortLinkModel shortLinkModel = _shortLinkService.GetShortLinkByUrlKey(urlKey, null);
            Assert.NotNull(shortLinkModel);

            shortLinkModel = _shortLinkService.GetShortLinkByUrlKey(wrongKey, null);
            Assert.Null(shortLinkModel);
        }

        /// <summary>
        /// Тест проверяет добавление нового объекта ShortLink и изменение его с помощью UpdateShortLink
        /// </summary>
        [Fact]
        public void Save_And_UpdateShortLink_Test()
        {
            string urlKey = Guid.NewGuid().ToString();
            long id = 1;

            ShortLink shortLink = new ShortLink { Id = id };
            _shortLinkService.SaveShortLink(shortLink, null);

            ShortLink shortLinkToUpdate = new ShortLink { Id = 1, UrlKey = urlKey };
            _shortLinkService.UpdateShortLink(shortLinkToUpdate, null);

            ShortLinkModel found = _shortLinkService.GetShortLinkById(id, null);
            Assert.NotNull(found);
            Assert.Equal(id, found.Id);
            Assert.Equal(urlKey, found.UrlKey);
        }

        /// <summary>
        /// Тест проверяет добавление нового объекта ShortLink и удаление его с помощью DeleteShortLink
        /// </summary>
        [Fact]
        public void Save_And_DeleteShortLink_Test()
        {
            const long id = 1;
            _shortLinkService.SaveShortLink(new ShortLink { Id = id }, null);
            bool success = _shortLinkService.DeleteShortLink(id);
            Assert.True(success);
        }
    }
}