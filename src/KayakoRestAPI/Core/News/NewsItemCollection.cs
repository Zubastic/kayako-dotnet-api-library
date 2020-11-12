﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace KayakoRestApi.Core.News
{
    /// <summary>
    ///     Represents a list of news items within the helpdesk
    ///     <remarks>
    ///         see: http://wiki.kayako.com/display/DEV/REST+-+NewsItem#REST-NewsItem-Response
    ///     </remarks>
    /// </summary>
    [XmlRoot("newsitems")]
    public class NewsItemCollection : List<NewsItem> { }
}