-- LegalZoom MVP Sample Data
-- This script inserts sample data for development and testing purposes

-- Clear existing data (in correct order due to foreign keys)
TRUNCATE TABLE "AIMessages" RESTART IDENTITY CASCADE;
TRUNCATE TABLE "AIConversations" RESTART IDENTITY CASCADE;
TRUNCATE TABLE "Payments" RESTART IDENTITY CASCADE;
TRUNCATE TABLE "UserForms" RESTART IDENTITY CASCADE;
TRUNCATE TABLE "Subscriptions" RESTART IDENTITY CASCADE;
TRUNCATE TABLE "FormTemplates" RESTART IDENTITY CASCADE;
TRUNCATE TABLE "Users" RESTART IDENTITY CASCADE;

-- Insert sample users
INSERT INTO "Users" ("Email", "PasswordHash", "FirstName", "LastName", "Role", "CreatedAt", "IsActive") VALUES
-- Password for all test users is: "TestPassword123!"
-- Hash generated with bcrypt
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

-- Display summary
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
FROM "AIMessages";

-- Show sample data verification
SELECT 'Sample data inserted successfully!' as status;
