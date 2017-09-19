using Xunit;
using Rabbit.WeiXin.MP.Api.Material;
using Rabbit.WeiXin.Tests.Utility;
using System;
using Newtonsoft.Json;

namespace Rabbit.WeiXin.Tests
{
    
    public class ForeverMaterialTest : ApiTestBase
    {
        #region Field

        private readonly IForeverMaterialService _materialService;

        #endregion Field

        #region Constructor

        public ForeverMaterialTest()
        {
            _materialService = new ForeverMaterialService(AccountModel);
        }

        #endregion Constructor

        #region Test Method

        [Fact]
        public void AddThumbnailsTest()
        {
            var result = _materialService.AddOtherThumbnails(ApiTestHelper.GetJpgBytes());
            Assert.NotNull(result.MediaId);
            Assert.True(Uri.IsWellFormedUriString(result.Url, UriKind.Absolute));
        }

        [Fact]
        public void AddJpgTest()
        {
            var result = _materialService.AddOther(ApiTestHelper.GetJpgBytes());
            Assert.NotNull(result.MediaId);
            Assert.True(Uri.IsWellFormedUriString(result.Url, UriKind.Absolute));
        }

        [Fact]
        public void AddMp4Test()
        {
            var result = _materialService.AddVoide(ApiTestHelper.GetMp4Bytes(), new AddVoideMaterialModel
            {
                Title = "视频标题",
                Description = "视频说明"
            });
            Assert.NotNull(result.MediaId);
            //            Assert.True(Uri.IsWellFormedUriString(result.Url, UriKind.Absolute));
        }

        [Fact]
        public void AddAmrTest()
        {
            var result = _materialService.AddOther(ApiTestHelper.GetAmrBytes());
            Assert.NotNull(result.MediaId);
            //            Assert.True(Uri.IsWellFormedUriString(result.Url, UriKind.Absolute));
        }

        [Fact]
        public void AddMp3Test()
        {
            var result = _materialService.AddOther(ApiTestHelper.GetMp3Bytes());
            Assert.NotNull(result.MediaId);
            //            Assert.True(Uri.IsWellFormedUriString(result.Url, UriKind.Absolute));
        }

        [Fact]
        public void AddNewsTest()
        {
            var result = _materialService.AddOther(ApiTestHelper.GetJpgBytes());
            _materialService.AddNews(new[]
            {
                new NewsArticleModel
                {
                    Author="author",
                    Content = "content",
                    Description = "description",
                    IsShowConverPicture = true,
                    ThumbnailsMediaId = result.MediaId,
                    Title = "标题1",
                    Url = "http://cn.bing.com"
                },
                new NewsArticleModel
                {
                    Author="author2",
                    Content = "content2",
                    Description = "description2",
                    IsShowConverPicture = true,
                    ThumbnailsMediaId = result.MediaId,
                    Title = "标题2",
                    Url = "http://cn.bing.com"
                },
            });
        }

        [Fact]
        public void GetOtherTest()
        {
            var result = _materialService.AddOther(ApiTestHelper.GetJpgBytes());
            var data = _materialService.GetOther(result.MediaId);
            Assert.NotNull(data);
            Assert.True(data.Length > 0);
        }

        [Fact]
        public void GetMediaCountTest()
        {
            var result = _materialService.GetMaterialCount(GetAccessToken());
            Assert.True(result.ImageCount > 0);
        }

        [Fact]
        public void DeleteTest()
        {
            var result = _materialService.AddOther(ApiTestHelper.GetJpgBytes());
            _materialService.Delete(result.MediaId);
            try
            {
                _materialService.GetOther(result.MediaId);
                Assert.True(false);
            }
            catch
            {
            }
        }

        [Fact]
        public void GetListTest()
        {
            var list = _materialService.GetList(new GetMaterialListFilter { Take = 20, Skip = 0, Type = MaterialType.Image });

            Assert.True(list.TotalCount > 0);
        }

        [Fact]
        public void ResultDeserialization()
        {
            var result = @"{
  'total_count': 100,
  'item_count': 1,
  'item': [{
      'media_id': 123123,
      'content': {
          'news_item': [{
              'title': '123',
              'thumb_media_id': 123123,
              'show_cover_pic': 1,
              'author': '12',
              'digest': 'DIGEST',
              'content': 'DIGEST',
              'url': 'http://baidu.com',
              'content_source_url': ''
          }
          ]
       },
       'update_time': 12334444555
   }
 ]
}";

            var output = JsonConvert.DeserializeObject<GetMaterialListResultModel>(result);

            Assert.NotNull(output);


            var result1 = @"{
  'total_count': 100,
  'item_count': 1,
  'item': [{
      'media_id':1231,
      'name': '11',
      'update_time': 123123213,
      'url':'asdfsdfsdf'
  }
  ]
}";
            var output1 = JsonConvert.DeserializeObject<GetMaterialListResultModel>(result1);

            Assert.NotNull(output1);
        }

        [Fact]
        public void GetNewsTest()
        {
            var news = _materialService.GetNews("K0pUMZZjT-kibJVKt8dQycPdJGU8C5xUi2Kzezzypr0");
            Assert.NotNull(news);
        }

        #endregion Test Method
    }
}