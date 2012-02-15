using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using HttpServer;
using System.Drawing;

namespace SeleniumLauncher
{
    public class ScreenController : Controller
    {
        public ScreenController() : base() { }

        public override HttpResponse Get(HttpRequest request)
        {
            Console.WriteLine("ScreenCaptureController.Get: " + request.RequestedResource);

            var img = new ScreenCapture().CaptureScreen();

            if (request.RequestedResource.Contains('.'))
            {
                var name = request.RequestedResource.Split('.')[0];
                var mimeType = "image/" + request.RequestedResource.Split('.')[1].Replace("jpg", "jpeg");

                if (name.Equals("screen"))
                    return HttpStatus.Ok(img, mimeType);
            }

            return HttpStatus.NotFound(request.RequestedResource);
        }
    }
}
