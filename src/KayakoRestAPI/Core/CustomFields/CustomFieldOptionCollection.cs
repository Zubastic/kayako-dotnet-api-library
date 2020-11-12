﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace KayakoRestApi.Core.CustomFields
{
    /// <summary>
    ///     Definition of a list of departments
    ///     <remarks>
    ///         See : http://wiki.kayako.com/display/DEV/REST+-+CustomField
    ///     </remarks>
    /// </summary>
    [XmlRoot("customfieldoptions")]
    public class CustomFieldOptionCollection : List<CustomFieldOption> { }
}