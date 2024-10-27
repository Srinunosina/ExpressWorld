
using ExpressWorld.Application.Commands;
using ExpressWorld.Application.Interfaces;
using MediatR;

namespace ExpressWorld.Application.Handlers
{
    //public class BookProductHandler : IRequestHandler<BookProductCommand, BookingResponseDTO>
    //{
    //    private readonly IDataSourceManager _dataSourceManager;
    //    private readonly IBookingRepository _bookingRepository;

    //    public BookProductHandler(IDataSourceManager dataSourceManager, IBookingRepository bookingRepository)
    //    {
    //        _dataSourceManager = dataSourceManager;
    //        _bookingRepository = bookingRepository;
    //    }

    //    public async Task<BookingResponseDTO> Handle(BookProductCommand request, CancellationToken cancellationToken)
    //    {
    //        var adapter = _dataSourceManager.GetAdapterForProduct(request.ProductId);
    //        var bookingResponse = await adapter.BookAsync(new BookingRequest
    //        {
    //            ProductId = request.ProductId,
    //            CustomerName = request.CustomerName,
    //            BookingDate = request.BookingDate
    //        });

    //        if (bookingResponse.IsBooked)
    //        {
    //            await _bookingRepository.SaveBookingAsync(new Booking
    //            {
    //                ProductId = request.ProductId,
    //                CustomerName = request.CustomerName,
    //                BookingDate = request.BookingDate,
    //                ConfirmationNumber = bookingResponse.ConfirmationNumber
    //            });
    //        }

    //        return new BookingResponseDTO
    //        {
    //            IsBooked = bookingResponse.IsBooked,
    //            ConfirmationNumber = bookingResponse.ConfirmationNumber
    //        };
    //    }
    //}

}
