import React from 'react';
import './Checkbox.css'; // You'll need to create this CSS file

function Checkbox({ label, ...props }) {
  return (
    <div className="checkbox-field">
      <label>
        <input type="checkbox" {...props} />
        {label}
      </label>
    </div>
  );
}

export default Checkbox;