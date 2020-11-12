﻿using System;
using System.Net;
using KayakoRestApi.Core.News;
using KayakoRestApi.Data;
using KayakoRestApi.Net;
using KayakoRestApi.RequestBase;
using KayakoRestApi.Text;

namespace KayakoRestApi.Controllers
{
	public interface INewsController
	{
		NewsCategoryCollection GetNewsCategories();

		NewsCategory GetNewsCategory(int newsCategoryId);

		NewsCategory CreateNewsCategory(NewsCategoryRequest newsCategoryRequest);

		NewsCategory UpdateNewsCategory(NewsCategoryRequest newsCategoryRequest);

		bool DeleteNewsCategory(int newsCategoryId);

		NewsItemCollection GetNewsItems(int newsCategoryId);

		NewsItemCollection GetNewsItems();

		NewsItem GetNewsItem(int newsItemId);

		NewsItem CreateNewsItem(NewsItemRequest newsItemRequest);

		NewsItem UpdateNewsItem(NewsItemRequest newsItemRequest);

		bool DeleteNewsItem(int newsItemId);

		NewsSubscriberCollection GetNewsSubscribers();

		NewsSubscriber GetNewsSubscriber(int newsSubscriberId);

		NewsSubscriber CreateNewsSubscriber(NewsSubscriberRequest newsSubscriberRequest);

		NewsSubscriber UpdateNewsSubscriber(NewsSubscriberRequest newsSubscriberRequest);

		bool DeleteNewsSubscriber(int newsSubscriberId);

		NewsItemCommentCollection GetNewsItemComments(int newsItemId);
		
		NewsItemComment GetNewsItemComment(int newsItemCommentId);

		NewsItemComment CreateNewsItemComment(NewsItemCommentRequest newsItemCommentRequest);

		bool DeleteNewsItemComment(int newsItemCommentId);
	}

	public sealed class NewsController : BaseController, INewsController
	{
        public NewsController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy)
            : base(apiKey, secretKey, apiUrl, proxy)
        {
        }

        public NewsController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy, ApiRequestType requestType)
			: base(apiKey, secretKey, apiUrl, proxy, requestType)
		{
		}

        public NewsController(IKayakoApiRequest kayakoApiRequest) 
			: base(kayakoApiRequest)
		{
		}

		private const string NewsCategoryBaseUrl = "/News/Category";
		private const string NewsItemBaseUrl = "/News/NewsItem";
		private const string NewsSubscriberBaseUrl = "/News/Subscriber";
		private const string NewsItemCommentBaseUrl = "/News/Comment";

		#region News Category Methods

		public NewsCategoryCollection GetNewsCategories()
		{
			return Connector.ExecuteGet<NewsCategoryCollection>(NewsCategoryBaseUrl);
		}

		public NewsCategory GetNewsCategory(int newsCategoryId)
		{
			string apiMethod = String.Format("{0}/{1}", NewsCategoryBaseUrl, newsCategoryId);

			NewsCategoryCollection newsCategories = Connector.ExecuteGet<NewsCategoryCollection>(apiMethod);

			if (newsCategories != null && newsCategories.Count > 0)
			{
				return newsCategories[0];
			}

			return null;
		}

		public NewsCategory CreateNewsCategory(NewsCategoryRequest newsCategoryRequest)
		{
			RequestBodyBuilder parameters = PopulateRequestParameters(newsCategoryRequest, RequestTypes.Create);

			NewsCategoryCollection newsCategories = Connector.ExecutePost<NewsCategoryCollection>(NewsCategoryBaseUrl, parameters.ToString());

			if (newsCategories != null && newsCategories.Count > 0)
			{
				return newsCategories[0];
			}

			return null;
		}

		public NewsCategory UpdateNewsCategory(NewsCategoryRequest newsCategoryRequest)
		{
			string apiMethod = String.Format("{0}/{1}", NewsCategoryBaseUrl, newsCategoryRequest.Id);
			RequestBodyBuilder parameters = PopulateRequestParameters(newsCategoryRequest, RequestTypes.Update);

			NewsCategoryCollection newsCategories = Connector.ExecutePut<NewsCategoryCollection>(apiMethod, parameters.ToString());

			if (newsCategories != null && newsCategories.Count > 0)
			{
				return newsCategories[0];
			}

			return null;
		}

		public bool DeleteNewsCategory(int newsCategoryId)
		{
			string apiMethod = String.Format("{0}/{1}", NewsCategoryBaseUrl, newsCategoryId);

			return Connector.ExecuteDelete(apiMethod);
		}

		private static RequestBodyBuilder PopulateRequestParameters(NewsCategoryRequest newsCategory, RequestTypes requestType)
		{
			newsCategory.EnsureValidData(requestType);

			RequestBodyBuilder parameters = new RequestBodyBuilder();

			if (!String.IsNullOrEmpty(newsCategory.Title))
			{
				parameters.AppendRequestData("title", newsCategory.Title);
			}

			parameters.AppendRequestData("visibilitytype", EnumUtility.ToApiString(newsCategory.VisibilityType));

			return parameters;
		}

		#endregion

		#region News Item Methods

		public NewsItemCollection GetNewsItems(int newsCategoryId)
		{
			string apiMethod = String.Format("{0}/ListAll/{1}", NewsItemBaseUrl, newsCategoryId);

			return Connector.ExecuteGet<NewsItemCollection>(apiMethod);
		}

		public NewsItemCollection GetNewsItems()
		{
			return Connector.ExecuteGet<NewsItemCollection>(NewsItemBaseUrl);
		}

		public NewsItem GetNewsItem(int newsItemId)
		{
			string apiMethod = String.Format("{0}/{1}", NewsItemBaseUrl, newsItemId);

			var newsItems = Connector.ExecuteGet<NewsItemCollection>(apiMethod);

			if (newsItems != null && newsItems.Count > 0)
			{
				return newsItems[0];
			}

			return null;
		}

		public NewsItem CreateNewsItem(NewsItemRequest newsItemRequest)
		{
			RequestBodyBuilder parameters = PopulateRequestParameters(newsItemRequest, RequestTypes.Create);

			NewsItemCollection newsItems = Connector.ExecutePost<NewsItemCollection>(NewsItemBaseUrl, parameters.ToString());

			if (newsItems != null && newsItems.Count > 0)
			{
				return newsItems[0];
			}

			return null;
		}

		public NewsItem UpdateNewsItem(NewsItemRequest newsItemRequest)
		{
			string apiMethod = string.Format("{0}/{1}", NewsItemBaseUrl, newsItemRequest.Id);

			RequestBodyBuilder parameters = PopulateRequestParameters(newsItemRequest, RequestTypes.Update);

			NewsItemCollection newsItems = Connector.ExecutePut<NewsItemCollection>(apiMethod, parameters.ToString());

			if (newsItems != null && newsItems.Count > 0)
			{
				return newsItems[0];
			}

			return null;
		}

		public bool DeleteNewsItem(int newsItemId)
		{
			string apiMethod = string.Format("{0}/{1}", NewsItemBaseUrl, newsItemId);

			return Connector.ExecuteDelete(apiMethod);
		}

		private static RequestBodyBuilder PopulateRequestParameters(NewsItemRequest newsItem, RequestTypes requestType)
		{
			newsItem.EnsureValidData(requestType);

			RequestBodyBuilder parameters = new RequestBodyBuilder();
			parameters.AppendRequestDataNonEmptyString("subject", newsItem.Subject);
			parameters.AppendRequestDataNonEmptyString("contents", newsItem.Contents);

			if (requestType == RequestTypes.Create)
			{
				parameters.AppendRequestDataNonNegativeInt("staffid", newsItem.StaffId);
			}
			else
			{
				parameters.AppendRequestDataNonNegativeInt("editedstaffid", newsItem.StaffId);
			}

			if (requestType == RequestTypes.Create && newsItem.NewsItemType.HasValue)
			{
				parameters.AppendRequestData("newstype", EnumUtility.ToApiString(newsItem.NewsItemType));
			}

			if (newsItem.NewsItemStatus.HasValue)
			{
				parameters.AppendRequestData("newsstatus", EnumUtility.ToApiString(newsItem.NewsItemStatus));
			}

			parameters.AppendRequestDataNonEmptyString("fromname", newsItem.FromName);
			parameters.AppendRequestDataNonEmptyString("email", newsItem.Email);
			parameters.AppendRequestDataNonEmptyString("customemailsubject", newsItem.CustomEmailSubject);
			parameters.AppendRequestDataBool("sendemail", newsItem.SendEmail);
			parameters.AppendRequestDataBool("allowcomments", newsItem.AllowComments);
			parameters.AppendRequestDataBool("uservisibilitycustom", newsItem.UserVisibilityCustom);
			parameters.AppendRequestDataArrayCommaSeparated("usergroupidlist", newsItem.UserGroupIdList);
			parameters.AppendRequestDataBool("staffvisibilitycustom", newsItem.StaffVisibilityCustom);
			parameters.AppendRequestDataArrayCommaSeparated("staffgroupidlist", newsItem.StaffGroupIdList);
			parameters.AppendRequestData("expiry", newsItem.Expiry.DateTime.ToString("M/d/yyyy"));
			parameters.AppendRequestDataArrayCommaSeparated("newscategoryidlist", newsItem.Categories);
			
			return parameters;
		}

		#endregion

		#region News Subscriber Methods

		public NewsSubscriberCollection GetNewsSubscribers()
		{
			return Connector.ExecuteGet<NewsSubscriberCollection>(NewsSubscriberBaseUrl);
		}

		public NewsSubscriber GetNewsSubscriber(int newsSubscriberId)
		{
			string apiMethod = String.Format("{0}/{1}", NewsSubscriberBaseUrl, newsSubscriberId);

			var newsSubscribers = Connector.ExecuteGet<NewsSubscriberCollection>(apiMethod);

			if (newsSubscribers != null && newsSubscribers.Count > 0)
			{
				return newsSubscribers[0];
			}

			return null;
		}

		public NewsSubscriber CreateNewsSubscriber(NewsSubscriberRequest newsSubscriberRequest)
		{
			RequestBodyBuilder parameters = PopulateRequestParameters(newsSubscriberRequest, RequestTypes.Create);

			NewsSubscriberCollection newsSubscriber = Connector.ExecutePost<NewsSubscriberCollection>(NewsSubscriberBaseUrl, parameters.ToString());

			if (newsSubscriber != null && newsSubscriber.Count > 0)
			{
				return newsSubscriber[0];
			}

			return null;
		}

		public NewsSubscriber UpdateNewsSubscriber(NewsSubscriberRequest newsSubscriberRequest)
		{
			string apiMethod = string.Format("{0}/{1}", NewsSubscriberBaseUrl, newsSubscriberRequest.Id);

			RequestBodyBuilder parameters = PopulateRequestParameters(newsSubscriberRequest, RequestTypes.Update);

			NewsSubscriberCollection newsSubscriber = Connector.ExecutePut<NewsSubscriberCollection>(apiMethod, parameters.ToString());

			if (newsSubscriber != null && newsSubscriber.Count > 0)
			{
				return newsSubscriber[0];
			}

			return null;
		}

		public bool DeleteNewsSubscriber(int newsSubscriberId)
		{
			string apiMethod = string.Format("{0}/{1}", NewsSubscriberBaseUrl, newsSubscriberId);

			return Connector.ExecuteDelete(apiMethod);
		}

		private RequestBodyBuilder PopulateRequestParameters(NewsSubscriberRequest newsSubscriberRequest, RequestTypes requestTypes)
		{
			newsSubscriberRequest.EnsureValidData(requestTypes);

			var requestBodyBuilder = new RequestBodyBuilder();
			requestBodyBuilder.AppendRequestDataNonEmptyString("email", newsSubscriberRequest.Email);

			if (requestTypes == RequestTypes.Create)
			{
				requestBodyBuilder.AppendRequestDataBool("isvalidated", newsSubscriberRequest.IsValidated);
			}

			return requestBodyBuilder;
		}

		#endregion

		#region News Item Comment Methods

		public NewsItemCommentCollection GetNewsItemComments(int newsItemId)
		{
			string apiMethod = string.Format("{0}/ListAll/{1}", NewsItemCommentBaseUrl, newsItemId);

			return Connector.ExecuteGet<NewsItemCommentCollection>(apiMethod);
		}

		public NewsItemComment GetNewsItemComment(int newsItemCommentId)
		{
			string apiMethod = string.Format("{0}/{1}", NewsItemCommentBaseUrl, newsItemCommentId);

			var newsItemComments = Connector.ExecuteGet<NewsItemCommentCollection>(apiMethod);

			if (newsItemComments != null && newsItemComments.Count > 0)
			{
				return newsItemComments[0];
			}

			return null;
		}

		public NewsItemComment CreateNewsItemComment(NewsItemCommentRequest newsItemCommentRequest)
		{
			newsItemCommentRequest.EnsureValidData(RequestTypes.Create);

			RequestBodyBuilder parameters = new RequestBodyBuilder();
			parameters.AppendRequestData("newsitemid", newsItemCommentRequest.NewsItemId);
			parameters.AppendRequestDataNonEmptyString("contents", newsItemCommentRequest.Contents);
			parameters.AppendRequestData("creatortype", EnumUtility.ToApiString(newsItemCommentRequest.CreatorType));

			if (newsItemCommentRequest.CreatorId != null)
			{
				parameters.AppendRequestData("creatorid", newsItemCommentRequest.CreatorId);
			}
			else
			{
				parameters.AppendRequestDataNonEmptyString("fullname", newsItemCommentRequest.FullName);
			}

			parameters.AppendRequestDataNonEmptyString("email", newsItemCommentRequest.Email);
			parameters.AppendRequestData("parentcommentid", newsItemCommentRequest.ParentCommentId);

			var newsItemComments = Connector.ExecutePost<NewsItemCommentCollection>(NewsItemCommentBaseUrl, parameters.ToString());

			if (newsItemComments != null && newsItemComments.Count > 0)
			{
				return newsItemComments[0];
			}

			return null;
		}

		public bool DeleteNewsItemComment(int newsItemCommentId)
		{
			string apiMethod = string.Format("{0}/{1}", NewsItemCommentBaseUrl, newsItemCommentId);

			return Connector.ExecuteDelete(apiMethod);
		}

		#endregion
	}
}
