﻿using System;
using System.Diagnostics;
using System.Globalization;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.Core.News;
using KayakoRestApi.Data;
using KayakoRestApi.IntegrationTests.TestBase;
using NUnit.Framework;

namespace KayakoRestApi.IntegrationTests.News
{
    [TestFixture]
    public class NewsItemTests : UnitTestBase
    {
        [Test]
        public void GetAllNewsItems()
        {
            var newsItems = TestSetup.KayakoApiService.News.GetNewsItems();

            Assert.IsNotNull(newsItems, "No news items were returned");
            Assert.IsNotEmpty(newsItems, "No news items were returned");
        }

        [TestCase(1)]
        public void GetNewsItemsByCategoryId(int categoryId)
        {
            var newsItems = TestSetup.KayakoApiService.News.GetNewsItems(categoryId);

            Assert.IsNotNull(newsItems, "No news items were returned");
            Assert.IsNotEmpty(newsItems, "No news items were returned");
        }

        [Test]
        public void GetNewsItemById()
        {
            var newsItems = TestSetup.KayakoApiService.News.GetNewsItems();

            Assert.IsNotNull(newsItems, "No news items were returned");
            Assert.IsNotEmpty(newsItems, "No news items were returned");

            var newsItemToGet = newsItems[new Random().Next(newsItems.Count)];
            Trace.WriteLine("GetNewsItem using news item id: " + newsItemToGet.Id);

            var newsItem = TestSetup.KayakoApiService.News.GetNewsItem(newsItemToGet.Id);

            AssertObjectXmlEqual(newsItem, newsItemToGet);
        }

        [Test]
        public void CreateUpdateDeleteNewsItem()
        {
            var newsItemRequest = new NewsItemRequest
            {
                Subject = "Subject",
                Contents = "Contents",
                StaffId = 1,
                NewsItemType = NewsItemType.Global,
                NewsItemStatus = NewsItemStatus.Draft,
                SendEmail = false,
                AllowComments = true,
                Expiry = new UnixDateTime(DateTime.Parse("31/12/2015 23:59:59", CultureInfo.CreateSpecificCulture("en-GB")))
            };

            var newsItem = TestSetup.KayakoApiService.News.CreateNewsItem(newsItemRequest);

            Assert.IsNotNull(newsItem);
            Assert.That(newsItem.Subject, Is.EqualTo(newsItemRequest.Subject));
            Assert.That(newsItem.Contents, Is.EqualTo(newsItemRequest.Contents));
            Assert.That(newsItem.StaffId, Is.EqualTo(newsItemRequest.StaffId));
            Assert.That(newsItem.NewsItemType, Is.EqualTo(newsItemRequest.NewsItemType));
            Assert.That(newsItem.NewsItemStatus, Is.EqualTo(newsItemRequest.NewsItemStatus));
            Assert.That(newsItem.AllowComments, Is.EqualTo(newsItemRequest.AllowComments));
            Assert.That(newsItem.Expiry.UnixTimeStamp, Is.EqualTo(newsItemRequest.Expiry.UnixTimeStamp));

            newsItemRequest = NewsItemRequest.FromResponseData(newsItem);
            newsItemRequest.Contents = "Contents Updated";
            newsItemRequest.Subject = "Subject Updated";

            newsItem = TestSetup.KayakoApiService.News.UpdateNewsItem(newsItemRequest);

            Assert.IsNotNull(newsItem);
            Assert.That(newsItem.Subject, Is.EqualTo(newsItemRequest.Subject));
            Assert.That(newsItem.Contents, Is.EqualTo(newsItemRequest.Contents));

            var deleteSuccess = TestSetup.KayakoApiService.News.DeleteNewsItem(newsItem.Id);

            Assert.IsTrue(deleteSuccess);
        }
    }
}