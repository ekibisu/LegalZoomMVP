# LegalZoom MVP Database Management

This directory contains all database-related scripts, schemas, and tools for the LegalZoom MVP application.

## 📁 Directory Structure

```
database/
├── backup/                 # Database backup and restore scripts
│   ├── create_full_backup.sh    # Linux/Mac backup script
│   ├── create_full_backup.bat   # Windows backup script
│   ├── restore_from_backup.sh   # Linux/Mac restore script
│   └── restore_from_backup.bat  # Windows restore script
├── schema/                 # Database schema files
│   └── schema.sql         # Complete database schema
├── seeds/                  # Sample data for development/testing
│   ├── sample_data.sql    # Core sample data
│   └── power_of_attorney_data.sql # POA-specific sample data
├── migrations/             # SQL migration scripts (legacy)
├── setup_database.sh      # Linux/Mac database setup script
├── setup_database.bat     # Windows database setup script
└── README.md              # This file
```

## 🚀 Quick Start

### Prerequisites

1. **PostgreSQL Server** (version 12 or higher)
2. **.NET 8.0 SDK** (for Entity Framework migrations)
3. **psql command line tool** (usually included with PostgreSQL)

### Environment Variables

Set these environment variables before running scripts:

```bash
export DB_HOST=localhost      # Database host
export DB_PORT=5432          # Database port
export DB_NAME=legalzoommvp  # Database name
export DB_USER=postgres      # Database user
```

For Windows Command Prompt:
```cmd
set DB_HOST=localhost
set DB_PORT=5432
set DB_NAME=legalzoommvp
set DB_USER=postgres
```

### Initial Database Setup

**Linux/Mac:**
```bash
chmod +x setup_database.sh
./setup_database.sh
```

**Windows:**
```cmd
setup_database.bat
```

The setup script offers these options:
1. **Full setup** - EF migrations + sample data (recommended)
2. **EF migrations only** - Just the database schema
3. **Sample data only** - Assumes schema already exists
4. **Schema from SQL file** - Legacy schema creation

## 📊 Sample Data

The database includes comprehensive sample data for development and testing:

### Users
- **Admin User**: `admin@legalzoom.com` (Role: Admin)
- **Test Clients**: `john.doe@email.com`, `jane.smith@email.com`, etc.
- **Password**: `TestPassword123!` (for all test users)

### Form Templates
- Power of Attorney - General ($29.99)
- Will and Testament - Basic ($49.99, Premium)
- Business Partnership Agreement ($79.99, Premium)

### Sample Transactions
- Completed payments for form purchases
- Active subscriptions (Premium and Basic plans)
- Completed and draft user forms
- AI conversation history

### Power of Attorney Records
- Multiple POA examples with different configurations
- Various power types (banking, real estate, legal, etc.)
- Different states and agent arrangements

## 🔄 Database Backup and Restore

### Creating Backups

**Linux/Mac:**
```bash
cd database/backup
chmod +x create_full_backup.sh
./create_full_backup.sh
```

**Windows:**
```cmd
cd database\backup
create_full_backup.bat
```

Backups include:
- Complete database schema
- All data (including sample data)
- Indexes and constraints
- Triggers and functions

Backup files are named: `legalzoommvp_backup_YYYYMMDD_HHMMSS.sql`

### Restoring from Backup

**Linux/Mac:**
```bash
cd database/backup
chmod +x restore_from_backup.sh
./restore_from_backup.sh [backup_file]
```

**Windows:**
```cmd
cd database\backup
restore_from_backup.bat [backup_file]
```

If no backup file is specified, the script uses the latest backup (`legalzoommvp_backup_latest.sql`).

⚠️ **Warning**: Restore operations will DROP and recreate the database, destroying all existing data.

## 🏗️ Database Schema

### Core Tables

- **Users** - User accounts and authentication
- **FormTemplates** - Legal document templates
- **UserForms** - User-completed forms
- **Payments** - Payment transactions
- **Subscriptions** - User subscription plans
- **AIConversations** - AI chat conversations
- **AIMessages** - Individual chat messages
- **PowerOfAttorney** - Power of Attorney specific data (via EF migration)

### Enums

- `user_role`: Client, Admin
- `form_status`: Draft, Completed, Exported
- `payment_status`: Pending, Completed, Failed, Refunded
- `payment_type`: OneTime, Subscription
- `subscription_status`: Active, Cancelled, Expired, PastDue
- `message_role`: User, Assistant, System

## 🔧 Entity Framework Migrations

The application uses Entity Framework Core for database migrations. Current migrations include:

- `20250825192740_UpdatePowerOfAttorneyFields` - Power of Attorney table and fields

### Running Migrations Manually

From the backend directory:
```bash
# Update database to latest migration
dotnet ef database update --project LegalZoomMVP.Infrastructure --startup-project LegalZoomMVP.Api

# Create new migration
dotnet ef migrations add MigrationName --project LegalZoomMVP.Infrastructure --startup-project LegalZoomMVP.Api

# Remove last migration
dotnet ef migrations remove --project LegalZoomMVP.Infrastructure --startup-project LegalZoomMVP.Api
```

## 🐛 Troubleshooting

### Common Issues

1. **Connection Failed**
   - Verify PostgreSQL is running
   - Check connection parameters (host, port, credentials)
   - Ensure database user has sufficient privileges

2. **Migration Failed**
   - Ensure .NET SDK is installed
   - Check Entity Framework tools: `dotnet tool install --global dotnet-ef`
   - Verify connection string in `appsettings.json`

3. **Backup/Restore Failed**
   - Ensure `pg_dump` and `psql` are in PATH
   - Check file permissions
   - Verify sufficient disk space

4. **Sample Data Errors**
   - Ensure schema exists before inserting data
   - Check for foreign key constraint violations
   - Verify enum types are created

### Password Hash Note

Sample users use bcrypt-hashed passwords. The hash `$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LwDVKLV8j8qV8wQQW` corresponds to the plaintext password `TestPassword123!`.

## 📝 Development Workflow

1. **Setup**: Run `setup_database.sh` with full setup option
2. **Development**: Make schema changes via Entity Framework migrations
3. **Testing**: Use sample data for consistent test scenarios
4. **Backup**: Create regular backups during development
5. **Deploy**: Use migration scripts for production deployments

## 🔒 Security Considerations

- **Production**: Use strong, unique passwords
- **Backup**: Store backups securely and encrypt sensitive data
- **Access**: Limit database user privileges to minimum required
- **Connection**: Use SSL connections in production environments

## 📋 Maintenance Tasks

### Regular Maintenance

- Create weekly database backups
- Monitor database size and performance
- Update sample data as application evolves
- Test backup/restore procedures regularly

### Before Major Changes

- Create full backup
- Test changes in development environment
- Document schema changes
- Update migration scripts
