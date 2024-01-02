using AutoMapper;
using TimeTracker.Api.Booking.Models;
using TimeTracker.Api.Booking.ViewModels;

namespace TimeTracker.Api.Booking;

public class BookingMappingProfile : Profile
{
    public BookingMappingProfile()
    {
        CreateMap<Models.Booking, BookingReadViewModel>();
        CreateMap<Models.Booking, BookingWriteViewModel>();
        CreateMap<BookingReadViewModel, Models.Booking>();
        CreateMap<BookingWriteViewModel, Models.Booking>();
        CreateMap<BookingReadViewModel, BookingWriteViewModel>();

        CreateMap<Models.Booking, BookingReadModel>();
    }
}