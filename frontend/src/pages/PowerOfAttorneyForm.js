import React, { useState } from "react";
import LoadingSpinner from "../components/LoadingSpinner";
import Alert from "../components/Alert";
import Input from "../POA/Input";
import Checkbox from "../POA/Checkbox";
import Radio from "../POA/Radio";
import StateSelect from "../POA/StateSelect";
import FormSection from "../POA/FormSection";

const initialState = {
  stateOfResidence: "",
  firstName: "",
  middleName: "",
  lastName: "",
  mobileNumber: "",
  isMarried: "false",
  streetAddress: "",
  city: "",
  state: "",
  zipCode: "",
  agentFirstName: "",
  agentLastName: "",
  agentCity: "",
  agentState: "",
  agentZipCode: "",
  alternateAgentName: "",
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
  effectiveDate: '',
  revokePrior: ''
};

const PowerOfAttorneyForm = () => {
  const [form, setForm] = useState(initialState);
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const validate = () => {
    const newErrors = {};
    if (!form.stateOfResidence) newErrors.stateOfResidence = "State required";
    if (!form.firstName) newErrors.firstName = "First name required";
    if (!form.lastName) newErrors.lastName = "Last name required";
    if (!form.mobileNumber) newErrors.mobileNumber = "Mobile number required";
    if (!form.streetAddress) newErrors.streetAddress = "Street address required";
    if (!form.city) newErrors.city = "City required";
    if (!form.state) newErrors.state = "State required";
    if (!form.zipCode) newErrors.zipCode = "Zip code required";
    if (!form.agentFirstName) newErrors.agentFirstName = "Agent first name required";
    if (!form.agentLastName) newErrors.agentLastName = "Agent last name required";
    if (!form.agentCity) newErrors.agentCity = "Agent city required";
    if (!form.agentState) newErrors.agentState = "Agent state required";
    if (!form.agentZipCode) newErrors.agentZipCode = "Agent zip code required";
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleChange = e => {
    const { name, value, type, checked } = e.target;
    setForm(prev => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value
    }));
  };

  const handleSubmit = async e => {
    e.preventDefault();
    if (!validate()) return;
    setLoading(true);
    setError("");
    setSuccess("");
    try {
      const { message } = await window.poaAPI.submitPOA(form);
      setSuccess(message || "POA form submitted successfully!");
      setForm(initialState);
    } catch (err) {
      setError(err.message || "Failed to submit POA form");
    } finally {
      setLoading(false);
    }
  };

  return (
  <div className="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
    <div className="max-w-2xl w-full space-y-8 bg-white p-8 rounded shadow">
      <h2 className="text-2xl font-bold text-center mb-6">Power of Attorney (POA) Form</h2>
      {error && <Alert type="error" message={error} onClose={() => setError("")} />}
      {success && <Alert type="success" message={success} onClose={() => setSuccess("")} />}
      {loading && <LoadingSpinner size="lg" />}
      <form className="space-y-6" onSubmit={handleSubmit}>
        {/* Section 1: About You */}
        <FormSection title="About You" description="Enter your personal information as it appears on your legal documents.">
          <StateSelect name="stateOfResidence" value={form.stateOfResidence} onChange={handleChange} />
          <Input label="First Name" name="firstName" value={form.firstName} onChange={handleChange} required />
          <Input label="Last Name" name="lastName" value={form.lastName} onChange={handleChange} required />
          <Input label="Middle Name (Optional)" name="middleName" value={form.middleName} onChange={handleChange} />
          <Input label="Mobile Number" name="mobileNumber" value={form.mobileNumber} onChange={handleChange} type="tel" required />
          <div className="form-row">
            <label>Marital Status</label>
            <Radio label="Yes" name="isMarried" value="yes" checked={form.isMarried === 'yes'} onChange={handleChange} />
            <Radio label="No" name="isMarried" value="no" checked={form.isMarried === 'no'} onChange={handleChange} />
          </div>
          <Input label="Street Address" name="streetAddress" value={form.streetAddress} onChange={handleChange} required />
          <Input label="City" name="city" value={form.city} onChange={handleChange} required />
          <Input label="Zip Code" name="zipCode" value={form.zipCode} onChange={handleChange} required />
        </FormSection>

        {/* Section 2: Your Agent */}
        <FormSection title="Your Agent" description="Enter the details of the person you are appointing as your agent.">
          <Input label="First Name" name="agentFirstName" value={form.agentFirstName} onChange={handleChange} required />
          <Input label="Last Name" name="agentLastName" value={form.agentLastName} onChange={handleChange} required />
          <Input label="City" name="agentCity" value={form.agentCity} onChange={handleChange} required />
          <StateSelect label="State" name="agentState" value={form.agentState} onChange={handleChange} />
          <Input label="Zip Code" name="agentZipCode" value={form.agentZipCode} onChange={handleChange} required />
          <Input label="Alternate Agent Name (Optional)" name="alternateAgentName" value={form.alternateAgentName} onChange={handleChange} />
        </FormSection>

        {/* Section 3: Powers */}
        <FormSection title="Powers" description="Select the specific powers you are granting your agent.">
          <Checkbox label="Real Estate Matters" name="realEstate" checked={form.realEstate} onChange={handleChange} />
          <Checkbox label="Personal Property Matters" name="personalProperty" checked={form.personalProperty} onChange={handleChange} />
          <Checkbox label="Banking Transactions" name="banking" checked={form.banking} onChange={handleChange} />
          <Checkbox label="Stock and Bond Transactions" name="stocks" checked={form.stocks} onChange={handleChange} />
          <Checkbox label="Business Operating Transactions" name="businessOperations" checked={form.businessOperations} onChange={handleChange} />
          <Checkbox label="Retirement Plans" name="retirementPlans" checked={form.retirementPlans} onChange={handleChange} />
          <Checkbox label="Insurance and Annuities" name="insurance" checked={form.insurance} onChange={handleChange} />
          <Checkbox label="Estate and Trust Matters" name="estateTrusts" checked={form.estateTrusts} onChange={handleChange} />
          <Checkbox label="Government Assistance" name="governmentAssistance" checked={form.governmentAssistance} onChange={handleChange} />
          <Checkbox label="Personal and Family Care" name="personalFamilyCare" checked={form.personalFamilyCare} onChange={handleChange} />
          <Checkbox label="Making Gifts" name="makingGifts" checked={form.makingGifts} onChange={handleChange} />
          <Checkbox label="Pet and Animal Care" name="petCare" checked={form.petCare} onChange={handleChange} />
        </FormSection>

        {/* Section 4: Details */}
        <FormSection title="Details" description="Specify when the Power of Attorney becomes effective and other details.">
          <div className="form-row">
            <label>When do the powers become effective?</label>
            <Radio label="Immediately upon signing" name="effectiveDate" value="immediately" checked={form.effectiveDate === 'immediately'} onChange={handleChange} />
            <Radio label="Only if I become incapacitated" name="effectiveDate" value="incapacitated" checked={form.effectiveDate === 'incapacitated'} onChange={handleChange} />
          </div>
          <div className="form-row">
            <label>Do you wish to revoke any prior powers of attorney?</label>
            <Radio label="Yes" name="revokePrior" value="yes" checked={form.revokePrior === 'yes'} onChange={handleChange} />
            <Radio label="No" name="revokePrior" value="no" checked={form.revokePrior === 'no'} onChange={handleChange} />
          </div>
          <Input label="Additional Instructions" name="additionalInstructions" value={form.additionalInstructions} onChange={handleChange} />
        </FormSection>

        <button type="submit" className="w-full bg-blue-600 text-white px-4 py-2 rounded font-semibold">Submit</button>
      </form>
    </div>
  </div>
 );
};

export default PowerOfAttorneyForm;
