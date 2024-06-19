import { useEffect, useState } from "react";
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';
import '../styles/PrescriptionsList.css';

function PrescriptionsList() {
  const [prescriptions, setPrescriptions] = useState([]);
  const navigate = useNavigate();

  const fetchData = async () => {
    const token = localStorage.getItem('authToken');
    try {
      const response = await fetch("https://localhost:7161/api/Prescription", {
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });

      if (!response.ok) {
        console.error(`HTTP error! status: ${response.status}`);
        return;
      }

      const data = await response.json(); // Directly parse the JSON
      setPrescriptions(data);
    } catch (error) {
      console.error('Error fetching prescriptions:', error);
    }
  };

  const handleDelete = async (id) => {
    const token = localStorage.getItem('authToken');
    try {
      const response = await fetch(`https://localhost:7161/api/Prescription/${id}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });

      if (response.ok) {
        setPrescriptions(prescriptions.filter(prescription => prescription.id !== id));
      } else {
        console.error(`Failed to delete prescription with id ${id}`);
      }
    } catch (error) {
      console.error('Error deleting prescription:', error);
    }
  };

  const handleUpdate = (id) => {
    navigate(`/update-prescription/${id}`);
  };

  useEffect(() => {
    fetchData();
  }, []);

  return (
    <div className="prescriptions-list-container">
      <Grid container spacing={3}>
        {prescriptions?.map(prescription => (
          <Grid item xs={12} sm={6} md={4} key={prescription.id}>
            <Card className="prescription-card">
              <CardContent>
                <Typography gutterBottom variant="h5" component="div" className="prescription-card-title">
                  Patient: {prescription.patientEmail}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Doctor: {prescription.doctorEmail}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Details: {prescription.details}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Date: {new Date(prescription.date).toLocaleString()}
                </Typography>
                <div className="prescription-card-actions">
                  <Button variant="contained" color="secondary" onClick={() => handleDelete(prescription.id)}>
                    Delete
                  </Button>
                  <Button variant="contained" color="primary" onClick={() => handleUpdate(prescription.id)} style={{ marginLeft: '10px' }}>
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

export default PrescriptionsList;
