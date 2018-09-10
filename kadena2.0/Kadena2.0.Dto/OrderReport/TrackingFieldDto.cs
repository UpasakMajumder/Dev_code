using Kadena.Dto.Shipping;
using System.Collections.Generic;

namespace Kadena.Dto.OrderReport
{
    public class TrackingFieldDto
    {
        public string Type => "tracking";

        public IEnumerable<TrackingInfoDto> Value { get; set; }
    }
}
