import React from 'react';
import './Radio.css'; // You'll need to create this CSS file

function Radio({ label, ...props }) {
  return (
    <div className="radio-field">
      <label>
        <input type="radio" {...props} />
        {label}
      </label>
    </div>
  );
}

export default Radio;