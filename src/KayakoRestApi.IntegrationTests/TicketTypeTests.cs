﻿using System;
using System.Diagnostics;
using KayakoRestApi.Core.Tickets.TicketType;
using KayakoRestApi.IntegrationTests.TestBase;
using NUnit.Framework;

namespace KayakoRestApi.IntegrationTests
{
    [TestFixture(Description = "A set of tests testing Api methods around Ticket Types")]
    public class TicketTypeTests : UnitTestBase
    {
        [Test]
        public void GetAllTicketTypes()
        {
            var ticketTypes = TestSetup.KayakoApiService.Tickets.GetTicketTypes();

            Assert.IsNotNull(ticketTypes, "No ticket types were returned");
            Assert.IsNotEmpty(ticketTypes, "No ticket types were returned");
        }

        [Test]
        public void GetTicketType()
        {
            var ticketTypes = TestSetup.KayakoApiService.Tickets.GetTicketTypes();

            Assert.IsNotNull(ticketTypes, "No ticket types were returned");
            Assert.IsNotEmpty(ticketTypes, "No ticket types were returned");

            var randomTicketTypeToGet = ticketTypes[new Random().Next(ticketTypes.Count)];

            Trace.WriteLine("GetTicketType using ticket type id: " + randomTicketTypeToGet.Id);

            var ticketType = TestSetup.KayakoApiService.Tickets.GetTicketType(randomTicketTypeToGet.Id);

            this.CompareTicketTypes(ticketType, randomTicketTypeToGet);
        }

        private void CompareTicketTypes(TicketType one, TicketType two)
        {
            Assert.AreEqual(one.DepartmentId, two.DepartmentId);
            Assert.AreEqual(one.DisplayIcon, two.DisplayIcon);
            Assert.AreEqual(one.DisplayOrder, two.DisplayOrder);
            Assert.AreEqual(one.Id, two.Id);
            Assert.AreEqual(one.Title, two.Title);
            Assert.AreEqual(one.Type, two.Type);
            Assert.AreEqual(one.UserGroupId, two.UserGroupId);
            Assert.AreEqual(one.UserVisibilityCustom, two.UserVisibilityCustom);

            AssertObjectXmlEqual(one, two);
        }
    }
}