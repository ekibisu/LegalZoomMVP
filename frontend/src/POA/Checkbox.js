import React from "react";
const Checkbox = ({ label, name, checked, onChange }) => (
  <label className="flex items-center mb-2">
    <input type="checkbox" name={name} checked={checked} onChange={onChange} className="mr-2" />
    {label}
  </label>
);
export default Checkbox;
