using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using HelpDeskTickets.Core.Models;
using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.DTOs.Responses;

namespace HelpDeskTickets.App.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateTicketRequest, Ticket>()
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => TicketStatus.Open));

            CreateMap<UpdateTicketRequest, Ticket>()
    .ForMember(dest => dest.Status,
               opt => opt.MapFrom(src => Enum.Parse<TicketStatus>(src.Status, true)));
            
            CreateMap<Ticket, TicketResponse>()
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CreateDepartmentRequest, Department>();
            CreateMap<Department, DepartmentResponse>();
            CreateMap<CreateCommentRequest, Comment>()
    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Comment, CommentResponse>();

        }
    }
}
