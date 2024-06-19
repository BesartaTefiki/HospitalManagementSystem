import React, { useState, useEffect } from 'react';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import dayjs from 'dayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import Container from '@mui/material/Container';
import Typography from '@mui/material/Typography';
import { useParams, useNavigate } from 'react-router-dom';
import '../styles/UpdateAppointment.css';

function UpdateAppointment() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [doctor, setDoctor] = useState('');
  const [appointmentDate, setAppointmentDate] = useState(dayjs());
  const [error, setError] = useState(null);

  useEffect(() => {
    // Fetch the appointment details using the id from the URL
    const fetchAppointmentDetails = async () => {
      try {
        const response = await fetch(`https://localhost:7161/api/Appointment/${id}`);
        const data = await response.json();
        if (response.ok) {
          setEmail(data.patientEmail);
          setDoctor(data.doctorEmail);
          setAppointmentDate(dayjs(data.appointmentDate));
        } else {
          setError("Failed to fetch appointment details");
        }
      } catch (error) {
        setError("An error occurred. Please try again.");
      }
    };
    fetchAppointmentDetails();
  }, [id]);

  const handleOnSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await fetch(`https://localhost:7161/api/Appointment/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          PatientEmail: email,
          DoctorEmail: doctor,
          AppointmentDate: appointmentDate.toISOString(),
        }),
      });
      if (response.ok) {
        navigate('/appointmentlist');
      } else {
        setError("Failed to update appointment");
      }
    } catch (error) {
      setError("An error occurred. Please try again.");
    }
  };

  return (
    <div className="update-appointment-background">
      <Container maxWidth="sm" className="update-appointment-container">
        <Typography variant="h4" component="h1" gutterBottom>
          Update Appointment
        </Typography>
        <form onSubmit={handleOnSubmit} className="update-appointment-form">
          <TextField
            variant="outlined"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            label="Patient Email"
            fullWidth
            margin="normal"
          />
          <TextField
            variant="outlined"
            value={doctor}
            onChange={(e) => setDoctor(e.target.value)}
            label="Doctor Email"
            fullWidth
            margin="normal"
          />
          <LocalizationProvider dateAdapter={AdapterDayjs}>
            <DatePicker
              label="Appointment Date"
              value={appointmentDate}
              onChange={(newValue) => setAppointmentDate(newValue)}
              renderInput={(params) => <TextField {...params} fullWidth margin="normal" />}
            />
          </LocalizationProvider>
          {error && <Typography color="error">{error}</Typography>}
          <Button type="submit" variant="contained" color="primary" fullWidth>
            Update Appointment
          </Button>
        </form>
      </Container>
    </div>
  );
}

export default UpdateAppointment;
