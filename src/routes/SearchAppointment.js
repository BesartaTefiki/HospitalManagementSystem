import React, { useState } from 'react';
import { TextField, Button, Container, Typography, Grid, Card, CardContent } from '@mui/material';

const SearchAppointment = () => {
  const [email, setEmail] = useState('');
  const [appointments, setAppointments] = useState([]);

  const handleSearch = async () => {
    const token = localStorage.getItem('authToken');
    try {
      const response = await fetch(`https://localhost:7161/api/Appointment/search/byPatientEmail/${email}`, {
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });

      if (response.ok) {
        const data = await response.json();
        setAppointments(data);
      } else {
        console.error('Failed to fetch appointments');
        setAppointments([]);
      }
    } catch (error) {
      console.error('Error fetching appointments:', error);
      setAppointments([]);
    }
  };

  return (
    <Container>
      <Typography variant="h4" component="h1" gutterBottom>
        Search Appointments by Patient Email
      </Typography>
      <TextField
        label="Patient Email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        fullWidth
        margin="normal"
      />
      <Button variant="contained" color="primary" onClick={handleSearch}>
        Search
      </Button>
      <Grid container spacing={2} style={{ marginTop: '20px' }}>
        {appointments.map((appointment) => (
          <Grid item xs={12} sm={6} md={4} key={appointment.id}>
            <Card>
              <CardContent>
                <Typography variant="h5" component="div">
                  Patient: {appointment.patientEmail}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Doctor: {appointment.doctorEmail}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Date: {new Date(appointment.appointmentDate).toLocaleString()}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Status: {appointment.isCancelled ? "Cancelled" : "Active"}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Container>
  );
};

export default SearchAppointment;
