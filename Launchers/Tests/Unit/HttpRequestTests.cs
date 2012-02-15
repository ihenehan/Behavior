using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HttpServer;

namespace Tests.Unit
{
    [TestFixture]
    public class HttpRequestTests
    {
        private HttpRequest request;
        private string controller;
        private string resource;

        [Test]
        public void should_parse_controller()
        {
            GivenUrl("/foo");

            controller.ShouldBe("foo");
        }

        [Test]
        public void should_parse_controller_with_resource()
        {
            GivenUrl("/foo/bar");

            controller.ShouldBe("foo");
        }

        [Test]
        public void url_without_controller_should_be_empty()
        {
            GivenUrl("/");

            controller.ShouldBe("");
        }

        [Test]
        public void should_parse_resource()
        {
            GivenUrl("/foo/bar");

            resource.ShouldBe("bar");
        }

        [Test]
        public void url_without_resource_should_be_empty()
        {
            GivenUrl("/foo");

            resource.ShouldBe("");
        }

        [Test]
        public void should_parse_resource_with_extension()
        {
            GivenUrl("/foo/bar.ext");

            resource.ShouldBe("bar.ext");
        }

        [Test]
        public void should_parse_resource_with_trailing_slash()
        {
            GivenUrl("/foo/bar/");

            resource.ShouldBe("bar");
        }

        public void GivenUrl(string url)
        {
            request = new HttpRequest();

            controller = request.ParseController(url);

            resource = request.ParseResource(url, controller);
        }
    }
}
