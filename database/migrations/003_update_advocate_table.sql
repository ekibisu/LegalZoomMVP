-- Migration: Update advocates table to match DTOs and backend usage
ALTER TABLE advocates
    RENAME COLUMN id_number TO national_id;

ALTER TABLE advocates
    ADD COLUMN passport_number VARCHAR(50);

-- If you want to allow NULLs for passport_number, adjust as needed
ALTER TABLE advocates
    ALTER COLUMN passport_number DROP NOT NULL;

-- Add role column for future use if needed
ALTER TABLE advocates
    ADD COLUMN role VARCHAR(20) DEFAULT 'Lawyer';

-- Remove middle_name if not used
ALTER TABLE advocates
    DROP COLUMN IF EXISTS middle_name;

-- Ensure all columns used by backend exist and are correct
-- (first_name, last_name, national_id, passport_number, gender, lsk_p105, mobile_number, email, password_hash, role)

-- Update timestamps if needed
ALTER TABLE advocates
    ALTER COLUMN updated_at SET DEFAULT CURRENT_TIMESTAMP;
