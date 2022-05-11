FROM mcr.microsoft.com/mssql/server:2019-CU10-ubuntu-20.04
USER root

EXPOSE 1433

# Install dos2unix
RUN apt-get -y update && \
    apt-get install -y dos2unix

RUN mkdir -p /usr/src/autobarn
WORKDIR /usr/src/autobarn
COPY . /usr/src/autobarn

#fix any DOS-style line endings
RUN dos2unix *
RUN dos2unix csv-data/*

# Grant permissions for the import-data script to be executable
RUN chmod 755 /usr/src/autobarn/import-data.sh

USER mssql
ENTRYPOINT /bin/bash ./entrypoint.sh
