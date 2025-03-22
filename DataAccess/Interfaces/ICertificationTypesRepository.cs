using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface ICertificationTypesRepository
    {
        Task<List<CertificationType>> GetAllAsync();
        Task<CertificationType?> CreateAsync(CertificationType certificationType);
        Task<CertificationType?> UpdateAsync(int id, CertificationType certificationType);
        Task<bool> DeleteAsync(int id);
    }
}
