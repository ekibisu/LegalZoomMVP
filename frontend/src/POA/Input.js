import React from "react";
const Input = ({ label, name, value, onChange, type = "text", required = false }) => (
  <div className="mb-4">
    <label className="block text-gray-700 font-semibold mb-1" htmlFor={name}>{label}</label>
    <input
      className="w-full border rounded px-3 py-2"
      id={name}
      name={name}
      value={value}
      onChange={onChange}
      type={type}
      required={required}
    />
  </div>
);
export default Input;
