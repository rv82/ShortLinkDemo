using ShortLinkDemo.Data;
using ShortLinkDemo.Models;
using ShortLinkDemo.Models.View;
using System.Collections.Generic;
using System.Linq;

namespace ShortLinkDemo.Tests
{
    /// <summary>
    /// Тестовая реализация ShortLinkDummyRepository на основе списка List
    /// </summary>
    public class ShortLinkDummyRepository : IShortLinkRepository
    {
        private List<ShortLinkModel> _shortLinks;

        public ShortLinkDummyRepository()
        {
            _shortLinks = new List<ShortLinkModel>();
        }

        public bool DeleteShortLink(long linkId)
        {
            ShortLinkModel shortLink = GetShortLinkById(linkId, null);
            return _shortLinks.Remove(shortLink);
        }

        public IEnumerable<ShortLinkModel> GetAllShortLinks(string url)
        {
            return _shortLinks;
        }

        public ShortLinkModel GetShortLinkById(long linkId, string url)
        {
            return _shortLinks.FirstOrDefault(x => x.Id == linkId);
        }

        public ShortLinkModel GetShortLinkByUrlKey(string urlKey, string url)
        {
            return _shortLinks.FirstOrDefault(x => x.UrlKey == urlKey);
        }

        public ShortLinkModel SaveShortLink(ShortLink shortLink, string url)
        {
            ShortLinkModel shortLinkModel = ShortLinkModel.FromEntity(shortLink, url);
            _shortLinks.Add(shortLinkModel);
            return shortLinkModel;
        }

        public ShortLinkModel UpdateShortLink(ShortLink shortLink, string url)
        {
            bool success = DeleteShortLink(shortLink.Id);
            if (!success)
                return null;
            SaveShortLink(shortLink, url);
            return ShortLinkModel.FromEntity(shortLink, url);
        }
    }
}