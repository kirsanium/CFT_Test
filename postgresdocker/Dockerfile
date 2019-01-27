FROM postgres:latest
EXPOSE 5432
COPY ./dbscripts/10-init.sh /docker-entrypoint-initdb.d/10-init.sh
COPY ./dbscripts/20-cftdb.sql /docker-entrypoint-initdb.d/20-cftdb.sql