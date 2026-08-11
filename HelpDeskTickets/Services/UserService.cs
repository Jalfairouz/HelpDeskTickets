using AutoMapper;
using HelpDeskTickets.Core;
using HelpDeskTickets.Core.DTOs.Responses;
using HelpDeskTickets.Core.Interfaces;
using HelpDeskTickets.Core.Models;
using HelpDeskTickets.DTOs.Responses;
using Microsoft.AspNetCore.Identity;

namespace HelpDeskTickets.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<IEnumerable<UserProfileDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();


            var userDtos = new List<UserProfileDto>();

            foreach (var user in users)
            {
                var dto = _mapper.Map<UserProfileDto>(user);
                var roles = await _userManager.GetRolesAsync(user);
                dto.Role = roles.FirstOrDefault() ?? "No Role";
                userDtos.Add(dto);
            }
            return userDtos;
        }
    }
}
