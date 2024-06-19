import { useEffect, useState } from "react";
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';
import '../styles/AppointmentList.css';

function AppointmentList() {
  const [appointments, setAppointments] = useState([]);
  const navigate = useNavigate();

  const fetchData = async () => {
    const token = localStorage.getItem('authToken');
    try {
      const response = await fetch("https://localhost:7161/api/Appointment", {
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });

      if (!response.ok) {
        console.error(`HTTP error! status: ${response.status}`);
        return;
      }

      const data = await response.json(); // Directly parse the JSON
      setAppointments(data);
    } catch (error) {
      console.error('Error fetching appointments:', error);
    }
  };

  const handleDelete = async (id) => {
    const token = localStorage.getItem('authToken');
    try {
      const response = await fetch(`https://localhost:7161/api/Appointment/${id}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });

      if (response.ok) {
        setAppointments(appointments.filter(appointment => appointment.id !== id));
      } else {
        console.error(`Failed to delete appointment with id ${id}`);
      }
    } catch (error) {
      console.error('Error deleting appointment:', error);
    }
  };

  const handleUpdate = (id) => {
    navigate(`/update-appointment/${id}`);
  };

  useEffect(() => {
    fetchData();
  }, []);

  return (
    <div className="appointment-list-container">
      <Grid container spacing={3}>
        {appointments?.map(appointment => (
          <Grid item xs={12} sm={6} md={4} key={appointment.id}>
            <Card className="appointment-card">
              <CardContent>
                <Typography gutterBottom variant="h5" component="div" className="appointment-card-title">
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
                <div className="appointment-card-actions">
                  <Button variant="contained" color="secondary" onClick={() => handleDelete(appointment.id)}>
                    Delete
                  </Button>
                  <Button variant="contained" color="primary" onClick={() => handleUpdate(appointment.id)} style={{ marginLeft: '10px' }}>
                    Update
                  </Button>
                </div>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    </div>
  );
}

export default AppointmentList;
