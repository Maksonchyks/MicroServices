using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Common.Dto;
using Orders.Bll.Interfaces;
using Orders.Dal.Interfaces;

namespace Orders.Bll.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PromotionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PromotionDto>> GetAllAsync()
        {
            var promos = await _unitOfWork.Promotions.GetAllAsync();
            return _mapper.Map<IEnumerable<PromotionDto>>(promos);
        }

        public async Task<PromotionDto?> GetByIdAsync(int id)
        {
            var promo = await _unitOfWork.Promotions.GetByIdAsync(id);
            return promo == null ? null : _mapper.Map<PromotionDto>(promo);
        }

        public async Task AddAsync(PromotionDto dto)
        {
            var entity = _mapper.Map<Orders.Domain.Enteties.Promotion>(dto);
            await _unitOfWork.Promotions.AddAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(PromotionDto dto)
        {
            var entity = _mapper.Map<Orders.Domain.Enteties.Promotion>(dto);
            await _unitOfWork.Promotions.UpdateAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Promotions.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }
    }
}
