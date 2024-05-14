using Microsoft.AspNetCore.JsonPatch;
using Test_API_New.BusinessLogicLayer.DataTransferObject;

namespace Test_API_New.BusinessLogicLayer.Services
{
    public interface IBaseService<TRequestDTO, TResponseDTO, T> where T : class
    {
        Task<IEnumerable<TResponseDTO>> GetAll();
        Task<TResponseDTO?> GetById(int id);
        Task<TResponseDTO> Add(TRequestDTO requestDTO);
        Task<bool> Delete(int id);
        Task<TResponseDTO?> Update(int id, TRequestDTO requestDTO);
        Task<TResponseDTO?> UpdatePatch(int id, JsonPatchDocument<T> patchDocument);
    }
}
