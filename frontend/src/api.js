// POA API
export const poaAPI = {
  submitPOA: (data) => apiClient.post('/poa', data)
};
const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:52741/api';

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
  register: (email, password, firstName, lastName) => 
    apiClient.post('/auth/register', { email, password, firstName, lastName }),
  getProfile: () => apiClient.get('/auth/profile'),
  updateProfile: (updates) => apiClient.put('/auth/profile', updates)
};

// Forms API
export const formsAPI = {
  getTemplates: () => apiClient.get('/forms/templates'),
  getTemplate: (id) => apiClient.get(`/forms/templates/${id}`),
  getUserForms: () => apiClient.get('/forms/user-forms'),
  getUserForm: (id) => apiClient.get(`/forms/user-forms/${id}`),
  createUserForm: (data) => apiClient.post('/forms/user-forms', data),
  updateUserForm: (id, data) => apiClient.put(`/forms/user-forms/${id}`, data),
  exportToPdf: (id) => {
    const token = localStorage.getItem('token');
    return fetch(`${API_BASE_URL}/forms/user-forms/${id}/export`, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }
};

// Payment API
export const paymentAPI = {
  createCheckoutSession: (data) => apiClient.post('/payments/checkout-session', data),
  getPaymentHistory: () => apiClient.get('/payments/history')
};

// AI Assistant API
export const aiAPI = {
  getConversations: () => apiClient.get('/aiassistant/conversations'),
  getConversation: (id) => apiClient.get(`/aiassistant/conversations/${id}`),
  createConversation: (data) => apiClient.post('/aiassistant/conversations', data),
  sendMessage: (conversationId, message) => 
    apiClient.post(`/aiassistant/conversations/${conversationId}/messages`, { message }),
  deleteConversation: (id) => apiClient.delete(`/aiassistant/conversations/${id}`)
};

// Admin API
export const adminAPI = {
  getDashboard: () => apiClient.get('/admin/dashboard'),
  getUsers: (page = 1, pageSize = 20) => 
    apiClient.get(`/admin/users?page=${page}&pageSize=${pageSize}`),
  updateUserStatus: (id, isActive) => 
    apiClient.put(`/admin/users/${id}/status`, isActive),
  getAllFormTemplates: (includeInactive = false) => 
    apiClient.get(`/admin/form-templates?includeInactive=${includeInactive}`),
  createFormTemplate: (data) => apiClient.post('/admin/form-templates', data),
  updateFormTemplate: (id, data) => apiClient.put(`/admin/form-templates/${id}`, data),
  deleteFormTemplate: (id) => apiClient.delete(`/admin/form-templates/${id}`)
};