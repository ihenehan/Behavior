using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using NUnit.Framework;
using HttpServer;
using System.IO;

namespace Tests.Unit
{
    [TestFixture]
    public class ControllerTests
    {
        private HttpRequest request;
        private Controller controller;

        [SetUp]
        public void Setup()
        {
            request = new HttpRequest();
            controller = new Controller();
        }

        [Test]
        public void controller_get_should_return_not_implemented()
        {
            request.HttpMethod = "Get";

            controller.Get(request).Content.ShouldBe("Controller.Get: Not Implemented.");
        }

        [Test]
        public void controller_post_should_return_not_implemented()
        {
            request.HttpMethod = "Post";

            controller.Post(request).Content.ShouldBe("Controller.Post: Not Implemented.");
        }

        [Test]
        public void controller_put_should_return_not_implemented()
        {
            request.HttpMethod = "Put";

            controller.Put(request).Content.ShouldBe("Controller.Put: Not Implemented.");
        }

        [Test]
        public void controller_delete_should_return_not_implemented()
        {
            request.HttpMethod = "Delete";

            controller.Delete(request).Content.ShouldBe("Controller.Delete: Not Implemented.");
        }

        [Test]
        public void derived_controller_should_return_not_implemented()
        {
            var testController = new TestController();

            request.HttpMethod = "Get";

            testController.Get(request).Content.ShouldBe("TestController.Get: Not Implemented.");
        }

        [Test]
        public void controller_should_read_request_body()
        {
            var request = new HttpRequest();

            request.InputStream = new MemoryStream(ASCIIEncoding.Default.GetBytes("foo"));

            request.Body.ShouldBe("foo");
        }
    }

    public class TestController : Controller {}
}
