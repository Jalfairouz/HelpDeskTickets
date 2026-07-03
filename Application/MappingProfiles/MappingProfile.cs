using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using HelpDeskTickets.Core;
using HelpDeskTickets.Core.Models;
using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.DTOs.Responses;

namespace HelpDeskTickets.App.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateTicketRequest, Ticket>();
            CreateMap<UpdateTicketRequest, Ticket>();
            CreateMap<Ticket, TicketResponse>();
            CreateMap<CreateDepartmentRequest, Department>();
            CreateMap<Department, DepartmentResponse>();
            CreateMap<CreateCommentRequest, Comment>();
            CreateMap<Comment, CommentResponse>();

        }
    }
}
