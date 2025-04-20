using FCSP.DTOs.DesignPreview;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

namespace FCSP.Services.DesignPreviewService
{
    public class DesignPreviewService : IDesignPreviewService
    {
        private readonly IDesignPreviewRepository _designPreviewRepository;

        public DesignPreviewService(IDesignPreviewRepository designPreviewRepository)
        {
            _designPreviewRepository = designPreviewRepository;
        }

        public async Task<IEnumerable<GetDesignPreviewByIdResponse>> GetAllDesignPreviews()
        {
            var previews = await _designPreviewRepository.GetAllAsync();
            return previews.Select(MapToResponse);
        }

        public async Task<GetDesignPreviewByIdResponse> GetDesignPreviewById(GetDesignPreviewByIdRequest request)
        {
            var preview = await _designPreviewRepository.FindAsync(request.Id);
            if (preview == null)
                throw new Exception("Design preview not found");

            return MapToResponse(preview);
        }

        public async Task<IEnumerable<GetDesignPreviewByIdResponse>> GetPreviewsByDesign(GetPreviewsByDesignRequest request)
        {
            var previews = await _designPreviewRepository.GetPreviewsByCustomShoeDesignIdAsync(request.CustomShoeDesignId);
            return previews.Select(MapToResponse);
        }

        public async Task<AddDesignPreviewResponse> AddDesignPreview(AddDesignPreviewRequest request)
        {
            var preview = new DesignPreview
            {
                CustomShoeDesignId = request.CustomShoeDesignId,
                PreviewImageUrl = request.PreviewImageUrl
            };

            var addedPreview = await _designPreviewRepository.AddAsync(preview);
            return new AddDesignPreviewResponse { DesignPreviewId = addedPreview.Id };
        }

        public async Task<UpdateDesignPreviewResponse> UpdateDesignPreview(UpdateDesignPreviewRequest request)
        {
            var preview = await _designPreviewRepository.FindAsync(request.Id);
            if (preview == null)
                throw new Exception("Design preview not found");

            preview.PreviewImageUrl = request.PreviewImageUrl;

            await _designPreviewRepository.UpdateAsync(preview);
            return new UpdateDesignPreviewResponse { DesignPreviewId = preview.Id };
        }

        public async Task<DeleteDesignPreviewResponse> DeleteDesignPreview(DeleteDesignPreviewRequest request)
        {
            await _designPreviewRepository.DeleteAsync(request.Id);
            return new DeleteDesignPreviewResponse { Success = true };
        }

        private GetDesignPreviewByIdResponse MapToResponse(DesignPreview preview)
        {
            return new GetDesignPreviewByIdResponse
            {
                Id = preview.Id,
                CustomShoeDesignId = preview.CustomShoeDesignId,
                PreviewImageUrl = preview.PreviewImageUrl,
                CreatedAt = preview.CreatedAt
            };
        }
    }
}