using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointmentBookingSystem.Modules
{
    public class Professional:User
    {
        public string Specialty { get; set; }

        public Professional(int id, string name, string email, string specialty)
            : base(id, name, email)
        {
            Specialty = specialty;
        }

    }
}
