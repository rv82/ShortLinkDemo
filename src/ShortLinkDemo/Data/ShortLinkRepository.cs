using ShortLinkDemo.Models;
using ShortLinkDemo.Models.View;
using ShortLinkDemo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShortLinkDemo.Data
{
    public class ShortLinkRepository : IShortLinkRepository
    {
        private readonly ShortLinkContext _context;

        public ShortLinkRepository(ShortLinkContext context)
        {
            _context = context;
        }

        public ShortLinkModel GetShortLinkByUrlKey(string urlKey, string url)
        {
            ShortLink shortLink = _context.ShortLinks.FirstOrDefault(l => l.UrlKey == urlKey);
            return shortLink == null ? null : ShortLinkModel.FromEntity(shortLink, url);
        }

        public ShortLinkModel UpdateShortLink(ShortLink shortLink, string url)
        {
            ShortLink result = _context.ShortLinks.FirstOrDefault(x => x.Id == shortLink.Id);
            if (result == null)
            {
                return null;
            }
            result.LongUrl = shortLink.LongUrl;
            result.UrlKey = shortLink.UrlKey;
            result.RedirectsCount = shortLink.RedirectsCount;
            _context.SaveChanges();
            return ShortLinkModel.FromEntity(result, url);
        }

        public IEnumerable<ShortLinkModel> GetAllShortLinks(string url)
        {
            return _context.ShortLinks.Select(m => ShortLinkModel.FromEntity(m, url));
        }

        public ShortLinkModel GetShortLinkById(long linkId, string url)
        {
            ShortLink shortLink = _context.ShortLinks.FirstOrDefault(link => link.Id == linkId);
            return ShortLinkModel.FromEntity(shortLink, url);
        }

        public ShortLinkModel SaveShortLink(ShortLink shortLink, string url)
        {
            shortLink.Id = 0;
            shortLink.InsertionDate = DateTimeOffset.Now;
            if (string.IsNullOrWhiteSpace(shortLink.UrlKey))
            {
                shortLink.UrlKey = StringUtils.GenRandomString(6);
            }
            var result = _context.ShortLinks.Add(shortLink);
            _context.SaveChanges();
            return ShortLinkModel.FromEntity(result.Entity, url);
        }

        /// <summary>
        /// Удаляет ссылку из БД.
        /// </summary>
        /// <returns>true, если ссылка удалена и false, если такой ссылки нет в БД.</returns>
        public bool DeleteShortLink(long linkId)
        {
            ShortLink shortLink = _context.ShortLinks.FirstOrDefault(link => link.Id == linkId);

            if (shortLink == null)
            {
                return false;
            }

            _context.ShortLinks.Remove(shortLink);
            _context.SaveChanges();
            return true;
        }
    }
}