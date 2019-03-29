# using vNext image
FROM mcr.microsoft.com/mssql/server:2017-latest

# create directory within SQL container for database files
RUN mkdir /var/opt/mssql/backup

#copy the database files from host to container
COPY DatabaseA.mdf C:\\SQLServer
COPY DatabaseA_log.ldf C:\\SQLServer

COPY Mango.mdf .
COPY Mango.ndf .
COPY Mango.ldf .

# set environment variables
ENV sa_password=celominds@098

ENV ACCEPT_EULA=Y

ENV attach_dbs="[{'dbName':'testdb_mango','dbFiles':['C:\\SQLServer\\DatabaseA.mdf','C:\\SQLServer\\DatabaseA_log.ldf']},{'dbName':'DatabaseB','dbFiles':['C:\\SQLServer\\DatabaseB.mdf','C:\\SQLServer\\DatabaseB_Data.ndf','C:\\SQLServer\\DatabaseB_log.ldf']}"