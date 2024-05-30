using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models.PhoneSpecs.PhoneScreen
{
    [Owned]
    public class PhoneScreenSpecs
    {
        public float ScreenSize { get; set; }

        public int ScreenResolution { get; set; }

        public int ScreenRefreshRate { get; set; }

        public PhoneScreenFeatureEnum ScreenFeature { get; set; }
    }
}
