using WebApplication1.Models.PhoneSpecs.PhoneBattery;
using WebApplication1.Models.PhoneSpecs.PhoneCamera;
using WebApplication1.Models.PhoneSpecs.PhoneScreen;

namespace WebApplication1.Models
{
    public class Phone:Product
	{
        public PhoneBatterySpecs PhoneBatterySpecs { get; set; }
		public PhoneCameraSpecs PhoneCameraSpecs { get; set; }
		public PhoneScreenSpecs PhoneScreenSpecs { get; set; }


	}
}
