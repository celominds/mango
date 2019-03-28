FROM microsoft/dotnet
COPY . /Mango
WORKDIR /Mango
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]
EXPOSE 80/tcp
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh
# ENTRYPOINT ["dotnet", "Mango.dll"]