import React from 'react';

function FormField({ field, value, onChange, error }) {
  const baseClasses = "mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500";
  const errorClasses = error ? "border-red-500 focus:ring-red-500 focus:border-red-500" : "";

  const handleChange = (e) => {
    const newValue = field.type === 'checkbox' ? e.target.checked : e.target.value;
    onChange(field.name, newValue);
  };

  const renderField = () => {
    switch (field.type) {
      case 'text':
      case 'email':
      case 'tel':
        return (
          <input
            type={field.type}
            id={field.name}
            name={field.name}
            value={value || ''}
            onChange={handleChange}
            placeholder={field.placeholder}
            required={field.required}
            className={`${baseClasses} ${errorClasses}`}
          />
        );
      
      case 'number':
        return (
          <input
            type="number"
            id={field.name}
            name={field.name}
            value={value || ''}
            onChange={handleChange}
            placeholder={field.placeholder}
            required={field.required}
            className={`${baseClasses} ${errorClasses}`}
          />
        );
      
      case 'date':
        return (
          <input
            type="date"
            id={field.name}
            name={field.name}
            value={value || ''}
            onChange={handleChange}
            required={field.required}
            className={`${baseClasses} ${errorClasses}`}
          />
        );
      
      case 'textarea':
        return (
          <textarea
            id={field.name}
            name={field.name}
            value={value || ''}
            onChange={handleChange}
            placeholder={field.placeholder}
            required={field.required}
            rows={4}
            className={`${baseClasses} ${errorClasses}`}
          />
        );
      
      case 'select':
        return (
          <select
            id={field.name}
            name={field.name}
            value={value || ''}
            onChange={handleChange}
            required={field.required}
            className={`${baseClasses} ${errorClasses}`}
          >
            <option value="">Select an option</option>
            {field.options?.map((option) => (
              <option key={option} value={option}>
                {option}
              </option>
            ))}
          </select>
        );
      
      case 'checkbox':
        return (
          <div className="flex items-center">
            <input
              type="checkbox"
              id={field.name}
              name={field.name}
              checked={!!value}
              onChange={handleChange}
              required={field.required}
              className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
            />
            <label htmlFor={field.name} className="ml-2 block text-sm text-gray-900">
              {field.label}
            </label>
          </div>
        );
      
      case 'radio':
        return (
          <div className="space-y-2">
            {field.options?.map((option) => (
              <div key={option} className="flex items-center">
                <input
                  type="radio"
                  id={`${field.name}-${option}`}
                  name={field.name}
                  value={option}
                  checked={value === option}
                  onChange={handleChange}
                  required={field.required}
                  className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300"
                />
                <label htmlFor={`${field.name}-${option}`} className="ml-2 block text-sm text-gray-900">
                  {option}
                </label>
              </div>
            ))}
          </div>
        );
      
      default:
        return (
          <input
            type="text"
            id={field.name}
            name={field.name}
            value={value || ''}
            onChange={handleChange}
            placeholder={field.placeholder}
            required={field.required}
            className={`${baseClasses} ${errorClasses}`}
          />
        );
    }
  };

  return (
    <div className="mb-4">
      {field.type !== 'checkbox' && (
        <label htmlFor={field.name} className="block text-sm font-medium text-gray-700 mb-1">
          {field.label}
          {field.required && <span className="text-red-500 ml-1">*</span>}
        </label>
      )}
      
      {renderField()}
      
      {error && (
        <p className="mt-1 text-sm text-red-600">{error}</p>
      )}
    </div>
  );
}

export default FormField;