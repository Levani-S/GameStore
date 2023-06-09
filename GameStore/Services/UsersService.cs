﻿using GameStore.Data.Repositories.RepositoryInterfaces;
using GameStore.Data.UnitOfWork;
using GameStore.Services.ServiceInterfaces;
using GameStore.ViewModels;
using System.Threading.Tasks;

namespace GameStore.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _usersRepository = _unitOfWork.Users;
        }
        public Task<UserViewModel> EditUser(UserViewModel user)
        {
            return _usersRepository.EditUser(user);
        }
    }
}