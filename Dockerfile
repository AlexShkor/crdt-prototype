# base dotnet core image
FROM microsoft/dotnet:latest

# create app folder
RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

# bundle app source
COPY . /usr/src/app
RUN dotnet restore ./src/Crdt.Core.API

EXPOSE 5000
CMD [ "dotnet", "run", "-p", "./src/Crdt.Core.API" ]


# build image
# docker build -t egortsaryk9/crdt-core-api .
# docker run -p 5000:5000 -d egortsaryk9/crdt-core-api
