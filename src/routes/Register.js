import React, { useState } from 'react';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Container from '@mui/material/Container';
import Typography from '@mui/material/Typography';
import { useNavigate } from 'react-router-dom';
import '../styles/Register.css';

function Register({ onRegisterSuccess }) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const handleOnSubmit = async (e) => {
    e.preventDefault();

    const payload = { email, password };
    console.log("Request Payload:", payload); // Log the request payload

    try {
      const response = await fetch('https://localhost:7161/api/Auth/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload),
      });

      let data;
      const text = await response.text();
      try {
        data = JSON.parse(text);
      } catch {
        data = text;
      }

      console.log("Response Data:", data); // Log the response data

      if (response.ok) {
        onRegisterSuccess(data.token);
        setEmail("");
        setPassword("");
        setError(null);
        navigate('/login');
      } else {
        setError(typeof data === 'string' ? data : data.message || "Registration failed");
      }
    } catch (error) {
      console.error("Error during registration:", error); // Log the error
      setError("An error occurred. Please try again.");
    }
  };

  return (
    <div className="register-background">
      <Container maxWidth="sm" className="register-container">
        <Typography variant="h4" component="h1" gutterBottom>
          Register
        </Typography>
        <form onSubmit={handleOnSubmit} className="register-form">
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
          <Button type="submit" variant="contained" color="secondary" fullWidth>
            Register
          </Button>
        </form>
      </Container>
    </div>
  );
}

export default Register;
