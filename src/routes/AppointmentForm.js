// src/routes/AppointmentForm.js
import React, { useState } from 'react';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import dayjs from 'dayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import Container from '@mui/material/Container';
import Typography from '@mui/material/Typography';
import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';
import '../styles/AppointmentForm.css';
import Navbar2 from '../components/Navbar2';

function AppointmentForm() {
  const [email, setEmail] = useState('');
  const [doctor, setDoctor] = useState('');
  const [appointmentDate, setAppointmentDate] = useState(dayjs());
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState('');

  const handleOnSubmit = async () => {
    if (!email || !doctor || !appointmentDate) {
      console.error("All fields are required.");
      return;
    }

    const payload = { 
      PatientEmail: email,
      DoctorEmail: doctor,
      AppointmentDate: appointmentDate.toISOString() // Convert to ISO string
    };

    console.log("Request Payload:", payload); // Log the payload

    try {
      const response = await fetch('https://localhost:7161/api/appointment', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload)
      });

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`HTTP error! status: ${response.status}, details: ${errorText}`);
      }

      setEmail("");
      setDoctor("");
      setAppointmentDate(dayjs());
      console.log("Appointment booked successfully!");
      setSnackbarMessage('Your appointment is booked');
      setSnackbarOpen(true);

    } catch (error) {
      console.error("Failed to book appointment:", error);
      setSnackbarMessage('Failed to book appointment');
      setSnackbarOpen(true);
    }
  };

  return (
    <>
      <Navbar2 />
      <div className="appointment-background">
        <Container maxWidth="sm" className="appointment-container">
          <Typography variant="h4" component="h1" gutterBottom>
            Book an Appointment
          </Typography>
          <form className="appointment-form">
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
            <Button onClick={handleOnSubmit} variant="contained" color="primary" fullWidth>
              Book Appointment
            </Button>
          </form>
        </Container>
        <Snackbar
          open={snackbarOpen}
          autoHideDuration={6000}
          onClose={() => setSnackbarOpen(false)}
        >
          <Alert onClose={() => setSnackbarOpen(false)} severity={snackbarMessage === 'Your appointment is booked' ? 'success' : 'error'}>
            {snackbarMessage}
          </Alert>
        </Snackbar>
      </div>
    </>
  );
}

export default AppointmentForm;
