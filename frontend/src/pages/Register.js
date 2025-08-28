import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../AuthContext';
import Alert from '../components/Alert';
import LoadingSpinner from '../components/LoadingSpinner';

function Register() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [userType, setUserType] = useState('customer');
  const [nationalId, setNationalId] = useState('');
  const [passportNumber, setPassportNumber] = useState('');
  const [gender, setGender] = useState('');
  const [lskP105, setLskP105] = useState('');
  const [mobileNumber, setMobileNumber] = useState('');
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const { register } = useAuth();
  const navigate = useNavigate();

  const validate = () => {
    const newErrors = {};
    if (!firstName) newErrors.firstName = 'First name required';
    if (!lastName) newErrors.lastName = 'Last name required';
    if (!email) newErrors.email = 'Email required';
    else if (!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(email)) newErrors.email = 'Invalid email';
    if (!password) newErrors.password = 'Password required';
    if (!mobileNumber) newErrors.mobileNumber = 'Mobile number required';
    if (!gender) newErrors.gender = 'Gender required';
    if (!lskP105) newErrors.lskP105 = 'LSK P105 required';
    if (userType === 'advocate') {
      if (!nationalId && !passportNumber) newErrors.nationalId = 'National ID or Passport Number required';
    } else {
      if (!nationalId && !passportNumber) newErrors.nationalId = 'National ID or Passport Number required';
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validate()) return;
    try {
      setError('');
      setLoading(true);
      const payload = {
        email,
        password,
        firstName,
        lastName,
        userType,
        nationalId,
        passportNumber,
        gender,
        lskP105,
        mobileNumber,
      };
      await register(payload);
      navigate('/dashboard');
    } catch (err) {
      setError(err.message || 'Failed to register');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-md w-full space-y-8">
        <div>
          <h2 className="mt-6 text-center text-3xl font-extrabold text-gray-900">
            Create your account
          </h2>
          <p className="mt-2 text-center text-sm text-gray-600">
            Or{' '}
            <Link
              to="/login"
              className="font-medium text-blue-600 hover:text-blue-500"
            >
              sign in to your account
            </Link>
          </p>
        </div>
        <form className="mt-8 space-y-6" onSubmit={handleSubmit}>
          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-700 mb-2">Register as:</label>
            <select
              value={userType}
              onChange={e => setUserType(e.target.value)}
              className="block w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
            >
              <option value="customer">Customer</option>
              <option value="advocate">Advocate / Lawyer</option>
            </select>
          </div>
          {error && (
            <Alert type="error" message={error} onClose={() => setError('')} />
          )}
          <div className="rounded-md shadow-sm -space-y-px">
            <div className="pb-4">
              <label htmlFor="firstName" className="block text-sm font-medium text-gray-700 mb-1">First Name</label>
              <input
                id="firstName"
                name="firstName"
                type="text"
                autoComplete="given-name"
                required
                value={firstName}
                onChange={e => setFirstName(e.target.value)}
                className="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="First Name"
              />
              {errors.firstName && <span className="text-red-500 text-xs">{errors.firstName}</span>}
            </div>
            <div className="pb-4">
              <label htmlFor="lastName" className="block text-sm font-medium text-gray-700 mb-1">Last Name</label>
              <input
                id="lastName"
                name="lastName"
                type="text"
                autoComplete="family-name"
                required
                value={lastName}
                onChange={e => setLastName(e.target.value)}
                className="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="Last Name"
              />
              {errors.lastName && <span className="text-red-500 text-xs">{errors.lastName}</span>}
            </div>
            <div className="pb-4">
              <label htmlFor="nationalId" className="block text-sm font-medium text-gray-700 mb-1">National ID</label>
              <input
                id="nationalId"
                name="nationalId"
                type="text"
                value={nationalId}
                onChange={e => setNationalId(e.target.value)}
                className="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="National ID"
              />
            </div>
            <div className="pb-4">
              <label htmlFor="passportNumber" className="block text-sm font-medium text-gray-700 mb-1">Passport Number</label>
              <input
                id="passportNumber"
                name="passportNumber"
                type="text"
                value={passportNumber}
                onChange={e => setPassportNumber(e.target.value)}
                className="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="Passport Number"
              />
              {errors.nationalId && <span className="text-red-500 text-xs">{errors.nationalId}</span>}
            </div>
            <div className="pb-4">
              <label htmlFor="gender" className="block text-sm font-medium text-gray-700 mb-1">Gender</label>
              <select
                id="gender"
                name="gender"
                required
                value={gender}
                onChange={e => setGender(e.target.value)}
                className="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              >
                <option value="">Select Gender</option>
                <option value="Male">Male</option>
                <option value="Female">Female</option>
              </select>
              {errors.gender && <span className="text-red-500 text-xs">{errors.gender}</span>}
            </div>
            {userType === 'advocate' && (
              <div className="pb-4">
                <label htmlFor="lskP105" className="block text-sm font-medium text-gray-700 mb-1">LSK P105</label>
                <input
                  id="lskP105"
                  name="lskP105"
                  type="text"
                  required
                  value={lskP105}
                  onChange={e => setLskP105(e.target.value)}
                  className="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  placeholder="LSK P105"
                />
                {errors.lskP105 && <span className="text-red-500 text-xs">{errors.lskP105}</span>}
              </div>
            )}
            <div className="pb-4">
              <label htmlFor="mobileNumber" className="block text-sm font-medium text-gray-700 mb-1">Mobile Number</label>
              <input
                id="mobileNumber"
                name="mobileNumber"
                type="text"
                required
                value={mobileNumber}
                onChange={e => setMobileNumber(e.target.value)}
                className="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="Mobile Number"
              />
              {errors.mobileNumber && <span className="text-red-500 text-xs">{errors.mobileNumber}</span>}
            </div>
            <div className="pb-4">
              <label htmlFor="email" className="block text-sm font-medium text-gray-700 mb-1">Email address</label>
              <input
                id="email"
                name="email"
                type="email"
                autoComplete="email"
                required
                value={email}
                onChange={e => setEmail(e.target.value)}
                className="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="Email address"
              />
            </div>
            <div className="pb-4">
              <label htmlFor="password" className="block text-sm font-medium text-gray-700 mb-1">Password</label>
              <input
                id="password"
                name="password"
                type="password"
                autoComplete="new-password"
                required
                value={password}
                onChange={e => setPassword(e.target.value)}
                className="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="Password"
              />
            </div>
          </div>
          <div>
            <button
              type="submit"
              disabled={loading}
              className="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
            >
              {loading ? <LoadingSpinner /> : 'Register'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default Register;
