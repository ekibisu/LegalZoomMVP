import React from 'react';
import './Input.css'; // You'll need to create this CSS file

function Input({ label, ...props }) {
  return (
    <div className="input-field">
      <label>{label}</label>
      <input {...props} />
    </div>
  );
}

export default Input;