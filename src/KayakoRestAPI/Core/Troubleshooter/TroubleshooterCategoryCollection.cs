﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace KayakoRestApi.Core.Troubleshooter
{
    /// <summary>
    ///     Represents a list of troubleshooter categories within the helpdesk
    ///     <remarks>
    ///         see: http://wiki.kayako.com/display/DEV/REST+-+TroubleshooterCategory#REST-TroubleshooterCategory-Response
    ///     </remarks>
    /// </summary>
    [XmlRoot("troubleshootercategories")]
    public class TroubleshooterCategoryCollection : List<TroubleshooterCategory> { }
}