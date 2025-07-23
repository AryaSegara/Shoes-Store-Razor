namespace Shoes_Store.Models.DTO
{
    public class UserProfileDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public string DateOfBirth { get; set; }

        public string Password { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
