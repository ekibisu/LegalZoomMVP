import React from 'react';

function Footer() {
  return (
    <footer className="bg-gray-800 text-white">
      <div className="max-w-7xl mx-auto py-8 px-4">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-8">
          <div>
            <h3 className="text-lg font-semibold mb-4">LegalZoom MVP</h3>
            <p className="text-gray-300 text-sm">
              Simplifying legal document creation with AI-powered assistance.
            </p>
          </div>
          
          <div>
            <h3 className="text-lg font-semibold mb-4">Services</h3>
            <ul className="space-y-2 text-sm">
              <li><a href="#" className="text-gray-300 hover:text-white">Business Formation</a></li>
              <li><a href="#" className="text-gray-300 hover:text-white">Contracts</a></li>
              <li><a href="#" className="text-gray-300 hover:text-white">Estate Planning</a></li>
              <li><a href="#" className="text-gray-300 hover:text-white">AI Legal Assistant</a></li>
            </ul>
          </div>
          
          <div>
            <h3 className="text-lg font-semibold mb-4">Support</h3>
            <ul className="space-y-2 text-sm">
              <li><a href="#" className="text-gray-300 hover:text-white">Help Center</a></li>
              <li><a href="#" className="text-gray-300 hover:text-white">Contact Us</a></li>
              <li><a href="#" className="text-gray-300 hover:text-white">FAQ</a></li>
              <li><a href="#" className="text-gray-300 hover:text-white">Live Chat</a></li>
            </ul>
          </div>
          
          <div>
            <h3 className="text-lg font-semibold mb-4">Legal</h3>
            <ul className="space-y-2 text-sm">
              <li><a href="#" className="text-gray-300 hover:text-white">Privacy Policy</a></li>
              <li><a href="#" className="text-gray-300 hover:text-white">Terms of Service</a></li>
              <li><a href="#" className="text-gray-300 hover:text-white">Disclaimer</a></li>
            </ul>
          </div>
        </div>
        
        <div className="border-t border-gray-700 mt-8 pt-8 text-center text-sm text-gray-300">
          <p>&copy; 2024 LegalZoom MVP. All rights reserved.</p>
          <p className="mt-2">
            This platform provides legal information, not legal advice. Consult with qualified attorneys for specific legal matters.
          </p>
        </div>
      </div>
    </footer>
  );
}

export default Footer;