import React, { useState } from 'react';
import { advocateAPI } from '../services/api';

const AdvocatePage = () => {
  const [form, setForm] = useState({
    firstName: '',
    middleName: '',
    lastName: '',
    idNumber: '',
    gender: '',
    lskP105: '',
    mobileNumber: '',
    email: '',
    password: ''
  });
  const [errors, setErrors] = useState({});
  const [success, setSuccess] = useState('');
  const [apiError, setApiError] = useState('');

  const validate = () => {
    const newErrors = {};
    if (!form.firstName) newErrors.firstName = 'First name required';
    if (!form.lastName) newErrors.lastName = 'Last name required';
    if (!form.idNumber) newErrors.idNumber = 'ID/Passport required';
    if (!form.gender) newErrors.gender = 'Gender required';
    if (!form.lskP105) newErrors.lskP105 = 'LSK P105 required';
    if (!form.mobileNumber) newErrors.mobileNumber = 'Mobile required';
    if (!form.email) newErrors.email = 'Email required';
    else if (!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(form.email)) newErrors.email = 'Invalid email';
    if (!form.password) newErrors.password = 'Password required';
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleChange = e => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async e => {
    e.preventDefault();
    setSuccess('');
    setApiError('');
    if (!validate()) return;
    try {
      await advocateAPI.updateProfile(form);
      setSuccess('Profile updated successfully!');
    } catch (err) {
      setApiError(err.message || 'API error');
    }
  };

  return (
    <div className="advocate-page">
      <h1>Advocate / Lawyer Dashboard</h1>
      <form className="advocate-profile-form" onSubmit={handleSubmit}>
        <label>First Name<br />
          <input type="text" name="firstName" value={form.firstName} onChange={handleChange} />
          {errors.firstName && <span className="error">{errors.firstName}</span>}
        </label><br />
        <label>Middle Name<br />
          <input type="text" name="middleName" value={form.middleName} onChange={handleChange} />
        </label><br />
        <label>Last Name<br />
          <input type="text" name="lastName" value={form.lastName} onChange={handleChange} />
          {errors.lastName && <span className="error">{errors.lastName}</span>}
        </label><br />
        <label>National ID or Passport Number<br />
          <input type="text" name="idNumber" value={form.idNumber} onChange={handleChange} />
          {errors.idNumber && <span className="error">{errors.idNumber}</span>}
        </label><br />
        <label>Gender<br />
          <input type="text" name="gender" value={form.gender} onChange={handleChange} />
          {errors.gender && <span className="error">{errors.gender}</span>}
        </label><br />
        <label>LSK P105<br />
          <input type="text" name="lskP105" value={form.lskP105} onChange={handleChange} />
          {errors.lskP105 && <span className="error">{errors.lskP105}</span>}
        </label><br />
        <label>Mobile Number<br />
          <input type="text" name="mobileNumber" value={form.mobileNumber} onChange={handleChange} />
          {errors.mobileNumber && <span className="error">{errors.mobileNumber}</span>}
        </label><br />
        <label>Email Address<br />
          <input type="email" name="email" value={form.email} onChange={handleChange} />
          {errors.email && <span className="error">{errors.email}</span>}
        </label><br />
        <label>Password<br />
          <input type="password" name="password" value={form.password} onChange={handleChange} />
          {errors.password && <span className="error">{errors.password}</span>}
        </label><br />
        <button type="submit">Update Profile</button>
        {success && <div className="success">{success}</div>}
        {apiError && <div className="error">{apiError}</div>}
      </form>
      {/* Add sections for cases, appointments, etc. */}
    </div>
  );
};

export default AdvocatePage;
