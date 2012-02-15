using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HttpServer;

namespace Tests.Unit
{
    [TestFixture]
    public class HttpStatusTests
    {
        [Test]
        public void should_return_not_found_content()
        {
            HttpStatus.NotFound("foo").Content.ShouldBe("foo not found.");
        }

        [Test]
        public void should_return_not_found_status_code()
        {
            HttpStatus.NotFound("foo").StatusCode.ShouldBe(404);
        }

        [Test]
        public void should_return_not_found_description()
        {
            HttpStatus.NotFound("foo").StatusDescription.ShouldBe("NotFound");
        }

        [Test]
        public void should_return_internal_error_content()
        {
            HttpStatus.InternalError("foo").Content.ShouldBe("foo");
        }

        [Test]
        public void should_return_internal_error_status_code()
        {
            HttpStatus.InternalError("foo").StatusCode.ShouldBe(500);
        }

        [Test]
        public void should_return_internal_error_description()
        {
            HttpStatus.InternalError("foo").StatusDescription.ShouldBe("InternalServerError");
        }

        [Test]
        public void should_return_not_implemented_content()
        {
            HttpStatus.NotImplemented("foo").Content.ShouldBe("foo");
        }

        [Test]
        public void should_return_not_implemented_status_code()
        {
            HttpStatus.NotImplemented("foo").StatusCode.ShouldBe(501);
        }

        [Test]
        public void should_return_not_implemented_description()
        {
            HttpStatus.NotImplemented("foo").StatusDescription.ShouldBe("NotImplemented");
        }

        [Test]
        public void should_return_ok_content()
        {
            HttpStatus.Ok("foo").Content.ShouldBe("foo");
        }

        [Test]
        public void should_return_ok_status_code()
        {
            HttpStatus.Ok("foo").StatusCode.ShouldBe(200);
        }

        [Test]
        public void should_return_ok_description()
        {
            HttpStatus.Ok("foo").StatusDescription.ShouldBe("OK");
        }
    }
}
