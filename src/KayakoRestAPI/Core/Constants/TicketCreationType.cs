﻿using System.Xml.Serialization;

namespace KayakoRestApi.Core.Constants
{
    public enum TicketCreationType
    {
        [XmlEnum("1")]
        Default,

        [XmlEnum("2")]
        Phone
    }
}