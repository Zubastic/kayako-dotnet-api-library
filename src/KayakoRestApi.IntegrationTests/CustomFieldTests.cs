﻿using System;
using KayakoRestApi.IntegrationTests.TestBase;
using NUnit.Framework;

namespace KayakoRestApi.IntegrationTests
{
    [TestFixture(Description = "A set of tests testing Api methods around Custom Fields")]
    public class CustomFieldTests : UnitTestBase
    {
        [Test]
        public void GetCustomFields()
        {
            var customFields = TestSetup.KayakoApiService.CustomFields.GetCustomFields();

            Assert.IsNotNull(customFields, "No custom fields were returned");
            Assert.IsNotEmpty(customFields, "No custom fields were returned");
        }

        [Test]
        public void GetCustomFieldOptions()
        {
            var customFields = TestSetup.KayakoApiService.CustomFields.GetCustomFields();

            Assert.IsNotNull(customFields, "No custom fields were returned");
            Assert.IsNotEmpty(customFields, "No custom fields were returned");

            var idToUse = -1;
            foreach (var customField in customFields)
            {
                var customFieldOptions = TestSetup.KayakoApiService.CustomFields.GetCustomFieldOptions(customField.CustomFieldId);
                if (customFieldOptions.Count > 0)
                {
                    idToUse = customField.CustomFieldId;
                    break;
                }
            }

            if (idToUse != -1)
            {
                var customFieldOptions = TestSetup.KayakoApiService.CustomFields.GetCustomFieldOptions(idToUse);

                Assert.IsNotNull(customFieldOptions, "No custom fields were returned");
                Assert.IsNotEmpty(customFieldOptions, "No custom fields were returned");
            }
            else
            {
                throw new Exception("No custom field options found");
            }
        }
    }
}