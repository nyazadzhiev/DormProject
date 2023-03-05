using DormProject.DormOffice.Services;
using DormProject.Messages;
using MassTransit;

namespace DormProject.DormOffice.Messages
{
    public class StudentAddedConsumer : IConsumer<StudentAddedMessage>
    {
        private readonly IStudentService _studentService;

        public StudentAddedConsumer(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task Consume(ConsumeContext<StudentAddedMessage> context)
        {
            var message = context.Message;

            await _studentService.CreateStudentAsync(new Data.Models.StudentRequestDTO()
            {
                FirstName = message.FirstName,
                LastName = message.LastName,
                Grade = message.Grade
            });
        }
    }
}
