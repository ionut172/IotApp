using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace IotApp.Models;

public class Device
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }

    // Foreign key property
    public string UserId { get; set; }

}

