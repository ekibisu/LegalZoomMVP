-- Migration script to convert Role column from string to integer in Users table
-- Converts 'Client' to 0 and 'Admin' to 1, then changes column type to integer

-- Step 1: Update string values to integer
UPDATE "Users" SET "Role" = '0' WHERE "Role" = 'Client';
UPDATE "Users" SET "Role" = '1' WHERE "Role" = 'Admin';

-- Step 2: Alter column type to integer
ALTER TABLE "Users" ALTER COLUMN "Role" TYPE integer USING "Role"::integer;

-- Optional: Set default value for Role
ALTER TABLE "Users" ALTER COLUMN "Role" SET DEFAULT 0;

-- Add a new column called role1 of type integer to Users table
ALTER TABLE "Users" ADD COLUMN "role1" integer;

-- Drop the Role column from Users table
ALTER TABLE "Users" DROP COLUMN "Role";

-- Rename role1 to Role
ALTER TABLE "Users" RENAME COLUMN "role1" TO "Role";
