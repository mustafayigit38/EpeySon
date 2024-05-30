using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models.PhoneSpecs.PhoneBattery
{
	[Owned]
	public class PhoneBatterySpecs
	{

		public int BatteryCapacity { get; set; }

		public bool FastCharging { get; set; }

		public int ChargingSpeed { get; set; }


	}
}
