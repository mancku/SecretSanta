using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta
{
    public interface IParticipant
    {
        string FirstName { get; set; }
        string Email { get; set; }
    }
}
