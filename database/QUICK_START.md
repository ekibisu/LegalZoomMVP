# LegalZoom MVP Database Setup - Quick Start

## 🚀 Simple Database Creation

Use the `create_complete_database.sql` script to set up your entire database in one go.

### Prerequisites
- PostgreSQL server running
- Access to PostgreSQL (pgAdmin, psql, or any PostgreSQL client)
- Database credentials: `postgres` / `4pl!ffG@nja` (as per your appsettings.json)

### Steps

1. **Create the Database** (if it doesn't exist):
   ```sql
   CREATE DATABASE legalzoommvp;
   ```

2. **Connect to the Database**:
   - In pgAdmin: Connect to `legalzoommvp` database
   - In psql: `\c legalzoommvp`

3. **Run the Script**:
   - Copy and paste the entire content of `create_complete_database.sql`
   - Or execute the file directly: `\i create_complete_database.sql`

### What Gets Created

#### 📋 Tables
- **Users** - User accounts and authentication
- **FormTemplates** - Legal document templates  
- **UserForms** - User-completed forms
- **Payments** - Payment transactions
- **Subscriptions** - User subscription plans
- **AIConversations** - AI chat conversations
- **AIMessages** - Individual chat messages
- **PowerOfAttorney** - Power of Attorney specific data

#### 👥 Sample Users (Password: `TestPassword123!`)
- `admin@legalzoom.com` (Admin)
- `john.doe@email.com` (Client)
- `jane.smith@email.com` (Client)  
- `michael.johnson@email.com` (Client)
- `sarah.wilson@email.com` (Client)

#### 📄 Sample Form Templates
- Power of Attorney - General ($29.99)
- Will and Testament - Basic ($49.99, Premium)
- Business Partnership Agreement ($79.99, Premium)

#### 💳 Sample Data
- Active subscriptions
- Completed payments
- Completed and draft forms
- AI conversation history
- Power of Attorney records

### Quick Test

After running the script, verify everything worked:

```sql
-- Check all tables have data
SELECT 
    schemaname,
    tablename,
    n_live_tup as row_count
FROM pg_stat_user_tables 
WHERE schemaname = 'public'
ORDER BY tablename;

-- Test login data
SELECT "Email", "FirstName", "LastName", "Role" 
FROM "Users";
```

### Next Steps

1. **Start your application**:
   ```bash
   cd backend
   dotnet run --project LegalZoomMVP.Api
   ```

2. **Test the API** at `http://localhost:5000`

3. **Login with sample users** using password `TestPassword123!`

## 🔧 Troubleshooting

### Common Issues

**Permission Denied**: Ensure your PostgreSQL user has CREATE privileges
```sql
GRANT ALL PRIVILEGES ON DATABASE legalzoommvp TO postgres;
```

**Database Already Exists**: The script will drop and recreate tables, but not the database itself. If you need a fresh start:
```sql
DROP DATABASE IF EXISTS legalzoommvp;
CREATE DATABASE legalzoommvp;
```

**Connection Issues**: Verify your credentials match what's in `appsettings.json`:
- Host: `localhost`  
- Port: `5432`
- Database: `legalzoommvp`
- Username: `postgres`
- Password: `4pl!ffG@nja`

## 📊 Database Schema Overview

```
Users (5 sample records)
├── FormTemplates (3 templates)
│   └── UserForms (4 sample forms)
├── Subscriptions (2 active plans)
│   └── Payments (5 transactions)
├── AIConversations (3 conversations)
│   └── AIMessages (8 messages)
└── PowerOfAttorney (3 POA records)
```

This setup gives you a fully functional database with realistic sample data for development and testing!
