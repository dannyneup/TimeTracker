using AutoMapper;
using TimeTracker.Api.Booking.ViewModels;

namespace TimeTracker.Api.Booking;

public class BookingMappingProfile : Profile
{
    public BookingMappingProfile()
    {
        CreateMap<Booking, BookingReadViewModel>();
        CreateMap<Booking, BookingWriteViewModel>();
        CreateMap<BookingReadViewModel, Booking>();
        CreateMap<BookingWriteViewModel, Booking>();
        CreateMap<BookingReadViewModel, BookingWriteViewModel>();
    }
}