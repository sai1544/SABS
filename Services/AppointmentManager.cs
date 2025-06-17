using SmartAppointmentBookingSystem.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointmentBookingSystem.Services
{
    public class AppointmentManager
    {
        private readonly string _appointmentsFilePath = "appointments.txt";

        public Appointment CreateAppointment(User client, Professional professional, DateTime appointmentDate)
        {
            try
            {
                if (client == null || professional == null)
                    throw new ArgumentNullException("Client and Professional must not be null.");

                if (appointmentDate <= DateTime.Now)
                    throw new ArgumentException("Appointment date must be in the future.");

                Appointment newAppointment = new Appointment(
                    id: new Random().Next(1, 1000),
                    client: client,
                    professional: professional,
                    appointmentDate: appointmentDate
                );

                SaveAppointmentToFile(newAppointment);
                return newAppointment;
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public void CancelAppointment(int appointmentId)
        {
            try
            {
                var appointments = LoadAppointmentsFromFile();
                var appointment = appointments.FirstOrDefault(a => a.Id == appointmentId);

                if (appointment == null)
                    throw new KeyNotFoundException("Appointment not found.");

                appointment.Cancel();
                SaveAppointmentsToFile(appointments);
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        private void SaveAppointmentToFile(Appointment appointment)
        {
            try
            {
                var appointments = LoadAppointmentsFromFile();
                appointments.Add(appointment);
                SaveAppointmentsToFile(appointments);
            }
            catch (IOException ex)
            {
                LogError(ex);
                throw new ApplicationException("Failed to save appointment data.", ex);
            }
        }

        private void SaveAppointmentsToFile(List<Appointment> appointments)
        {
            try
            {
                using (var writer = new StreamWriter(_appointmentsFilePath))
                {
                    foreach (var appointment in appointments)
                    {
                        writer.WriteLine($"{appointment.Id},{appointment.Client.Name},{appointment.Professional.Name},{appointment.AppointmentDate},{appointment.Status}");
                    }
                }
            }
            catch (IOException ex)
            {
                LogError(ex);
                throw new ApplicationException("Failed to write appointments to file.", ex);
            }
        }

        private List<Appointment> LoadAppointmentsFromFile()
        {
            try
            {
                if (!File.Exists(_appointmentsFilePath))
                    return new List<Appointment>();

                var appointments = new List<Appointment>();
                using (var reader = new StreamReader(_appointmentsFilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var parts = line.Split(',');

                        if (parts.Length == 5)
                        {
                            appointments.Add(new Appointment(
                                id: int.Parse(parts[0]),
                                client: new User(0, parts[1], ""),
                                professional: new Professional(0, parts[2], "", ""),
                                appointmentDate: DateTime.Parse(parts[3])
                            ));
                        }
                    }
                }
                return appointments;
            }
            catch (IOException ex)
            {
                LogError(ex);
                throw new ApplicationException("Failed to read appointments from file.", ex);
            }
        }

        private void LogError(Exception ex)
        {
            // Implement logging logic here (e.g., log to a file, database, or monitoring system)
            Console.WriteLine($"Error: {ex.Message}");
        }

    }
}
