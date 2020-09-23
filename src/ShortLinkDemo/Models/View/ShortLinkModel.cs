namespace ShortLinkDemo.Models.View
{
    public class ShortLinkModel : ShortLink
    {
        //private HttpRequest _request;
        private string _hostPrefix;

        public string ShortUrl =>
            string.Concat(_hostPrefix, "/", UrlKey);

        public static ShortLinkModel FromEntity(ShortLink model, string hostPrefix) =>
            new ShortLinkModel
            {
                _hostPrefix = hostPrefix,
                Id = model.Id,
                LongUrl = model.LongUrl,
                UrlKey = model.UrlKey,
                InsertionDate = model.InsertionDate,
                RedirectsCount = model.RedirectsCount
            };
    }
}