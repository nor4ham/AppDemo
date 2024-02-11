#!/bin/bash
set -e

# Function to check if PostgreSQL is ready
function waitForPostgres() {
    while ! nc -z $DB_HOST $DB_PORT; do
        echo "Waiting for PostgreSQL to be ready..."
        sleep 1
    done
}

# Replace these with your actual environment variables
DB_HOST=postgresql_database
DB_PORT=5432

# Wait for PostgreSQL
waitForPostgres

# Run database migrations
dotnet ef database update

# Start the application
dotnet AppDemo.dll