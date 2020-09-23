using ShortLinkDemo.Models;
using ShortLinkDemo.Models.View;
using System.Collections.Generic;

namespace ShortLinkDemo.Data
{
    public interface IShortLinkRepository
    {
        /// <summary>
        /// Удаляет ссылку.
        /// </summary>
        /// <param name="linkId">идентификатор ссылки.</param>
        bool DeleteShortLink(long linkId);

        /// <summary>
        /// Возвращает полный список ссылок
        /// </summary>
        /// <param name="url">используется для форматирования реального адреса</param>
        IEnumerable<ShortLinkModel> GetAllShortLinks(string url);

        /// <summary>
        /// Возвращает конкретную ссылку.
        /// Если запись имеется в БД, то счётчик количества переходов увеличивается на 1.
        /// </summary>
        /// <param name="linkId">идентификатор ссылки.</param>
        /// <param name="url">используется для форматирования реального адреса</param>
        ShortLinkModel GetShortLinkById(long linkId, string url);

        /// <summary>
        /// Возвращает конкретную ссылку.
        /// </summary>
        /// <param name="urlKey"></param>
        /// <param name="url">используется для форматирования реального адреса</param>
        ShortLinkModel GetShortLinkByUrlKey(string urlKey, string url);

        /// <summary>
        /// Сохраняет новую ссылку.
        /// </summary>
        /// <param name="shortLink">объект ShortLink, получаемый из http-запроса</param>
        /// <param name="url">используется для форматирования реального адреса</param>
        ShortLinkModel SaveShortLink(ShortLink shortLink, string url);

        /// <summary>
        /// Обновляет объект ссылки.
        /// </summary>
        /// <param name="shortLink">объект ShortLink, получаемый из http-запроса.</param>
        /// <param name="url">используется для форматирования реального адреса</param>
        ShortLinkModel UpdateShortLink(ShortLink shortLink, string url);
    }
}