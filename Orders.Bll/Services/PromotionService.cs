using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
using Orders.Bll.Interfaces;
using Orders.Bll.Mappers;
using Orders.Dal.Interfaces;

namespace Orders.Bll.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PromotionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PromotionDto>> GetAllAsync()
        {
            var promos = await _unitOfWork.Promotions.GetAllAsync();
            return promos.Select(PromotionMapper.ToDto);
        }

        public async Task<PromotionDto?> GetByIdAsync(int id)
        {
            var promo = await _unitOfWork.Promotions.GetByIdAsync(id);
            return promo == null ? null : PromotionMapper.ToDto(promo);
        }

        public async Task AddAsync(PromotionDto dto)
        {
            var entity = PromotionMapper.ToEntity(dto);
            await _unitOfWork.Promotions.AddAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(PromotionDto dto)
        {
            var entity = PromotionMapper.ToEntity(dto);
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
