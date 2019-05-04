using System;
using System.ComponentModel.DataAnnotations;

namespace FlatRent.Models.Requests
{
    public class FaultFixForm
    {
        [Required]
        [Range(0, double.MaxValue)]
        public float Price { get; set; }
    }
}