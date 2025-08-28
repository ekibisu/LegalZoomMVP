import React from "react";
const states = ["AL","AK","AZ","AR","CA","CO","CT","DE","FL","GA","HI","ID","IL","IN","IA","KS","KY","LA","ME","MD","MA","MI","MN","MS","MO","MT","NE","NV","NH","NJ","NM","NY","NC","ND","OH","OK","OR","PA","RI","SC","SD","TN","TX","UT","VT","VA","WA","WV","WI","WY"];
const StateSelect = ({ name, value, onChange, label = "State" }) => (
  <div className="mb-4">
    <label className="block text-gray-700 font-semibold mb-1" htmlFor={name}>{label}</label>
    <select
      className="w-full border rounded px-3 py-2"
      id={name}
      name={name}
      value={value}
      onChange={onChange}
    >
      <option value="">Select a state</option>
      {states.map(s => <option key={s} value={s}>{s}</option>)}
    </select>
  </div>
);
export default StateSelect;
