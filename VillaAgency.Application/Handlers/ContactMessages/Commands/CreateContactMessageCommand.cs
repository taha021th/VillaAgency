using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Application.Common.Interfaces.Services;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.ContactMessages.Commands;

public class CreateContactMessageCommand : IRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
}

public class CreateContactMessageCommandHandler : IRequestHandler<CreateContactMessageCommand>
{
    private readonly IContactMessageRepository _contectMessageRepository;
    private readonly IEmailService _emailService;

    public CreateContactMessageCommandHandler(IContactMessageRepository contectMessageRepository, IEmailService emailService)
    {
        _contectMessageRepository = contectMessageRepository;
        _emailService = emailService;
    }
    public async Task Handle(CreateContactMessageCommand request, CancellationToken cancellationToken)
    {
        var contactMessage = new ContactMessage
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Subject = request.Subject,
            Message = request.Message,
            CreatedAt = DateTime.UtcNow,
        };
        await _contectMessageRepository.AddAsync(contactMessage);


        try
        {
            var emailSubject = $"پیام جدید از فرم تماس: {request.Subject}";
            var emailMessage = $"شما یک پیام جدید از <b>{request.Name}</b> ({request.Email}) دریافت کرده‌اید.<br/><br/><b>متن پیام:</b><br/>{request.Message}";

            // ایمیل مدیر را اینجا وارد کنید
            await _emailService.SendEmailAsync("taha.golbon@gmail.com", emailSubject, emailMessage);
        }
        catch (Exception ex)
        {
            // در اینجا می‌توانید خطای ارسال ایمیل را لاگ کنید
            // اما نباید باعث شکست کل عملیات شوید
        }
    }
}

