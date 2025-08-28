-- LegalZoom MVP Complete Database Creation Script
-- This script creates the complete database schema and inserts sample data
-- Run this script directly in your PostgreSQL client (pgAdmin, psql, etc.)

-- Drop database if exists and recreate (optional - uncomment if needed)
-- DROP DATABASE IF EXISTS legalzoommvp;
-- CREATE DATABASE legalzoommvp;

-- Connect to the legalzoommvp database before running the rest
-- \c legalzoommvp;

-- Create ENUM types
DO $$ 
BEGIN
    -- Drop existing types if they exist
    DROP TYPE IF EXISTS user_role CASCADE;
    DROP TYPE IF EXISTS form_status CASCADE;
    DROP TYPE IF EXISTS payment_status CASCADE;
    DROP TYPE IF EXISTS payment_type CASCADE;
    DROP TYPE IF EXISTS subscription_status CASCADE;
    DROP TYPE IF EXISTS message_role CASCADE;

    -- Create ENUM types
    CREATE TYPE user_role AS ENUM ('Client', 'Admin');
    CREATE TYPE form_status AS ENUM ('Draft', 'Completed', 'Exported');
    CREATE TYPE payment_status AS ENUM ('Pending', 'Completed', 'Failed', 'Refunded');
    CREATE TYPE payment_type AS ENUM ('OneTime', 'Subscription');
    CREATE TYPE subscription_status AS ENUM ('Active', 'Cancelled', 'Expired', 'PastDue');
    CREATE TYPE message_role AS ENUM ('User', 'Assistant', 'System');
END $$;

-- Drop existing tables if they exist (in correct order due to foreign keys)
DROP TABLE IF EXISTS "AIMessages" CASCADE;
DROP TABLE IF EXISTS "AIConversations" CASCADE;
DROP TABLE IF EXISTS "Payments" CASCADE;
DROP TABLE IF EXISTS "UserForms" CASCADE;
DROP TABLE IF EXISTS "Subscriptions" CASCADE;
DROP TABLE IF EXISTS "FormTemplates" CASCADE;
DROP TABLE IF EXISTS "PowerOfAttorney" CASCADE;
DROP TABLE IF EXISTS "Users" CASCADE;

-- Create update trigger function
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW."UpdatedAt" = NOW();
    RETURN NEW;
END;
$$ language 'plpgsql';

-- Users table
CREATE TABLE "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Email" VARCHAR(255) NOT NULL UNIQUE,
    "PasswordHash" VARCHAR(255) NOT NULL,
    "FirstName" VARCHAR(100) NOT NULL,
    "LastName" VARCHAR(100) NOT NULL,
    "Role" INTEGER NOT NULL DEFAULT 0, -- 0 = Client, 1 = Admin
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE
);

-- FormTemplates table
CREATE TABLE "FormTemplates" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,
    "Description" TEXT NOT NULL DEFAULT '',
    "Category" VARCHAR(100) NOT NULL,
    "Price" DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    "IsPremium" BOOLEAN NOT NULL DEFAULT FALSE,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "FormSchema" JSONB NOT NULL,
    "HtmlTemplate" TEXT NOT NULL DEFAULT '',
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    "CreatedByUserId" INTEGER NOT NULL,
    "Status" INTEGER,
    FOREIGN KEY ("CreatedByUserId") REFERENCES "Users"("Id") ON DELETE RESTRICT
);

-- Subscriptions table
CREATE TABLE "Subscriptions" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL,
    "StripeSubscriptionId" VARCHAR(255) NOT NULL UNIQUE,
    "PlanName" VARCHAR(100) NOT NULL,
    "MonthlyPrice" DECIMAL(10,2) NOT NULL,
    "StartDate" TIMESTAMP WITH TIME ZONE NOT NULL,
    "EndDate" TIMESTAMP WITH TIME ZONE,
    "NextBillingDate" TIMESTAMP WITH TIME ZONE NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "Status" INTEGER, -- 0 = Active, 1 = Cancelled, 2 = Expired, 3 = PastDue
    FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
);

-- Payments table
CREATE TABLE "Payments" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL,
    "StripePaymentIntentId" VARCHAR(255) NOT NULL UNIQUE,
    "Amount" DECIMAL(10,2) NOT NULL,
    "Currency" VARCHAR(3) NOT NULL DEFAULT 'USD',
    "FormTemplateId" INTEGER,
    "SubscriptionId" INTEGER,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "CompletedAt" TIMESTAMP WITH TIME ZONE,
    "Type" INTEGER, -- 0 = OneTime, 1 = Subscription
    "Status" INTEGER, -- 0 = Pending, 1 = Completed, 2 = Failed, 3 = Refunded
    FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE,
    FOREIGN KEY ("FormTemplateId") REFERENCES "FormTemplates"("Id") ON DELETE SET NULL,
    FOREIGN KEY ("SubscriptionId") REFERENCES "Subscriptions"("Id") ON DELETE SET NULL
);

-- UserForms table
CREATE TABLE "UserForms" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL,
    "FormTemplateId" INTEGER NOT NULL,
    "FormData" JSONB NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "CompletedAt" TIMESTAMP WITH TIME ZONE,
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    "Status" INTEGER, -- 0 = Draft, 1 = Completed, 2 = Exported
    FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE,
    FOREIGN KEY ("FormTemplateId") REFERENCES "FormTemplates"("Id") ON DELETE RESTRICT
);

-- AIConversations table
CREATE TABLE "AIConversations" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL,
    "Title" VARCHAR(255) NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
);

-- AIMessages table
CREATE TABLE "AIMessages" (
    "Id" SERIAL PRIMARY KEY,
    "ConversationId" INTEGER NOT NULL,
    "Content" TEXT NOT NULL,
    "Role" INTEGER NOT NULL, -- 0 = User, 1 = Assistant, 2 = System
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    FOREIGN KEY ("ConversationId") REFERENCES "AIConversations"("Id") ON DELETE CASCADE
);

-- PowerOfAttorney table (from EF Core migration)
CREATE TABLE "PowerOfAttorney" (
    "Id" SERIAL PRIMARY KEY,
    "PrincipalName" VARCHAR(255) NOT NULL,
    "PrincipalAddress" VARCHAR(500),
    "PrincipalCity" VARCHAR(100),
    "PrincipalState" VARCHAR(50),
    "PrincipalZipCode" VARCHAR(20),
    "AgentName" VARCHAR(255) NOT NULL,
    "AgentAddress" VARCHAR(500),
    "AgentCity" VARCHAR(100),
    "AgentState" VARCHAR(50),
    "AgentZipCode" VARCHAR(20),
    "SuccessorAgentName" VARCHAR(255),
    "SuccessorAgentAddress" VARCHAR(500),
    "SuccessorAgentCity" VARCHAR(100),
    "SuccessorAgentState" VARCHAR(50),
    "SuccessorAgentZipCode" VARCHAR(20),
    "EffectiveDate" DATE,
    "ExpirationDate" DATE,
    "IsDurable" BOOLEAN NOT NULL DEFAULT FALSE,
    "IsImmediatelyEffective" BOOLEAN NOT NULL DEFAULT TRUE,
    "BankingPowers" BOOLEAN NOT NULL DEFAULT FALSE,
    "RealEstatePowers" BOOLEAN NOT NULL DEFAULT FALSE,
    "LegalPowers" BOOLEAN NOT NULL DEFAULT FALSE,
    "TaxPowers" BOOLEAN NOT NULL DEFAULT FALSE,
    "InsurancePowers" BOOLEAN NOT NULL DEFAULT FALSE,
    "BusinessPowers" BOOLEAN NOT NULL DEFAULT FALSE,
    "SpecialInstructions" TEXT,
    "NotaryRequired" BOOLEAN NOT NULL DEFAULT TRUE,
    "WitnessRequired" BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    "UserId" INTEGER,
    FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE SET NULL
);

-- Create indexes for better performance
CREATE INDEX "IX_Users_Email" ON "Users" ("Email");
CREATE INDEX "IX_Users_IsActive" ON "Users" ("IsActive");
CREATE INDEX "IX_FormTemplates_Category" ON "FormTemplates" ("Category");
CREATE INDEX "IX_FormTemplates_IsActive" ON "FormTemplates" ("IsActive");
CREATE INDEX "IX_FormTemplates_IsPremium" ON "FormTemplates" ("IsPremium");
CREATE INDEX "IX_FormTemplates_CreatedByUserId" ON "FormTemplates" ("CreatedByUserId");
CREATE INDEX "IX_Subscriptions_UserId" ON "Subscriptions" ("UserId");
CREATE INDEX "IX_Subscriptions_NextBillingDate" ON "Subscriptions" ("NextBillingDate");
CREATE INDEX "IX_Payments_UserId" ON "Payments" ("UserId");
CREATE INDEX "IX_Payments_FormTemplateId" ON "Payments" ("FormTemplateId");
CREATE INDEX "IX_Payments_SubscriptionId" ON "Payments" ("SubscriptionId");
CREATE INDEX "IX_Payments_CreatedAt" ON "Payments" ("CreatedAt");
CREATE INDEX "IX_UserForms_UserId" ON "UserForms" ("UserId");
CREATE INDEX "IX_UserForms_FormTemplateId" ON "UserForms" ("FormTemplateId");
CREATE INDEX "IX_UserForms_CreatedAt" ON "UserForms" ("CreatedAt");
CREATE INDEX "IX_AIConversations_UserId" ON "AIConversations" ("UserId");
CREATE INDEX "IX_AIConversations_CreatedAt" ON "AIConversations" ("CreatedAt");
CREATE INDEX "IX_AIMessages_ConversationId" ON "AIMessages" ("ConversationId");
CREATE INDEX "IX_AIMessages_CreatedAt" ON "AIMessages" ("CreatedAt");
CREATE INDEX "IX_AIMessages_Role" ON "AIMessages" ("Role");

-- Create update triggers
CREATE TRIGGER update_users_updated_at BEFORE UPDATE ON "Users" FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
CREATE TRIGGER update_form_templates_updated_at BEFORE UPDATE ON "FormTemplates" FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
CREATE TRIGGER update_user_forms_updated_at BEFORE UPDATE ON "UserForms" FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
CREATE TRIGGER update_ai_conversations_updated_at BEFORE UPDATE ON "AIConversations" FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
CREATE TRIGGER update_power_of_attorney_updated_at BEFORE UPDATE ON "PowerOfAttorney" FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

-- Insert sample users
-- Password for all test users is: "TestPassword123!" (bcrypt hash)
INSERT INTO "Users" ("Email", "PasswordHash", "FirstName", "LastName", "Role", "CreatedAt", "IsActive") VALUES
('admin@legalzoom.com', '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LwDVKLV8j8qV8wQQW', 'Admin', 'User', 1, NOW(), true),
('john.doe@email.com', '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LwDVKLV8j8qV8wQQW', 'John', 'Doe', 0, NOW(), true),
('jane.smith@email.com', '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LwDVKLV8j8qV8wQQW', 'Jane', 'Smith', 0, NOW(), true),
('michael.johnson@email.com', '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LwDVKLV8j8qV8wQQW', 'Michael', 'Johnson', 0, NOW(), true),
('sarah.wilson@email.com', '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LwDVKLV8j8qV8wQQW', 'Sarah', 'Wilson', 0, NOW(), true);

-- Insert sample form templates
INSERT INTO "FormTemplates" ("Name", "Description", "Category", "Price", "IsPremium", "IsActive", "FormSchema", "HtmlTemplate", "CreatedAt", "CreatedByUserId") VALUES
(
    'Power of Attorney - General',
    'A comprehensive power of attorney form for general financial and legal matters.',
    'Legal Documents',
    29.99,
    false,
    true,
    '{
        "title": "Power of Attorney - General",
        "fields": [
            {
                "id": "principal_name",
                "type": "text",
                "label": "Principal Full Name",
                "required": true,
                "placeholder": "Enter the principal''s full legal name"
            },
            {
                "id": "principal_address",
                "type": "textarea",
                "label": "Principal Address",
                "required": true,
                "placeholder": "Enter the principal''s full address"
            },
            {
                "id": "agent_name",
                "type": "text",
                "label": "Agent Full Name",
                "required": true,
                "placeholder": "Enter the agent''s full legal name"
            },
            {
                "id": "agent_address",
                "type": "textarea",
                "label": "Agent Address",
                "required": true,
                "placeholder": "Enter the agent''s full address"
            },
            {
                "id": "powers_granted",
                "type": "checkbox_group",
                "label": "Powers Granted",
                "required": true,
                "options": [
                    "Banking and financial transactions",
                    "Real estate transactions",
                    "Legal proceedings",
                    "Tax matters",
                    "Insurance matters",
                    "Business operations"
                ]
            },
            {
                "id": "effective_date",
                "type": "date",
                "label": "Effective Date",
                "required": true
            },
            {
                "id": "durable",
                "type": "checkbox",
                "label": "This power of attorney shall remain in effect if I become incapacitated",
                "required": false
            }
        ]
    }',
    '<div class="form-template"><h1>{{title}}</h1>{{fields}}</div>',
    NOW(),
    1
),
(
    'Will and Testament - Basic',
    'A basic last will and testament template for simple estates.',
    'Estate Planning',
    49.99,
    true,
    true,
    '{
        "title": "Last Will and Testament",
        "fields": [
            {
                "id": "testator_name",
                "type": "text",
                "label": "Testator Full Name",
                "required": true
            },
            {
                "id": "testator_address",
                "type": "textarea",
                "label": "Testator Address",
                "required": true
            },
            {
                "id": "executor_name",
                "type": "text",
                "label": "Executor Name",
                "required": true
            },
            {
                "id": "beneficiaries",
                "type": "repeater",
                "label": "Beneficiaries",
                "fields": [
                    {
                        "id": "name",
                        "type": "text",
                        "label": "Beneficiary Name"
                    },
                    {
                        "id": "relationship",
                        "type": "text",
                        "label": "Relationship"
                    },
                    {
                        "id": "inheritance",
                        "type": "textarea",
                        "label": "Inheritance Description"
                    }
                ]
            }
        ]
    }',
    '<div class="will-template"><h1>{{title}}</h1>{{fields}}</div>',
    NOW(),
    1
),
(
    'Business Partnership Agreement',
    'A comprehensive partnership agreement template for business partnerships.',
    'Business Documents',
    79.99,
    true,
    true,
    '{
        "title": "Business Partnership Agreement",
        "fields": [
            {
                "id": "business_name",
                "type": "text",
                "label": "Business Name",
                "required": true
            },
            {
                "id": "partners",
                "type": "repeater",
                "label": "Partners",
                "fields": [
                    {
                        "id": "name",
                        "type": "text",
                        "label": "Partner Name"
                    },
                    {
                        "id": "ownership_percentage",
                        "type": "number",
                        "label": "Ownership Percentage"
                    }
                ]
            },
            {
                "id": "business_purpose",
                "type": "textarea",
                "label": "Business Purpose",
                "required": true
            }
        ]
    }',
    '<div class="partnership-template"><h1>{{title}}</h1>{{fields}}</div>',
    NOW(),
    1
);

-- Insert sample subscriptions
INSERT INTO "Subscriptions" ("UserId", "StripeSubscriptionId", "PlanName", "MonthlyPrice", "StartDate", "NextBillingDate", "CreatedAt", "Status") VALUES
(2, 'sub_test_john_premium', 'Premium Plan', 19.99, NOW() - INTERVAL '15 days', NOW() + INTERVAL '15 days', NOW() - INTERVAL '15 days', 0),
(3, 'sub_test_jane_basic', 'Basic Plan', 9.99, NOW() - INTERVAL '5 days', NOW() + INTERVAL '25 days', NOW() - INTERVAL '5 days', 0);

-- Insert sample payments
INSERT INTO "Payments" ("UserId", "StripePaymentIntentId", "Amount", "Currency", "FormTemplateId", "SubscriptionId", "CreatedAt", "CompletedAt", "Type", "Status") VALUES
(2, 'pi_test_john_poa', 29.99, 'USD', 1, NULL, NOW() - INTERVAL '10 days', NOW() - INTERVAL '10 days', 0, 1),
(3, 'pi_test_jane_will', 49.99, 'USD', 2, NULL, NOW() - INTERVAL '5 days', NOW() - INTERVAL '5 days', 0, 1),
(4, 'pi_test_michael_partnership', 79.99, 'USD', 3, NULL, NOW() - INTERVAL '2 days', NOW() - INTERVAL '2 days', 0, 1),
(2, 'pi_test_john_subscription', 19.99, 'USD', NULL, 1, NOW() - INTERVAL '15 days', NOW() - INTERVAL '15 days', 1, 1),
(3, 'pi_test_jane_subscription', 9.99, 'USD', NULL, 2, NOW() - INTERVAL '5 days', NOW() - INTERVAL '5 days', 1, 1);

-- Insert sample user forms
INSERT INTO "UserForms" ("UserId", "FormTemplateId", "FormData", "CreatedAt", "CompletedAt", "Status") VALUES
(2, 1, '{
    "principal_name": "John Doe",
    "principal_address": "123 Main St, Anytown, ST 12345",
    "agent_name": "Jane Doe",
    "agent_address": "456 Oak Ave, Anytown, ST 12345",
    "powers_granted": ["Banking and financial transactions", "Real estate transactions", "Tax matters"],
    "effective_date": "2024-01-15",
    "durable": true
}', NOW() - INTERVAL '10 days', NOW() - INTERVAL '9 days', 1),
(3, 2, '{
    "testator_name": "Jane Smith",
    "testator_address": "789 Pine St, Anytown, ST 12345",
    "executor_name": "Robert Smith",
    "beneficiaries": [
        {
            "name": "Alice Smith",
            "relationship": "Daughter",
            "inheritance": "50% of estate"
        },
        {
            "name": "Bob Smith",
            "relationship": "Son",
            "inheritance": "50% of estate"
        }
    ]
}', NOW() - INTERVAL '5 days', NOW() - INTERVAL '4 days', 1),
(4, 1, '{
    "principal_name": "Michael Johnson",
    "principal_address": "321 Elm St, Anytown, ST 12345",
    "agent_name": "Linda Johnson",
    "agent_address": "321 Elm St, Anytown, ST 12345",
    "powers_granted": ["Banking and financial transactions", "Legal proceedings", "Business operations"],
    "effective_date": "2024-02-01",
    "durable": false
}', NOW() - INTERVAL '3 days', NULL, 0),
(5, 3, '{
    "business_name": "Wilson & Associates LLC",
    "partners": [
        {
            "name": "Sarah Wilson",
            "ownership_percentage": 60
        },
        {
            "name": "David Wilson",
            "ownership_percentage": 40
        }
    ],
    "business_purpose": "Consulting services in technology and business strategy"
}', NOW() - INTERVAL '1 day', NULL, 0);

-- Insert sample AI conversations
INSERT INTO "AIConversations" ("UserId", "Title", "CreatedAt") VALUES
(2, 'Help with Power of Attorney', NOW() - INTERVAL '10 days'),
(3, 'Questions about Will Template', NOW() - INTERVAL '5 days'),
(4, 'Business Partnership Advice', NOW() - INTERVAL '2 days');

-- Insert sample AI messages
INSERT INTO "AIMessages" ("ConversationId", "Content", "Role", "CreatedAt") VALUES
(1, 'I need help understanding what powers to grant in my power of attorney document.', 0, NOW() - INTERVAL '10 days'),
(1, 'I''d be happy to help you understand the different powers you can grant in a power of attorney. The main categories include: Banking and financial transactions, Real estate transactions, Legal proceedings, Tax matters, Insurance matters, and Business operations. Each serves different purposes depending on your needs.', 1, NOW() - INTERVAL '10 days'),
(1, 'What''s the difference between durable and non-durable power of attorney?', 0, NOW() - INTERVAL '10 days'),
(1, 'A durable power of attorney remains in effect even if you become incapacitated, while a non-durable power of attorney becomes invalid if you lose mental capacity. For most people, a durable power of attorney provides better protection.', 1, NOW() - INTERVAL '10 days'),

(2, 'Can you explain the difference between a will and a living trust?', 0, NOW() - INTERVAL '5 days'),
(2, 'A will is a legal document that takes effect after death and goes through probate, while a living trust takes effect immediately and can help avoid probate. Wills are typically simpler and less expensive to create, while trusts offer more privacy and can provide ongoing management of assets.', 1, NOW() - INTERVAL '5 days'),

(3, 'What should I consider when setting up a business partnership?', 0, NOW() - INTERVAL '2 days'),
(3, 'Key considerations for a business partnership include: ownership percentages, profit and loss distribution, decision-making authority, roles and responsibilities, exit strategies, dispute resolution processes, and capital contributions. It''s important to clearly define these in a partnership agreement.', 1, NOW() - INTERVAL '2 days');

-- Insert sample Power of Attorney records
INSERT INTO "PowerOfAttorney" (
    "PrincipalName", "PrincipalAddress", "PrincipalCity", "PrincipalState", "PrincipalZipCode",
    "AgentName", "AgentAddress", "AgentCity", "AgentState", "AgentZipCode",
    "SuccessorAgentName", "SuccessorAgentAddress", "SuccessorAgentCity", "SuccessorAgentState", "SuccessorAgentZipCode",
    "EffectiveDate", "ExpirationDate", "IsDurable", "IsImmediatelyEffective",
    "BankingPowers", "RealEstatePowers", "LegalPowers", "TaxPowers", "InsurancePowers", "BusinessPowers",
    "SpecialInstructions", "NotaryRequired", "WitnessRequired", "CreatedAt", "UserId"
) VALUES
(
    'John Michael Doe',
    '123 Main Street',
    'Springfield',
    'IL',
    '62701',
    'Jane Elizabeth Doe',
    '123 Main Street',
    'Springfield',
    'IL',
    '62701',
    'Robert James Doe',
    '456 Oak Avenue',
    'Springfield',
    'IL',
    '62702',
    '2024-01-15',
    NULL,
    true,
    true,
    true,
    true,
    true,
    true,
    false,
    false,
    'This power of attorney is granted for the management of all financial affairs during the principal''s temporary absence.',
    true,
    true,
    NOW() - INTERVAL '30 days',
    2
),
(
    'Sarah Michelle Wilson',
    '789 Pine Street',
    'Chicago',
    'IL',
    '60601',
    'David Charles Wilson',
    '789 Pine Street',
    'Chicago',
    'IL',
    '60601',
    'Margaret Ann Wilson',
    '321 Elm Street',
    'Chicago',
    'IL',
    '60602',
    '2024-02-01',
    '2025-02-01',
    false,
    false,
    true,
    false,
    false,
    true,
    true,
    true,
    'Limited power of attorney for business transactions only. Agent may not sell real estate.',
    true,
    false,
    NOW() - INTERVAL '20 days',
    3
),
(
    'Michael Anthony Johnson',
    '456 Maple Drive',
    'Peoria',
    'IL',
    '61601',
    'Linda Susan Johnson',
    '456 Maple Drive',
    'Peoria',
    'IL',
    '61601',
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    '2024-03-01',
    NULL,
    true,
    true,
    true,
    true,
    true,
    true,
    true,
    true,
    'Broad powers granted for all financial and legal matters. This is a durable power of attorney.',
    true,
    true,
    NOW() - INTERVAL '10 days',
    4
);

-- Display summary of created data
SELECT 'Tables created and data inserted successfully!' as status;

SELECT 
    'Users' as table_name, 
    COUNT(*) as record_count 
FROM "Users"
UNION ALL
SELECT 
    'FormTemplates' as table_name, 
    COUNT(*) as record_count 
FROM "FormTemplates"
UNION ALL
SELECT 
    'Subscriptions' as table_name, 
    COUNT(*) as record_count 
FROM "Subscriptions"
UNION ALL
SELECT 
    'Payments' as table_name, 
    COUNT(*) as record_count 
FROM "Payments"
UNION ALL
SELECT 
    'UserForms' as table_name, 
    COUNT(*) as record_count 
FROM "UserForms"
UNION ALL
SELECT 
    'AIConversations' as table_name, 
    COUNT(*) as record_count 
FROM "AIConversations"
UNION ALL
SELECT 
    'AIMessages' as table_name, 
    COUNT(*) as record_count 
FROM "AIMessages"
UNION ALL
SELECT 
    'PowerOfAttorney' as table_name, 
    COUNT(*) as record_count 
FROM "PowerOfAttorney";

-- Show sample users for testing
SELECT 
    'Sample Users for Testing:' as info,
    '' as email,
    '' as password
UNION ALL
SELECT 
    '',
    "Email",
    'TestPassword123!'
FROM "Users"
ORDER BY info DESC, email;