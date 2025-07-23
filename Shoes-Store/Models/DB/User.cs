using static Shoes_Store.Models.GeneralStatus;

namespace Shoes_Store.Models.DB
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        //public string salt { get; set; }
        public string Password { get; set; }
        public string DateOfBirth { get; set; }
        public GeneralStatusData UserStatus { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Cart Cart { get; set; } // untuk satu ke satu / one to one

        // untuk satu ke banyak / banyak ke satu / many to one / one to many
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<UserSaldo> UserSaldos { get; set; } = new List<UserSaldo>();
    }
}
