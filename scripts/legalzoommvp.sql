-- LegalZoom MVP PostgreSQL Database Script
-- Drop existing tables if they exist (in correct order due to foreign keys)
DROP TABLE IF EXISTS "AIMessages" CASCADE;
DROP TABLE IF EXISTS "AIConversations" CASCADE;
DROP TABLE IF EXISTS "Payments" CASCADE;
DROP TABLE IF EXISTS "UserForms" CASCADE;
DROP TABLE IF EXISTS "Subscriptions" CASCADE;
DROP TABLE IF EXISTS "FormTemplates" CASCADE;
DROP TABLE IF EXISTS "Users" CASCADE;

-- Create ENUM types
CREATE TYPE user_role AS ENUM ('Client', 'Admin');
CREATE TYPE form_status AS ENUM ('Draft', 'Completed', 'Exported');
CREATE TYPE payment_status AS ENUM ('Pending', 'Completed', 'Failed', 'Refunded');
CREATE TYPE payment_type AS ENUM ('OneTime', 'Subscription');
CREATE TYPE subscription_status AS ENUM ('Active', 'Cancelled', 'Expired', 'PastDue');
CREATE TYPE message_role AS ENUM ('User', 'Assistant', 'System');

-- Users table
CREATE TABLE "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Email" VARCHAR(255) NOT NULL UNIQUE,
    "PasswordHash" VARCHAR(255) NOT NULL,
    "FirstName" VARCHAR(100) NOT NULL,
    "LastName" VARCHAR(100) NOT NULL,
    "Role" user_role NOT NULL DEFAULT 'Client',
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
    FOREIGN KEY ("CreatedByUserId") REFERENCES "Users"("Id") ON DELETE RESTRICT
);

-- Subscriptions table
CREATE TABLE "Subscriptions" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL,
    "StripeSubscriptionId" VARCHAR(255) NOT NULL UNIQUE,
    "PlanName" VARCHAR(100) NOT NULL,
    "MonthlyPrice" DECIMAL(10,2) NOT NULL,
    "Status" subscription_status NOT NULL DEFAULT 'Active',
    "StartDate" TIMESTAMP WITH TIME ZONE NOT NULL,
    "EndDate" TIMESTAMP WITH TIME ZONE,
    "NextBillingDate" TIMESTAMP WITH TIME ZONE NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
);

-- UserForms table
CREATE TABLE "UserForms" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL,
    "FormTemplateId" INTEGER NOT NULL,
    "FormData" JSONB NOT NULL,
    "Status" form_status NOT NULL DEFAULT 'Draft',
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "CompletedAt" TIMESTAMP WITH TIME ZONE,
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE,
    FOREIGN KEY ("FormTemplateId") REFERENCES "FormTemplates"("Id") ON DELETE RESTRICT
);

-- Payments table
CREATE TABLE "Payments" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL,
    "StripePaymentIntentId" VARCHAR(255) NOT NULL UNIQUE,
    "Amount" DECIMAL(10,2) NOT NULL,
    "Currency" VARCHAR(3) NOT NULL DEFAULT 'USD',
    "Status" payment_status NOT NULL DEFAULT 'Pending',
    "Type" payment_type NOT NULL DEFAULT 'OneTime',
    "FormTemplateId" INTEGER,
    "SubscriptionId" INTEGER,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "CompletedAt" TIMESTAMP WITH TIME ZONE,
    FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE,
    FOREIGN KEY ("FormTemplateId") REFERENCES "FormTemplates"("Id") ON DELETE SET NULL,
    FOREIGN KEY ("SubscriptionId") REFERENCES "Subscriptions"("Id") ON DELETE SET NULL
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
    "Role" message_role NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    FOREIGN KEY ("ConversationId") REFERENCES "AIConversations"("Id") ON DELETE CASCADE
);

-- Create indexes for better performance
CREATE INDEX "IX_Users_Email" ON "Users"("Email");
CREATE INDEX "IX_Users_Role" ON "Users"("Role");
CREATE INDEX "IX_Users_IsActive" ON "Users"("IsActive");

CREATE INDEX "IX_FormTemplates_Category" ON "FormTemplates"("Category");
CREATE INDEX "IX_FormTemplates_IsPremium" ON "FormTemplates"("IsPremium");
CREATE INDEX "IX_FormTemplates_IsActive" ON "FormTemplates"("IsActive");
CREATE INDEX "IX_FormTemplates_CreatedByUserId" ON "FormTemplates"("CreatedByUserId");

CREATE INDEX "IX_UserForms_UserId" ON "UserForms"("UserId");
CREATE INDEX "IX_UserForms_FormTemplateId" ON "UserForms"("FormTemplateId");
CREATE INDEX "IX_UserForms_Status" ON "UserForms"("Status");
CREATE INDEX "IX_UserForms_CreatedAt" ON "UserForms"("CreatedAt");

CREATE INDEX "IX_Payments_UserId" ON "Payments"("UserId");
CREATE INDEX "IX_Payments_Status" ON "Payments"("Status");
CREATE INDEX "IX_Payments_Type" ON "Payments"("Type");
CREATE INDEX "IX_Payments_FormTemplateId" ON "Payments"("FormTemplateId");
CREATE INDEX "IX_Payments_SubscriptionId" ON "Payments"("SubscriptionId");
CREATE INDEX "IX_Payments_CreatedAt" ON "Payments"("CreatedAt");

CREATE INDEX "IX_Subscriptions_UserId" ON "Subscriptions"("UserId");
CREATE INDEX "IX_Subscriptions_Status" ON "Subscriptions"("Status");
CREATE INDEX "IX_Subscriptions_NextBillingDate" ON "Subscriptions"("NextBillingDate");

CREATE INDEX "IX_AIConversations_UserId" ON "AIConversations"("UserId");
CREATE INDEX "IX_AIConversations_CreatedAt" ON "AIConversations"("CreatedAt");

CREATE INDEX "IX_AIMessages_ConversationId" ON "AIMessages"("ConversationId");
CREATE INDEX "IX_AIMessages_Role" ON "AIMessages"("Role");
CREATE INDEX "IX_AIMessages_CreatedAt" ON "AIMessages"("CreatedAt");

-- Create triggers for UpdatedAt timestamps
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW."UpdatedAt" = NOW();
    RETURN NEW;
END;
$$ language 'plpgsql';

CREATE TRIGGER update_users_updated_at BEFORE UPDATE ON "Users" FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
CREATE TRIGGER update_form_templates_updated_at BEFORE UPDATE ON "FormTemplates" FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
CREATE TRIGGER update_user_forms_updated_at BEFORE UPDATE ON "UserForms" FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
CREATE TRIGGER update_ai_conversations_updated_at BEFORE UPDATE ON "AIConversations" FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

-- Insert seed data
-- Admin user
INSERT INTO "Users" ("Email", "PasswordHash", "FirstName", "LastName", "Role", "CreatedAt")
VALUES 
    ('admin@legalzoom.com', '$2a$11$LQyGmOnVFBqKq3/hzjS1Ee8dTwOZJp3bGPmhHq.ZjxZgJQZH4eN8K', 'System', 'Admin', 'Admin', NOW()),
    ('john.doe@email.com', '$2a$11$LQyGmOnVFBqKq3/hzjS1Ee8dTwOZJp3bGPmhHq.ZjxZgJQZH4eN8K', 'John', 'Doe', 'Client', NOW()),
    ('jane.smith@email.com', '$2a$11$LQyGmOnVFBqKq3/hzjS1Ee8dTwOZJp3bGPmhHq.ZjxZgJQZH4eN8K', 'Jane', 'Smith', 'Client', NOW()),
    ('bob.wilson@email.com', '$2a$11$LQyGmOnVFBqKq3/hzjS1Ee8dTwOZJp3bGPmhHq.ZjxZgJQZH4eN8K', 'Bob', 'Wilson', 'Client', NOW());

-- Form templates
INSERT INTO "FormTemplates" ("Name", "Description", "Category", "Price", "IsPremium", "FormSchema", "HtmlTemplate", "CreatedByUserId")
VALUES 
    (
        'Simple Will',
        'Create a basic last will and testament',
        'Estate Planning',
        29.99,
        FALSE,
        '{
            "fields": [
                {"name": "fullName", "type": "text", "label": "Full Legal Name", "required": true},
                {"name": "address", "type": "textarea", "label": "Address", "required": true},
                {"name": "executor", "type": "text", "label": "Executor Name", "required": true},
                {"name": "beneficiaries", "type": "array", "label": "Beneficiaries", "required": true}
            ]
        }',
        '<html><body><h1>Last Will and Testament of {{fullName}}</h1><p>I, {{fullName}}, residing at {{address}}, being of sound mind...</p></body></html>',
        1
    ),
    (
        'Business Formation - LLC',
        'File articles of organization for Limited Liability Company',
        'Business Formation',
        199.99,
        TRUE,
        '{
            "fields": [
                {"name": "companyName", "type": "text", "label": "Company Name", "required": true},
                {"name": "state", "type": "select", "label": "State of Formation", "required": true, "options": ["CA", "NY", "TX", "FL", "WA"]},
                {"name": "registeredAgent", "type": "text", "label": "Registered Agent", "required": true},
                {"name": "members", "type": "array", "label": "LLC Members", "required": true},
                {"name": "purpose", "type": "textarea", "label": "Business Purpose", "required": true}
            ]
        }',
        '<html><body><h1>Articles of Organization - {{companyName}}</h1><p>State: {{state}}</p></body></html>',
        1
    ),
    (
        'Power of Attorney',
        'Grant someone legal authority to act on your behalf',
        'Legal Documents',
        49.99,
        FALSE,
        '{
            "fields": [
                {"name": "principal", "type": "text", "label": "Principal Name", "required": true},
                {"name": "agent", "type": "text", "label": "Agent Name", "required": true},
                {"name": "powers", "type": "multiselect", "label": "Powers Granted", "required": true, "options": ["Financial", "Healthcare", "Real Estate", "Legal"]},
                {"name": "effective", "type": "date", "label": "Effective Date", "required": true}
            ]
        }',
        '<html><body><h1>Power of Attorney</h1><p>Principal: {{principal}}</p><p>Agent: {{agent}}</p></body></html>',
        1
    ),
    (
        'Non-Disclosure Agreement',
        'Protect confidential information in business relationships',
        'Business Contracts',
        39.99,
        FALSE,
        '{
            "fields": [
                {"name": "disclosingParty", "type": "text", "label": "Disclosing Party", "required": true},
                {"name": "receivingParty", "type": "text", "label": "Receiving Party", "required": true},
                {"name": "purpose", "type": "textarea", "label": "Purpose of Disclosure", "required": true},
                {"name": "duration", "type": "number", "label": "Duration (years)", "required": true}
            ]
        }',
        '<html><body><h1>Non-Disclosure Agreement</h1><p>Between {{disclosingParty}} and {{receivingParty}}</p></body></html>',
        1
    );

-- Subscriptions
INSERT INTO "Subscriptions" ("UserId", "StripeSubscriptionId", "PlanName", "MonthlyPrice", "Status", "StartDate", "NextBillingDate")
VALUES 
    (2, 'sub_1234567890abcdef', 'Premium Plan', 19.99, 'Active', NOW() - INTERVAL '15 days', NOW() + INTERVAL '15 days'),
    (3, 'sub_abcdef1234567890', 'Basic Plan', 9.99, 'Active', NOW() - INTERVAL '5 days', NOW() + INTERVAL '25 days');

-- User forms
INSERT INTO "UserForms" ("UserId", "FormTemplateId", "FormData", "Status", "CreatedAt", "CompletedAt")
VALUES 
    (
        2, 
        1, 
        '{
            "fullName": "John Doe",
            "address": "123 Main St, Anytown, CA 90210",
            "executor": "Jane Doe",
            "beneficiaries": [
                {"name": "Jane Doe", "relationship": "Spouse", "percentage": 60},
                {"name": "Tommy Doe", "relationship": "Son", "percentage": 40}
            ]
        }', 
        'Completed',
        NOW() - INTERVAL '3 days',
        NOW() - INTERVAL '2 days'
    ),
    (
        3, 
        2, 
        '{
            "companyName": "Smith Consulting LLC",
            "state": "CA",
            "registeredAgent": "Jane Smith",
            "members": [
                {"name": "Jane Smith", "percentage": 100}
            ],
            "purpose": "Management consulting services"
        }', 
        'Draft',
        NOW() - INTERVAL '1 day',
        NULL
    ),
    (
        4, 
        3, 
        '{
            "principal": "Bob Wilson",
            "agent": "Alice Wilson",
            "powers": ["Financial", "Healthcare"],
            "effective": "2024-01-01"
        }', 
        'Completed',
        NOW() - INTERVAL '7 days',
        NOW() - INTERVAL '6 days'
    ),
    (
        2, 
        4, 
        '{
            "disclosingParty": "John Doe",
            "receivingParty": "ABC Corporation",
            "purpose": "Discussing potential business partnership",
            "duration": 2
        }', 
        'Exported',
        NOW() - INTERVAL '10 days',
        NOW() - INTERVAL '9 days'
    );

-- Payments
INSERT INTO "Payments" ("UserId", "StripePaymentIntentId", "Amount", "Status", "Type", "FormTemplateId", "SubscriptionId", "CreatedAt", "CompletedAt")
VALUES 
    (2, 'pi_1234567890abcdef01', 29.99, 'Completed', 'OneTime', 1, NULL, NOW() - INTERVAL '3 days', NOW() - INTERVAL '3 days'),
    (3, 'pi_abcdef1234567890ab', 199.99, 'Completed', 'OneTime', 2, NULL, NOW() - INTERVAL '1 day', NOW() - INTERVAL '1 day'),
    (4, 'pi_fedcba0987654321fe', 49.99, 'Completed', 'OneTime', 3, NULL, NOW() - INTERVAL '7 days', NOW() - INTERVAL '7 days'),
    (2, 'pi_subscription_123abc', 19.99, 'Completed', 'Subscription', NULL, 1, NOW() - INTERVAL '15 days', NOW() - INTERVAL '15 days'),
    (3, 'pi_subscription_456def', 9.99, 'Completed', 'Subscription', NULL, 2, NOW() - INTERVAL '5 days', NOW() - INTERVAL '5 days'),
    (2, 'pi_nda_payment_789ghi', 39.99, 'Completed', 'OneTime', 4, NULL, NOW() - INTERVAL '10 days', NOW() - INTERVAL '10 days');

-- AI Conversations
INSERT INTO "AIConversations" ("UserId", "Title", "CreatedAt")
VALUES 
    (2, 'Help with Will Creation', NOW() - INTERVAL '4 days'),
    (3, 'LLC Formation Questions', NOW() - INTERVAL '2 days'),
    (4, 'Power of Attorney Guidance', NOW() - INTERVAL '8 days'),
    (2, 'NDA Review', NOW() - INTERVAL '11 days');

-- AI Messages
INSERT INTO "AIMessages" ("ConversationId", "Content", "Role", "CreatedAt")
VALUES 
    (1, 'I need help creating a will. What information do I need to provide?', 'User', NOW() - INTERVAL '4 days'),
    (1, 'To create a will, you''ll need to provide your full legal name, address, choose an executor, and list your beneficiaries with how you''d like your assets distributed. You may also want to specify guardians for minor children and any specific bequests.', 'Assistant', NOW() - INTERVAL '4 days'),
    (1, 'What makes a good executor?', 'User', NOW() - INTERVAL '4 days'),
    (1, 'A good executor should be trustworthy, organized, and willing to take on the responsibility. They should be someone who can handle financial matters and communicate well with beneficiaries. It''s often best to choose someone younger than you who lives nearby.', 'Assistant', NOW() - INTERVAL '4 days'),
    
    (2, 'What are the benefits of forming an LLC?', 'User', NOW() - INTERVAL '2 days'),
    (2, 'LLCs provide personal asset protection, tax flexibility, operational simplicity, and professional credibility. Your personal assets are generally protected from business debts, and you can choose how to be taxed. LLCs also have fewer formal requirements than corporations.', 'Assistant', NOW() - INTERVAL '2 days'),
    
    (3, 'When does a power of attorney become effective?', 'User', NOW() - INTERVAL '8 days'),
    (3, 'A power of attorney can be effective immediately upon signing, or it can be "springing," meaning it only becomes effective when a specific event occurs (like incapacitation). The document should clearly specify when it becomes effective.', 'Assistant', NOW() - INTERVAL '8 days'),
    
    (4, 'What should be included in an NDA?', 'User', NOW() - INTERVAL '11 days'),
    (4, 'A comprehensive NDA should include: definition of confidential information, obligations of the receiving party, exceptions to confidentiality, duration of the agreement, consequences of breach, and return of confidential materials. It should also specify the governing law.', 'Assistant', NOW() - INTERVAL '11 days');

-- Display summary of inserted data
SELECT 
    'Users' as table_name, COUNT(*) as record_count FROM "Users"
UNION ALL
SELECT 
    'FormTemplates', COUNT(*) FROM "FormTemplates"
UNION ALL
SELECT 
    'UserForms', COUNT(*) FROM "UserForms"
UNION ALL
SELECT 
    'Payments', COUNT(*) FROM "Payments"
UNION ALL
SELECT 
    'Subscriptions', COUNT(*) FROM "Subscriptions"
UNION ALL
SELECT 
    'AIConversations', COUNT(*) FROM "AIConversations"
UNION ALL
SELECT 
    'AIMessages', COUNT(*) FROM "AIMessages";

-- Example queries to test the data
SELECT 'Sample Users:' as info;
SELECT "Id", "Email", "FirstName", "LastName", "Role" FROM "Users";

SELECT 'Sample Form Templates:' as info;
SELECT "Id", "Name", "Category", "Price", "IsPremium" FROM "FormTemplates";

SELECT 'User Forms with Template Names:' as info;
SELECT 
    uf."Id",
    u."FirstName" || ' ' || u."LastName" as "UserName",
    ft."Name" as "FormName",
    uf."Status",
    uf."CreatedAt"
FROM "UserForms" uf
JOIN "Users" u ON uf."UserId" = u."Id"
JOIN "FormTemplates" ft ON uf."FormTemplateId" = ft."Id";

COMMIT;