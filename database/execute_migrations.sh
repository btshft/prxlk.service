#!/bin/bash
# Launch MSSQL and send to background
/opt/mssql/bin/sqlservr &
# Wait for it to be available
echo "Waiting for MS SQL to be available"
/opt/mssql-tools/bin/sqlcmd -l 30 -S localhost -h-1 -V1 -U sa -P ${SA_PASSWORD} -Q "SET NOCOUNT ON SELECT \"DB IS UP\" , @@servername"
is_up=$?
while [ ${is_up} -ne 0 ] ; do 
  /opt/mssql-tools/bin/sqlcmd -l 30 -S localhost -h-1 -V1 -U sa -P ${SA_PASSWORD} -Q "SET NOCOUNT ON SELECT \"DB IS UP\" , @@servername"
  is_up=$?
  sleep 5
done

# Run every script in /scripts
for script in database/scripts/*.sql
  do /opt/mssql-tools/bin/sqlcmd -U sa -P ${SA_PASSWORD} -l 30 -e -i ${script}
done

# So that the container doesn't shut down, sleep this thread
sleep infinity