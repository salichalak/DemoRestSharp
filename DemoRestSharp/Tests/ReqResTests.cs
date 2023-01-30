using DemoRestSharp.Models;
using DemoRestSharp.TestData;
using RestSharp;
using System.Net;

namespace DemoRestSharp.Tests
{
    public class ReqResTests
    {
        public RestClient _client;

        [SetUp]
        public void Setup()
        {
            this._client = new RestClient(CommonTestData.BaseUrl);
        }

        [TearDown]
        public void Teardown()
        {
            this._client.Dispose();
        }

        [Test]
        public void GetAllPosts()
        {
            var request = new RestRequest("/posts");
            var response = this._client.ExecuteGet<List<Post>>(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "The request was not successful.");
            Assert.AreEqual(response.Data.Count, CommonTestData.DefaultPostCount, "The current count does not match the initial post count.");
        }

        [Test]
        public void GetPost()
        {
            var request = new RestRequest("/posts");
            var response = this._client.ExecuteGet<List<Post>>(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "The request was not successful.");

            var expectedPost = response.Data.Find(e => e.Title.Equals("rem alias distinctio quo quis"));
            request = new RestRequest("/posts/{postId}");
            request.AddUrlSegment("postId", CommonTestData.PostId);
            var actualPost = this._client.ExecuteGet<Post>(request);

            Assert.That(actualPost.Data.Title, Is.EqualTo(expectedPost.Title), "The title of the post is incorrect.");
            Assert.That(actualPost.Data.Id, Is.EqualTo(expectedPost.Id), "The ID of the post is incorrect.");
            Assert.That(actualPost.Data.UserId, Is.EqualTo(expectedPost.UserId), "The userID of the post is incorrect.");
            Assert.That(actualPost.Data.Body, Is.EqualTo(expectedPost.Body), "The body of the post is incorrect.");
        }

        [Test]
        public void GetPostComments()
        {
            var request = new RestRequest("/posts/{postId}/comments");
            request.AddUrlSegment("postId", CommonTestData.PostId);
            var response = this._client.ExecuteGet<List<Comment>>(request);

            Assert.That(response.Data.Count, Is.EqualTo(CommonTestData.ExpectedCommentCount), $"The comments count {response.Data.Count} is not as expected {CommonTestData.PostCommentCount}.");
            Assert.IsTrue(response.Data.All(e => e.PostId == CommonTestData.PostId));
        }

        [Test]
        public void CreatePost()
        {
            var request = new RestRequest("/posts", Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");

            var body = new Post { Title = CommonTestData.CreatedPostTitle, Body = CommonTestData.CreatedPostBody, Id = CommonTestData.CreatedPostId };
            request.AddJsonBody(body);
            var response = this._client.ExecutePost<Post>(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "The resource was not created successfuly.");
            Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Completed), "The resource was not completed.");
        }

        [Test]
        public void UpdatePost()
        {
            var request = new RestRequest("/posts/{postId}");
            request.AddUrlSegment("postId", CommonTestData.PostId);
            var initialPost = this._client.ExecuteGet<Post>(request);

            request = new RestRequest("/posts/{postId}", Method.Put);
            request.AddUrlSegment("postId", CommonTestData.PostId);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");

            var body = new Post { Title = CommonTestData.CreatedPostTitle, Body = CommonTestData.CreatedPostBody, Id = CommonTestData.CreatedPostId };
            request.AddJsonBody(body);
            var updatedPost = this._client.ExecutePut<Post>(request);

            Assert.AreNotEqual(initialPost.Data.Title, updatedPost.Data.Title, "The title was not updated.");
            Assert.AreNotEqual(initialPost.Data.Body, updatedPost.Data.Body, "The body was not updated.");
            Assert.AreNotEqual(initialPost.Data.UserId, updatedPost.Data.UserId, "The userID was not updated.");
        }

        [Test]
        public void DeletePost()
        {
            var request = new RestRequest("/posts/{postId}", Method.Delete);
            request.AddUrlSegment("postId", CommonTestData.PostId);
            var response = this._client.Delete(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "The resource was not deleted.");
        }
    }
}
