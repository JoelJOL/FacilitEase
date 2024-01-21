// EmployeeInputModel.cs
using System;
using System.ComponentModel.DataAnnotations;

public class EmployeeInputModel
{
    [Required]
    public string EmployeeCode { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    
    /*public DateOnly Date { get; set; }*/

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Gender { get; set; }

    // Other properties as needed
}
