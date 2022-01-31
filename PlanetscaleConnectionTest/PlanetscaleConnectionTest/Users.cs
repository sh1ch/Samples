using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetscaleConnectionTest;

[Table("users")]
public class Users
{
    [Key]
    [Column("id")]
    public int? Id { get; set; }

    [Column("email")]
    public string Email { get; set; } = "";

    [Column("first_name")]
    public string FirstName { get; set; } = "";

    [Column("last_name")]
    public string LastName { get; set; } = "";
}
