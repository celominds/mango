FROM microsoft/aspnetcore-build:lts
COPY . /Mango
WORKDIR /Mango
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]
EXPOSE 6500/tcp
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh