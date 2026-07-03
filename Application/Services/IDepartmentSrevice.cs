using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.App.Services
{
    public interface IDepartmentSrevice
    {
        Task<DepartmentResponse> CreateDepartmentAsync(CreateDepartmentRequest request);
        Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync();

    }
}
