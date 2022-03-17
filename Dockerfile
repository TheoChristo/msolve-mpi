#Download base image ubuntu 16.04
FROM ubuntu:18.04
WORKDIR /mpidocker
COPY . .

# Update Software repository
RUN apt-get update
RUN apt-get install -y wget build-essential ssh git

# MPICH
RUN apt-get install -y mpich

# DOTNET SDK 5.0
# - Trust MS package singing
RUN wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb
# - Install .net SDK 5.0
RUN apt-get update 
RUN apt-get install -y apt-transport-https
RUN apt-get update
RUN apt-get install -y dotnet-sdk-5.0

# MPI.NET
# - Download 
RUN git clone https://github.com/rogancarr/MpiDotNet.git
# - Build library
RUN apt-get update 
RUN apt-get install -y dos2unix
WORKDIR /mpidocker/MpiDotNet/MpiLibrary/MpiLibrary
RUN make all

# .NET App
# - Build .net App
WORKDIR /mpidocker/MpiDotNet/MpiDotNetApp/MpiDotNetApp
RUN dotnet build -c Release -r ubuntu.18.04-x64
# - Run .net App
WORKDIR /mpidocker/MpiDotNet
RUN wget https://raw.githubusercontent.com/dotnet/machinelearning/024bd4452e1d3660214c757237a19d6123f951ca/test/data/housing.txt
RUN echo 'export LD_LIBRARY_PATH="$(pwd)/MpiLibrary/MpiLibrary/x64/linux/"' >> ~/.profile 
RUN echo 'export LD_LIBRARY_PATH="$(pwd)/MpiLibrary/MpiLibrary/x64/linux/"' >> ~/.bashrc  
# mpirun -np 2 dotnet MpiDotNetApp/MpiDotNetApp/bin/Release/netcoreapp2.1/ubuntu.18.04-x64/MpiDotNetApp.dll housing.txt