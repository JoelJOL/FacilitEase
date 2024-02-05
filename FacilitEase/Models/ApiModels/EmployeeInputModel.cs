// EmployeeInputModel.cs
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the input model for an employee and employee detail table , which act as a bulk upload model also.
/// </summary>
public class EmployeeInputModel
{
    [Required]
    public string EmployeeCode { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public DateOnly DOB { get; set; }

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