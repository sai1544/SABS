using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointmentBookingSystem.Modules
{
    public class Appointment
    {
        public int Id { get; set; }
        public User Client { get; set; }
        public Professional Professional { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }

        public Appointment(int id, User client, Professional professional, DateTime appointmentDate)
        {
            Id = id;
            Client = client;
            Professional = professional;
            AppointmentDate = appointmentDate;
            Status = "Scheduled";
        }

        public void Confirm()
        {
            Status = "Confirmed";
        }

        public void Cancel()
        {
            Status = "Cancelled";
        }

    }
}
