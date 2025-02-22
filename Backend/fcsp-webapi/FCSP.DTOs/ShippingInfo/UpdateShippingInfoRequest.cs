using System.ComponentModel.DataAnnotations;

namespace FCSP.DTOs.ShippingInfo
{
    public class UpdateShippingInfoRequest
    {
        public long Id { get; set; }

        public string? StreetAddress { get; set; }

        public string? Ward { get; set; }

        public string? District { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
