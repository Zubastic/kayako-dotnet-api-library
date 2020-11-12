﻿using System.Collections.Generic;
using System.Linq;
using KayakoRestApi.Controllers;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.Core.Tickets.Ticket;
using KayakoRestApi.Core.Tickets.TicketCustomField;
using KayakoRestApi.Net;
using KayakoRestApi.UnitTests.Utilities;
using Moq;
using NUnit.Framework;

namespace KayakoRestApi.UnitTests.Tickets
{
    [TestFixture]
    public class TicketControllerTests
    {
        [SetUp]
        public void Setup()
        {
            this.kayakoApiRequest = new Mock<IKayakoApiRequest>();

            this.ticketController = new TicketController(this.kayakoApiRequest.Object);

            this.responseTicketCollection = new TicketCollection
            {
                new Ticket()
            };

            this.createTicketRequestRequiredFields = new TicketRequest
            {
                Subject = "Subject",
                FullName = "Fullname",
                Email = "email@email.com",
                Contents = "Contents",
                DepartmentId = 1,
                TicketStatusId = 2,
                TicketPriorityId = 3,
                TicketTypeId = 4
            };

            this.createTicketRequiredFieldsParameters = "subject=Subject&fullname=Fullname&email=email@email.com&contents=Contents&departmentid=1&ticketstatusid=2&ticketpriorityid=3&tickettypeid=4";

            this.responseTicketCustomFields = new TicketCustomFields
            {
                FieldGroups = new List<TicketCustomFieldGroup>
                {
                    new TicketCustomFieldGroup
                    {
                        Id = 1,
                        Title = "Title",
                        Fields = new[]
                        {
                            new TicketCustomField
                            {
                                Type = TicketCustomFieldType.Text,
                                Name = "FieldName1",
                                FieldContent = "content1"
                            },
                            new TicketCustomField
                            {
                                Type = TicketCustomFieldType.Text,
                                Name = "FieldName2",
                                FieldContent = "content2"
                            }
                        }
                    }
                }
            };
        }

        private ITicketController ticketController;
        private Mock<IKayakoApiRequest> kayakoApiRequest;

        private TicketCollection responseTicketCollection;
        private TicketRequest createTicketRequestRequiredFields;
        private string createTicketRequiredFieldsParameters;
        private TicketCustomFields responseTicketCustomFields;

        [Test]
        public void UpdateTicket()
        {
            var ticketRequest = new TicketRequest
            {
                Id = 39,
                Subject = "Subject",
                FullName = "Fullname",
                Email = "email@email.com",
                DepartmentId = 1,
                TicketStatusId = 2,
                TicketPriorityId = 3,
                TicketTypeId = 4,
                OwnerStaffId = 5,
                UserId = 6,
                TemplateGroupId = 7
            };

            var parameters = "subject=Subject&fullname=Fullname&email=email@email.com&departmentid=1&ticketstatusid=2&ticketpriorityid=3&tickettypeid=4&ownerstaffid=5&userid=6&templategroup=7";

            this.UpdateTicketRequest(parameters, ticketRequest);
        }

        [Test]
        public void UpdateTicket_TemplateGroupId()
        {
            var ticketRequest = new TicketRequest
            {
                Id = 39,
                TemplateGroupId = 1
            };

            const string parameters = "templategroup=1";

            this.UpdateTicketRequest(parameters, ticketRequest);
        }

        [Test]
        public void UpdateTicket_TemplateGroupName()
        {
            var ticketRequest = new TicketRequest
            {
                Id = 39,
                TemplateGroupName = "templatename"
            };

            const string parameters = "templategroup=templatename";

            this.UpdateTicketRequest(parameters, ticketRequest);
        }

        private void UpdateTicketRequest(string parameters, TicketRequest ticketRequest)
        {
            var apiMethod = string.Format("/Tickets/Ticket/{0}", ticketRequest.Id);

            this.kayakoApiRequest.Setup(x => x.ExecutePut<TicketCollection>(apiMethod, parameters)).Returns(this.responseTicketCollection);

            var ticket = this.ticketController.UpdateTicket(ticketRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePut<TicketCollection>(apiMethod, parameters), Times.Once());

            Assert.That(ticket, Is.EqualTo(this.responseTicketCollection.FirstOrDefault()));
        }

        [Test]
        public void CreateTicket()
        {
            this.createTicketRequestRequiredFields.AutoUserId = true;

            var parameters = string.Format("{0}&autouserid=1", this.createTicketRequiredFieldsParameters);

            this.CreateTicketRequest(parameters, this.createTicketRequestRequiredFields);
        }

        [Test]
        public void CreateTicket_TemplateGroupId()
        {
            this.createTicketRequestRequiredFields.TemplateGroupId = 1;

            var parameters = string.Format("{0}&templategroup=1", this.createTicketRequiredFieldsParameters);

            this.CreateTicketRequest(parameters, this.createTicketRequestRequiredFields);
        }

        [Test]
        public void CreateTicket_TemplateGroupName()
        {
            this.createTicketRequestRequiredFields.TemplateGroupName = "templatename";

            var parameters = string.Format("{0}&templategroup=templatename", this.createTicketRequiredFieldsParameters);

            this.CreateTicketRequest(parameters, this.createTicketRequestRequiredFields);
        }

        private void CreateTicketRequest(string parameters, TicketRequest ticketRequest)
        {
            const string apiMethod = "/Tickets/Ticket";

            this.kayakoApiRequest.Setup(x => x.ExecutePost<TicketCollection>(apiMethod, parameters)).Returns(this.responseTicketCollection);

            var ticket = this.ticketController.CreateTicket(ticketRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePost<TicketCollection>(apiMethod, parameters), Times.Once());

            Assert.That(ticket, Is.EqualTo(this.responseTicketCollection.FirstOrDefault()));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetTicketCustomFields(int ticketId)
        {
            var apiMethod = string.Format("/Tickets/TicketCustomField/{0}", ticketId);

            this.kayakoApiRequest.Setup(x => x.ExecuteGet<TicketCustomFields>(apiMethod)).Returns(this.responseTicketCustomFields);

            var ticketCustomFields = this.ticketController.GetTicketCustomFields(ticketId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<TicketCustomFields>(apiMethod), Times.Once());
            AssertUtility.ObjectsEqual(ticketCustomFields, this.responseTicketCustomFields);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void UpdateTicketCustomField(int ticketId)
        {
            var apiMethod = string.Format("/Tickets/TicketCustomField/{0}", ticketId);
            const string parameters = "FieldName1=content1&FieldName2=content2";

            this.kayakoApiRequest.Setup(x => x.ExecutePost<TicketCustomFields>(apiMethod, parameters)).Returns(this.responseTicketCustomFields);

            var ticketCustomFields = this.ticketController.UpdateTicketCustomFields(ticketId, this.responseTicketCustomFields);

            this.kayakoApiRequest.Verify(x => x.ExecutePost<TicketCustomFields>(apiMethod, parameters), Times.Once());
            AssertUtility.ObjectsEqual(ticketCustomFields, this.responseTicketCustomFields);
        }
    }
}