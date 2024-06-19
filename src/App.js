import React, { useState } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import Login from './routes/Login';
import Home from './routes/Home';
import Register from './routes/Register';
import AppointmentHome from './routes/AppointmentHome';
import AppointmentForm from './routes/AppointmentForm';
import AppointmentList from './routes/AppointmentList';
import UpdateAppointment from './routes/UpdateAppointment';
import AddPrescription from './routes/AddPrescription';
import PrescriptionsList from './routes/PrescriptionsList';
import UpdatePrescription from './routes/UpdatePrescription'; // Add this import
import Navbar1 from './components/Navbar1';
import Navbar from './components/Navbar';
import SearchAppointment from './routes/SearchAppointment';

function App() {
  const [token, setToken] = useState(localStorage.getItem('authToken'));

  const handleLoginSuccess = (token) => {
    setToken(token);
    localStorage.setItem('authToken', token);
  };

  const handleRegisterSuccess = (token) => {
    setToken(token);
    localStorage.setItem('authToken', token);
  };

  return (
    <Router>
      <Routes>
        <Route path='/home' element={<><Navbar1 /><Home /></>} />
        <Route path='/appointmenthome' element={<><Navbar1 /><AppointmentHome /></>} />
        <Route path='/appointmentform' element={<><AppointmentForm /></>} />
        <Route path='/searchAppointment' element={<><Navbar /><SearchAppointment /></>} />
        <Route path='/appointmentlist' element={<><Navbar /><AppointmentList /></>} />
        <Route path='/update-appointment/:id' element={<><Navbar /><UpdateAppointment /></>} />
        <Route path='/add-prescription' element={<><Navbar /><AddPrescription /></>} />
        <Route path='/prescriptionslist' element={<><Navbar /><PrescriptionsList /></>} />
        <Route path='/update-prescription/:id' element={<><Navbar /><UpdatePrescription /></>} />
        <Route path="/login" element={<Login onLoginSuccess={handleLoginSuccess} />} />
        <Route path="/register" element={<Register onRegisterSuccess={handleRegisterSuccess} />} />
        <Route path="/" element={token ? <Navigate to="/home" /> : <Navigate to="/login" />} />
      </Routes>
    </Router>
  );
}

export default App;
