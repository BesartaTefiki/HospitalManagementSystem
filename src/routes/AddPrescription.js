import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { TextField, Button, Container, Typography, Snackbar, Alert } from '@mui/material';
import '../styles/AddPrescription.css'; // Import the CSS file

const AddPrescription = () => {
  const [formData, setFormData] = useState({
    patientEmail: '',
    doctorEmail: '',
    details: '',
    date: ''
  });
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: '' });
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const token = localStorage.getItem('authToken');
    console.log("Token:", token); // Log the token

    const payload = {
      patientEmail: formData.patientEmail,
      doctorEmail: formData.doctorEmail,
      details: formData.details,
      date: new Date(formData.date).toISOString()
    };

    console.log("Submitting Prescription:", payload); // Log the request payload

    try {
      const response = await fetch('https://localhost:7161/api/Prescription', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify(payload),
      });

      console.log("Response status:", response.status); // Log the response status
      const responseBody = await response.text(); // Read response as text
      console.log("Response body:", responseBody); // Log the response body

      if (response.ok) {
        setSnackbar({ open: true, message: 'Prescription added successfully', severity: 'success' });
        setTimeout(() => navigate('/prescriptionslist'), 2000); // Redirect after 2 seconds
      } else {
        console.error('Failed to add prescription:', responseBody);
        setSnackbar({ open: true, message: `Failed to add prescription: ${responseBody}`, severity: 'error' });
      }
    } catch (error) {
      console.error('Error adding prescription:', error);
      setSnackbar({ open: true, message: 'Error adding prescription', severity: 'error' });
    }
  };

  return (
    <div className="add-prescription-container">
      <Container className="add-prescription-form">
        <Typography variant="h4" component="h1" gutterBottom>
          Add Prescription
        </Typography>
        <form onSubmit={handleSubmit}>
          <TextField
            label="Patient Email"
            name="patientEmail"
            value={formData.patientEmail}
            onChange={handleChange}
            fullWidth
            margin="normal"
          />
          <TextField
            label="Doctor Email"
            name="doctorEmail"
            value={formData.doctorEmail}
            onChange={handleChange}
            fullWidth
            margin="normal"
          />
          <TextField
            label="Details"
            name="details"
            value={formData.details}
            onChange={handleChange}
            fullWidth
            margin="normal"
          />
          <TextField
            label="Date"
            type="datetime-local"
            name="date"
            value={formData.date}
            onChange={handleChange}
            fullWidth
            margin="normal"
            InputLabelProps={{
              shrink: true,
            }}
          />
          <Button type="submit" variant="contained" color="primary" style={{ marginTop: '20px' }}>
            Add Prescription
          </Button>
        </form>
        <Snackbar
          open={snackbar.open}
          autoHideDuration={6000}
          onClose={() => setSnackbar({ ...snackbar, open: false })}
        >
          <Alert onClose={() => setSnackbar({ ...snackbar, open: false })} severity={snackbar.severity}>
            {snackbar.message}
          </Alert>
        </Snackbar>
      </Container>
    </div>
  );
};

export default AddPrescription;
