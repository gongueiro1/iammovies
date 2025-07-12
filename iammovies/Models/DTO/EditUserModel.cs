
namespace iammovies.Models.Dto
{
    public class EditUserModel
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "user";
    }
}
