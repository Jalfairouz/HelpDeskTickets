using AutoMapper;
using HelpDeskTickets.App.Services;
using HelpDeskTickets.Core;
using HelpDeskTickets.Core.DTOs.Responses;

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

        var orderedHistories = histories
            .OrderByDescending(h => h.CreatedAt);

        return _mapper.Map<IEnumerable<HistoryResponse>>(orderedHistories);
    }
}