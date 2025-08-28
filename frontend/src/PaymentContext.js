import React, { createContext, useContext } from 'react';
import { loadStripe } from '@stripe/stripe-js';
import { paymentAPI } from './services/api';

const PaymentContext = createContext();

export function usePayment() {
  const context = useContext(PaymentContext);
  if (!context) {
    throw new Error('usePayment must be used within a PaymentProvider');
  }
  return context;
}

export function PaymentProvider({ children }) {
  const purchaseForm = async (formTemplateId) => {
    try {
      const session = await paymentAPI.createCheckoutSession({ formTemplateId });
      const stripe = await loadStripe(session.publishableKey);
      
      const result = await stripe.redirectToCheckout({
        sessionId: session.sessionId
      });

      if (result.error) {
        throw new Error(result.error.message);
      }
    } catch (error) {
      throw error;
    }
  };

  const subscribe = async (subscriptionPlan) => {
    try {
      const session = await paymentAPI.createCheckoutSession({ subscriptionPlan });
      const stripe = await loadStripe(session.publishableKey);
      
      const result = await stripe.redirectToCheckout({
        sessionId: session.sessionId
      });

      if (result.error) {
        throw new Error(result.error.message);
      }
    } catch (error) {
      throw error;
    }
  };

  const value = {
    purchaseForm,
    subscribe
  };

  return (
    <PaymentContext.Provider value={value}>
      {children}
    </PaymentContext.Provider>
  );
}
