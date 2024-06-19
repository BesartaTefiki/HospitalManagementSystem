import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { TextField, Button, Container, Typography, Snackbar, Alert } from '@mui/material';
import '../styles/UpdatePrescription.css';

const UpdatePrescription = () => {
  const { id } = useParams();
  const [formData, setFormData] = useState({
    patientEmail: '',
    doctorEmail: '',
    details: '',
    date: ''
  });
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: '' });
  const navigate = useNavigate();

  useEffect(() => {
    const fetchPrescription = async () => {
      const token = localStorage.getItem('authToken');
      try {
        const response = await fetch(`https://localhost:7161/api/Prescription/${id}`, {
          headers: {
            'Authorization': `Bearer ${token}`,
          },
        });

        if (response.ok) {
          const data = await response.json();
          setFormData({
            patientEmail: data.patientEmail || '',
            doctorEmail: data.doctorEmail || '',
            details: data.details || '',
            date: data.date ? new Date(data.date).toISOString().slice(0, 16) : '' // Format date for datetime-local input
          });
        } else {
          console.error(`Failed to fetch prescription with id ${id}`);
        }
      } catch (error) {
        console.error('Error fetching prescription:', error);
      }
    };

    fetchPrescription();
  }, [id]);

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const token = localStorage.getItem('authToken');
    const payload = {
      patientEmail: formData.patientEmail,
      doctorEmail: formData.doctorEmail,
      details: formData.details,
      date: new Date(formData.date).toISOString()
    };

    try {
      const response = await fetch(`https://localhost:7161/api/Prescription/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify(payload),
      });

      const responseBody = await response.text();
      if (response.ok) {
        setSnackbar({ open: true, message: 'Prescription updated successfully', severity: 'success' });
        setTimeout(() => navigate('/prescriptionslist'), 2000); // Redirect after 2 seconds
      } else {
        console.error('Failed to update prescription:', responseBody);
        setSnackbar({ open: true, message: `Failed to update prescription: ${responseBody}`, severity: 'error' });
      }
    } catch (error) {
      console.error('Error updating prescription:', error);
      setSnackbar({ open: true, message: 'Error updating prescription', severity: 'error' });
    }
  };

  return (
    <div className="update-prescription-background">
      <Container maxWidth="sm" className="update-prescription-container">
        <Typography variant="h4" component="h1" gutterBottom>
          Update Prescription
        </Typography>
        <form onSubmit={handleSubmit} className="update-prescription-form">
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
          <Button type="submit" variant="contained" color="primary" fullWidth style={{ marginTop: '20px' }}>
            Update Prescription
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

export default UpdatePrescription;
