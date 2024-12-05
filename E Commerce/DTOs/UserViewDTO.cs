namespace E_Commerce.DTOs
{
    public class UserViewDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone {  get; set; }
        public bool IsBlocked { get; set; }
    }
}
