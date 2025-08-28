import React, { useState } from 'react';
import FormSection from './FormSection';
import Input from './Input';
import Checkbox from './Checkbox';
import Radio from './Radio';
import StateSelect from './StateSelect';

function App() {
  const [formData, setFormData] = useState({
    // Section 1: About You
    stateOfResidence: '',
    firstName: '',
    lastName: '',
    middleName: '',
    mobileNumber: '',
    isMarried: '',
    streetAddress: '',
    city: '',
    zipCode: '',

    // Section 2: Your Agent
    agentFirstName: '',
    agentLastName: '',
    agentCity: '',
    agentState: '',
    agentZipCode: '',
    alternateAgentName: '',

    // Section 3: Powers
    realEstate: false,
    personalProperty: false,
    banking: false,
    stocks: false,
    businessOperations: false,
    retirementPlans: false,
    insurance: false,
    estateTrusts: false,
    governmentAssistance: false,
    personalFamilyCare: false,
    makingGifts: false,
    petCare: false,

    // Section 4: Details
    effectiveDate: '',
    revokePrior: ''
  });

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prevData => ({
      ...prevData,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log('Form Data Submitted:', formData);
    // Here you would typically send the data to an API
  };

  return (
    <div className="form-container">
      <form onSubmit={handleSubmit}>
        <h1>Power of Attorney Form</h1>
        <p>Complete this form to create a Durable Power of Attorney.</p>

        {/* Section 1: About You */}
        <FormSection title="About You" description="Enter your personal information as it appears on your legal documents.">
          <StateSelect name="stateOfResidence" value={formData.stateOfResidence} onChange={handleChange} />
          <Input label="First Name" name="firstName" value={formData.firstName} onChange={handleChange} required />
          <Input label="Last Name" name="lastName" value={formData.lastName} onChange={handleChange} required />
          <Input label="Middle Name (Optional)" name="middleName" value={formData.middleName} onChange={handleChange} />
          <Input label="Mobile Number" name="mobileNumber" value={formData.mobileNumber} onChange={handleChange} type="tel" required />
          <div className="form-row">
            <label>Marital Status</label>
            <Radio label="Yes" name="isMarried" value="yes" checked={formData.isMarried === 'yes'} onChange={handleChange} />
            <Radio label="No" name="isMarried" value="no" checked={formData.isMarried === 'no'} onChange={handleChange} />
          </div>
          <Input label="Street Address" name="streetAddress" value={formData.streetAddress} onChange={handleChange} required />
          <Input label="City" name="city" value={formData.city} onChange={handleChange} required />
          <Input label="Zip Code" name="zipCode" value={formData.zipCode} onChange={handleChange} required />
        </FormSection>

        {/* Section 2: Your Agent */}
        <FormSection title="Your Agent" description="Enter the details of the person you are appointing as your agent.">
          <Input label="First Name" name="agentFirstName" value={formData.agentFirstName} onChange={handleChange} required />
          <Input label="Last Name" name="agentLastName" value={formData.agentLastName} onChange={handleChange} required />
          <Input label="City" name="agentCity" value={formData.agentCity} onChange={handleChange} required />
          <StateSelect label="State" name="agentState" value={formData.agentState} onChange={handleChange} />
          <Input label="Zip Code" name="agentZipCode" value={formData.agentZipCode} onChange={handleChange} required />
          <Input label="Alternate Agent Name (Optional)" name="alternateAgentName" value={formData.alternateAgentName} onChange={handleChange} />
        </FormSection>

        {/* Section 3: Powers */}
        <FormSection title="Powers" description="Select the specific powers you are granting your agent.">
          <Checkbox label="Real Estate Matters" name="realEstate" checked={formData.realEstate} onChange={handleChange} />
          <Checkbox label="Personal Property Matters" name="personalProperty" checked={formData.personalProperty} onChange={handleChange} />
          <Checkbox label="Banking Transactions" name="banking" checked={formData.banking} onChange={handleChange} />
          <Checkbox label="Stock and Bond Transactions" name="stocks" checked={formData.stocks} onChange={handleChange} />
          <Checkbox label="Business Operating Transactions" name="businessOperations" checked={formData.businessOperations} onChange={handleChange} />
          <Checkbox label="Retirement Plans" name="retirementPlans" checked={formData.retirementPlans} onChange={handleChange} />
          <Checkbox label="Insurance and Annuities" name="insurance" checked={formData.insurance} onChange={handleChange} />
          <Checkbox label="Estate and Trust Matters" name="estateTrusts" checked={formData.estateTrusts} onChange={handleChange} />
          <Checkbox label="Government Assistance" name="governmentAssistance" checked={formData.governmentAssistance} onChange={handleChange} />
          <Checkbox label="Personal and Family Care" name="personalFamilyCare" checked={formData.personalFamilyCare} onChange={handleChange} />
          <Checkbox label="Making Gifts" name="makingGifts" checked={formData.makingGifts} onChange={handleChange} />
          <Checkbox label="Pet and Animal Care" name="petCare" checked={formData.petCare} onChange={handleChange} />
        </FormSection>

        {/* Section 4: Details */}
        <FormSection title="Details" description="Specify when the Power of Attorney becomes effective and other details.">
          <div className="form-row">
            <p>When do the powers become effective?</p>
            <Radio label="Immediately upon signing" name="effectiveDate" value="immediately" checked={formData.effectiveDate === 'immediately'} onChange={handleChange} />
            <Radio label="Only if I become incapacitated" name="effectiveDate" value="incapacitated" checked={formData.effectiveDate === 'incapacitated'} onChange={handleChange} />
          </div>
          <div className="form-row">
            <p>Do you wish to revoke any prior powers of attorney?</p>
            <Radio label="Yes" name="revokePrior" value="yes" checked={formData.revokePrior === 'yes'} onChange={handleChange} />
            <Radio label="No" name="revokePrior" value="no" checked={formData.revokePrior === 'no'} onChange={handleChange} />
          </div>
        </FormSection>

        <button type="submit" className="submit-button">Submit</button>
      </form>
    </div>
  );
}

export default App;