﻿using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class WishList
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }

}
