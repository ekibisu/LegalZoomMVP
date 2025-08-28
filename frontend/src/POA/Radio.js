import React from "react";
const Radio = ({ label, name, value, checked, onChange }) => (
  <label className="mr-4">
    <input type="radio" name={name} value={value} checked={checked} onChange={onChange} className="mr-1" />
    {label}
  </label>
);
export default Radio;
