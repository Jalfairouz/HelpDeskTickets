using AutoMapper;
using HelpDeskTickets.App.Services;
using HelpDeskTickets.Core;
using HelpDeskTickets.Core.DTOs.Responses;
using HelpDeskTickets.Core.Models;
using Microsoft.EntityFrameworkCore;

public class HistoryService : IHistoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public HistoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<HistoryResponse>> GetHistoryByTicketAsync(int ticketId)
    {
        var histories = await _unitOfWork.Histories.FindAsync(h => h.TicketId == ticketId);
        IQueryable<History> query = _unitOfWork.Histories.GetQueryable().Where(t=> t.TicketId == ticketId);
        var ticketHistory = query.ToList();
        return _mapper.Map<List<HistoryResponse>>(ticketHistory);
    }
}