using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models.PhoneSpecs.PhoneCamera
{
    [Owned]
    public class PhoneCameraSpecs
    {
        public int CameraResolution { get; set; }

        public int CameraZoom { get; set; }

        public int CameraFps { get; set; }

    }
}
