import React, { useState } from 'react';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Container from '@mui/material/Container';
import Typography from '@mui/material/Typography';
import { Link, useNavigate } from 'react-router-dom';
import '../styles/Login.css';

function Login({ onLoginSuccess }) {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const handleOnSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await fetch('https://localhost:7161/api/auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
      });

      const data = await response.json();

      if (response.ok) {
        localStorage.setItem('userId', data.userId); // Store the user ID
        localStorage.setItem('roles', JSON.stringify(data.roles)); // Store roles
        onLoginSuccess(data.token, data.roles);
        setEmail('');
        setPassword('');
        setError(null);

        // Navigate based on roles
        if (data.roles.includes('Doctor')) {
          navigate('/appointmentlist'); // Redirect to admin page
        } else {
          navigate('/AppointmentForm'); // Redirect to user page
        }
      } else {
        setError(data.message || 'Login failed');
      }
    } catch (error) {
      setError('An error occurred. Please try again.');
      console.error('Login error:', error);
    }
  };

  return (
    <div className="login-background">
      <Container maxWidth="sm" className="login-container">
        <Typography variant="h4" component="h1" gutterBottom>
          Login
        </Typography>
        <form onSubmit={handleOnSubmit} className="login-form">
          <TextField
            variant="outlined"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            label="Email"
            fullWidth
            margin="normal"
          />
          <TextField
            variant="outlined"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            label="Password"
            fullWidth
            margin="normal"
          />
          {error && <Typography color="error">{error}</Typography>}
          <Button type="submit" variant="contained" color="primary" fullWidth>
            Login
          </Button>
            <Link to="/register">Dont Have an Account? Sign Up Here!</Link>
            </form>
      </Container>
    </div>
  );
}

export default Login;
