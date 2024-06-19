import React from "react";
import '../styles/Home.css';

const Home = () => {
  return (
    <div className="hero-backgroundHome">
      <div className="hero-content">
        <h1>Welcome to Our Hospital Management System</h1>
        <p>Providing quality healthcare services to our community.</p>
        <a href="/login">Login</a>
      </div>
    </div>
  );
}

export default Home;
