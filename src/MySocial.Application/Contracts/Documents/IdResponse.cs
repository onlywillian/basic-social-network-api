using MySocial.Application.Validations;

namespace MySocial.Application.Contracts.Documents
{
    public class IdResponse : Notifiable
    {
        public int Id { get; set; }
    }
}
