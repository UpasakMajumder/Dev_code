import React from 'react';
import { Link } from 'react-router-dom';

const MailProcessing = () => {
  return (
    <div>
      <h1>Thank you</h1>
      <Link to="/new-mailing.html">Create new mailing list</Link>
      <a href="#">See products</a>
    </div>
  );
};

export default MailProcessing;
