// Moved from src/api.js
const API_BASE_URL = process.env.REACT_APP_API_URL || 'https://localhost:52740/api';

class ApiClient {
  constructor(baseURL) {
    this.baseURL = baseURL;
  }

  async request(endpoint, options = {}) {
    const url = `${this.baseURL}${endpoint}`;
    const token = localStorage.getItem('token');

    const config = {
      headers: {
        'Content-Type': 'application/json',
        ...(token && { Authorization: `Bearer ${token}` }),
        ...options.headers
      },
      ...options
    };

    if (options.body && typeof options.body === 'object') {
      config.body = JSON.stringify(options.body);
    }

    const response = await fetch(url, config);

    if (!response.ok) {
      const error = await response.json().catch(() => ({ message: 'An error occurred' }));
      throw new Error(error.message || `HTTP ${response.status}`);
    }

    return response.json().catch(() => null);
  }

  get(endpoint, options = {}) {
    return this.request(endpoint, { method: 'GET', ...options });
  }

  post(endpoint, body, options = {}) {
    return this.request(endpoint, { method: 'POST', body, ...options });
  }

  put(endpoint, body, options = {}) {
    return this.request(endpoint, { method: 'PUT', body, ...options });
  }

  delete(endpoint, options = {}) {
    return this.request(endpoint, { method: 'DELETE', ...options });
  }
}

const apiClient = new ApiClient(API_BASE_URL);

// Auth API
export const authAPI = {
  login: (email, password) => apiClient.post('/auth/login', { email, password }),
  register: (payload) => apiClient.post('/auth/register', payload),
  getProfile: () => apiClient.get('/auth/profile'),
  updateProfile: (updates) => apiClient.put('/auth/profile', updates)
};

// Placeholder for payment API
export const paymentAPI = {
  getPayments: () => Promise.resolve([]),
  makePayment: (details) => Promise.resolve({ success: true }),
  getPaymentHistory: () => Promise.resolve([]) // Placeholder for dashboard
};

// Placeholder for forms API
export const formsAPI = {
  getForms: () => Promise.resolve([]),
  createForm: (form) => Promise.resolve({ success: true }),
  getUserForms: () => Promise.resolve([]) // Placeholder for dashboard
};

// Add other API modules as needed
// Advocate/Lawyer API
export const advocateAPI = {
  getProfile: () => apiClient.get('/advocate/profile'),
  updateProfile: (updates) => apiClient.put('/advocate/profile', updates),
  getCases: () => apiClient.get('/advocate/cases'),
  addCase: (caseData) => apiClient.post('/advocate/cases', caseData)
};
