using System.ComponentModel.DataAnnotations;

namespace ApiAuthorization.Contexts.Models
{
    public partial class User
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public RoleId RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}

