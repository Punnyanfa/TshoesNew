using FCSP.DTOs.Design;
using FCSP.Models.Entities;
using FCSP.Repositories;

namespace FCSP.Services.DesignService
{
    public class DesignService : IDesignService
    {
        private readonly IDesignRepository _designRepository;

        public DesignService(IDesignRepository designRepository)
        {
            _designRepository = designRepository;
        }

        public async Task<IEnumerable<CustomShoeDesign>> GetAllTemplate()
        {
            var response = await _designRepository.GetAllAsync();
            return response;
        }

        public async Task<GetDesignByIdResponse> GetTemplateById(GetDesignByIdRequest request)
        {
            CustomShoeDesign customShoeDesign = GetEntityFromGetByIdRequest(request);
            return new GetDesignByIdResponse
            {
                UserId = customShoeDesign.UserId,
                CustomShoeDesignTemplateId = customShoeDesign.CustomShoeDesignTemplateId,
                DesignData = customShoeDesign.DesignData,
                Preview3DFileUrl = customShoeDesign.Preview3DFileUrl,
                Price = customShoeDesign.Price,
            };
        }

        public async Task<GetDesignByIdResponse> AddDesign(AddDesignRequest request)
        {
            CustomShoeDesign customShoeDesign = GetEntityFromAddRequest(request);
            await _designRepository.AddAsync(customShoeDesign);
            return new GetDesignByIdResponse();
        }

        public async Task<GetDesignByIdResponse> UpdateDesign(UpdateDesignRequest request)
        {
            CustomShoeDesign customShoeDesign = GetEntityFromUpdateRequest(request);
            await _designRepository.UpdateAsync(customShoeDesign);
            return new GetDesignByIdResponse();
        }

        public async Task<GetDesignByIdResponse> DeleteDesign(DeleteDesignRequest request)
        {
            CustomShoeDesign customShoeDesign = GetEntityFromDeleteRequest(request);
            await _designRepository.DeleteAsync(customShoeDesign.Id);
            return new GetDesignByIdResponse();
        }

        private CustomShoeDesign GetEntityFromGetByIdRequest(GetDesignByIdRequest request)
        {
            CustomShoeDesign template = _designRepository.Find(request.Id);
            if (template == null)
            {
                throw new InvalidOperationException();
            }
            return template;
        }

        private CustomShoeDesign GetEntityFromAddRequest(AddDesignRequest request)
        {
            return new CustomShoeDesign
            {
                UserId = request.UserId,
                CustomShoeDesignTemplateId = request.CustomShoeDesignTemplateId,
                DesignData = request.DesignData,
                Preview3DFileUrl = request.Preview3DFileUrl,
                Price = request.Price,
            };
        }

        private CustomShoeDesign GetEntityFromUpdateRequest(UpdateDesignRequest request)
        {
            CustomShoeDesign customShoeDesign = _designRepository.Find(request.Id);
            if (customShoeDesign == null)
            {
                throw new InvalidOperationException();
            }
            customShoeDesign.CustomShoeDesignTemplateId = request.CustomShoeDesignTemplateId ?? customShoeDesign.CustomShoeDesignTemplateId;
            customShoeDesign.DesignData = request.DesignData ?? customShoeDesign.DesignData;
            customShoeDesign.Price = request.Price ?? customShoeDesign.Price;
            customShoeDesign.Preview3DFileUrl = request.Preview3DFileUrl ?? customShoeDesign.Preview3DFileUrl;
            customShoeDesign.UpdatedAt = DateTime.Now;
            return customShoeDesign;
        }

        private CustomShoeDesign GetEntityFromDeleteRequest(DeleteDesignRequest request)
        {
            CustomShoeDesign template = _designRepository.Find(request.Id);
            if (template == null)
            {
                throw new InvalidOperationException();
            }
            return template;
        }
    }
}
