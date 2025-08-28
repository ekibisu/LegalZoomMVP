import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../AuthContext';
import { formsAPI, paymentAPI } from '../services/api';
import LoadingSpinner from '../components/LoadingSpinner';
import Alert from '../components/Alert';

function Dashboard() {
  const { user } = useAuth();
  const [recentForms, setRecentForms] = useState([]);
  const [recentPayments, setRecentPayments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      const [formsData, paymentsData] = await Promise.all([
        formsAPI.getUserForms(),
        paymentAPI.getPaymentHistory()
      ]);
      
      setRecentForms(formsData.slice(0, 5));
      setRecentPayments(paymentsData.slice(0, 5));
    } catch (err) {
      setError('Failed to load dashboard data');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <LoadingSpinner size="xl" />
      </div>
    );
  }

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900">
          Welcome back, {user?.firstName}!
        </h1>
        <p className="text-gray-600 mt-2">
          Here's what's happening with your legal documents
        </p>
      </div>

      {error && (
        <Alert
          type="error"
          message={error}
          onClose={() => setError('')}
          className="mb-6"
        />
      )}

      {/* Quick Actions */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-6 mb-8">
        <Link
          to="/forms"
          className="bg-blue-600 hover:bg-blue-700 text-white p-6 rounded-lg shadow-lg transition-colors group"
        >
          <div className="flex items-center">
            <svg className="w-8 h-8 mr-3 group-hover:scale-110 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
            </svg>
            <div>
              <h3 className="font-semibold">Browse Forms</h3>
              <p className="text-blue-100 text-sm">Find legal documents</p>
            </div>
          </div>
        </Link>

        <Link
          to="/my-forms"
          className="bg-green-600 hover:bg-green-700 text-white p-6 rounded-lg shadow-lg transition-colors group"
        >
          <div className="flex items-center">
            <svg className="w-8 h-8 mr-3 group-hover:scale-110 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 5H7a2 2 0 00-2 2v10a2 2 0 002 2h8a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
            </svg>
            <div>
              <h3 className="font-semibold">My Forms</h3>
              <p className="text-green-100 text-sm">View your documents</p>
            </div>
          </div>
        </Link>

        <Link
          to="/ai-assistant"
          className="bg-purple-600 hover:bg-purple-700 text-white p-6 rounded-lg shadow-lg transition-colors group"
        >
          <div className="flex items-center">
            <svg className="w-8 h-8 mr-3 group-hover:scale-110 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M8 10h.01M12 10h.01M16 10h.01M9 16H5a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v8a2 2 0 01-2 2h-5l-5 5v-5z" />
            </svg>
            <div>
              <h3 className="font-semibold">AI Assistant</h3>
              <p className="text-purple-100 text-sm">Get legal help</p>
            </div>
          </div>
        </Link>

        <div className="bg-yellow-600 hover:bg-yellow-700 text-white p-6 rounded-lg shadow-lg transition-colors group cursor-pointer">
          <div className="flex items-center">
            <svg className="w-8 h-8 mr-3 group-hover:scale-110 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1" />
            </svg>
            <div>
              <h3 className="font-semibold">Upgrade</h3>
              <p className="text-yellow-100 text-sm">Get premium features</p>
            </div>
          </div>
        </div>
      </div>

      {/* Recent Activity */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
        {/* Recent Forms */}
        <div className="bg-white rounded-lg shadow-lg p-6">
          <div className="flex items-center justify-between mb-6">
            <h2 className="text-xl font-semibold text-gray-900">Recent Forms</h2>
            <Link to="/my-forms" className="text-blue-600 hover:text-blue-800 text-sm font-medium">
              View all →
            </Link>
          </div>
          
          {recentForms.length === 0 ? (
            <div className="text-center py-8">
              <svg className="w-12 h-12 text-gray-400 mx-auto mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
              </svg>
              <p className="text-gray-600">No forms created yet</p>
              <Link to="/forms" className="text-blue-600 hover:text-blue-800 text-sm font-medium mt-2 inline-block">
                Browse available forms
              </Link>
            </div>
          ) : (
            <div className="space-y-4">
              {recentForms.map((form) => (
                <div key={form.id} className="border border-gray-200 rounded-lg p-4">
                  <div className="flex items-center justify-between">
                    <div>
                      <h3 className="font-medium text-gray-900">{form.formTemplateName}</h3>
                      <p className="text-sm text-gray-600">
                        Created {new Date(form.createdAt).toLocaleDateString()}
                      </p>
                    </div>
                    <span className={`px-2 py-1 text-xs font-semibold rounded-full ${
                      form.status === 'Completed' 
                        ? 'bg-green-100 text-green-800'
                        : form.status === 'Draft'
                        ? 'bg-yellow-100 text-yellow-800'
                        : 'bg-blue-100 text-blue-800'
                    }`}>
                      {form.status}
                    </span>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>

        {/* Recent Payments */}
        <div className="bg-white rounded-lg shadow-lg p-6">
          <div className="flex items-center justify-between mb-6">
            <h2 className="text-xl font-semibold text-gray-900">Recent Payments</h2>
            <Link to="/payments" className="text-blue-600 hover:text-blue-800 text-sm font-medium">
              View all →
            </Link>
          </div>
          
          {recentPayments.length === 0 ? (
            <div className="text-center py-8">
              <svg className="w-12 h-12 text-gray-400 mx-auto mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M3 10h18M7 15h1m4 0h1m-7 4h12a3 3 0 003-3V8a3 3 0 00-3-3H6a3 3 0 00-3 3v8a3 3 0 003 3z" />
              </svg>
              <p className="text-gray-600">No payments yet</p>
            </div>
          ) : (
            <div className="space-y-4">
              {recentPayments.map((payment) => (
                <div key={payment.id} className="border border-gray-200 rounded-lg p-4">
                  <div className="flex items-center justify-between">
                    <div>
                      <h3 className="font-medium text-gray-900">
                        {payment.formTemplateName || payment.subscriptionPlan}
                      </h3>
                      <p className="text-sm text-gray-600">
                        {new Date(payment.createdAt).toLocaleDateString()}
                      </p>
                    </div>
                    <div className="text-right">
                      <p className="font-semibold text-gray-900">
                        ${payment.amount.toFixed(2)}
                      </p>
                      <span className={`px-2 py-1 text-xs font-semibold rounded-full ${
                        payment.status === 'Completed' 
                          ? 'bg-green-100 text-green-800'
                          : 'bg-yellow-100 text-yellow-800'
                      }`}>
                        {payment.status}
                      </span>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>

      {/* Subscription Status */}
      {user?.hasActiveSubscription && (
        <div className="mt-8 bg-blue-50 border border-blue-200 rounded-lg p-6">
          <div className="flex items-center">
            <svg className="w-6 h-6 text-blue-600 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
            <div>
              <h3 className="text-lg font-semibold text-blue-900">Premium Member</h3>
              <p className="text-blue-700">You have access to all premium features and forms.</p>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default Dashboard;