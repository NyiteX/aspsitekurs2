﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace aspsitekurs2.Models
{
    public class UserModel
    {
        public int? ID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Invalid name format")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid email format")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        public bool? isAdmin { get; set; }
        public string? Pic { get; set; }

    }
}
