﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace KayakoRestApi.Core.Staff
{
    /// <summary>
    ///     Definition of a list of staff groups.
    ///     <remarks>
    ///         See : http://wiki.kayako.com/display/DEV/REST+-+StaffGroup
    ///     </remarks>
    /// </summary>
    [XmlRoot("staffgroups")]
    public class StaffGroupCollection : List<StaffGroup> { }
}