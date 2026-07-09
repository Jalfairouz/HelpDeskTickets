using AutoMapper;
using HelpDeskTickets.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HelpDeskTickets.Core.Models;
using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.DTOs.Responses;
using HelpDeskTickets.Core;


namespace HelpDeskTickets.App.Services
{
    public class DepartmentSrevice : IDepartmentSrevice
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentSrevice(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DepartmentResponse> CreateDepartmentAsync(CreateDepartmentRequest request)
        {
            var department = _mapper.Map<Department>(request);

            await _unitOfWork.Departments.AddAsync(department);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DepartmentResponse>(department);
        }
        public async Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync()
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            return _mapper.Map<IEnumerable<DepartmentResponse>>(departments);
        }
        
    }
}
