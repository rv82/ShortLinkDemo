using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShortLinkDemo.Data;
using ShortLinkDemo.Models.View;
using ShortLinkDemo.Services;
using ShortLinkDemo.Utils;
using System;

namespace ShortLinkDemo.Controllers
{
    /// <summary>
    /// Контроллер для обработки коротких ссылок.
    /// </summary>
    [Route("link")]
    public class LinkController : ControllerBase
    {
        private readonly IShortLinkService _shortLinkService;
        private readonly ILogger<LinkController> _log;

        private string UrlPrefix =>
            new UriBuilder(Request.Scheme, Request.Host.Host, Request.Host.Port.Value, "l").ToString();

        /// <summary>
        /// Контроллер по умолчанию для инициализации служб - контекста БД и пр.
        /// </summary>
        /// <param name="context"></param>
        public LinkController(IShortLinkRepository repo, IShortLinkService shortLinkService, ILogger<LinkController> logger)
        {
            _shortLinkService = shortLinkService;
            _log = logger;
        }

        /// <summary>
        /// Действие-обработчик короткой ссылки. Производится поиск записи, хранящей реальную ссылку.
        /// Если запись имеется в БД, то счётчик количества переходов увеличивается на 1 и происходит
        /// перенаправление по реальной ссылке.
        /// </summary>
        /// <param name="urlKey">короткий псевдоним ссылки.</param>
        [HttpGet("/l/{urlKey}")]
        public IActionResult Get(string urlKey)
        {
            try
            {
                ShortLinkModel link = _shortLinkService.GetShortLinkByUrlKey(urlKey, UrlPrefix);
                if(link==null)
                {
                    _log.LogWarning(Constants.Messages.LinkNotFound);
                    return NotFound();
                }
                return Redirect(link.LongUrl);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return Problem(Constants.Messages.InternalServerError);
            }
        }

        /// <summary>
        /// Возвращает полный список ссылок
        /// </summary>
        [HttpGet("all")]
        [Produces("application/json")]
        public IActionResult Get()
        {
            try
            {
                return Ok(_shortLinkService.GetAllShortLinks(UrlPrefix));
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return Problem(Constants.Messages.InternalServerError);
            }
        }

        /// <summary>
        /// Возвращает конкретную ссылку.
        /// </summary>
        /// <param name="linkId">идентификатор ссылки.</param>
        /// <returns></returns>
        [HttpGet("{linkId}")]
        [Produces("application/json")]
        public IActionResult GetShortLink(long linkId)
        {
            try
            {
                ShortLinkModel shortLink = _shortLinkService.GetShortLinkById(linkId, UrlPrefix);

                if (shortLink == null)
                {
                    _log.LogError(Constants.Messages.LinkNotFound);
                    return NotFound();
                }

                return Ok(shortLink);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return Problem(Constants.Messages.InternalServerError);
            }
        }

        /// <summary>
        /// Сохраняет новую ссылку.
        /// </summary>
        /// <param name="shortLink">объект ShortLink, получаемый из http-запроса</param>
        [HttpPost]
        public IActionResult Save([FromBody] ShortLinkModel shortLink)
        {
            if (shortLink == null)
            {
                _log.LogError(Constants.Messages.CannotSaveLink);
                return Problem(Constants.Messages.CannotSaveLink);
            }

            if (string.IsNullOrWhiteSpace(shortLink.LongUrl.Trim()))
            {
                _log.LogError(Constants.Messages.LinkAddressIsEmpty);
                return Problem(title: Constants.Messages.LinkAddressIsEmpty);
            }

            try
            {
                var result = _shortLinkService.SaveShortLink(shortLink, UrlPrefix);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return Problem(Constants.Messages.CannotSaveLink);
            }
        }

        /// <summary>
        /// Обновляет объект ссылки.
        /// </summary>
        /// <param name="shortLink">объект ShortLink, получаемый из http-запроса.</param>
        [HttpPut]
        public IActionResult Update([FromBody] ShortLinkModel shortLink)
        {
            if (shortLink == null)
            {
                _log.LogError(Constants.Messages.CannotUpdateChanges);
                return Problem(Constants.Messages.CannotUpdateChanges);
            }

            if (string.IsNullOrWhiteSpace(shortLink.LongUrl.Trim()))
            {
                _log.LogError(Constants.Messages.LinkAddressIsEmpty);
                return Problem(Constants.Messages.LinkAddressIsEmpty);
            }

            try
            {
                if (string.IsNullOrWhiteSpace(shortLink.UrlKey))
                {
                    shortLink.UrlKey = StringUtils.GenRandomString(6);
                }
                var result = _shortLinkService.UpdateShortLink(shortLink, UrlPrefix);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return Problem(Constants.Messages.CannotUpdateChanges);
            }
        }

        /// <summary>
        /// Удаляет ссылку.
        /// </summary>
        /// <param name="linkId">идентификатор ссылки.</param>
        [HttpDelete("{linkId}")]
        public IActionResult Delete(long linkId)
        {
            try
            {
                bool success = _shortLinkService.DeleteShortLink(linkId);

                if (!success)
                {
                    _log.LogError(Constants.Messages.LinkNotFound);
                    return NotFound();
                }
                return Ok(linkId);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return Problem(Constants.Messages.CannotDeleteLink);
            }
        }
    }
}