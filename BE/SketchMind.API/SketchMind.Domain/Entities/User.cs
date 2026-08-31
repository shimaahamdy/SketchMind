using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        public User(string name, string email)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
        }


    }
}
