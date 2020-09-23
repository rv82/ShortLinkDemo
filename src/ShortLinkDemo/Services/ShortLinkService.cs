using ShortLinkDemo.Data;
using ShortLinkDemo.Models;
using ShortLinkDemo.Models.View;
using System.Collections.Generic;

namespace ShortLinkDemo.Services
{
    public class ShortLinkService : IShortLinkService
    {
        private readonly IShortLinkRepository _repo;

        public ShortLinkService(IShortLinkRepository repo)
        {
            _repo = repo;
        }

        public bool DeleteShortLink(long linkId)
        {
            bool success = _repo.DeleteShortLink(linkId);
            return success;
        }

        public IEnumerable<ShortLinkModel> GetAllShortLinks(string url)
        {
            IEnumerable<ShortLinkModel> shortLinks = _repo.GetAllShortLinks(url);
            return shortLinks;
        }

        public ShortLinkModel GetShortLinkById(long linkId, string url)
        {
            ShortLinkModel shortLink = _repo.GetShortLinkById(linkId, url);
            return shortLink;
        }

        public ShortLinkModel GetShortLinkByUrlKey(string urlKey, string url)
        {
            ShortLinkModel link = _repo.GetShortLinkByUrlKey(urlKey, url);
            if (link == null)
            {
                return link;
            }

            link.RedirectsCount++;
            ShortLinkModel result = _repo.UpdateShortLink(link, url);
            if (result == null)
            {
                return null;
            }

            return link;
        }

        public ShortLinkModel SaveShortLink(ShortLink shortLink, string url)
        {
            ShortLinkModel result = _repo.SaveShortLink(shortLink, url);
            return result;
        }

        public ShortLinkModel UpdateShortLink(ShortLink shortLink, string url)
        {
            ShortLinkModel result = _repo.UpdateShortLink(shortLink, url);
            return result;
        }
    }
}