// src/components/Navbar1.js
import React from 'react';
import { Link } from 'react-router-dom';

const Navbar2 = () => {
  return (
    <nav style={styles.navbar}>
      <h1 style={styles.logo}>Hospital Management System</h1>
      <ul style={styles.navLinks}>
        <li style={styles.navItem}>
          <Link to="/" style={styles.navLink}>Home</Link>
        </li>
        <li style={styles.navItem}>
          <Link to="/" style={styles.navLink}>Logout</Link>
        </li>
      </ul>
    </nav>
  );
}

const styles = {
  navbar: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    padding: '0.5rem 1rem',
    backgroundColor: '#333',
    color: '#fff',
  },
  logo: {
    margin: 0,
  },
  navLinks: {
    listStyleType: 'none',
    display: 'flex',
    margin: 0,
    padding: 0,
  },
  navItem: {
    marginLeft: '1rem',
  },
  navLink: {
    color: '#fff',
    textDecoration: 'none',
  },
};

export default Navbar2;
