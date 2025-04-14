using FCSP.DTOs;

namespace FCSP.DTOs.Size
{
    public class SizeDto
    {
        public long Id { get; set; }
        public int SizeValue { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ShoeSizes
    {
        public long Id { get; set; }
        public int SizeValue { get; set; }
    }

    public class SizeResponse : BaseResponseModel<SizeDto>
    {
    }

    public class SizeListResponse : BaseResponseModel<List<SizeDto>>
    {
    }

    public class AddSizeRequest
    {
        public int SizeValue { get; set; }
    }

    public class DeleteSizeRequest
    {
        public long Id { get; set; }
    }

    public class GetSizeByIdRequest
    {
        public long Id { get; set; }
    }

    public class UpdateSizeRequest
    {
        public long Id { get; set; }
        public int SizeValue { get; set; }
        public bool IsDeleted { get; set; }
    }
} 