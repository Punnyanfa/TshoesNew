using FCSP.DTOs;

namespace FCSP.DTOs.Size
{
    public class UpdateSizeRequest
    {
        public long Id { get; set; }
        public int SizeValue { get; set; }
        public bool IsDeleted { get; set; }
    }
} 