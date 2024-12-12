namespace E_Commerce.DTOs
{
    public class AddressDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Street { get; set; }

        public string Pincode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }
    }

}
