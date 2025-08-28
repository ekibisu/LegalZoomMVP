import React from "react";
const FormSection = ({ title, description, children }) => (
  <section className="mb-8">
    <h3 className="text-lg font-bold mb-2">{title}</h3>
    {description && <p className="text-gray-600 mb-4">{description}</p>}
    <div>{children}</div>
  </section>
);
export default FormSection;
