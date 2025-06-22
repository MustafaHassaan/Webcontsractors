using Domain.Models;
using System;
using System.Collections.Generic;

namespace Webcontsractors.Domain.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Opration> Oprations { get; set; } = new List<Opration>();
    public ICollection<Userpermission> UserPermissions { get; set; }
}
