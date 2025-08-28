import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './AuthContext';
import { useAuth } from './AuthContext';
import { PaymentProvider } from './PaymentContext';
import Navbar from './components/Navbar';
import PowerOfAttorneyForm from './pages/PowerOfAttorneyForm';
import Footer from './components/Footer';
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import Dashboard from './pages/Dashboard';
import FormTemplates from './pages/FormTemplates';
import FormBuilder from './pages/FormBuilder';
import MyForms from './pages/MyForms';
import AIAssistant from './pages/AIAssistant';
import PaymentSuccess from './pages/PaymentSuccess';
import PaymentCancel from './pages/PaymentCancel';
import AdminDashboard from './pages/admin/AdminDashboard';
import AdminUsers from './pages/admin/AdminUsers';
import AdminForms from './pages/admin/AdminForms';
import Payments from './pages/Payments';
import './App.css';

function ProtectedRoute({ children, adminOnly = false }) {
  const { user, loading } = useAuth();
  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-blue-500"></div>
      </div>
    );
  }
  if (!user) {
    window.location.href = '/login';
    return null;
  }
  if (adminOnly && user.role !== 'Admin') {
    window.location.href = '/dashboard';
    return null;
  }
  return children;
}

function App() {
  return (
    <AuthProvider>
      <PaymentProvider>
        <div className="min-h-screen bg-gray-50 flex flex-col">
          <Navbar />
          <main className="flex-grow">
            <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<Register />} />
              <Route path="/forms" element={<FormTemplates />} />
              <Route path="/forms/power-of-attorney" element={<ProtectedRoute><PowerOfAttorneyForm /></ProtectedRoute>} />
              <Route path="/dashboard" element={<ProtectedRoute><Dashboard /></ProtectedRoute>} />
              <Route path="/form-builder" element={<FormBuilder />} />
              <Route path="/my-forms" element={<MyForms />} />
              <Route path="/ai-assistant" element={<ProtectedRoute><AIAssistant /></ProtectedRoute>} />
              <Route path="/payment-success" element={<ProtectedRoute><PaymentSuccess /></ProtectedRoute>} />
              <Route path="/payment-cancel" element={<ProtectedRoute><PaymentCancel /></ProtectedRoute>} />
              <Route path="/payments" element={<Payments />} />
              <Route path="/admin" element={<Navigate to="/admin/dashboard" replace />} />
              <Route path="/admin/dashboard" element={<ProtectedRoute adminOnly={true}><AdminDashboard /></ProtectedRoute>} />
              <Route path="/admin/users" element={<ProtectedRoute adminOnly={true}><AdminUsers /></ProtectedRoute>} />
              <Route path="/admin/forms" element={<ProtectedRoute adminOnly={true}><AdminForms /></ProtectedRoute>} />
            </Routes>
          </main>
          <Footer />
        </div>
      </PaymentProvider>
    </AuthProvider>
  );
}

export default App;