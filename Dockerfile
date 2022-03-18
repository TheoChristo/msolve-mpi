FROM ubuntu:18.04
WORKDIR /mpidocker
COPY . .

RUN apt-get update
RUN apt-get install -y ssh mpich wget

# DOTNET SDK
RUN wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb
RUN apt-get update 
RUN apt-get install -y apt-transport-https
RUN apt-get update
RUN apt-get install -y dotnet-sdk-5.0

# MSolve App
WORKDIR /mpidocker/MSolveApp/Msolve.One.MPI
RUN dotnet build -c Release

WORKDIR /mpidocker
# mpirun -np 2 dotnet /mpidocker/MSolveApp/Msolve.One.MPI/Msolve.One.MPI/bin/Release/net5.0/Msolve.One.MPI.dll