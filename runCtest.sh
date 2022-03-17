#!/bin/bash

echo "Running C test for openMPI"
echo "-> Compiling"
mpicc -o ./ompiCTest/hello ./ompiCTest/hello.c
# mpicc-mpich-mp -o hello ./hello.c
echo "-> Running"
mpiexec --allow-run-as-root -np 2 ./ompiCTest/hello
# mpiexec-mpich-mp -np 2 ./hello