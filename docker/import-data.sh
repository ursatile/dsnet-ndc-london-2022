#!/bin/bash
#run the setup script to create the DB and the schema in the DB
#do this in a loop because the timing for when the SQL instance is ready is indeterminate
for i in {1..50};
do
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P P@ssw0rd -d master -i create-autobarn-schema.sql
    if [ $? -eq 0 ]
    then
        echo "Autobarn database created"
        break
    else
        echo "SQL Server not available yet - retrying in 1 second..."
        sleep 1
    fi
done

echo "Importing data from CSV files using bcp"
/opt/mssql-tools/bin/bcp Autobarn.dbo.Manufacturers in "/usr/src/autobarn/csv-data/manufacturers.csv" -c -t',' -S localhost -U autobarn -P autobarn
/opt/mssql-tools/bin/bcp Autobarn.dbo.Models in "/usr/src/autobarn/csv-data/models.csv" -c -t',' -S localhost -U autobarn -P autobarn
/opt/mssql-tools/bin/bcp Autobarn.dbo.Vehicles in "/usr/src/autobarn/csv-data/vehicles.csv" -c -t',' -S localhost -U autobarn -P autobarn
echo "Database created and populated with test data"
