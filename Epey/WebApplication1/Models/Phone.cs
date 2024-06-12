using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Phone
{
    public int Id { get; set; }

    public int BrandId { get; set; }

    public int CategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string ImagePath { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int PhoneBatterySpecsBatteryCapacity { get; set; }

    public int PhoneBatterySpecsChargingSpeed { get; set; }

    public int PhoneBatterySpecsFastCharging { get; set; }

    public int PhoneCameraSpecsCameraFps { get; set; }

    public int PhoneCameraSpecsCameraResolution { get; set; }

    public int PhoneCameraSpecsCameraZoom { get; set; }

    public int PhoneScreenSpecsScreenRefreshRate { get; set; }

    public int PhoneScreenSpecsScreenResolution { get; set; }

    public double PhoneScreenSpecsScreenSize { get; set; }

    public double Price { get; set; }

    public string Seller { get; set; } = null!;

    public virtual Brand Brand { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
