import React from 'react';

function FormTemplates() {
  return (
    <div className="max-w-2xl mx-auto p-6">
      <h2 className="text-xl font-bold mb-4">Browse Legal Forms</h2>
      <ul className="space-y-4">
        <li>
          <a href="/forms/power-of-attorney" className="text-blue-600 hover:underline font-semibold">
            Power of Attorney (POA)
          </a>
          <p className="text-gray-600 text-sm">Appoint an agent to act on your behalf for legal and financial matters.</p>
        </li>
        {/* Add more forms here as needed */}
      </ul>
    </div>
  );
}

export default FormTemplates;
