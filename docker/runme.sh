#!/bin/bash
docker stop autobarn-mssql2019-container
docker rm autobarn-mssql2019-container
docker image rm autobarn-mssql2019-image

cp ..\dotnet\Autobarn.Data\csv-data\ .\csv-data\

docker build -t autobarn-mssql2019-image .

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P@ssw0rd" --name autobarn-mssql2019-container -p 1433:1433 -d -it autobarn-mssql2019-image

docker exec -u mssql -d autobarn-mssql2019-container /usr/src/autobarn/import-data.sh 