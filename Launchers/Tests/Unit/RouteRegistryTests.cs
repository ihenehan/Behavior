using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HttpServer;

namespace Tests.Unit
{
    [TestFixture]
    public class RouteRegistryTests
    {
        private List<string> rels;
        private RegController controller;
        private RouteRegistry registry;

        [SetUp]
        public void Setup()
        {
            rels = new List<string>() { "foo", "bar" };

            controller = new RegController();

            registry = new RouteRegistry();

            registry.Register(controller);
        }

        [Test]
        public void should_add_map_for_each_controller()
        {
            registry.Keys.ShouldBe(new List<string>() { "reg" });
        }

        [Test]
        public void should_have_controller_for_each_map()
        {
            registry.Values.ToList().ForEach(v => v.ShouldBe(controller));
        }

        [Test]
        public void route_post_should_return_post()
        {
            var request = new HttpRequest() { HttpMethod = "post", RequestedController = "reg" };

            registry.Route(request).Content.ShouldBe("post");
        }

        [Test]
        public void given_wrong_http_verb_should_return_unknown_method()
        {
            var request = new HttpRequest() { HttpMethod = "FOO", RequestedController = "reg" };

            registry.Route(request).Content.ShouldBe("Unknown HTTP method: FOO");
        }
    }

    public class RegController : Controller
    {
        public override HttpResponse Post(HttpRequest request)
        {
            return HttpStatus.Ok("post");
        }
    }
}
