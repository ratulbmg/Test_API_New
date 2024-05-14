using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Test_API_New.BusinessLogicLayer.DataTransferObject;
using Test_API_New.DataAccessLayer.Entities;
using Test_API_New.DataAccessLayer.Repository;

namespace Test_API_New.BusinessLogicLayer.Services
{
    public interface IUserService : IBaseService<UserRequestDTO, UserResponseDTO, User>
    {
    }
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IMapper mapper;
        public UserService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
        }

        public async Task<UserResponseDTO> Add(UserRequestDTO requestDTO)
        {
            var user = mapper.Map<User>(requestDTO);
            var userResponse = await this.repositoryWrapper.UserRepository.CreateAsync(user);
            await this.repositoryWrapper.SaveAsync();
            var result = mapper.Map<UserResponseDTO>(userResponse);
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await this.repositoryWrapper.UserRepository.DeleteAsync(id);
            await this.repositoryWrapper.SaveAsync();
            return result;
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAll()
        {
            var users = await this.repositoryWrapper.UserRepository.GetAllAsync("Product");
            var result = mapper.Map<IEnumerable<UserResponseDTO>>(users);
            return result;
        }

        public async Task<UserResponseDTO?> GetById(int id)
        {
            var user = await this.repositoryWrapper.UserRepository.GetById(id);
            if (user is null) return null;
            var result = mapper.Map<UserResponseDTO>(user);
            return result;
        }

        public async Task<UserResponseDTO?> Update(int id, UserRequestDTO requestDTO)
        {
            var user = mapper.Map<User>(requestDTO);
            user.Id = id;
            var userResponse = await this.repositoryWrapper.UserRepository.UpdateAsync(id, user);
            if (userResponse is not null)
            {
                await this.repositoryWrapper.SaveAsync();
                return mapper.Map<UserResponseDTO>(userResponse);
            }
            return null;
        }

        public async Task<UserResponseDTO?> UpdatePatch(int id, JsonPatchDocument<User> request)
        {
            var userResponse = await this.repositoryWrapper.UserRepository.UpdatePatchAsync(id, request);
            if (userResponse is not null)
            {
                await this.repositoryWrapper.SaveAsync();
                return mapper.Map<UserResponseDTO>(userResponse);
            }
            return null;
        }
    }
}
