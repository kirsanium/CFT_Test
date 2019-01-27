export PGUSER=postgres
psql <<- EOSQL
    CREATE USER cftdb;
    CREATE DATABASE cftdb;
    GRANT ALL PRIVILEGES ON DATABASE cftdb TO cftdb;
EOSQL