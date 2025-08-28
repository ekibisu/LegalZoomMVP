import React from 'react';

function Alert({ type = 'info', title, message, onClose }) {
  const baseClasses = "p-4 rounded-md border";
  const typeClasses = {
    success: "bg-green-50 border-green-200 text-green-800",
    error: "bg-red-50 border-red-200 text-red-800",
    warning: "bg-yellow-50 border-yellow-200 text-yellow-800",
    info: "bg-blue-50 border-blue-200 text-blue-800"
  };

  const iconClasses = {
    success: "text-green-400",
    error: "text-red-400", 
    warning: "text-yellow-400",
    info: "text-blue-400"
  };

  const icons = {
    success: "\u2713",
    error: "\u2715",
    warning: "\u26a0",
    info: "\u2139"
  };

  return (
    <div className={`${baseClasses} ${typeClasses[type]} relative`}>
      <div className="flex">
        <div className="flex-shrink-0">
          <span className={`text-xl ${iconClasses[type]}`}>
            {icons[type]}
          </span>
        </div>
        <div className="ml-3 flex-1">
          {title && (
            <h3 className="text-sm font-medium">
              {title}
            </h3>
          )}
          {message && (
            <div className={`${title ? 'mt-2' : ''} text-sm`}>
              {message}
            </div>
          )}
        </div>
        {onClose && (
          <div className="ml-auto pl-3">
            <button
              onClick={onClose}
              className="inline-flex text-gray-400 hover:text-gray-600 focus:outline-none focus:text-gray-600"
            >
              <span className="sr-only">Close</span>
              <svg className="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                <path fillRule="evenodd" d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z" clipRule="evenodd" />
              </svg>
            </button>
          </div>
        )}
      </div>
    </div>
  );
}

export default Alert;
