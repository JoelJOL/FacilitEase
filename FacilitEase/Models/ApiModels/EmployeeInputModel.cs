// EmployeeInputModel.cs
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


public class EmployeeInputModel
{
    [Required]
    public int EmployeeCode { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

   /* public DateOnly DOB { get; set; }*/

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Gender { get; set; }

    [Required]
    public int ManagerId { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    [Required]
    public int PositionId { get; set; }

    [Required]
    public int LocationId { get; set; }
}
