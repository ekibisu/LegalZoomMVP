-- Migration: Add customer and update advocate fields
CREATE TABLE IF NOT EXISTS customers (
    id SERIAL PRIMARY KEY,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    national_id VARCHAR(50),
    passport_number VARCHAR(50),
    gender VARCHAR(20) NOT NULL,
    mobile_number VARCHAR(20) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Update advocates table
ALTER TABLE advocates
    ADD COLUMN national_id VARCHAR(50),
    ADD COLUMN passport_number VARCHAR(50);

ALTER TABLE advocates
    DROP COLUMN middle_name,
    DROP COLUMN id_number;
